# Custom properties

[Tiled facilitates a very flexible way to store custom data in your maps using properties](https://doc.mapeditor.org/en/stable/manual/custom-properties/#custom-properties). Accessing these properties is a common task when working with Tiled maps in your game since it will allow you to fully utilize the strengths of Tiled, such as customizing the behavior of your game objects or setting up the initial state of your game world.

## All classes that can contain properties

All classes that can contain custom properties implement the interface <xref:DotTiled.IHasProperties> in some way. Below is an exhaustive list of all classes that can contain custom properties:

- <xref:DotTiled.BaseLayer> 
    - <xref:DotTiled.TileLayer>
    - <xref:DotTiled.ObjectLayer>
    - <xref:DotTiled.ImageLayer>
    - <xref:DotTiled.Group>
- <xref:DotTiled.ClassProperty> (allows for recursive property objects)
- <xref:DotTiled.CustomClassDefinition> (used to define custom Tiled property types)
- <xref:DotTiled.Object>
    - <xref:DotTiled.EllipseObject>
    - <xref:DotTiled.PointObject>
    - <xref:DotTiled.PolygonObject>
    - <xref:DotTiled.PolylineObject>
    - <xref:DotTiled.RectangleObject>
    - <xref:DotTiled.TextObject>
    - <xref:DotTiled.TileObject>
- <xref:DotTiled.Tileset>
- <xref:DotTiled.Tile>
- <xref:DotTiled.WangTile>
- <xref:DotTiled.WangColor>

## How to access properties

To access the properties on one of the classes listed above, you will make use of the <xref:DotTiled.IHasProperties> interface.

In situations where you know that a property must exist, and you simply want to retrieve it, you can use the <xref:DotTiled.IHasProperties.GetProperty``1(System.String)> method like so:

```csharp
var map = LoadMap();
var propertyValue = map.GetProperty<BoolProperty>("boolPropertyInMap").Value;
```

If you are unsure whether a property exists, or you want to provide some kind of default behaviour if the property is not present, you can instead use the <xref:DotTiled.IHasProperties.TryGetProperty``1(System.String,``0@)> method like so:

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

For both methods, you can replace `BoolProperty` with any of the property types that Tiled supports. You can find a list of all property types and their corresponding classes in the [next section](#all-types-of-properties).

## All types of properties

Tiled supports a variety of property types, which are represented in the DotTiled library as classes that implement the <xref:DotTiled.IProperty`1> interface. Below is a list of all property types that Tiled supports and their corresponding classes in DotTiled:

- `bool` - <xref:DotTiled.BoolProperty>
- `color` - <xref:DotTiled.ColorProperty>
- `float` - <xref:DotTiled.FloatProperty>
- `file` - <xref:DotTiled.FileProperty>
- `int` - <xref:DotTiled.IntProperty>
- `object` - <xref:DotTiled.ObjectProperty>
- `string` - <xref:DotTiled.StringProperty>

In addition to these primitive property types, [Tiled also supports more complex property types](https://doc.mapeditor.org/en/stable/manual/custom-properties/#custom-types). These custom property types are defined in Tiled according to the linked documentation, and to work with them in DotTiled, you *must* define their equivalences as a <xref:DotTiled.ICustomTypeDefinition>. This is because of how Tiled handles default values for custom property types, and DotTiled needs to know these defaults to be able to populate the properties correctly.

## Custom types

Tiled allows you to define custom property types that can be used in your maps. These custom property types can be of type `class` or `enum`. DotTiled supports custom property types by allowing you to define the equivalent in C#. This section will guide you through how to define custom property types in DotTiled and how to map properties in loaded maps to C# classes or enums.

> [!NOTE]
> While custom types are powerful, they will incur a bit of overhead as you attempt to sync them between Tiled and DotTiled. Defining custom types is recommended, but not necessary for simple use cases as Tiled supports arbitrary strings as classes.

> [!IMPORTANT]
> If you choose to use custom types in your maps, but don't define them properly in DotTiled, you may get inconsistencies between the map in Tiled and the loaded map with DotTiled. If you still want to use custom types in Tiled without having to define them in DotTiled, it is recommended to set the `Resolve object types and properties` setting in Tiled to `true`. This will make Tiled resolve the custom types for you, but it will still require you to define the custom types in DotTiled if you want to access the properties in a type-safe manner.

### Class properties

Whenever DotTiled encounters a property that is of type `class` in a Tiled file, it will use the supplied custom type resolver function to retrieve the custom type definition. It will then use that definition to know the default values of the properties of that class, and then override those defaults with the values found in the Tiled file when populating a <xref:DotTiled.ClassProperty> instance. `class` properties allow you to create hierarchical structures of properties.

For example, if you have a `class` property in Tiled that looks like this:

![MonsterSpawner class in Tiled UI](../../images/monster-spawner-class.png)

The equivalent definition in DotTiled would look like the following:

```csharp
var monsterSpawnerDefinition = new CustomClassDefinition
{
  Name = "MonsterSpawner",
  UseAs = CustomClassUseAs.All, // Not really validated by DotTiled
  Members = [ // Make sure that the default values match the Tiled UI
    new BoolProperty   { Name = "Enabled",        Value = true },
    new IntProperty    { Name = "MaxSpawnAmount", Value = 10 },
    new IntProperty    { Name = "MinSpawnAmount", Value = 0 },
    new StringProperty { Name = "MonsterNames",   Value = "" }
  ]
};
```

Luckily, you don't have to manually define these custom class definitions, even though you most definitively can for scenarios that require it. DotTiled provides a way to automatically generate these definitions for you from a C# class. This is done by using the <xref:DotTiled.CustomClassDefinition.FromClass``1> method, or one of its overloads. This method will generate a <xref:DotTiled.CustomClassDefinition> from a given C# class, and you can then use this definition when loading your maps.

```csharp
class MonsterSpawner
{
  public bool Enabled { get; set; } = true;
  public int MaxSpawnAmount { get; set; } = 10;
  public int MinSpawnAmount { get; set; } = 0;
  public string MonsterNames { get; set; } = "";
}

// ...

// These are all valid ways to create your custom class definitions from a C# class
// The first two require the class to have a default, parameterless constructor
var monsterSpawnerDefinition1 = CustomClassDefinition.FromClass<MonsterSpawner>();
var monsterSpawnerDefinition2 = CustomClassDefinition.FromClass(typeof(MonsterSpawner)); 
var monsterSpawnerDefinition3 = CustomClassDefinition.FromClass(() => new MonsterSpawner
{
  Enabled = false // This will use the property values in the instance created by a factory method as the default values
}); 
```

The last one is especially useful if you have classes that may not have parameterless constructors, or if you want to provide custom default values for the properties. Finally, the generated custom class definition will be identical to the one defined manually in the first example.

### Enum properties  

Tiled also allows you to define custom property types that work as enums. Similarly to `class` properties, you must define the equivalent in DotTiled as a <xref:DotTiled.CustomEnumDefinition>. You can then return the corresponding definition in the resolving function.

For example, if you have a custom property type in Tiled that looks like this:

![EntityType enum in Tiled UI](../../images/entity-type-enum.png)

The equivalent definition in DotTiled would look like the following:

```csharp
var entityTypeDefinition = new CustomEnumDefinition
{
  Name = "EntityType",
  StorageType = CustomEnumStorageType.String,
  ValueAsFlags = false,
  Values = [
    "Bomb",
    "Chest",
    "Flower",
    "Chair"
  ]
};
```

Similarly to custom class definitions, you can also automatically generate custom enum definitions from C# enums. This is done by using the <xref:DotTiled.CustomEnumDefinition.FromEnum``1(DotTiled.CustomEnumStorageType)> method, or one of its overloads. This method will generate a <xref:DotTiled.CustomEnumDefinition> from a given C# enum, and you can then use this definition when loading your maps.

```csharp
enum EntityType
{
  Bomb,
  Chest,
  Flower,
  Chair
}

// ...

// These are both valid ways to create your custom enum definitions from a C# enum
var entityTypeDefinition1 = CustomEnumDefinition.FromEnum<EntityType>();
var entityTypeDefinition2 = CustomEnumDefinition.FromEnum(typeof(EntityType));
```

The generated custom enum definition will be identical to the one defined manually in the first example.

For enum definitions, the <xref:System.FlagsAttribute> can be used to indicate that the enum should be treated as a flags enum. This will make it so the enum definition will have `ValueAsFlags = true` and the enum values will be treated as flags when working with them in DotTiled.

> [!NOTE]
> Tiled supports enums which can store their values as either strings or integers, and depending on the storage type you have specified in Tiled, you must make sure to have the same storage type in your <xref:DotTiled.CustomEnumDefinition>. This can be done by setting the `StorageType` property to either `CustomEnumStorageType.String` or `CustomEnumStorageType.Int` when creating the definition, or by passing the storage type as an argument to the <xref:DotTiled.CustomEnumDefinition.FromEnum``1(DotTiled.CustomEnumStorageType)> method. To be consistent with Tiled, <xref:DotTiled.CustomEnumDefinition.FromEnum``1(DotTiled.CustomEnumStorageType)> will default to `CustomEnumStorageType.String` for the storage type parameter.

## Mapping properties to C# classes or enums

So far, we have only discussed how to define custom property types in DotTiled, and why they are needed. However, the most important part is how you can map properties inside your maps to their corresponding C# classes or enums. 

The interface <xref:DotTiled.IHasProperties> has two overloads of the <xref:DotTiled.IHasProperties.MapPropertiesTo``1> method. These methods allow you to map a collection of properties to a given C# class. Let's look at an example:

```csharp
// Define a few Tiled compatible custom types
enum EntityType
{
  Player,
  Enemy,
  Collectible,
  Explosive
}

class EntityData
{
  public EntityType Type { get; set; } = EntityType.Player;
  public int Health { get; set; } = 100;
  public string Name { get; set; } = "Unnamed Entity";
}

var entityTypeDef = CustomEnumDefinition.FromEnum<EntityType>();
var entityDataDef = CustomClassDefinition.FromClass<EntityData>();
```

The above gives us two custom type definitions that we can supply to our map loader. Given a map that looks like this:

```xml
<?xml version="1.0" encoding="UTF-8"?>
<map version="1.10" tiledversion="1.11.0" orientation="orthogonal" renderorder="right-down" width="5" height="5" tilewidth="32" tileheight="32" infinite="0" nextlayerid="8" nextobjectid="7">
 <tileset firstgid="1" source="tileset.tsx"/>
 <layer id="1" name="Tile Layer 1" width="5" height="5">
  <data encoding="csv">
  1,1,1,1,1,
  0,0,0,0,0,
  0,0,0,0,0,
  0,0,0,0,0,
  0,0,0,0,0
  </data>
 </layer>
 <objectgroup id="3" name="Objects">
  <object id="4" name="Circle1" type="EntityData" x="77" y="72.3333" width="34.6667" height="34.6667">
   <properties>
    <property name="Health" type="int" value="1"/>
    <property name="Name" value="Bomb Chest"/>
    <property name="Type" type="int" propertytype="EntityType" value="3"/>
   </properties>
   <ellipse/>
  </object>
 </objectgroup>
</map>
```

We can see that there is an ellipse object in the map that has the type `EntityData` and it has set the three properties to some value other than their defaults. After having loaded this map, we can map the properties of the object to the `EntityData` class like this:

```csharp
var map = LoadMap([entityTypeDef, entityDataDef]); // Load the map somehow, using DotTiled.Loader or similar

// Retrieve the object layer from the map in some way
var objectLayer = map.Layers.Skip(1).First() as ObjectLayer;

// Retrieve the object from the object layer
var entityObject = objectLayer.Objects.First();

// Map the properties of the object to the EntityData class
var entityData = entityObject.MapPropertiesTo<EntityData>();
```

The above snippet will map the properties of the object to the `EntityData` class using reflection based on the property names. The `entityData` object will now have the properties set to the values found in the object in the map. If a property is not found in the object, the default value of the property in the `EntityData` class will be used. It will even map the nested enum to its corresponding value in C#. This will work for several levels of depth as it performs this kind of mapping recursively. <xref:DotTiled.IHasProperties.MapPropertiesTo``1> only supports mapping to classes that have a default, parameterless constructor.

### [Future] Exporting custom types

It might be possible to also make some kind of exporting functionality for <xref:DotTiled.ICustomTypeDefinition>. Given a collection of custom type definitions, DotTiled could generate a corresponding `propertytypes.json` file that you then can import into Tiled. This would make it so that you only have to define your custom property types once (in C#) and then import them into Tiled to use them in your maps.