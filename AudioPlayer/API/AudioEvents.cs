using Exiled.Events;
using Exiled.Events.Extensions;

namespace AudioPlayer.API;

public static class AudioEvents
{
    public static event Events.CustomEventHandler AudioStopped;
    public static event Events.CustomEventHandler AudioStarted;
    public static event Events.CustomEventHandler AudioLooped;

    public static void OnAudioStopped() => AudioStopped.InvokeSafely();
    public static void OnAudioStarted() => AudioStarted.InvokeSafely();
    public static void OnAudioLooped() => AudioLooped.InvokeSafely();
}