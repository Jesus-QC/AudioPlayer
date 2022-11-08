using System;
using System.Collections.Generic;
using System.IO;
using AudioPlayer.API;
using Dissonance.Audio.Capture;
using Exiled.API.Features;
using MEC;
using NAudio.Wave;
using UnityEngine;

namespace AudioPlayer.Core.Components;

// Streaming audio as a fake microphone.
// https://placeholder-software.co.uk/dissonance/docs/Tutorials/Custom-Microphone-Capture.html#step-4-file-streaming
public class CustomMicrophone : MonoBehaviour, IMicrophoneCapture
{
    public bool IsRecording { get; private set; }
    public TimeSpan Latency { get; private set; }
        
    public FileStream File;
    public bool stop;
        
    private readonly List<IMicrophoneSubscriber> _subscribers = new();

    private readonly WaveFormat _format = new WaveFormat(48000, 1);
    private readonly float[] _frame = new float[960];
    private readonly byte[] _frameBytes = new byte[960 * 4];
        
    private float _elapsedTime;

    public WaveFormat StartCapture(string micName)
    {
        if (stop)
            return null;
        
        Log.Debug("Starting capture.", AudioPlayer.Singleton.Config.ShowDebugLogs);

        AudioController.Comms._capture._network = AudioController.Comms._net;
            
        IsRecording = true;
        Latency = TimeSpan.Zero;
        
        AudioEvents.OnAudioStarted();
        
        return _format;
    }

    public void StopCapture()
    {
        Log.Debug("Stopping capture.", AudioPlayer.Singleton.Config.ShowDebugLogs);

        IsRecording = false;
        
        Timing.RunCoroutine(TryToSafelyReleaseResources());
    }
    
    private IEnumerator<float> TryToSafelyReleaseResources()
    {
        if (!stop)
        {
            yield return Timing.WaitForSeconds(1f);
            if (IsRecording)
            {
                Log.Debug("Dissonance tried to restart audio.", AudioPlayer.Singleton.Config.ShowDebugLogs);
                yield break;
            }
        }

        File?.Dispose();
        File = null;
            
        AudioEvents.OnAudioStopped();
    }
        
    public void Subscribe(IMicrophoneSubscriber listener) => _subscribers.Add(listener);
    public bool Unsubscribe(IMicrophoneSubscriber listener) => _subscribers.Remove(listener);
        
    public bool UpdateSubscribers()
    {
        _elapsedTime += Time.unscaledDeltaTime;

        while (_elapsedTime > 0.02f)
        {
            _elapsedTime -= 0.02f;

            // Read bytes from file
            int readLength = File.Read(_frameBytes, 0, _frameBytes.Length);

            // Zero the entire buffer so bits not written to will be silent
            Array.Clear(_frame, 0, _frame.Length);

            // Copy the bytes that were read into the audio buffer as floats
            Buffer.BlockCopy(_frameBytes, 0, _frame, 0, readLength);

            foreach (IMicrophoneSubscriber subscriber in _subscribers)
                subscriber.ReceiveMicrophoneData(new ArraySegment<float>(_frame), _format);
        }

        if (stop)
            return true;

        if (File.Position != File.Length) 
            return false;

        if (AudioController.LoopMusic)
        {
            File.Position = 0;
            AudioEvents.OnAudioLooped();
        }
        else
        {
            stop = true;
            return true;
        }

        return false;
    }
}