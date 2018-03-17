using System;

namespace Entitas.Serialization
{
    [Serializable]
    public class SerializableMember {
        public readonly string Name;
        public readonly object Value;

        public SerializableMember(string name, object value) 
        {
            this.Name = name;
            this.Value = value;
        }
    }
}