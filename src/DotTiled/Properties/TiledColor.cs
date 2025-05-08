using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.InteropServices;

namespace DotTiled;

/// <summary>
/// Represents a Tiled color.
/// </summary>
[StructLayout(LayoutKind.Explicit)]
public struct TiledColor : IParsable<TiledColor>, IEquatable<TiledColor>
{
  #region Static_TiledColors

  /// <summary>
  /// Transparent <see cref="TiledColor"/> (A:0, R:0, G:0, B:0)
  /// </summary>
  public static TiledColor Transparent => new TiledColor();

  /// <summary>
  /// Alice Blue <see cref="TiledColor"/> (A: 255, R:240, G:248, B:255)
  /// </summary>
  public static TiledColor AliceBlue => new TiledColor(0xF0, 0xF8, 0xFF);

  /// <summary>
  /// Antique White <see cref="TiledColor"/> (A: 255, R:250, G:235, B:215)
  /// </summary>
  public static TiledColor AntiqueWhite => new TiledColor(0xFA, 0xEB, 0xD7);

  /// <summary>
  /// Aqua <see cref="TiledColor"/> (A: 255, R:0, G:255, B:255)
  /// </summary>
  public static TiledColor Aqua => new TiledColor(0x00, 0xFF, 0xFF);

  /// <summary>
  /// Aquamarine <see cref="TiledColor"/> (A: 255, R:127, G:255, B:212)
  /// </summary>
  public static TiledColor Aquamarine => new TiledColor(0x7F, 0xFF, 0xD4);

  /// <summary>
  /// Azure <see cref="TiledColor"/> (A: 255, R:240, G:255, B:255)
  /// </summary>
  public static TiledColor Azure => new TiledColor(0xF0, 0xFF, 0xFF);

  /// <summary>
  /// Beige <see cref="TiledColor"/> (A: 255, R:245, G:245, B:220)
  /// </summary>
  public static TiledColor Beige => new TiledColor(0xF5, 0xF5, 0xDC);

  /// <summary>
  /// Bisque <see cref="TiledColor"/> (A: 255, R:255, G:228, B:196)
  /// </summary>
  public static TiledColor Bisque => new TiledColor(0xFF, 0xE4, 0xC4);

  /// <summary>
  /// Black <see cref="TiledColor"/> (A: 255, R:0, G:0, B:0)
  /// </summary>
  public static TiledColor Black => new TiledColor(0x00, 0x00, 0x00);

  /// <summary>
  /// Blanched Almond <see cref="TiledColor"/> (A: 255, R:255, G:235, B:205)
  /// </summary>
  public static TiledColor BlanchedAlmond => new TiledColor(0xFF, 0xEB, 0xCD);

  /// <summary>
  /// Blue <see cref="TiledColor"/> (A: 255, R:0, G:0, B:255)
  /// </summary>
  public static TiledColor Blue => new TiledColor(0x00, 0x00, 0xFF);

  /// <summary>
  /// Blue Violet <see cref="TiledColor"/> (A: 255, R:138, G:43, B:226)
  /// </summary>
  public static TiledColor BlueViolet => new TiledColor(0x8A, 0x2B, 0xE2);

  /// <summary>
  /// Brown <see cref="TiledColor"/> (A: 255, R:165, G:42, B:42)
  /// </summary>
  public static TiledColor Brown => new TiledColor(0xA5, 0x2A, 0x2A);

  /// <summary>
  /// Burly Wood <see cref="TiledColor"/> (A: 255, R:222, G:184, B:135)
  /// </summary>
  public static TiledColor BurlyWood => new TiledColor(0xDE, 0xB8, 0x87);

  /// <summary>
  /// Cadet Blue <see cref="TiledColor"/> (A: 255, R:95, G:158, B:160)
  /// </summary>
  public static TiledColor CadetBlue => new TiledColor(0x5F, 0x9E, 0xA0);

  /// <summary>
  /// Chartreuse <see cref="TiledColor"/> (A: 255, R:127, G:255, B:0)
  /// </summary>
  public static TiledColor Chartreuse => new TiledColor(0x7F, 0xFF, 0x00);

  /// <summary>
  /// Chocolate <see cref="TiledColor"/> (A: 255, R:210, G:105, B:30)
  /// </summary>
  public static TiledColor Chocolate => new TiledColor(0xD2, 0x69, 0x1E);

  /// <summary>
  /// Coral <see cref="TiledColor"/> (A: 255, R:255, G:127, B:80)
  /// </summary>
  public static TiledColor Coral => new TiledColor(0xFF, 0x7F, 0x50);

  /// <summary>
  /// Cornflower Blue <see cref="TiledColor"/> (A: 255, R:100, G:149, B:237)
  /// </summary>
  public static TiledColor CornflowerBlue => new TiledColor(0x64, 0x95, 0xED);

  /// <summary>
  /// Cornsilk <see cref="TiledColor"/> (A: 255, R:255, G:248, B:220)
  /// </summary>
  public static TiledColor Cornsilk => new TiledColor(0xFF, 0xF8, 0xDC);

  /// <summary>
  /// Crimson <see cref="TiledColor"/> (A: 255, R:220, G:20, B:60)
  /// </summary>
  public static TiledColor Crimson => new TiledColor(0xDC, 0x14, 0x3C);

