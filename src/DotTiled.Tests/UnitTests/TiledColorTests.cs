using System.Globalization;

namespace DotTiled.Tests.UnitTests;

public class TiledColorTests
{
  [Fact]
  public void TiledColor_Size() => Assert.Equal(4, System.Runtime.InteropServices.Marshal.SizeOf<TiledColor>());

  [Fact]
  public void DefaultConstructor()
  {
    var tiledColor = new TiledColor();

    Assert.Equal<byte>(0x00, tiledColor.A);
    Assert.Equal<byte>(0x00, tiledColor.R);
    Assert.Equal<byte>(0x00, tiledColor.G);
    Assert.Equal<byte>(0x00, tiledColor.B);
  }

  [Fact]
  public void UInt32_Explicit_Cast()
  {
    TiledColor tiledColor = (TiledColor)0x11223344u;

    Assert.Equal<byte>(0x11, tiledColor.A);
    Assert.Equal<byte>(0x22, tiledColor.R);
    Assert.Equal<byte>(0x33, tiledColor.G);
    Assert.Equal<byte>(0x44, tiledColor.B);
  }

  [Fact]
  public void TiledColor_Explicit_Cast()
  {
    var tiledColor = new TiledColor(0x11, 0x22, 0x33, 0x44);

    Assert.Equal<uint>(0x11223344u, (uint)tiledColor);
  }

  [Fact]
  public void NonAlphaConstructor()
  {
    var tiledColor = new TiledColor(0x11, 0x22, 0x33);

    Assert.Equal<byte>(0xFF, tiledColor.A);
    Assert.Equal<byte>(0x11, tiledColor.R);
    Assert.Equal<byte>(0x22, tiledColor.G);
    Assert.Equal<byte>(0x33, tiledColor.B);
  }

  [Fact]
  public void AlphaConstructor()
  {
    var tiledColor = new TiledColor(0x11, 0x22, 0x33, 0x44);

    Assert.Equal<byte>(0x11, tiledColor.A);
    Assert.Equal<byte>(0x22, tiledColor.R);
    Assert.Equal<byte>(0x33, tiledColor.G);
    Assert.Equal<byte>(0x44, tiledColor.B);
  }

  [Fact]
  public void NonAlphaParsing()
  {
    var tiledColor = TiledColor.Parse("#112233", CultureInfo.InvariantCulture);

    Assert.Equal<byte>(0x11, tiledColor.R);
    Assert.Equal<byte>(0x22, tiledColor.G);
    Assert.Equal<byte>(0x33, tiledColor.B);
  }

  [Fact]
  public void AlphaParsing()
  {
    var tiledColor = TiledColor.Parse("#11223344", CultureInfo.InvariantCulture);

    Assert.Equal<byte>(0x11, tiledColor.A);
    Assert.Equal<byte>(0x22, tiledColor.R);
    Assert.Equal<byte>(0x33, tiledColor.G);
    Assert.Equal<byte>(0x44, tiledColor.B);
  }

