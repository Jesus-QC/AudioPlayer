using Assets._Scripts.Dissonance;
using AudioPlayer.API;
using Exiled.API.Features;
using Exiled.Events.EventArgs;
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

            if (!string.IsNullOrWhiteSpace(AudioPlayer.Singleton.Config.AudioName))
                Server.Host.ReferenceHub.nicknameSync.Network_myNickSync = AudioPlayer.Singleton.Config.AudioName;
            
            Server.Host.Radio.Network_syncPrimaryVoicechatButton = true;
            Server.Host.DissonanceUserSetup.NetworkspeakingFlags = SpeakingFlags.IntercomAsHuman;

            AudioPlayer.Singleton.Config.LobbyMusic.Play(true, true);
        }

        public void OnStarted()
        {
            if (AudioController.AutomaticMusic)
                AudioController.Stop();
        }

        public void OnRespawningTeam(RespawningTeamEventArgs ev)
        {
            switch (ev.NextKnownTeam)
            {
                case SpawnableTeamType.ChaosInsurgency:
                    AudioPlayer.Singleton.Config.ChaosSpawnMusic.Play(false, true);
                    break;
                case SpawnableTeamType.NineTailedFox:
                    AudioPlayer.Singleton.Config.MtfSpawnMusic.Play(false, true);
                    break;
            }
        }
    }
}