  /// <summary>
  /// Cyan <see cref="TiledColor"/> (A: 255, R:0, G:255, B:255)
  /// </summary>
  public static TiledColor Cyan => new TiledColor(0x00, 0xFF, 0xFF);

  /// <summary>
  /// Dark Blue <see cref="TiledColor"/> (A: 255, R:0, G:0, B:139)
  /// </summary>
  public static TiledColor DarkBlue => new TiledColor(0x00, 0x00, 0x8B);

  /// <summary>
  /// Dark Cyan <see cref="TiledColor"/> (A: 255, R:0, G:139, B:139)
  /// </summary>
  public static TiledColor DarkCyan => new TiledColor(0x00, 0x8B, 0x8B);

  /// <summary>
  /// Dark Golden Rod <see cref="TiledColor"/> (A: 255, R:184, G:134, B:11)
  /// </summary>
  public static TiledColor DarkGoldenRod => new TiledColor(0xB8, 0x86, 0x0B);

  /// <summary>
  /// Dark Gray <see cref="TiledColor"/> (A: 255, R:169, G:169, B:169)
  /// </summary>
  public static TiledColor DarkGray => new TiledColor(0xA9, 0xA9, 0xA9);

  /// <summary>
  /// Dark Grey <see cref="TiledColor"/> (A: 255, R:169, G:169, B:169)
  /// </summary>
  public static TiledColor DarkGrey => new TiledColor(0xA9, 0xA9, 0xA9);

  /// <summary>
  /// Dark Green <see cref="TiledColor"/> (A: 255, R:0, G:100, B:0)
  /// </summary>
  public static TiledColor DarkGreen => new TiledColor(0x00, 0x64, 0x00);

  /// <summary>
  /// Dark Khaki <see cref="TiledColor"/> (A: 255, R:189, G:183, B:107)
  /// </summary>
  public static TiledColor DarkKhaki => new TiledColor(0xBD, 0xB7, 0x6B);

  /// <summary>
  /// Dark Magenta <see cref="TiledColor"/> (A: 255, R:139, G:0, B:139)
  /// </summary>
  public static TiledColor DarkMagenta => new TiledColor(0x8B, 0x00, 0x8B);

  /// <summary>
  /// Dark Olive Green <see cref="TiledColor"/> (A: 255, R:85, G:107, B:47)
  /// </summary>
  public static TiledColor DarkOliveGreen => new TiledColor(0x55, 0x6B, 0x2F);

  /// <summary>
  /// Dark Orange <see cref="TiledColor"/> (A: 255, R:255, G:140, B:0)
  /// </summary>
  public static TiledColor DarkOrange => new TiledColor(0xFF, 0x8C, 0x00);

  /// <summary>
  /// Dark Orchid <see cref="TiledColor"/> (A: 255, R:153, G:50, B:204)
  /// </summary>
  public static TiledColor DarkOrchid => new TiledColor(0x99, 0x32, 0xCC);

  /// <summary>
  /// Dark Red <see cref="TiledColor"/> (A: 255, R:139, G:0, B:0)
  /// </summary>
  public static TiledColor DarkRed => new TiledColor(0x8B, 0x00, 0x00);

  /// <summary>
  /// Dark Salmon <see cref="TiledColor"/> (A: 255, R:233, G:150, B:122)
  /// </summary>
  public static TiledColor DarkSalmon => new TiledColor(0xE9, 0x96, 0x7A);

  /// <summary>
  /// Dark Sea Green <see cref="TiledColor"/> (A: 255, R:143, G:188, B:143)
  /// </summary>
  public static TiledColor DarkSeaGreen => new TiledColor(0x8F, 0xBC, 0x8F);

  /// <summary>
  /// Dark Slate Blue <see cref="TiledColor"/> (A: 255, R:72, G:61, B:139)
  /// </summary>
  public static TiledColor DarkSlateBlue => new TiledColor(0x48, 0x3D, 0x8B);

  /// <summary>
  /// Dark Slate Gray <see cref="TiledColor"/> (A: 255, R:47, G:79, B:79)
  /// </summary>
  public static TiledColor DarkSlateGray => new TiledColor(0x2F, 0x4F, 0x4F);

  /// <summary>
  /// Dark Slate Grey <see cref="TiledColor"/> (A: 255, R:47, G:79, B:79)
  /// </summary>
  public static TiledColor DarkSlateGrey => new TiledColor(0x2F, 0x4F, 0x4F);

  /// <summary>
  /// Dark Turquoise <see cref="TiledColor"/> (A: 255, R:0, G:206, B:209)
  /// </summary>
  public static TiledColor DarkTurquoise => new TiledColor(0x00, 0xCE, 0xD1);

  /// <summary>
  /// Dark Violet <see cref="TiledColor"/> (A: 255, R:148, G:0, B:211)
  /// </summary>
  public static TiledColor DarkViolet => new TiledColor(0x94, 0x00, 0xD3);

  /// <summary>
  /// Deep Pink <see cref="TiledColor"/> (A: 255, R:255, G:20, B:147)
  /// </summary>
  public static TiledColor DeepPink => new TiledColor(0xFF, 0x14, 0x93);

