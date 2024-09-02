# Representation model

Tiled map files contain various types of data, such as tilesets, layers, and object groups. The representation model is a way to represent this data in a structured way. By using the `.tmx` file format as inspiration, the representation model is a collection of classes which mimic the structure of a Tiled map file.

Certain properties throughout the representation model are marked as *optional* by being either wrapped in a <xref:DotTiled.Optional`1> or by having a set default value.

- Properties that make use of the [required](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/required) keyword must be present in the file, otherwise an error will be raised.
- Properties that have default values will use the default value if the property is not present in the file, and are not marked as required or optional since you must not provide a value for them.
- Properties that are wrapped in <xref:DotTiled.Optional`1> may or may not be present in the file, and have no default value.