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

        Type GetComponentType()
        {
            var componentName = m_name + "Component";
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var type = assembly.GetType(componentName);
                if (type != null) 
                {
                    return type;
                }
            }
            return null;
        }

        public Dictionary<string, PublicMemberInfo> GetComponentMembers()
        {
            if (m_type == null)
                m_type = GetComponentType();

            if (m_type == null)
            {
                throw new ComponentBlueprintException($"Type '{m_name}Component'  doesn't exist in any assembly!" ,
                    "Please check the full type name.");
            }
            
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
            if (m_index == -1)
            {
                m_index = Array.IndexOf(entity.contextInfo.componentTypes, m_type);
            }

            if (m_index == -1)
            {
                throw new ComponentBlueprintException($"Could not find '{m_name}Component' in '{entity.contextInfo.name}Context'",
                    "Please check the componnet definition");
            }

            var component = entity.CreateComponent(m_index, m_type);
            foreach (var m in Members)
            {
                if (m_componentMembers.TryGetValue(m.Name, out var memberInfo))
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
        public ComponentBlueprintException(string message, string hint) :base(message,hint)
        {
        }
    }
}