  /// <summary>
  /// Deep Sky Blue <see cref="TiledColor"/> (A: 255, R:0, G:191, B:255)
  /// </summary>
  public static TiledColor DeepSkyBlue => new TiledColor(0x00, 0xBF, 0xFF);

  /// <summary>
  /// Dim Gray <see cref="TiledColor"/> (A: 255, R:105, G:105, B:105)
  /// </summary>
  public static TiledColor DimGray => new TiledColor(0x69, 0x69, 0x69);

  /// <summary>
  /// Dim Grey <see cref="TiledColor"/> (A: 255, R:105, G:105, B:105)
  /// </summary>
  public static TiledColor DimGrey => new TiledColor(0x69, 0x69, 0x69);

  /// <summary>
  /// Dodger Blue <see cref="TiledColor"/> (A: 255, R:30, G:144, B:255)
  /// </summary>
  public static TiledColor DodgerBlue => new TiledColor(0x1E, 0x90, 0xFF);

  /// <summary>
  /// Fire Brick <see cref="TiledColor"/> (A: 255, R:178, G:34, B:34)
  /// </summary>
  public static TiledColor FireBrick => new TiledColor(0xB2, 0x22, 0x22);

  /// <summary>
  /// Floral White <see cref="TiledColor"/> (A: 255, R:255, G:250, B:240)
  /// </summary>
  public static TiledColor FloralWhite => new TiledColor(0xFF, 0xFA, 0xF0);

  /// <summary>
  /// Forest Green <see cref="TiledColor"/> (A: 255, R:34, G:139, B:34)
  /// </summary>
  public static TiledColor ForestGreen => new TiledColor(0x22, 0x8B, 0x22);

  /// <summary>
  /// Fuchsia <see cref="TiledColor"/> (A: 255, R:255, G:0, B:255)
  /// </summary>
  public static TiledColor Fuchsia => new TiledColor(0xFF, 0x00, 0xFF);

  /// <summary>
  /// Gainsboro <see cref="TiledColor"/> (A: 255, R:220, G:220, B:220)
  /// </summary>
  public static TiledColor Gainsboro => new TiledColor(0xDC, 0xDC, 0xDC);

  /// <summary>
  /// Ghost White <see cref="TiledColor"/> (A: 255, R:248, G:248, B:255)
  /// </summary>
  public static TiledColor GhostWhite => new TiledColor(0xF8, 0xF8, 0xFF);

  /// <summary>
  /// Gold <see cref="TiledColor"/> (A: 255, R:255, G:215, B:0)
  /// </summary>
  public static TiledColor Gold => new TiledColor(0xFF, 0xD7, 0x00);

  /// <summary>
  /// Golden Rod <see cref="TiledColor"/> (A: 255, R:218, G:165, B:32)
  /// </summary>
  public static TiledColor GoldenRod => new TiledColor(0xDA, 0xA5, 0x20);

  /// <summary>
  /// Gray <see cref="TiledColor"/> (A: 255, R:128, G:128, B:128)
  /// </summary>
  public static TiledColor Gray => new TiledColor(0x80, 0x80, 0x80);

  /// <summary>
  /// Grey <see cref="TiledColor"/> (A: 255, R:128, G:128, B:128)
  /// </summary>
  public static TiledColor Grey => new TiledColor(0x80, 0x80, 0x80);

  /// <summary>
  /// Green <see cref="TiledColor"/> (A: 255, R:0, G:128, B:0)
  /// </summary>
  public static TiledColor Green => new TiledColor(0x00, 0x80, 0x00);

  /// <summary>
  /// Green Yellow <see cref="TiledColor"/> (A: 255, R:173, G:255, B:47)
  /// </summary>
  public static TiledColor GreenYellow => new TiledColor(0xAD, 0xFF, 0x2F);

  /// <summary>
  /// Honey Dew <see cref="TiledColor"/> (A: 255, R:240, G:255, B:240)
  /// </summary>
  public static TiledColor HoneyDew => new TiledColor(0xF0, 0xFF, 0xF0);

  /// <summary>
  /// Hot Pink <see cref="TiledColor"/> (A: 255, R:255, G:105, B:180)
  /// </summary>
  public static TiledColor HotPink => new TiledColor(0xFF, 0x69, 0xB4);

  /// <summary>
  /// Indian Red <see cref="TiledColor"/> (A: 255, R:205, G:92, B:92)
  /// </summary>
  public static TiledColor IndianRed => new TiledColor(0xCD, 0x5C, 0x5C);

  /// <summary>
  /// Indigo <see cref="TiledColor"/> (A: 255, R:75, G:0, B:130)
  /// </summary>
  public static TiledColor Indigo => new TiledColor(0x4B, 0x00, 0x82);

  /// <summary>
  /// Ivory <see cref="TiledColor"/> (A: 255, R:255, G:255, B:240)
  /// </summary>
  public static TiledColor Ivory => new TiledColor(0xFF, 0xFF, 0xF0);

  /// <summary>
  /// Khaki <see cref="TiledColor"/> (A: 255, R:240, G:230, B:140)
  /// </summary>
  public static TiledColor Khaki => new TiledColor(0xF0, 0xE6, 0x8C);

  /// <summary>
  /// Lavender <see cref="TiledColor"/> (A: 255, R:230, G:230, B:250)
  /// </summary>
  public static TiledColor Lavender => new TiledColor(0xE6, 0xE6, 0xFA);

