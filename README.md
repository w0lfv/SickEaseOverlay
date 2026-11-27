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
- Application-specific capture overlay — follows the window's client area, shows only while that app (or the settings window) is focused, and re-attaches automatically when the app is restarted  
- Crosshair shapes: Cross, X, Circle, Square, Diamond — with adjustable size, length, center gap (keeps the game's own crosshair visible), and hollow outlines  
- Fully customizable opacity and colors  
- Optional **Secondary** visual anchor in its own color — edge bars, corner brackets, a thin border, or evenly spaced dots across the screen — so you can pick one that doesn't cover your game's HUD  
- Click-through overlay that does not interfere with input  
- Global hotkey to show/hide the overlay (e.g. in menus or cutscenes) — default **Ctrl+Shift+F10**; click the **Hotkey** box and press a new combination to change it (must include Ctrl or Alt, or be an F-key; Backspace clears, Esc cancels)  
- Settings are saved automatically and restored on next launch  

## Download

Pre-built binaries are available on the [Releases page](https://github.com/w0lfv/SickEaseOverlay/releases). Two variants are provided — pick whichever fits your environment:

| Variant | Size | .NET 10 Runtime required? | Description |
| --- | --- | --- | --- |
| **Self-contained** (`...-self-contained.zip`) | ~52 MB | No | Single `SickEaseOverlay.exe`. Bundles the entire .NET runtime — works on any Windows machine without extra installs. |
| **Framework-dependent** (`...-framework-dependent.zip`) | ~190 KB | Yes | Lightweight build that uses the system-installed [.NET 10 Desktop Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/10.0). |

Download the variant you want, extract the archive, and run `SickEaseOverlay.exe`.

## Requirements

- Windows 10 or later
- [.NET 10 Desktop Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/10.0) — framework-dependent build only

## Build from Source

Requires the [.NET 10 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/10.0). Clone the repository and build:

```sh
git clone https://github.com/w0lfv/SickEaseOverlay.git
cd SickEaseOverlay
dotnet build -c Release
```

To produce the same release binaries that are published on the Releases page, use one of the following `dotnet publish` commands.

**Self-contained, single-file build** — produces a single compressed `SickEaseOverlay.exe` (~52 MB) that runs on any Windows 10+ machine without requiring the .NET runtime:

```sh
dotnet publish SickEaseOverlay/SickEaseOverlay.csproj -c Release -r win-x64 \
  --self-contained true \
  -p:PublishSingleFile=true \
  -p:IncludeNativeLibrariesForSelfExtract=true   -p:EnableCompressionInSingleFile=true \
  -p:DebugType=None -p:DebugSymbols=false \
  -o publish/self-contained
```

**Framework-dependent build** — produces a small (~190 KB) multi-file bundle that requires the [.NET 10 Desktop Runtime](https://dotnet.microsoft.com/en-us/download/dotnet/10.0) to be installed on the target machine:

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

### 1.1.0
- Upgraded to .NET 10 (.NET 8 reached end of support on November 10, 2026). The framework-dependent build now needs the .NET 10 Desktop Runtime.
- Self-contained build is now compressed (~155 MB → ~52 MB).
- Added a **Show Overlay** toggle with a recordable global hotkey (default **Ctrl+Shift+F10**).
- Application mode: the crosshair is now centered on the window's client area instead of including the title bar and borders.
- Application mode: the overlay is shown only while the target app (or the settings window) is focused, and hides when the target is minimized or closed.
- Application mode: re-attaches automatically when the saved app starts after SickEaseOverlay or is restarted; falls back to matching by process name when the window title has changed. The app list refreshes each time the dropdown opens.
- Fixed the monitor/app selection reverting to the startup value after a display change (e.g. a game switching resolution) or a mode switch.
- Only one instance can run at a time.
- Per-monitor DPI awareness: the overlay stays sharp on monitors with different scaling, and crosshair sizes now scale with each monitor's display scaling (e.g. 2x at 200%) so they look the same size everywhere.
- The **Secondary** crosshair now has selectable shapes: Edges (previous style), Corners, Border, or None.
- Added a **Length** setting for the Cross crosshair, independent of line thickness (5–200 px).
- New crosshair shapes **X** and **Diamond**, a **Gap** setting for an empty center on Cross/X, and a **Hollow** option for outlined Circle/Square/Diamond.
- New Secondary shape **Dots** (evenly spaced dots across the screen as a fixed visual reference), and the Secondary crosshair now has its own color.
- Secondary size now adjusts in finer steps (1–20, 5 px per step). Existing settings are converted automatically and look exactly the same.
- The Circle crosshair is now the same size as the Square at the same size setting.
- Choosing the overlay's internal transparency color (Wheat) no longer makes the crosshair invisible.

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
