# AudioPlayer [TESTING]

Plays audio

Commands: `audioplayer play/stop <path>`


CONVERT AUDIO TO STREAMABLE AUDIO:
With ffmpeg where AudioFile.wav is your mp3 or wav audio:

```
ffmpeg.exe -re -i AudioFile.wav -f f32le -ar 48000 -ac 1 output.raw
```

output.raw is the converted audio.

https://user-images.githubusercontent.com/69375249/169623435-a51e0c3a-43e0-45b0-bd4c-ab68ab2813b4.mp4