  /// <summary>
  /// Lavender Blush <see cref="TiledColor"/> (A: 255, R:255, G:240, B:245)
  /// </summary>
  public static TiledColor LavenderBlush => new TiledColor(0xFF, 0xF0, 0xF5);

  /// <summary>
  /// Lawn Green <see cref="TiledColor"/> (A: 255, R:124, G:252, B:0)
  /// </summary>
  public static TiledColor LawnGreen => new TiledColor(0x7C, 0xFC, 0x00);

  /// <summary>
  /// Lemon Chiffon <see cref="TiledColor"/> (A: 255, R:255, G:250, B:205)
  /// </summary>
  public static TiledColor LemonChiffon => new TiledColor(0xFF, 0xFA, 0xCD);

  /// <summary>
  /// Light Blue <see cref="TiledColor"/> (A: 255, R:173, G:216, B:230)
  /// </summary>
  public static TiledColor LightBlue => new TiledColor(0xAD, 0xD8, 0xE6);

  /// <summary>
  /// Light Coral <see cref="TiledColor"/> (A: 255, R:240, G:128, B:128)
  /// </summary>
  public static TiledColor LightCoral => new TiledColor(0xF0, 0x80, 0x80);

  /// <summary>
  /// Light Cyan <see cref="TiledColor"/> (A: 255, R:224, G:255, B:255)
  /// </summary>
  public static TiledColor LightCyan => new TiledColor(0xE0, 0xFF, 0xFF);

  /// <summary>
  /// Light Golden Rod Yellow <see cref="TiledColor"/> (A: 255, R:250, G:250, B:210)
  /// </summary>
  public static TiledColor LightGoldenRodYellow => new TiledColor(0xFA, 0xFA, 0xD2);

  /// <summary>
  /// Light Gray <see cref="TiledColor"/> (A: 255, R:211, G:211, B:211)
  /// </summary>
  public static TiledColor LightGray => new TiledColor(0xD3, 0xD3, 0xD3);

  /// <summary>
  /// Light Grey <see cref="TiledColor"/> (A: 255, R:211, G:211, B:211)
  /// </summary>
  public static TiledColor LightGrey => new TiledColor(0xD3, 0xD3, 0xD3);

  /// <summary>
  /// Light Green <see cref="TiledColor"/> (A: 255, R:144, G:238, B:144)
  /// </summary>
  public static TiledColor LightGreen => new TiledColor(0x90, 0xEE, 0x90);

  /// <summary>
  /// Light Pink <see cref="TiledColor"/> (A: 255, R:255, G:182, B:193)
  /// </summary>
  public static TiledColor LightPink => new TiledColor(0xFF, 0xB6, 0xC1);

  /// <summary>
  /// Light Salmon <see cref="TiledColor"/> (A: 255, R:255, G:160, B:122)
  /// </summary>
  public static TiledColor LightSalmon => new TiledColor(0xFF, 0xA0, 0x7A);

  /// <summary>
  /// Light Sea Green <see cref="TiledColor"/> (A: 255, R:32, G:178, B:170)
  /// </summary>
  public static TiledColor LightSeaGreen => new TiledColor(0x20, 0xB2, 0xAA);

  /// <summary>
  /// Light Sky Blue <see cref="TiledColor"/> (A: 255, R:135, G:206, B:250)
  /// </summary>
  public static TiledColor LightSkyBlue => new TiledColor(0x87, 0xCE, 0xFA);

  /// <summary>
  /// Light Slate Gray <see cref="TiledColor"/> (A: 255, R:119, G:136, B:153)
  /// </summary>
  public static TiledColor LightSlateGray => new TiledColor(0x77, 0x88, 0x99);

  /// <summary>
  /// Light Slate Grey <see cref="TiledColor"/> (A: 255, R:119, G:136, B:153)
  /// </summary>
  public static TiledColor LightSlateGrey => new TiledColor(0x77, 0x88, 0x99);

  /// <summary>
  /// Light Steel Blue <see cref="TiledColor"/> (A: 255, R:176, G:196, B:222)
  /// </summary>
  public static TiledColor LightSteelBlue => new TiledColor(0xB0, 0xC4, 0xDE);

  /// <summary>
  /// Light Yellow <see cref="TiledColor"/> (A: 255, R:255, G:255, B:224)
  /// </summary>
  public static TiledColor LightYellow => new TiledColor(0xFF, 0xFF, 0xE0);

  /// <summary>
  /// Lime <see cref="TiledColor"/> (A: 255, R:0, G:255, B:0)
  /// </summary>
  public static TiledColor Lime => new TiledColor(0x00, 0xFF, 0x00);

  /// <summary>
  /// Lime Green <see cref="TiledColor"/> (A: 255, R:50, G:205, B:50)
  /// </summary>
  public static TiledColor LimeGreen => new TiledColor(0x32, 0xCD, 0x32);

  /// <summary>
  /// Linen <see cref="TiledColor"/> (A: 255, R:250, G:240, B:230)
  /// </summary>
  public static TiledColor Linen => new TiledColor(0xFA, 0xF0, 0xE6);

  /// <summary>
  /// Magenta <see cref="TiledColor"/> (A: 255, R:255, G:0, B:255)
  /// </summary>
  public static TiledColor Magenta => new TiledColor(0xFF, 0x00, 0xFF);

