using System;
using System.Collections.Generic;
using Entitas;

namespace Entitas.Serialization
{
    public class ComponentBlueprint
    {
        public SerializableMember[] Members; // Component members
        private int m_index;
        private readonly string m_name; // Component name without suffix

        Dictionary<string, PublicMemberInfo> m_componentMembers;

        private Type m_type;

        public ComponentBlueprint(string name)
        {
            m_name = name;
            m_index = -1;
        }

        Tuple<Type, int> GetComponentType(Entity entity)
        {
            for (int i = 0; i < entity.contextInfo.componentTypes.Length; i++)
            {
                var t = entity.contextInfo.componentTypes[i];
                if(t.Name.Length == m_name.Length + "Component".Length 
                && t.Name.StartsWith(m_name) && t.Name.EndsWith("Component"))
                {
                    return Tuple.Create(t, i);
                }
            }
            return null;
        }

        Dictionary<string, PublicMemberInfo> GetComponentMembers()
        {
            if (m_componentMembers == null)
            {
                var memberInfos = m_type.GetPublicMemberInfos();
                m_componentMembers = new Dictionary<string, PublicMemberInfo>(memberInfos.Count);
                for (int i = 0, memberInfosLength = memberInfos.Count; i < memberInfosLength; i++)
                {
                    var info = memberInfos[i];
                    m_componentMembers.Add(info.name, info);
                }
            }

            return m_componentMembers;
        }

        public void Apply(Entity entity)
        {
            if (m_type == null)
            {
                var tuple = GetComponentType(entity);
                if (tuple == null)
                {
                    throw new ComponentBlueprintException($"Could not find '{m_name}Component' in '{entity.contextInfo.name}Context'",
                        "Please check component definition");
                }
                m_type = tuple.Item1;
                m_index = tuple.Item2;

                var publicMembers = GetComponentMembers();
                foreach(var member in Members)
                {
                    member.Parse(publicMembers[member.Name]);
                }
            }

            var component = entity.CreateComponent(m_index, m_type);
            foreach (var m in Members)
            {
                PublicMemberInfo memberInfo;
                if (m_componentMembers.TryGetValue(m.Name, out memberInfo))
                {
                    memberInfo.SetValue(component, m.Value);
                }
                else
                {
                    throw new ComponentBlueprintException($"Could not find member '{m.Name} ' in type '{m_name}Component'!",
                        "Only non-static public members are supported.");
                }
            }

            entity.AddComponent(m_index, component);
        }
    }

    public class ComponentBlueprintException : EntitasException
    {
        public ComponentBlueprintException(string message, string hint) : base(message, hint)
        {
        }
    }
}