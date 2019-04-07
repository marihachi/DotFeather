# <img src="docs/logo.svg"/> 
[![GitHub issues](https://img.shields.io/github/issues/xeltica/dotfeather.svg?style=flat-square)][issues] [![GitHub pull requests](https://img.shields.io/github/issues-pr/xeltica/dotfeather.svg?style=flat-square)][pulls] [![GitHub Releases](https://img.shields.io/github/release/xeltica/DotFeather.svg?style=flat-square)][releases] [![License](https://img.shields.io/github/license/xeltica/dotfeather.svg?style=flat-square)](LICENSE)


[issues]: //github.com/xeltica/dotfeather/issues
[pulls]: //github.com/xeltica/dotfeather/pulls
[releases]: //github.com/xeltica/dotfeather/releases

DotFeather is a lightweight generic 2D gameengine for C#/.NET Standard 2.0.

[日本語](README-ja.md) ・ English

## To Build

```
git clone https://github.com/xeltica/DotFeather.git
cd DotFeather
nuget restore
dotnet build
```

## Features

- Lightweight processing
    - It can display 10000 sprites at 60fps [<sup>*1</sup>](#f1)
- 2D-specified Graphics System
    - Sprite - Display textures on the screen
    - Tilemap - Map textures on the grid
    - Graphic - Draw lines, rectangles etc
- Keyboard Input
- Mouse Input
- Playing music
- Playing SFX
- High Extensibility
    - Add original rendering method
    - Add original audio processor

----

<p id="f1">1: It depends on your computer's spec.</p>


## Documents

[Documents](docs/index.md)

## Contributing

coming soon


## Donate

You want to donate for me? Thank you very much! Please see [this page](//xeltica.work/en/donation.html) how to pay me.

...or let's become my patron!

[![become_a_patron](https://c5.patreon.com/external/logo/become_a_patron_button@2x.png)](https://patreon.com/xeltica)

## LICENSE

[MIT](LICENSE)
