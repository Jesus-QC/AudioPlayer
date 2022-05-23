using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using AudioPlayer.Core.Components;
using Dissonance;
using Dissonance.Integrations.MirrorIgnorance;
using MEC;
using UnityEngine;
using Xabe.FFmpeg;
using Log = Exiled.API.Features.Log;

namespace AudioPlayer.API
{
    public static class AudioController
    {
        public static DissonanceComms Comms => Radio.comms;

        public static bool AutomaticMusic = false;
        public static bool LoopMusic = false;
        
        public static async Task PlayFromFile(string path, bool loop = false, bool automatic = false)
        {
            if(string.IsNullOrWhiteSpace(path))
                return;
            
            if (!File.Exists(path))
            {
                Log.Error($"Error trying to play: {path}. File not found.");
                return;
            }
            if (path.EndsWith(".mp3"))
            {
                string[] pathParts = path.Split(Path.DirectorySeparatorChar);
                await Convert(pathParts, path);
                path = Path.Combine(AudioPlayer.AudioPath, $"{pathParts.ElementAt(pathParts.Length - 1).Replace(".mp3", "")}.raw");
            }

            Stop();

            var newMic = Comms.gameObject.AddComponent<CustomMicrophone>();
            newMic.File = File.OpenRead(path);
            
            Comms._capture._microphone = newMic;
            Comms.ResetMicrophoneCapture();
            Comms.IsMuted = false;

            AutomaticMusic = automatic;
            LoopMusic = loop;
        }

        public static void Stop()
        {
            if (!Comms.gameObject.TryGetComponent<CustomMicrophone>(out var oldMic)) 
                return;
            
            oldMic.StopCapture();
            Object.Destroy(oldMic);
            foreach (string file in Directory.GetFiles(AudioPlayer.AudioPath))
                File.Delete(file);

            Log.Debug("Stopped and destroyed the mic.", AudioPlayer.Singleton.Config.ShowDebugLogs);
        }

        private static async Task Convert(string[] pathParts, string path) => await FFmpeg.Conversions.New().Start($"-i {path} -f f32le -ar 48000 -ac 1 {Path.Combine(AudioPlayer.AudioPath, $"{pathParts.ElementAt(pathParts.Length - 1).Replace(".mp3", "")}.raw")}");

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