# EntitasBlueprint

Blueprint parser for entitas. It uses [Litjson](https://github.com/LitJSON/litjson) as the default json parser.

## Dependencies

* [Entitas](https://github.com/sschmid/Entitas-CSharp)

## Installation

* Download [release](https://github.com/carlself/EntitasBlueprint/releases/) package.

## Usage

Please see [Example](https://github.com/carlself/EntitasBlueprint/tree/master/Examples) for component definitions.

The json format

```json
{

  "Entity1": {
    "Movable": {},
    "Position": {
      "value": {
        "x": 1.0,
        "y": 2.0
      }
    },
    "Speed": {
      "value": 1.0
    },
    "Asset": {
      "value": "Player"
    }
  },
  "Entity2": {
    "Asset": {
      "value": "Player"
    }
  }
}
```

Unity code

```csharp
using Entitas.Serialization;
using Entitas.Serialization.Json;

public class Test : MonoBehavior
{
    void Awake()
    {
        IBlueprints jsonBlueprints = new JsonBlueprints();
        var text = Resources.Load<TextAsset>("blueprints");
        var blueprints = jsonBlueprints.ToBlueprints(text.bytes);

        var entity = Contexts.sharedInstance.game.CreateEntity();
        blueprints["Entity1"].Apply(entity);
    }
}
```

## TODO

* Array data member in component support
* Entity serialization
* Protobuffer format support