﻿using AudioPlayer.API;
using Exiled.API.Features;
using HarmonyLib;
using AudioPlayer.Core.Events;
using Exiled.API.Enums;
using Version = System.Version;

namespace AudioPlayer;

public class AudioPlayer : Plugin<Config>
{
    public override string Author { get; } = "Jesus-QC";
    public override string Name { get; } = "AudioPlayer";
    public override string Prefix { get; } = "audio_player";
    public override PluginPriority Priority { get; } = PluginPriority.High;
    public override Version Version { get; } = new (1, 1, 0);
    public override Version RequiredExiledVersion { get; } = new (5, 2, 2);

    public static AudioPlayer Singleton;

    private Harmony _harmony;
    private CustomEvents _events;
        
    public override void OnEnabled()
    {
        Singleton = this;

        _harmony = new Harmony("com.jesusqc.audioplayer");
        _harmony.PatchAll();
            
        _events = new CustomEvents();

        Exiled.Events.Handlers.Server.RestartingRound += _events.OnRestartingRound;
        Exiled.Events.Handlers.Server.WaitingForPlayers += _events.OnWaitingForPlayers;
        Exiled.Events.Handlers.Server.RoundStarted += _events.OnStarted;
        Exiled.Events.Handlers.Server.RoundEnded += _events.OnEnded;
        Exiled.Events.Handlers.Server.RespawningTeam += _events.OnRespawningTeam;
        Exiled.Events.Handlers.Player.Verified += _events.OnVerified;
            
        AudioEvents.AudioStopped += _events.OnAudioStopped;
            
        base.OnEnabled();
    }

    public override void OnDisabled()
    {
        _harmony.UnpatchAll(_harmony.Id);
        _harmony = null;

        Singleton = null;

        AudioEvents.AudioStopped -= _events.OnAudioStopped;
            
        Exiled.Events.Handlers.Server.RestartingRound -= _events.OnRestartingRound;
        Exiled.Events.Handlers.Server.WaitingForPlayers -= _events.OnWaitingForPlayers;
        Exiled.Events.Handlers.Server.RoundStarted -= _events.OnStarted;
        Exiled.Events.Handlers.Server.RoundEnded -= _events.OnEnded;
        Exiled.Events.Handlers.Server.RespawningTeam -= _events.OnRespawningTeam;
        Exiled.Events.Handlers.Player.Verified -= _events.OnVerified;
            
        _events = null;
            
        base.OnDisabled();
    }
}