  /// <summary>
  /// Maroon <see cref="TiledColor"/> (A: 255, R:128, G:0, B:0)
  /// </summary>
  public static TiledColor Maroon => new TiledColor(0x80, 0x00, 0x00);

  /// <summary>
  /// Medium Aqua Marine <see cref="TiledColor"/> (A: 255, R:102, G:205, B:170)
  /// </summary>
  public static TiledColor MediumAquaMarine => new TiledColor(0x66, 0xCD, 0xAA);

  /// <summary>
  /// Medium Blue <see cref="TiledColor"/> (A: 255, R:0, G:0, B:205)
  /// </summary>
  public static TiledColor MediumBlue => new TiledColor(0x00, 0x00, 0xCD);

  /// <summary>
  /// Medium Orchid <see cref="TiledColor"/> (A: 255, R:186, G:85, B:211)
  /// </summary>
  public static TiledColor MediumOrchid => new TiledColor(0xBA, 0x55, 0xD3);

  /// <summary>
  /// Medium Purple <see cref="TiledColor"/> (A: 255, R:147, G:112, B:219)
  /// </summary>
  public static TiledColor MediumPurple => new TiledColor(0x93, 0x70, 0xDB);

  /// <summary>
  /// Medium Sea Green <see cref="TiledColor"/> (A: 255, R:60, G:179, B:113)
  /// </summary>
  public static TiledColor MediumSeaGreen => new TiledColor(0x3C, 0xB3, 0x71);

  /// <summary>
  /// Medium Slate Blue <see cref="TiledColor"/> (A: 255, R:123, G:104, B:238)
  /// </summary>
  public static TiledColor MediumSlateBlue => new TiledColor(0x7B, 0x68, 0xEE);

  /// <summary>
  /// Medium Spring Green <see cref="TiledColor"/> (A: 255, R:0, G:250, B:154)
  /// </summary>
  public static TiledColor MediumSpringGreen => new TiledColor(0x00, 0xFA, 0x9A);

  /// <summary>
  /// Medium Turquoise <see cref="TiledColor"/> (A: 255, R:72, G:209, B:204)
  /// </summary>
  public static TiledColor MediumTurquoise => new TiledColor(0x48, 0xD1, 0xCC);

  /// <summary>
  /// Medium Violet Red <see cref="TiledColor"/> (A: 255, R:199, G:21, B:133)
  /// </summary>
  public static TiledColor MediumVioletRed => new TiledColor(0xC7, 0x15, 0x85);

  /// <summary>
  /// Midnight Blue <see cref="TiledColor"/> (A: 255, R:25, G:25, B:112)
  /// </summary>
  public static TiledColor MidnightBlue => new TiledColor(0x19, 0x19, 0x70);

  /// <summary>
  /// Mint Cream <see cref="TiledColor"/> (A: 255, R:245, G:255, B:250)
  /// </summary>
  public static TiledColor MintCream => new TiledColor(0xF5, 0xFF, 0xFA);

  /// <summary>
  /// Misty Rose <see cref="TiledColor"/> (A: 255, R:255, G:228, B:225)
  /// </summary>
  public static TiledColor MistyRose => new TiledColor(0xFF, 0xE4, 0xE1);

  /// <summary>
  /// Moccasin <see cref="TiledColor"/> (A: 255, R:255, G:228, B:181)
  /// </summary>
  public static TiledColor Moccasin => new TiledColor(0xFF, 0xE4, 0xB5);

  /// <summary>
  /// Navajo White <see cref="TiledColor"/> (A: 255, R:255, G:222, B:173)
  /// </summary>
  public static TiledColor NavajoWhite => new TiledColor(0xFF, 0xDE, 0xAD);

  /// <summary>
  /// Navy <see cref="TiledColor"/> (A: 255, R:0, G:0, B:128)
  /// </summary>
  public static TiledColor Navy => new TiledColor(0x00, 0x00, 0x80);

  /// <summary>
  /// Old Lace <see cref="TiledColor"/> (A: 255, R:253, G:245, B:230)
  /// </summary>
  public static TiledColor OldLace => new TiledColor(0xFD, 0xF5, 0xE6);

  /// <summary>
  /// Olive <see cref="TiledColor"/> (A: 255, R:128, G:128, B:0)
  /// </summary>
  public static TiledColor Olive => new TiledColor(0x80, 0x80, 0x00);

  /// <summary>
  /// Olive Drab <see cref="TiledColor"/> (A: 255, R:107, G:142, B:35)
  /// </summary>
  public static TiledColor OliveDrab => new TiledColor(0x6B, 0x8E, 0x23);

  /// <summary>
  /// Orange <see cref="TiledColor"/> (A: 255, R:255, G:165, B:0)
  /// </summary>
  public static TiledColor Orange => new TiledColor(0xFF, 0xA5, 0x00);

  /// <summary>
  /// Orange Red <see cref="TiledColor"/> (A: 255, R:255, G:69, B:0)
  /// </summary>
  public static TiledColor OrangeRed => new TiledColor(0xFF, 0x45, 0x00);

  /// <summary>
  /// Orchid <see cref="TiledColor"/> (A: 255, R:218, G:112, B:214)
  /// </summary>
  public static TiledColor Orchid => new TiledColor(0xDA, 0x70, 0xD6);

