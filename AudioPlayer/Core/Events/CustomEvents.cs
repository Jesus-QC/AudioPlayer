using Assets._Scripts.Dissonance;
using AudioPlayer.API;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
using MEC;
using Respawning;

namespace AudioPlayer.Core.Events
{
    public class CustomEvents
    {
        public void OnRestartingRound()
        {
            AudioController.Comms.OnPlayerJoinedSession -= AudioController.OnPlayerJoinedSession;
            AudioController.Comms.OnPlayerLeftSession -= AudioController.OnPlayerLeftSession;
        } 

        public void OnWaitingForPlayers()
        {
            AudioController.Comms.OnPlayerJoinedSession += AudioController.OnPlayerJoinedSession;
            AudioController.Comms.OnPlayerLeftSession += AudioController.OnPlayerLeftSession;

            if(!string.IsNullOrWhiteSpace(AudioPlayer.Singleton.Config.AudioName))
                Server.Host.ReferenceHub.nicknameSync.Network_myNickSync = AudioPlayer.Singleton.Config.AudioName;
            
            Server.Host.Radio.Network_syncPrimaryVoicechatButton = true;
            Server.Host.DissonanceUserSetup.NetworkspeakingFlags = SpeakingFlags.IntercomAsHuman;

            Timing.RunCoroutine(AudioController.PlayFromFile(AudioPlayer.Singleton.Config.LobbyMusic, true, true));
        }

        public void OnStarted()
        {
            if (AudioController.AutomaticMusic)
                AudioController.Stop();
        }

        public void OnRespawningTeam(RespawningTeamEventArgs ev)
        {
            string path = "";
            
            switch (ev.NextKnownTeam)
            {
                case SpawnableTeamType.ChaosInsurgency:
                    path = AudioPlayer.Singleton.Config.ChaosSpawnMusic;
                    break;
                case SpawnableTeamType.NineTailedFox:
                    path = AudioPlayer.Singleton.Config.MtfSpawnMusic;
                    break;
            }
            
            AudioController.PlayFromFile(path, false, true);
        }
    }
}