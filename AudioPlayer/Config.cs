using System.Collections.Generic;
using System.ComponentModel;
using AudioPlayer.Core.Structures;
using Exiled.API.Features;
using Exiled.API.Interfaces;

namespace AudioPlayer;

public class Config : IConfig
{
    public bool IsEnabled { get; set; } = true;
    public bool ShowDebugLogs { get; set; } = false;
    
    [Description("Special Events Automatic Music, blank to disable.")]
    public List<AudioFile> LobbyPlaylist { get; set; } = new () { new AudioFile(), new AudioFile(){Path = System.IO.Path.Combine(Paths.Configs, "Audios", "test2.mp3")} };
    public AudioFile RoundStartClip { get; set; } = new ();
    public AudioFile RoundEndClip { get; set; } = new ();
    public AudioFile MtfSpawnClip { get; set; } = new ();
    public AudioFile ChaosSpawnClip { get; set; } = new ();
}