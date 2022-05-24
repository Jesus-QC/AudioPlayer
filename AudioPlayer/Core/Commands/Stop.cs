using System;
using AudioPlayer.API;
using CommandSystem;
using Exiled.Permissions.Extensions;

namespace AudioPlayer.Core.Commands
{
    public class Stop : ICommand
    {
        public static Stop Instance { get; } = new Stop();
        
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("audioplayer.stop"))
            {
                response = "You dont have perms to do that";
                return false;
            }
            
            AudioController.Stop();
            
            response = "Stopped.";
            return true;
        }

        public string Command { get; } = "Stop";
        public string[] Aliases { get; } = {"s"};
        public string Description { get; } = "Stops the current playing audio.";
    }
}