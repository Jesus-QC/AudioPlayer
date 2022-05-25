using System;
using System.Collections.Generic;
using AudioPlayer.API;
using Dissonance.Audio.Capture;
using NAudio.Wave;
using NLayer;
using UnityEngine;
using Log = Exiled.API.Features.Log;

namespace AudioPlayer.Core.Components
{
    // Streaming audio as a fake microphone.
    // https://placeholder-software.co.uk/dissonance/docs/Tutorials/Custom-Microphone-Capture.html#step-4-file-streaming
    public class CustomMpegMicrophone : MonoBehaviour, IMicrophoneCapture
    {
        public bool IsRecording { get; private set; }
        public TimeSpan Latency { get; private set; }
        
        public MpegFile File;
        public bool stop;
        
        private readonly List<IMicrophoneSubscriber> _subscribers = new List<IMicrophoneSubscriber>();

        private WaveFormat _format = new WaveFormat(44100, 1);
        private readonly float[] _frame = new float[980];
        private readonly byte[] _frameBytes = new byte[980 * 4];
        
        private float _elapsedTime;

        public WaveFormat StartCapture(string micName)
        {
            if (stop)
                return null;
            
            Log.Debug("Starting capture.", AudioPlayer.Singleton.Config.ShowDebugLogs);
            Log.Debug($"Mic details: {File.SampleRate}hz ({File.Channels} channels) {File.Duration} Volume: {AudioController.Volume}", AudioPlayer.Singleton.Config.ShowDebugLogs);
            
            AudioController.Comms._capture._network = AudioController.Comms._net;

            File.StereoMode = StereoMode.DownmixToMono;
            _format = new WaveFormat(File.SampleRate, 1);

            IsRecording = true;
            Latency = TimeSpan.Zero;
            return _format;
        }

        public void StopCapture()
        {
            Log.Debug("Stopping capture.", AudioPlayer.Singleton.Config.ShowDebugLogs);

            IsRecording = false;
            
            if (File == null)
                return;
            
            File.Dispose();
            File = null;
        }
        
        public void Subscribe(IMicrophoneSubscriber listener) => _subscribers.Add(listener);
        public bool Unsubscribe(IMicrophoneSubscriber listener) => _subscribers.Remove(listener);
        
        public bool UpdateSubscribers()
        {
            _elapsedTime += Time.unscaledDeltaTime;

            while (_elapsedTime > 0.022f)
            {
                _elapsedTime -= 0.022f;
                
                // Read bytes from file
                var readLength = File.ReadSamples(_frameBytes, 0, _frameBytes.Length);

                // Zero the entire buffer so bits not written to will be silent
                Array.Clear(_frame, 0, _frame.Length);

                // Copy the bytes that were read into the audio buffer as floats
                Buffer.BlockCopy(_frameBytes, 0, _frame, 0, readLength);

                foreach (var subscriber in _subscribers)
                    subscriber.ReceiveMicrophoneData(new ArraySegment<float>(_frame), _format);
            }

            if (stop)
                return true;
            
            if (File.Position * File.Channels < File.Length - 9216) 
                return false;
            
            if (AudioController.LoopMusic)
                File.Position = 0;
            else
            {
                stop = true;
                return true;
            }

            return false;
        }
    }
}