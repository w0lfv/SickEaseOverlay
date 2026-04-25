# SickEaseOverlay

A lightweight external overlay for game screens that helps reduce 3D motion sickness (a.k.a. simulator sickness) by providing fixed visual cues like a crosshair.

## Description

Many gamers around the world experience motion sickness caused by fast-moving visuals, especially in first-person games or during rapid camera movements. One commonly shared trick in online communities is to place a physical marker—like a Post-it note or a crosshair—at the center of the screen to provide a stable visual anchor.

![GcbS4kpXcAAcJ-I](https://github.com/user-attachments/assets/70edcf33-c7c5-4367-ac75-b55ada6b2451)

Inspired by this idea, I experimented with placing a Post-it note at the center of my screen and found that it noticeably reduced my symptoms of motion sickness.

However, physically sticking something onto your monitor isn't ideal. That's why I developed this alternative.

## Features

![screenshot](https://github.com/user-attachments/assets/922d1887-5772-4324-80bd-9b08269cf7a8)

- Fullscreen overlay with multi-monitor support  
- Application-specific capture overlay  
- Fully customizable: adjust size, opacity, color, and crosshair type  
- Click-through overlay that does not interfere with input  
- Settings are saved automatically and restored on next launch  

## Download

Pre-built binaries are available on the [Releases page](https://github.com/w0lfv/SickEaseOverlay/releases). Two variants are provided — pick whichever fits your environment:

| Variant | Size | .NET 8 Runtime required? | Description |
| --- | --- | --- | --- |
| **Self-contained** (`...-self-contained.zip`) | ~155 MB | No | Single `SickEaseOverlay.exe`. Bundles the entire .NET runtime — works on any Windows machine without extra installs. |
| **Framework-dependent** (`...-framework-dependent.zip`) | ~180 KB | Yes | Lightweight build that uses the system-installed [.NET 8 Desktop Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/8.0). |

Download the variant you want, extract the archive, and run `SickEaseOverlay.exe`.

## Requirements

- Windows 10 or later
- [.NET 8 Desktop Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)

## Build from Source

Clone the repository and run a debug build:

```sh
git clone https://github.com/w0lfv/SickEaseOverlay.git
cd SickEaseOverlay
dotnet build -c Release
```

To produce the same release binaries that are published on the Releases page, use one of the following `dotnet publish` commands.

**Self-contained, single-file build** — produces a single `SickEaseOverlay.exe` (~155 MB) that runs on any Windows 10+ machine without requiring the .NET runtime:

```sh
dotnet publish SickEaseOverlay/SickEaseOverlay.csproj -c Release -r win-x64 \
  --self-contained true \
  -p:PublishSingleFile=true \
  -p:IncludeNativeLibrariesForSelfExtract=true \
  -p:DebugType=None -p:DebugSymbols=false \
  -o publish/self-contained
```

**Framework-dependent build** — produces a small (~180 KB) multi-file bundle that requires the [.NET 8 Desktop Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) to be installed on the target machine:

```sh
dotnet publish SickEaseOverlay/SickEaseOverlay.csproj -c Release -r win-x64 \
  --self-contained false \
  -p:DebugType=None -p:DebugSymbols=false \
  -o publish/framework-dependent
```

The outputs will be written to `publish/self-contained/` and `publish/framework-dependent/` respectively.

## Known Limitations

- Exclusive fullscreen games may render on top of the overlay. Switching the game to **Borderless Windowed** or **Windowed** mode resolves this.
- To overlay on top of an application that runs as administrator (e.g. some anti-cheat protected games), SickEaseOverlay must also be launched as administrator.
- Application mode follows a window's position and size, but cannot follow a window across virtual desktops.
- The overlay is click-through and cannot be focused — close the application from the main settings window.

## Privacy

SickEaseOverlay runs entirely on your machine. It does not connect to the internet, collect telemetry, or transmit any data. Settings are stored locally at `%AppData%\SickEaseOverlay\settings.json`.

## Changelog

### 1.0.1
- Added persistent settings — your last used configuration is restored on next launch (saved at `%AppData%\SickEaseOverlay\settings.json`).
- Fixed an initialization bug where the crosshair type could be invalid on first launch.
- Fixed the overlay not being applied to the selected monitor on startup.
- Improved performance of the application window list scanning.
- Fixed a `Pen` resource leak in the overlay form.
- Added bounds and handle validity checks to prevent rare crashes.

### 1.0.0
- Initial release.

## Disclaimer

This is not a medical product, and results may vary. It is intended as a simple, optional aid for users who suffer from mild to moderate motion sickness during computer use.

## License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for more information.
