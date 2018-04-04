using System;

namespace Entitas.Serialization
{
    [Serializable]
    public class SerializableMember
    {
        public string Name;
        public object Value;

        public SerializableMember(string name, object value)
        {
            this.Name = name;
            this.Value = value;
        }

        public SerializableMember()
        {

        }

        public virtual void Parse(PublicMemberInfo memberInfo)
        {

        }
    }
}