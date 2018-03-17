using System.Collections.Generic;

namespace Entitas.Serialization
{
    public interface IBlueprints
    {
        Dictionary<string, Blueprint> ToBlueprints(byte[] data);
        byte[] ToBytes(Entity entity);
    }
}