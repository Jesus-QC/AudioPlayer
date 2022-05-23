using System.ComponentModel;
using System.IO;
using Exiled.API.Features;
using Exiled.API.Interfaces;

namespace AudioPlayer
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;

        public bool ShowDebugLogs { get; set; } = false;

        [Description("Must had FFmpeg installed on machine, use only if you have a prestant machine.")]
        public bool UseFFmpegConverter { get; set; } = true;

        [Description("FFmpeg executable path")]
        public string FFmpegPath { get; set; } = "path";

        [Description("Special Events Automatic Music, blank to disable.")]
        public string LobbyMusic { get; set; } = Path.Combine(Paths.Configs, "lobby.mp3");
        public string MtfSpawnMusic { get; set; } = Path.Combine(Paths.Configs, "mtf.mp3");
        public string ChaosSpawnMusic { get; set; } = Path.Combine(Paths.Configs, "chaos.mp3");
        
    }
}