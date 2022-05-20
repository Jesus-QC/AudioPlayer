# AudioPlayer [TESTING]

Plays audio


CONVERT AUDIO TO STREAMABLE AUDIO:
With ffmpeg where AudioFile.wav is your mp3 or wav audio:

```
ffmpeg.exe -re -i AudioFile.wav -f f32le -ar 48000 -ac 1 output.raw
```

output.raw is the converted audio.