  /// <summary>
  /// Pale Golden Rod <see cref="TiledColor"/> (A: 255, R:238, G:232, B:170)
  /// </summary>
  public static TiledColor PaleGoldenRod => new TiledColor(0xEE, 0xE8, 0xAA);

  /// <summary>
  /// Pale Green <see cref="TiledColor"/> (A: 255, R:152, G:251, B:152)
  /// </summary>
  public static TiledColor PaleGreen => new TiledColor(0x98, 0xFB, 0x98);

  /// <summary>
  /// Pale Turquoise <see cref="TiledColor"/> (A: 255, R:175, G:238, B:238)
  /// </summary>
  public static TiledColor PaleTurquoise => new TiledColor(0xAF, 0xEE, 0xEE);

  /// <summary>
  /// Pale Violet Red <see cref="TiledColor"/> (A: 255, R:219, G:112, B:147)
  /// </summary>
  public static TiledColor PaleVioletRed => new TiledColor(0xDB, 0x70, 0x93);

  /// <summary>
  /// Papaya Whip <see cref="TiledColor"/> (A: 255, R:255, G:239, B:213)
  /// </summary>
  public static TiledColor PapayaWhip => new TiledColor(0xFF, 0xEF, 0xD5);

  /// <summary>
  /// Peach Puff <see cref="TiledColor"/> (A: 255, R:255, G:218, B:185)
  /// </summary>
  public static TiledColor PeachPuff => new TiledColor(0xFF, 0xDA, 0xB9);

  /// <summary>
  /// Peru <see cref="TiledColor"/> (A: 255, R:205, G:133, B:63)
  /// </summary>
  public static TiledColor Peru => new TiledColor(0xCD, 0x85, 0x3F);

  /// <summary>
  /// Pink <see cref="TiledColor"/> (A: 255, R:255, G:192, B:203)
  /// </summary>
  public static TiledColor Pink => new TiledColor(0xFF, 0xC0, 0xCB);

  /// <summary>
  /// Plum <see cref="TiledColor"/> (A: 255, R:221, G:160, B:221)
  /// </summary>
  public static TiledColor Plum => new TiledColor(0xDD, 0xA0, 0xDD);

  /// <summary>
  /// Powder Blue <see cref="TiledColor"/> (A: 255, R:176, G:224, B:230)
  /// </summary>
  public static TiledColor PowderBlue => new TiledColor(0xB0, 0xE0, 0xE6);

  /// <summary>
  /// Purple <see cref="TiledColor"/> (A: 255, R:128, G:0, B:128)
  /// </summary>
  public static TiledColor Purple => new TiledColor(0x80, 0x00, 0x80);

  /// <summary>
  /// Rebecca Purple <see cref="TiledColor"/> (A: 255, R:102, G:51, B:153)
  /// </summary>
  public static TiledColor RebeccaPurple => new TiledColor(0x66, 0x33, 0x99);

  /// <summary>
  /// Red <see cref="TiledColor"/> (A: 255, R:255, G:0, B:0)
  /// </summary>
  public static TiledColor Red => new TiledColor(0xFF, 0x00, 0x00);

  /// <summary>
  /// Rosy Brown <see cref="TiledColor"/> (A: 255, R:188, G:143, B:143)
  /// </summary>
  public static TiledColor RosyBrown => new TiledColor(0xBC, 0x8F, 0x8F);

  /// <summary>
  /// Royal Blue <see cref="TiledColor"/> (A: 255, R:65, G:105, B:225)
  /// </summary>
  public static TiledColor RoyalBlue => new TiledColor(0x41, 0x69, 0xE1);

  /// <summary>
  /// Saddle Brown <see cref="TiledColor"/> (A: 255, R:139, G:69, B:19)
  /// </summary>
  public static TiledColor SaddleBrown => new TiledColor(0x8B, 0x45, 0x13);

  /// <summary>
  /// Salmon <see cref="TiledColor"/> (A: 255, R:250, G:128, B:114)
  /// </summary>
  public static TiledColor Salmon => new TiledColor(0xFA, 0x80, 0x72);

  /// <summary>
  /// Sandy Brown <see cref="TiledColor"/> (A: 255, R:244, G:164, B:96)
  /// </summary>
  public static TiledColor SandyBrown => new TiledColor(0xF4, 0xA4, 0x60);

  /// <summary>
  /// Sea Green <see cref="TiledColor"/> (A: 255, R:46, G:139, B:87)
  /// </summary>
  public static TiledColor SeaGreen => new TiledColor(0x2E, 0x8B, 0x57);

  /// <summary>
  /// Sea Shell <see cref="TiledColor"/> (A: 255, R:255, G:245, B:238)
  /// </summary>
  public static TiledColor SeaShell => new TiledColor(0xFF, 0xF5, 0xEE);

  /// <summary>
  /// Sienna <see cref="TiledColor"/> (A: 255, R:160, G:82, B:45)
  /// </summary>
  public static TiledColor Sienna => new TiledColor(0xA0, 0x52, 0x2D);

  /// <summary>
  /// Silver <see cref="TiledColor"/> (A: 255, R:192, G:192, B:192)
  /// </summary>
  public static TiledColor Silver => new TiledColor(0xC0, 0xC0, 0xC0);

