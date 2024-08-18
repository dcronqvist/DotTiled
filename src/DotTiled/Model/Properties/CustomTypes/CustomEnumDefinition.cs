using System.Collections.Generic;

namespace DotTiled.Model;

public enum CustomEnumStorageType
{
  Int,
  String
}

public class CustomEnumDefinition : CustomTypeDefinition
{
  public CustomEnumStorageType StorageType { get; set; }
  public List<string> Values { get; set; } = [];
  public bool ValueAsFlags { get; set; }
}
