using System;
using System.IO;
using System.Linq;
using AudioPlayer.API;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;

namespace AudioPlayer.Core.Commands
{
    public class Test : ICommand
    {
        public static Test Instance { get; } = new Test();
        
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            foreach (var player in AudioController.Comms.Players)
            {
                Log.Info(player.Playback);
            }
            
            response = "";
            return true;
        }

        public string Command { get; } = "test";
        public string[] Aliases { get; } = {"t"};
        public string Description { get; } = "Plays audio.";
    }
}