  /// <summary>
  /// Sky Blue <see cref="TiledColor"/> (A: 255, R:135, G:206, B:235)
  /// </summary>
  public static TiledColor SkyBlue => new TiledColor(0x87, 0xCE, 0xEB);

  /// <summary>
  /// Slate Blue <see cref="TiledColor"/> (A: 255, R:106, G:90, B:205)
  /// </summary>
  public static TiledColor SlateBlue => new TiledColor(0x6A, 0x5A, 0xCD);

  /// <summary>
  /// Slate Gray <see cref="TiledColor"/> (A: 255, R:112, G:128, B:144)
  /// </summary>
  public static TiledColor SlateGray => new TiledColor(0x70, 0x80, 0x90);

  /// <summary>
  /// Slate Grey <see cref="TiledColor"/> (A: 255, R:112, G:128, B:144)
  /// </summary>
  public static TiledColor SlateGrey => new TiledColor(0x70, 0x80, 0x90);

  /// <summary>
  /// Snow <see cref="TiledColor"/> (A: 255, R:255, G:250, B:250)
  /// </summary>
  public static TiledColor Snow => new TiledColor(0xFF, 0xFA, 0xFA);

  /// <summary>
  /// Spring Green <see cref="TiledColor"/> (A: 255, R:0, G:255, B:127)
  /// </summary>
  public static TiledColor SpringGreen => new TiledColor(0x00, 0xFF, 0x7F);

  /// <summary>
  /// Steel Blue <see cref="TiledColor"/> (A: 255, R:70, G:130, B:180)
  /// </summary>
  public static TiledColor SteelBlue => new TiledColor(0x46, 0x82, 0xB4);

  /// <summary>
  /// Tan <see cref="TiledColor"/> (A: 255, R:210, G:180, B:140)
  /// </summary>
  public static TiledColor Tan => new TiledColor(0xD2, 0xB4, 0x8C);

  /// <summary>
  /// Teal <see cref="TiledColor"/> (A: 255, R:0, G:128, B:128)
  /// </summary>
  public static TiledColor Teal => new TiledColor(0x00, 0x80, 0x80);

  /// <summary>
  /// Thistle <see cref="TiledColor"/> (A: 255, R:216, G:191, B:216)
  /// </summary>
  public static TiledColor Thistle => new TiledColor(0xD8, 0xBF, 0xD8);

  /// <summary>
  /// Tomato <see cref="TiledColor"/> (A: 255, R:255, G:99, B:71)
  /// </summary>
  public static TiledColor Tomato => new TiledColor(0xFF, 0x63, 0x47);

  /// <summary>
  /// Turquoise <see cref="TiledColor"/> (A: 255, R:64, G:224, B:208)
  /// </summary>
  public static TiledColor Turquoise => new TiledColor(0x40, 0xE0, 0xD0);

  /// <summary>
  /// Violet <see cref="TiledColor"/> (A: 255, R:238, G:130, B:238)
  /// </summary>
  public static TiledColor Violet => new TiledColor(0xEE, 0x82, 0xEE);

  /// <summary>
  /// Wheat <see cref="TiledColor"/> (A: 255, R:245, G:222, B:179)
  /// </summary>
  public static TiledColor Wheat => new TiledColor(0xF5, 0xDE, 0xB3);

  /// <summary>
  /// White <see cref="TiledColor"/> (A: 255, R:255, G:255, B:255)
  /// </summary>
  public static TiledColor White => new TiledColor(0xFF, 0xFF, 0xFF);

  /// <summary>
  /// White Smoke <see cref="TiledColor"/> (A: 255, R:245, G:245, B:245)
  /// </summary>
  public static TiledColor WhiteSmoke => new TiledColor(0xF5, 0xF5, 0xF5);

  /// <summary>
  /// Yellow <see cref="TiledColor"/> (A: 255, R:255, G:255, B:0)
  /// </summary>
  public static TiledColor Yellow => new TiledColor(0xFF, 0xFF, 0x00);

  /// <summary>
  /// Yellow Green <see cref="TiledColor"/> (A: 255, R:154, G:205, B:50)
  /// </summary>
  public static TiledColor YellowGreen => new TiledColor(0x9A, 0xCD, 0x32);



  #endregion Static_TiledColors

  /// <summary>
  /// Constructs an ARGB color from scalars representing red, green, and blue values.
  /// </summary>
  /// <param name="red"></param>
  /// <param name="green"></param>
  /// <param name="blue"></param>
  public TiledColor(byte red, byte green, byte blue) : this(0xFF, red, green, blue) { }

  /// <summary>
  /// Constructs an ARGB color from scalars representing alpha, red, green, and blue values.
  /// </summary>
  /// <param name="alpha"></param>
  /// <param name="red"></param>
  /// <param name="green"></param>
  /// <param name="blue"></param>
  public TiledColor(byte alpha, byte red, byte green, byte blue)
  {
    A = alpha;
    R = red;
    G = green;
    B = blue;
  }

  /// <summary>
  /// Gets or sets the alpha component.
  /// </summary>
  [field: FieldOffset(0)]
  public byte A { get; set; }

  /// <summary>
  /// Gets or sets the red component.
  /// </summary>
  [field: FieldOffset(1)]
  public byte R { get; set; }

