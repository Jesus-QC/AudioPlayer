using System.Collections.Generic;
using System.IO;
using System.Linq;
using AudioPlayer.Core.Components;
using Dissonance;
using MEC;
using UnityEngine;
using Log = Exiled.API.Features.Log;

namespace AudioPlayer.API;

public static class AudioController
{
    public static DissonanceComms Comms => Radio.comms;
    public static CustomMicrophone Microphone;
    public static RoomChannel? Channel;
        
    public static bool AutomaticMusic;
    public static bool LoopMusic;
    public static float Volume = 1;

    public static void PlayFromFile(string path, float volume = 100, bool loop = false, bool automatic = false)
    {
        Timing.RunCoroutine(Play(path, volume, loop, automatic));
    }
        
    private static IEnumerator<float> Play(string path, float volume = 25, bool loop = false, bool automatic = false)
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
            
        Microphone!.File = File.OpenRead(path);
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
        
        if (Channel is not null)
            Comms.RoomChannels.Close(Channel.Value);
        
        Log.Debug("Stopped the mic.", AudioPlayer.Singleton.Config.ShowDebugLogs);
    }

    public static void RefreshChannels()
    {
        if (Channel is not null)
            Comms.RoomChannels.Close(Channel.Value);
        
        Channel = Comms.RoomChannels.Open("Intercom", false, ChannelPriority.Default, Volume);
    }
    
    public static void AddMic() => Microphone = Comms.gameObject.AddComponent<CustomMicrophone>();
}