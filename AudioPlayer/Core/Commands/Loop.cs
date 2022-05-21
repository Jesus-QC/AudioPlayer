using System;
using AudioPlayer.API;
using CommandSystem;
using Exiled.Permissions.Extensions;

namespace AudioPlayer.Core.Commands
{
    public class Loop : ICommand
    {
        public static Loop Instance { get; } = new Loop();
        
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("audioplayer.loop"))
            {
                response = "You dont have perms to do that";
                return false;
            }
            
            AudioController.LoopMusic = !AudioController.LoopMusic;
            
            response = $"Loop = {AudioController.LoopMusic}";
            return true;
        }

        public string Command { get; } = "loop";
        public string[] Aliases { get; } = {"l"};
        public string Description { get; } = "Loops the song.";
    }
}