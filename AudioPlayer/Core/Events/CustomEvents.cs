using System;
using AudioPlayer.API;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using Mirror;
using Respawning;

namespace AudioPlayer.Core.Events
{
    public class CustomEvents
    {
        public void OnRestartingRound()
        {
            AudioController.Comms.OnPlayerJoinedSession -= AudioController.OnPlayerJoinedSession;
            AudioController.Comms.OnPlayerLeftSession -= AudioController.OnPlayerLeftSession; ;
        } 

        public void OnWaitingForPlayers()
        {
            AudioController.Comms.OnPlayerJoinedSession += AudioController.OnPlayerJoinedSession;
            AudioController.Comms.OnPlayerLeftSession += AudioController.OnPlayerLeftSession;
            
            AudioController.PlayFromFile(AudioPlayer.Singleton.Config.LobbyMusic, true, true);
        }

        public void OnStarting()
        {
            if(AudioController.AutomaticMusic)
                AudioController.Stop();
        }

        /*public void OnRespawningTeam(RespawningTeamEventArgs ev)
        {
            string path = "";
            
            switch (ev.NextKnownTeam)
            {
                case SpawnableTeamType.ChaosInsurgency:
                    path = AudioPlayer.Singleton.Config.MtfSpawnMusic;
                    break;
                case SpawnableTeamType.NineTailedFox:
                    path = AudioPlayer.Singleton.Config.ChaosSpawnMusic;
                    break;
            }
            
            AudioController.PlayFromFile(path, false, true);
        }*/
    }
}