  /// <summary>
  /// Gets or sets the green component.
  /// </summary>
  [field: FieldOffset(2)]
  public byte G { get; set; }

  /// <summary>
  /// Gets or sets the blue component.
  /// </summary>
  [field: FieldOffset(3)]
  public byte B { get; set; }

  /// <summary>
  /// Attempts to parse the specified string into a <see cref="TiledColor"/>. Expects strings in the format <c>#RRGGBB</c> or <c>#AARRGGBB</c>.
  /// The leading <c>#</c> is optional.
  /// </summary>
  /// <param name="s">A string value to parse into a <see cref="TiledColor"/></param>
  /// <param name="provider">An object that supplies culture-specific information about the format of s.</param>
  /// <returns>The parsed <see cref="TiledColor"/></returns>
  /// <exception cref="FormatException">Thrown in case the provided string <paramref name="s"/> is not in a valid format.</exception>
  public static TiledColor Parse(string s, IFormatProvider provider)
  {
    if (TryParse(s, provider, out var result))
      return result;

    throw new FormatException($"Invalid format for TiledColor: {s}");
  }

  /// <summary>
  /// Attempts to parse the specified string into a <see cref="TiledColor"/>. Expects strings in the format <c>#RRGGBB</c> or <c>#AARRGGBB</c>.
  /// The leading <c>#</c> is optional.
  /// </summary>
  /// <param name="s">A string value to parse into a <see cref="TiledColor"/></param>
  /// <param name="provider">An object that supplies culture-specific information about the format of s.</param>
  /// <param name="result">When this method returns, contains the parsed <see cref="TiledColor"/> or <c>null</c> on failure.</param>
  /// <returns><c>true</c> if <paramref name="s"/> was successfully parsed; otherwise, <c>false</c>.</returns>
  public static bool TryParse(
    [NotNullWhen(true)] string s,
    IFormatProvider provider,
    [MaybeNullWhen(false)] out TiledColor result)
  {
    if (s is not null && !s.StartsWith('#'))
      return TryParse($"#{s}", provider, out result);

    // Format: #RRGGBB or #AARRGGBB
    if (s is null || (s.Length != 7 && s.Length != 9) || s[0] != '#')
    {
      result = default;
      return false;
    }

    if (s.Length == 7)
    {
      result = new TiledColor(
        byte.Parse(s[1..3], NumberStyles.HexNumber, provider),
        byte.Parse(s[3..5], NumberStyles.HexNumber, provider),
        byte.Parse(s[5..7], NumberStyles.HexNumber, provider)
      );
    }
    else
    {
      result = new TiledColor(
        byte.Parse(s[1..3], NumberStyles.HexNumber, provider),
        byte.Parse(s[3..5], NumberStyles.HexNumber, provider),
        byte.Parse(s[5..7], NumberStyles.HexNumber, provider),
        byte.Parse(s[7..9], NumberStyles.HexNumber, provider)
      );
    }

    return true;
  }

  /// <summary>
  /// Compares whether two <see cref="TiledColor"/> instances are equal.
  /// </summary>
  /// <param name="left"><see cref="TiledColor"/> instance on the left of the equal sign.</param>
  /// <param name="right"><see cref="TiledColor"/> instance on the right of the equal sign.</param>
  /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
  public static bool operator ==(TiledColor left, TiledColor right)
  {
    return left.A == right.A && left.R == right.R && left.G == left.G && left.B == left.B;
  }

  /// <summary>
  /// Compares where two <see cref="TiledColor"/> instances are not equal.
  /// </summary>
  /// <param name="left"><see cref="TiledColor"/> instance on the left of the not equal sign.</param>
  /// <param name="right"><see cref="TiledColor"/> instance on the right of the not equal sign.</param>
  /// <returns><c>true</c> if the instances are not equal; <c>false</c> otherwise.</returns>
  public static bool operator !=(TiledColor left, TiledColor right)
  {
    return left.A != right.A || left.R != right.R || left.G != right.G || left.B != right.B;
  }

  /// <inheritdoc/>
  public readonly bool Equals(TiledColor other) => A == other.A && R == other.R && G == other.G && B == other.B;

  /// <inheritdoc/>
  public override readonly bool Equals(object obj) => obj is TiledColor other && Equals(other);

  /// <inheritdoc/>
  public override readonly int GetHashCode() => (A, R, G, B).GetHashCode();

  /// <inheritdoc/>
  public override readonly string ToString() => $"#{A:x2}{R:x2}{G:x2}{B:x2}";

  /// <summary>
  /// Explicit conversion from a <see cref="TiledColor"/> to a <see cref="uint"/>.
  /// </summary>
  /// <param name="value"></param>
  public static explicit operator uint(TiledColor value)
  {
    return ((uint)value.A << 24) | ((uint)value.R << 16) | ((uint)value.G << 8) | ((uint)value.B << 0);
  }

  /// <summary>
  /// Explicit conversion from a <see cref="uint"/> to a <see cref="TiledColor"/>.
  /// </summary>
  /// <param name="value"></param>
  public static explicit operator TiledColor(uint value)
  {
    return new((byte)(value >> 24), (byte)(value >> 16), (byte)(value >> 8), (byte)(value >> 0));
  }
}
