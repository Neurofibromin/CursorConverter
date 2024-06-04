﻿# CursorConverter

A program to convert mouse cursors between formats.

## Aims:

Convert **from** and **to**:
<div>

- `.ani`
- `.ico`
- `.xcg`
- `.cur`
- `.CursorFX`
- `.CurXPTheme`
- `hyprcursor`
- `jpg`
- ani, ico, xcg, cur, png, jpg, hyprcursor, cursorfx, curxptheme

## Compatibility matrix

| to &darr; \\ from &rarr; 	| ani		| ico	| xcg	| cur	| png	| jpg	| hyprcursor| cursorfx	| curxptheme|
|-------------------------	|-------	|-----	|-----	|-----	|-------|-----	|-----		|-----		|   -----	|
| ani                     	| &check; 	|		|     	|   	|		|		|			|			|			|
| ico		              	| &check;	|     	|     	|   	|		|		|			|			|			|
| xcg                    	|		 	|     	|     	|   	|		|		|			|			|			|
| cur                    	|		 	|     	|     	|   	|		|		|			|			|			|
| png                    	|		 	|&check;|     	|   	|		|		|			|			|			|
| jpg                    	|		 	|     	|     	|   	|		|		|			|			|			|
| hyprcursor                |		 	|     	|     	|   	|		|		|			|			|			|
| cursorfx                  |		 	|     	|     	|   	|		|		|			|			|			|
| curxptheme                |		 	|     	|     	|   	|		|		|			|			|			|


## Releases

| Windows  | Linux | OSX (semi-supported) |
| ------------- | ------------- | ------------- |
| [x64](https://github.com/Neurofibromin/CursorConverter/releases/download/0.1.3/CursorConverter-win-x64-8.0.x.exe) | [x64](https://github.com/Neurofibromin/CursorConverter/releases/download/0.1.3/CursorConverter-linux-x64-8.0.x) | [x64](https://github.com/Neurofibromin/CursorConverter/releases/download/0.1.3/CursorConverter-osx-x64-8.0.x)
| [x32](https://github.com/Neurofibromin/CursorConverter/releases/download/0.1.3/CursorConverter-win-x86-8.0.x.exe) | N/A | N/A |
| [arm64](https://github.com/Neurofibromin/CursorConverter/releases/download/0.1.3/CursorConverter-win-arm64-8.0.x.exe) | [arm64](https://github.com/Neurofibromin/CursorConverter/releases/download/0.1.3/CursorConverter-linux-arm64-8.0.x) | [arm64](https://github.com/Neurofibromin/CursorConverter/releases/download/0.1.3/CursorConverter-osx-arm64) |

Additional releases:
| package  | x64 | arm64 |
| ------------- | ------------- | ------------- |
| Flatpak	|		[](https://github.com/Neurofibromin/CursorConverter/releases/download/0.1.3/CursorConverter-0.1.3-1.x86_64.flatpak)		|	[](https://github.com/Neurofibromin/CursorConverter/releases/download/0.1.3/CursorConverter-0.1.3-1.aarch64.flatpak)			|
|    RPM	|		[](https://github.com/Neurofibromin/CursorConverter/releases/download/0.1.3/CursorConverter_0.1.3-1.x86_64.rpm)		|	N/A			|
|    DEB	|		[](https://github.com/Neurofibromin/CursorConverter/releases/download/0.1.3/CursorConverter_0.1.3-1_amd64.deb)		|	[](https://github.com/Neurofibromin/CursorConverter/releases/download/0.1.3/CursorConverter_0.1.3-1_arm64.deb)			|
| AppImage	|		N/A		|	[](https://github.com/Neurofibromin/CursorConverter/releases/download/0.1.3/CursorConverter-0.1.3-1.x86_64.AppImage)			|


### Status
[![.NET](https://github.com/Neurofibromin/CursorConverter/actions/workflows/master.yml/badge.svg)](https://github.com/Neurofibromin/CursorConverter/actions/workflows/master.yml)


## On the shoulders of giants:
The following projects gave me inspiration, in many cases I reimplemented their algorithms in c#.

- ani2ico by TeoBigusGeekus
- [cursor-converter](https://github.com/avagordon01/cursor-converter) by avagordon01
- [Metamorphosis](https://github.com/SystemRage/Metamorphosis) by SystemRage
- [Iconolatry](https://github.com/SystemRage/Iconolatry) by SystemRage
- [cfx2xc](https://github.com/coolwanglu/cfx2xc) by coolwanglu
- [CURConvertor](https://github.com/CactusCata/CURConvertor) by CactusCata
- [BmpConverter](https://github.com/AM71113363/BmpConverter) by AM71113363
- [xcursorgen](https://gitlab.freedesktop.org/xorg/app/xcursorgen)

- [xcur2png](https://github.com/eworm-de/xcur2png) maintained by eworm-de
- [hyprcursor](https://github.com/hyprwm/hyprcursor) from the Hypr project
- [hyprlang](https://github.com/hyprwm/hyprlang) from the Hypr project


Similar projects:
- [ani_file](https://github.com/HoangEevee/ani_file) by HoangEevee