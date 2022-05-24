using AudioPlayer.API;
using Exiled.API.Features;
using MEC;

namespace AudioPlayer.Core.Structures
{
    public class AudioFile
    {
        public string Path { get; set; } = System.IO.Path.Combine(Paths.Configs, "Audios", "test.mp3");
        public float Volume { get; set; } = 100;

        public void Play(bool loop = false, bool automatic = false)
        {
            Timing.RunCoroutine(AudioController.PlayFromFile(Path, Volume, loop, automatic));
        }
    }
}