  [Fact]
  public void Static_TiledColors_ToString()
  {
    Assert.Equal("#00000000", TiledColor.Transparent.ToString());
    Assert.Equal("#fff0f8ff", TiledColor.AliceBlue.ToString());
    Assert.Equal("#fffaebd7", TiledColor.AntiqueWhite.ToString());
    Assert.Equal("#ff00ffff", TiledColor.Aqua.ToString());
    Assert.Equal("#ff7fffd4", TiledColor.Aquamarine.ToString());
    Assert.Equal("#fff0ffff", TiledColor.Azure.ToString());
    Assert.Equal("#fff5f5dc", TiledColor.Beige.ToString());
    Assert.Equal("#ffffe4c4", TiledColor.Bisque.ToString());
    Assert.Equal("#ff000000", TiledColor.Black.ToString());
    Assert.Equal("#ffffebcd", TiledColor.BlanchedAlmond.ToString());
    Assert.Equal("#ff0000ff", TiledColor.Blue.ToString());
    Assert.Equal("#ff8a2be2", TiledColor.BlueViolet.ToString());
    Assert.Equal("#ffa52a2a", TiledColor.Brown.ToString());
    Assert.Equal("#ffdeb887", TiledColor.BurlyWood.ToString());
    Assert.Equal("#ff5f9ea0", TiledColor.CadetBlue.ToString());
    Assert.Equal("#ff7fff00", TiledColor.Chartreuse.ToString());
    Assert.Equal("#ffd2691e", TiledColor.Chocolate.ToString());
    Assert.Equal("#ffff7f50", TiledColor.Coral.ToString());
    Assert.Equal("#ff6495ed", TiledColor.CornflowerBlue.ToString());
    Assert.Equal("#fffff8dc", TiledColor.Cornsilk.ToString());
    Assert.Equal("#ffdc143c", TiledColor.Crimson.ToString());
    Assert.Equal("#ff00ffff", TiledColor.Cyan.ToString());
    Assert.Equal("#ff00008b", TiledColor.DarkBlue.ToString());
    Assert.Equal("#ff008b8b", TiledColor.DarkCyan.ToString());
    Assert.Equal("#ffb8860b", TiledColor.DarkGoldenRod.ToString());
    Assert.Equal("#ffa9a9a9", TiledColor.DarkGray.ToString());
    Assert.Equal("#ffa9a9a9", TiledColor.DarkGrey.ToString());
    Assert.Equal("#ff006400", TiledColor.DarkGreen.ToString());
    Assert.Equal("#ffbdb76b", TiledColor.DarkKhaki.ToString());
    Assert.Equal("#ff8b008b", TiledColor.DarkMagenta.ToString());
    Assert.Equal("#ff556b2f", TiledColor.DarkOliveGreen.ToString());
    Assert.Equal("#ffff8c00", TiledColor.DarkOrange.ToString());
    Assert.Equal("#ff9932cc", TiledColor.DarkOrchid.ToString());
    Assert.Equal("#ff8b0000", TiledColor.DarkRed.ToString());
    Assert.Equal("#ffe9967a", TiledColor.DarkSalmon.ToString());
    Assert.Equal("#ff8fbc8f", TiledColor.DarkSeaGreen.ToString());
    Assert.Equal("#ff483d8b", TiledColor.DarkSlateBlue.ToString());
    Assert.Equal("#ff2f4f4f", TiledColor.DarkSlateGray.ToString());
    Assert.Equal("#ff2f4f4f", TiledColor.DarkSlateGrey.ToString());
    Assert.Equal("#ff00ced1", TiledColor.DarkTurquoise.ToString());
    Assert.Equal("#ff9400d3", TiledColor.DarkViolet.ToString());
    Assert.Equal("#ffff1493", TiledColor.DeepPink.ToString());
    Assert.Equal("#ff00bfff", TiledColor.DeepSkyBlue.ToString());
    Assert.Equal("#ff696969", TiledColor.DimGray.ToString());
    Assert.Equal("#ff696969", TiledColor.DimGrey.ToString());
    Assert.Equal("#ff1e90ff", TiledColor.DodgerBlue.ToString());
    Assert.Equal("#ffb22222", TiledColor.FireBrick.ToString());
    Assert.Equal("#fffffaf0", TiledColor.FloralWhite.ToString());
    Assert.Equal("#ff228b22", TiledColor.ForestGreen.ToString());
    Assert.Equal("#ffff00ff", TiledColor.Fuchsia.ToString());
    Assert.Equal("#ffdcdcdc", TiledColor.Gainsboro.ToString());
    Assert.Equal("#fff8f8ff", TiledColor.GhostWhite.ToString());
    Assert.Equal("#ffffd700", TiledColor.Gold.ToString());
    Assert.Equal("#ffdaa520", TiledColor.GoldenRod.ToString());
    Assert.Equal("#ff808080", TiledColor.Gray.ToString());
    Assert.Equal("#ff808080", TiledColor.Grey.ToString());
    Assert.Equal("#ff008000", TiledColor.Green.ToString());
    Assert.Equal("#ffadff2f", TiledColor.GreenYellow.ToString());
    Assert.Equal("#fff0fff0", TiledColor.HoneyDew.ToString());
    Assert.Equal("#ffff69b4", TiledColor.HotPink.ToString());
    Assert.Equal("#ffcd5c5c", TiledColor.IndianRed.ToString());
    Assert.Equal("#ff4b0082", TiledColor.Indigo.ToString());
    Assert.Equal("#fffffff0", TiledColor.Ivory.ToString());
    Assert.Equal("#fff0e68c", TiledColor.Khaki.ToString());
    Assert.Equal("#ffe6e6fa", TiledColor.Lavender.ToString());
    Assert.Equal("#fffff0f5", TiledColor.LavenderBlush.ToString());
    Assert.Equal("#ff7cfc00", TiledColor.LawnGreen.ToString());
    Assert.Equal("#fffffacd", TiledColor.LemonChiffon.ToString());
    Assert.Equal("#ffadd8e6", TiledColor.LightBlue.ToString());
    Assert.Equal("#fff08080", TiledColor.LightCoral.ToString());
    Assert.Equal("#ffe0ffff", TiledColor.LightCyan.ToString());
    Assert.Equal("#fffafad2", TiledColor.LightGoldenRodYellow.ToString());
    Assert.Equal("#ffd3d3d3", TiledColor.LightGray.ToString());
    Assert.Equal("#ffd3d3d3", TiledColor.LightGrey.ToString());
    Assert.Equal("#ff90ee90", TiledColor.LightGreen.ToString());
    Assert.Equal("#ffffb6c1", TiledColor.LightPink.ToString());
    Assert.Equal("#ffffa07a", TiledColor.LightSalmon.ToString());
    Assert.Equal("#ff20b2aa", TiledColor.LightSeaGreen.ToString());
    Assert.Equal("#ff87cefa", TiledColor.LightSkyBlue.ToString());
    Assert.Equal("#ff778899", TiledColor.LightSlateGray.ToString());
    Assert.Equal("#ff778899", TiledColor.LightSlateGrey.ToString());
    Assert.Equal("#ffb0c4de", TiledColor.LightSteelBlue.ToString());
    Assert.Equal("#ffffffe0", TiledColor.LightYellow.ToString());
    Assert.Equal("#ff00ff00", TiledColor.Lime.ToString());
    Assert.Equal("#ff32cd32", TiledColor.LimeGreen.ToString());
    Assert.Equal("#fffaf0e6", TiledColor.Linen.ToString());
    Assert.Equal("#ffff00ff", TiledColor.Magenta.ToString());
    Assert.Equal("#ff800000", TiledColor.Maroon.ToString());
    Assert.Equal("#ff66cdaa", TiledColor.MediumAquaMarine.ToString());
    Assert.Equal("#ff0000cd", TiledColor.MediumBlue.ToString());
    Assert.Equal("#ffba55d3", TiledColor.MediumOrchid.ToString());
    Assert.Equal("#ff9370db", TiledColor.MediumPurple.ToString());
    Assert.Equal("#ff3cb371", TiledColor.MediumSeaGreen.ToString());
    Assert.Equal("#ff7b68ee", TiledColor.MediumSlateBlue.ToString());
    Assert.Equal("#ff00fa9a", TiledColor.MediumSpringGreen.ToString());
    Assert.Equal("#ff48d1cc", TiledColor.MediumTurquoise.ToString());
    Assert.Equal("#ffc71585", TiledColor.MediumVioletRed.ToString());
    Assert.Equal("#ff191970", TiledColor.MidnightBlue.ToString());
    Assert.Equal("#fff5fffa", TiledColor.MintCream.ToString());
    Assert.Equal("#ffffe4e1", TiledColor.MistyRose.ToString());
    Assert.Equal("#ffffe4b5", TiledColor.Moccasin.ToString());
    Assert.Equal("#ffffdead", TiledColor.NavajoWhite.ToString());
    Assert.Equal("#ff000080", TiledColor.Navy.ToString());
    Assert.Equal("#fffdf5e6", TiledColor.OldLace.ToString());
    Assert.Equal("#ff808000", TiledColor.Olive.ToString());
    Assert.Equal("#ff6b8e23", TiledColor.OliveDrab.ToString());
    Assert.Equal("#ffffa500", TiledColor.Orange.ToString());
    Assert.Equal("#ffff4500", TiledColor.OrangeRed.ToString());
    Assert.Equal("#ffda70d6", TiledColor.Orchid.ToString());
    Assert.Equal("#ffeee8aa", TiledColor.PaleGoldenRod.ToString());
    Assert.Equal("#ff98fb98", TiledColor.PaleGreen.ToString());
    Assert.Equal("#ffafeeee", TiledColor.PaleTurquoise.ToString());
    Assert.Equal("#ffdb7093", TiledColor.PaleVioletRed.ToString());
    Assert.Equal("#ffffefd5", TiledColor.PapayaWhip.ToString());
    Assert.Equal("#ffffdab9", TiledColor.PeachPuff.ToString());
    Assert.Equal("#ffcd853f", TiledColor.Peru.ToString());
    Assert.Equal("#ffffc0cb", TiledColor.Pink.ToString());
    Assert.Equal("#ffdda0dd", TiledColor.Plum.ToString());
    Assert.Equal("#ffb0e0e6", TiledColor.PowderBlue.ToString());
    Assert.Equal("#ff800080", TiledColor.Purple.ToString());
    Assert.Equal("#ff663399", TiledColor.RebeccaPurple.ToString());
    Assert.Equal("#ffff0000", TiledColor.Red.ToString());
    Assert.Equal("#ffbc8f8f", TiledColor.RosyBrown.ToString());
    Assert.Equal("#ff4169e1", TiledColor.RoyalBlue.ToString());
    Assert.Equal("#ff8b4513", TiledColor.SaddleBrown.ToString());
    Assert.Equal("#fffa8072", TiledColor.Salmon.ToString());
    Assert.Equal("#fff4a460", TiledColor.SandyBrown.ToString());
    Assert.Equal("#ff2e8b57", TiledColor.SeaGreen.ToString());
    Assert.Equal("#fffff5ee", TiledColor.SeaShell.ToString());
    Assert.Equal("#ffa0522d", TiledColor.Sienna.ToString());
    Assert.Equal("#ffc0c0c0", TiledColor.Silver.ToString());
    Assert.Equal("#ff87ceeb", TiledColor.SkyBlue.ToString());
    Assert.Equal("#ff6a5acd", TiledColor.SlateBlue.ToString());
    Assert.Equal("#ff708090", TiledColor.SlateGray.ToString());
    Assert.Equal("#ff708090", TiledColor.SlateGrey.ToString());
    Assert.Equal("#fffffafa", TiledColor.Snow.ToString());
    Assert.Equal("#ff00ff7f", TiledColor.SpringGreen.ToString());
    Assert.Equal("#ff4682b4", TiledColor.SteelBlue.ToString());
    Assert.Equal("#ffd2b48c", TiledColor.Tan.ToString());
    Assert.Equal("#ff008080", TiledColor.Teal.ToString());
    Assert.Equal("#ffd8bfd8", TiledColor.Thistle.ToString());
    Assert.Equal("#ffff6347", TiledColor.Tomato.ToString());
    Assert.Equal("#ff40e0d0", TiledColor.Turquoise.ToString());
    Assert.Equal("#ffee82ee", TiledColor.Violet.ToString());
    Assert.Equal("#fff5deb3", TiledColor.Wheat.ToString());
    Assert.Equal("#ffffffff", TiledColor.White.ToString());
    Assert.Equal("#fff5f5f5", TiledColor.WhiteSmoke.ToString());
    Assert.Equal("#ffffff00", TiledColor.Yellow.ToString());
    Assert.Equal("#ff9acd32", TiledColor.YellowGreen.ToString());
  }
}
