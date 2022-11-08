using System.IO;
using AudioPlayer.API;
using Exiled.API.Features;

namespace AudioPlayer.Core.Structures;

public class AudioFile
{
    public string Path { get; set; } = System.IO.Path.Combine(Paths.Configs, "Audios", "test.raw");
    public float Volume { get; set; } = 100;

    public void Play(bool loop = false, bool automatic = false)
    {
        if (!File.Exists(Path))
            Log.Debug($"File not found on path {Path}", AudioPlayer.Singleton.Config.ShowDebugLogs);
        
        AudioController.PlayFromFile(Path, Volume, loop, automatic);
    }
}