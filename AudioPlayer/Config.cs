using System.Collections.Generic;
using System.ComponentModel;
using AudioPlayer.Core.Structures;
using Exiled.API.Interfaces;

namespace AudioPlayer
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool ShowDebugLogs { get; set; } = false;
        
        [Description("EXPERIMENTAL: Can cause issues. (Leave blank for default)")]
        public string AudioName { get; set; } = null;

        [Description("Special Events Automatic Music, blank to disable.")]
        public AudioFile LobbyMusic { get; set; } = new AudioFile();
        public AudioFile MtfSpawnMusic { get; set; } = new AudioFile();
        public AudioFile ChaosSpawnMusic { get; set; } = new AudioFile();

        public Dictionary<string, AudioFile> CassieToAudio { get; set; } = new Dictionary<string, AudioFile>
        {
            ["1 out of 3 generators activated."] = new AudioFile()
        };
    }
}