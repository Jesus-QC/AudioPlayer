using System.Collections.Generic;
using System.IO;
using System.Linq;
using AudioPlayer.Core.Components;
using Dissonance;
using Dissonance.Integrations.MirrorIgnorance;
using Mirror;
using UnityEngine;
using Log = Exiled.API.Features.Log;

namespace AudioPlayer.API
{
    public static class AudioController
    {
        private static DissonanceComms _comms;
        public static DissonanceComms Comms => _comms ??= Object.FindObjectOfType<DissonanceComms>();

        public static bool AutomaticMusic = false;
        public static bool LoopMusic = false;
        
        public static void PlayFromFile(string path, bool loop = false, bool automatic = false)
        {
            if(string.IsNullOrWhiteSpace(path))
                return;
            
            if (!File.Exists(path))
            {
                Log.Error($"Error trying to play: {path}. File not found.");
                return;
            }
            
            Stop();

            var newMic = Comms.gameObject.AddComponent<CustomMicrophone>();
            newMic.File = File.OpenRead(path);

            Comms._capture._microphone = newMic;
            Comms.ResetMicrophoneCapture();

            RefreshChannels();

            AutomaticMusic = automatic;
            LoopMusic = loop;
        }

        public static void Stop()
        {
            if (!Comms.gameObject.TryGetComponent<CustomMicrophone>(out var oldMic)) 
                return;

            oldMic.StopCapture();
            Object.Destroy(oldMic);
            
            Log.Debug("Stopped and destroyed the mic.", AudioPlayer.Singleton.Config.ShowDebugLogs);
        }
        
        public static void RefreshChannels()
        {
            foreach (var player in Comms.Players)
            {
                Log.Info(player._name);
                Comms.PlayerChannels.Open(player._name);
            }
        }
        
        public static void OnPlayerJoinedSession(VoicePlayerState player)
        {
            Log.Debug($"A player joined the session. ({player.Name})", AudioPlayer.Singleton.Config.ShowDebugLogs);

            Comms.PlayerChannels.Open(player._name);
        }

        public static void OnPlayerLeftSession(VoicePlayerState player)
        {
            Log.Debug($"A player left the session. ({player.Name})", AudioPlayer.Singleton.Config.ShowDebugLogs);
        }
    }
}