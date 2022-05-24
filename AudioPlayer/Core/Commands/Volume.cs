using System;
using AudioPlayer.API;
using CommandSystem;
using Exiled.Permissions.Extensions;
using UnityEngine;

namespace AudioPlayer.Core.Commands
{
    public class Volume : ICommand
    {
        public static Volume Instance { get; } = new Volume();
        
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("audioplayer.volume"))
            {
                response = "You dont have perms to do that";
                return false;
            }

            if (!float.TryParse(arguments.At(0), out float volume))
            {
                response = "Couldn't parse that volume, make sure it is a integer between 0 and 100";
            }
            
            AudioController.Volume = Mathf.Clamp(volume, 0, 100) / 100;
            AudioController.RefreshChannels();
            
            response = $"Changed the volume to {volume}.";
            return true;
        }

        public string Command { get; } = "volume";
        public string[] Aliases { get; } = {"v"};
        public string Description { get; } = "volumes the current playing audio.";
    }
}