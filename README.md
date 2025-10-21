# Game With MonoGame C#

## Content/Res/Tile
- characters sprite sheet by ErisEsra()
- tile map by 

## Usage
FOR LEARNING PURPOSE ONLY !!!

If you do not have .NET package please read the document and download: 

If you've had .NET on your system:
- Open the repos / directory:
- Then run ```dotnet run```

### Release / Excutable File


## Bug Fixing (If Happened)
dotnet mgcb-editor Content/Content.mgcb

dotnet mgcb /@:"Content/Content.mgcb"

dotnet clean

dotnet run

dotnet add package Serilog

dotnet add package Serilog.Sinks.File

dotnet add  package MonoGame.Extend.Tiled


Trong ./Content/Content.mgcb, ngay dưới hàng References, thêm các dòng ref dll này vào:
/reference:C:/Users/hoang/.nuget/packages/monogame.extended.content.pipeline/5.1.0/tools/MonoGame.Extended.Content.Pipeline.dll
/reference:C:/Users/hoang/.nuget/packages/monogame.extended.content.pipeline/5.1.0/tools/MonoGame.Extended.dll


## Directory

<pre>
📁 Root
├── 📁 Content
│   ├── 📁 bin
│   ├── 📁 character
│   │   ├── Sheet*
│   │   └── *.aseprite
│   │
│   ├── 📁 map
│   │   ├── map.tmx
│   │   ├── rules1.tmx
│   │   └── rules.txt
│   ├── 📁 obj
│   └── Content.mgcb
│
├── 📁 obj
├── Game1.cs
├── Program.cs
├── Player.cs
├── InterfacePlayerStatus.cs
├── TileMapManager.cs
├── Knight.csproj
└── .gitignore
</pre>