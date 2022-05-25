using System.Collections.Generic;
using System.IO;
using System.Linq;
using AudioPlayer.Core.Components;
using Dissonance;
using MEC;
using NLayer;
using UnityEngine;
using Log = Exiled.API.Features.Log;

namespace AudioPlayer.API
{
    public static class AudioController
    {
        public static DissonanceComms Comms => Radio.comms;
        public static CustomMpegMicrophone Microphone;
        
        public static bool AutomaticMusic = false;
        public static bool LoopMusic = false;
        public static float Volume = 1;

        public static readonly List<string> MutedPlayers = new List<string>();

        public static IEnumerator<float> PlayFromFile(string path, float volume = 100, bool loop = false, bool automatic = false)
        {
            if(string.IsNullOrWhiteSpace(path))
                yield break;
            
            if (!File.Exists(path))
            {
                Log.Error($"Error trying to play: {path}. File not found.");
                yield break;
            }
            
            Stop();

            yield return Timing.WaitForOneFrame;
            yield return Timing.WaitForOneFrame;

            Volume = Mathf.Clamp(volume, 0, 100) / 100;
            RefreshChannels();

            if (Microphone is null)
                AddMic();
            
            Microphone!.File = new MpegFile(path);
            Microphone.stop = false;

            Comms._capture._microphone = Microphone;
            Comms.ResetMicrophoneCapture();
            Comms.IsMuted = false;

            AutomaticMusic = automatic;
            LoopMusic = loop;
        }

        public static void Stop()
        {
            if (Microphone is null)
                return;

            Microphone.stop = true;
            
            Log.Debug("Stopped the mic.", AudioPlayer.Singleton.Config.ShowDebugLogs);
        }

        public static void RefreshChannels()
        {
            foreach (var channel in Comms.PlayerChannels._openChannelsBySubId.Values.ToList())
            {
                Comms.PlayerChannels.Close(channel);
                Comms.PlayerChannels.Open(channel.TargetId, false, ChannelPriority.Default, Volume);
            }
        }

        public static void OnPlayerJoinedSession(VoicePlayerState player)
        {
            Log.Debug($"A player joined the session. ({player.Name})", AudioPlayer.Singleton.Config.ShowDebugLogs);

            Comms.PlayerChannels.Open(player._name, false, ChannelPriority.Default, Volume);
        }

        public static void OnPlayerLeftSession(VoicePlayerState player)
        {
            Log.Debug($"A player left the session. ({player.Name})", AudioPlayer.Singleton.Config.ShowDebugLogs);
        }
        
        public static void AddMic() => Microphone = Comms.gameObject.AddComponent<CustomMpegMicrophone>();
    }
}