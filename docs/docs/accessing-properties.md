# Accessing properties

[Tiled facilitates a very flexible way to store custom data in your maps using properties](https://doc.mapeditor.org/en/stable/manual/custom-properties/#custom-properties). Accessing these properties is a common task when working with Tiled maps in your game since it will allow you to fully utilize the strengths of Tiled, such as customizing the behavior of your game objects or setting up the initial state of your game world.

### All classes that can contain properties

All classes that can contain custom properties implement the interface <xref:DotTiled.Model.IHasProperties> in some way. Below is an exhaustive list of all classes that can contain custom properties:

- <xref:DotTiled.Model.BaseLayer> 
    - <xref:DotTiled.Model.TileLayer>
    - <xref:DotTiled.Model.ObjectLayer>
    - <xref:DotTiled.Model.ImageLayer>
    - <xref:DotTiled.Model.Group>
- <xref:DotTiled.Model.ClassProperty> (allows for recursive property objects)
- <xref:DotTiled.Model.CustomClassDefinition> (used to define custom Tiled property types)
- <xref:DotTiled.Model.Object>
    - <xref:DotTiled.Model.EllipseObject>
    - <xref:DotTiled.Model.PointObject>
    - <xref:DotTiled.Model.PolygonObject>
    - <xref:DotTiled.Model.PolylineObject>
    - <xref:DotTiled.Model.RectangleObject>
    - <xref:DotTiled.Model.TextObject>
    - <xref:DotTiled.Model.TileObject>
- <xref:DotTiled.Model.Tileset>
- <xref:DotTiled.Model.Tile>
- <xref:DotTiled.Model.WangTile>
- <xref:DotTiled.Model.WangColor>

### How to access properties

To access the properties on one of the classes listed above, you will make use of the <xref:DotTiled.Model.IHasProperties> interface.

In situations where you know that a property must exist, and you simply want to retrieve it, you can use the <xref:DotTiled.Model.IHasProperties.GetProperty``1(System.String)> method like so:

```csharp
var map = LoadMap();
var propertyValue = map.GetProperty<BoolProperty>("boolPropertyInMap").Value;
```

If you are unsure whether a property exists, or you want to provide some kind of default behaviour if the property is not present, you can instead use the <xref:DotTiled.Model.IHasProperties.TryGetProperty``1(System.String,``0@)> method like so:

```csharp
var map = LoadMap();
if (map.TryGetProperty<BoolProperty>("boolPropertyInMap", out var property))
{
  // Do something with existing property
  var propertyValue = property.Value;
}
else
{
  // Do something if property does not exist
}
```

### All types of properties

Tiled supports a variety of property types, which are represented in the DotTiled library as classes that implement the <xref:DotTiled.Model.IProperty`1> interface. Below is a list of all property types that Tiled supports and their corresponding classes in DotTiled:

- `bool` - <xref:DotTiled.Model.BoolProperty>
- `color` - <xref:DotTiled.Model.ColorProperty>
- `float` - <xref:DotTiled.Model.FloatProperty>
- `file` - <xref:DotTiled.Model.FileProperty>
- `int` - <xref:DotTiled.Model.IntProperty>
- `object` - <xref:DotTiled.Model.ObjectProperty>
- `string` - <xref:DotTiled.Model.StringProperty>

In addition to these primitive property types, [Tiled also supports more complex property types](https://doc.mapeditor.org/en/stable/manual/custom-properties/#custom-types). These custom property types are defined in Tiled according to the linked documentation, and to work with them in DotTiled, you *must* define their equivalences as a collection of <xref:DotTiled.Model.ICustomTypeDefinition>. This collection of definitions shall then be passed to the corresponding reader when loading a map, tileset, or template.

Whenever DotTiled encounters a property that is of type `class` in a Tiled file, it will attempt to find the corresponding definition, and if it does not find one, it will throw an exception. However, if it does find the definition, it will use that definition to know the default values of the properties of that class, and then override those defaults with the values found in the Tiled file. More information about these `class` properties can be found in [the next section](#class-properties).

Finally, Tiled also allows you to define custom property types that work as enums. These custom property types are just parsed and retrieved as their corresponding storage type. So for a custom property type that is defined as an enum where the values are stored as strings, DotTiled will just parse those as <xref:DotTiled.Model.StringProperty>. Similarly, if the values are stored as integers, DotTiled will parse those as <xref:DotTiled.Model.IntProperty>.

### Class properties

As mentioned, Tiled supports `class` properties which allow you to create hierarchical structures of properties. DotTiled supports this feature through the <xref:DotTiled.Model.ClassProperty> class. For all your custom `class` types in Tiled, you must create an equivalent <xref:DotTiled.Model.CustomClassDefinition> and pass it to the corresponding reader when loading a map, tileset, or template.

For example, if you have a `class` property in Tiled that looks like this:

![MonsterSpawner class in Tiled UI](../images/monster-spawner-class.png)

The equivalent definition in DotTiled would look like the following:

```csharp
var monsterSpawnerDefinition = new CustomClassDefinition
{
  Name = "MonsterSpawner",
  UseAs = CustomClassUseAs.All, // Not really validated by DotTiled
  Members = [ // Make sure that the default values match the Tiled UI
    new BoolProperty { Name = "enabled", Value = true },
    new IntProperty { Name = "maxSpawnAmount", Value = 10 },
    new IntProperty { Name = "minSpawnAmount", Value = 0 },
    new StringProperty { Name = "monsterNames", Value = "" }
  ]
};
```