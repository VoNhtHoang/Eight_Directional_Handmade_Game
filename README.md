# Game With MonoGame C#

## Content/Res/Tile
- characters sprite sheet by ErisEsra()
- tile map by 

## Usage
FOR LEARNING PURPOSE ONLY !!!


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