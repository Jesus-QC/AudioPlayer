using System.Collections.Generic;
using AudioPlayer.API;
using AudioPlayer.Core.Structures;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using Mirror;
using Respawning;
using UnityEngine;

namespace AudioPlayer.Core.Events;

public class CustomEvents
{
    private NetworkIdentity _manager;

    public void OnRestartingRound()
    {
        AudioController.Stop();
        AudioController.Channel = null;
    }

    public void OnWaitingForPlayers()
    {
        List<AudioFile> playlist = AudioPlayer.Singleton.Config.LobbyPlaylist;
            
        if (playlist.Count > 0)
            playlist[Random.Range(0, playlist.Count)].Play(false, true);
            
        _manager = GameObject.Find("GameManager").GetComponent<NetworkIdentity>();
    }

    public void OnVerified(VerifiedEventArgs ev)
    {
        ev.Player.Connection.Send(new ObjectDestroyMessage {netId = _manager.netId});
        MirrorExtensions.SendSpawnMessageMethodInfo.Invoke(null, new object[] { _manager, ev.Player.Connection });
    }

    public void OnStarted()
    {
        if (AudioController.AutomaticMusic)
            AudioController.Stop();
        
        AudioPlayer.Singleton.Config.RoundStartClip.Play(false, true);
    }

    public void OnRespawningTeam(RespawningTeamEventArgs ev)
    {
        switch (ev.NextKnownTeam)
        {
            case SpawnableTeamType.ChaosInsurgency:
                AudioPlayer.Singleton.Config.ChaosSpawnClip.Play(false, true);
                break;
            case SpawnableTeamType.NineTailedFox:
                AudioPlayer.Singleton.Config.MtfSpawnClip.Play(false, true);
                break;
        }
    }

    public void OnEnded(RoundEndedEventArgs ev) => AudioPlayer.Singleton.Config.RoundEndClip.Play(false, true);

    public void OnAudioStopped()
    {
        if (!Round.IsLobby || !AudioController.AutomaticMusic) 
            return;
            
        List<AudioFile> playlist = AudioPlayer.Singleton.Config.LobbyPlaylist;
            
        if (playlist.Count > 0)
            playlist[Random.Range(0, playlist.Count)].Play(false, true);
    }
}