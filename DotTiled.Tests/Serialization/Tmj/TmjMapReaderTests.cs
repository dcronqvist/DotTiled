namespace DotTiled.Tests;

public partial class TmjMapReaderTests
{
  [Fact]
  public void Test1()
  {
    // Arrange
    var jsonString =
    """
    {
      "backgroundcolor":"#656667",
      "height":4,
      "nextobjectid":1,
      "nextlayerid":1,
      "orientation":"orthogonal",
      "properties": [
        {
          "name":"mapProperty1",
          "type":"string",
          "value":"one"
        },
        {
          "name":"mapProperty3",
          "type":"string",
          "value":"twoeee"
        }
      ],
      "renderorder":"right-down",
      "tileheight":32,
      "tilewidth":32,
      "version":"1",
      "tiledversion":"1.0.3",
      "width":4,
      "tilesets": [
        {
          "columns":19,
          "firstgid":1,
          "image":"image/fishbaddie_parts.png",
          "imageheight":480,
          "imagewidth":640,
          "margin":3,
          "name":"",
          "properties":[
            {
              "name":"myProperty1",
              "type":"string",
              "value":"myProperty1_value"
            }],
          "spacing":1,
          "tilecount":266,
          "tileheight":32,
          "tilewidth":32
        }
      ]
    }
    """;

    // Act
    using var tmjMapReader = new TmjMapReader(jsonString);

    // Assert
    var map = tmjMapReader.ReadMap();
  }
}
