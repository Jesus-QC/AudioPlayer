using System;
using System.Linq;
using AudioPlayer.API;
using CommandSystem;
using Dissonance;
using Exiled.API.Features;

namespace AudioPlayer.Core.Commands
{
    public class Mute : ICommand
    {
        public static Mute Instance { get; } = new Mute();
        
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            var ply = Player.Get(sender);

            if (ply == null)
            {
                response = "Only players are allowed to run this command.";
                return false;
            }

            bool mute = AudioController.MutedPlayers.Contains(ply.Radio.mirrorIgnorancePlayer._playerId);
            
            if (mute)
                UnmutePlayer(ply);
            else
                MutePlayer(ply);

            response = $"Server audio muted = {mute}";
            return true;
        }

        public string Command { get; } = "mute";
        public string[] Aliases { get; } = {"m"};
        public string Description { get; } = "Stops the current playing audio for you.";

        private static void UnmutePlayer(Player player)
        {
            var id = player.Radio.mirrorIgnorancePlayer._playerId;
            AudioController.MutedPlayers.Remove(id);
            AudioController.Comms.PlayerChannels.Open(id, false, ChannelPriority.Default, AudioController.Volume);
        }
        
        private static void MutePlayer(Player player)
        {
            var id = player.Radio.mirrorIgnorancePlayer._playerId;
            AudioController.MutedPlayers.Add(id);
            var channel = AudioController.Comms.PlayerChannels._openChannelsBySubId.FirstOrDefault(x => x.Value.TargetId == id);
            AudioController.Comms.PlayerChannels.Close(channel.Value);
        }
    }
}