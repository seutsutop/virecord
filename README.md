# VIrecord

Screen recorder for Windows — capture screen, encode with FFmpeg, no watermark.

## Features

- **Screen capture** via GDI (works on all Windows)
- **Hardware encoding** — NVENC (NVIDIA), QSV (Intel), AMF (AMD)
- **Software encoding** — x264
- **Output formats** — MP4, AVI, MKV, GIF
- **Audio recording** — system + microphone
- **Hotkeys** — global shortcuts
- **No watermark** — MIT licensed, fully open source

## Requirements

- Windows 10+
- [.NET 8 Runtime](https://dotnet.microsoft.com/download/dotnet/8.0) (or use self-contained build)
- [FFmpeg](https://ffmpeg.org/) in PATH (for encoding)

## Download

Get the latest release from [Releases](https://github.com/seutsutop/virecord/releases).

## Building

```bash
dotnet restore VIrecord.sln
dotnet build VIrecord.sln -c Release
```

Or open `VIrecord.sln` in Visual Studio 2022+.

## License

[MIT License](LICENSE.txt) — Copyright (c) 2026 VIbrah