using System;
using System.Collections.Generic;
using System.Text;
using LitJson;

namespace Entitas.Serialization.Json
{
    public class JsonBlueprints:IBlueprints
    {
        public JsonBlueprints()
        {
            JsonMapper.RegisterExporter<float>((obj, writer) => writer.Write(Convert.ToDouble(obj)));
            JsonMapper.RegisterImporter<double, float>(Convert.ToSingle);
        }
        
        public Dictionary<string,Blueprint> ToBlueprints(byte[] data)
        {
            var text = Encoding.UTF8.GetString(data);
            var root = JsonMapper.ToObject(text);
            
            var blueprints = new Dictionary<string,Blueprint>();
            
            foreach (KeyValuePair<string, JsonData> n in root)
            {
                blueprints[n.Key] = ParseBlueprint(n.Key, n.Value);
            }

            return blueprints;
        }

        public byte[] ToBytes(Entity entity)
        {
            return null;
        }

        Blueprint ParseBlueprint(string name, JsonData blueprintData)
        {
            var components = new ComponentBlueprint[blueprintData.Count];
            int i = 0;
            foreach (KeyValuePair<string,JsonData> n in blueprintData)
            {
                components[i] = ParseComponentBlueprint(n.Key, n.Value);
                i++;
            }

            return new Blueprint(name, components);
        }

        ComponentBlueprint ParseComponentBlueprint(string name, JsonData componentData)
        {
            var componentBlueprint = new ComponentBlueprint(name);
            var publicMembers = componentBlueprint.GetComponentMembers();
            var members = new SerializableMember[componentData.Count];
            int i = 0;
            foreach (KeyValuePair<string, JsonData> n in componentData)
            {
                members[i] = ParseMember(publicMembers[n.Key], n.Key, n.Value);
                i++;
            }

            componentBlueprint.Members = members;
            return componentBlueprint;
        }

        // TODO: support array
        SerializableMember ParseMember(PublicMemberInfo memberInfo, string memberName, JsonData memberData)
        {
            object obj = null;
            switch (memberData.GetJsonType())
            {
                case JsonType.None:
                    break;
                case JsonType.Object:
                    obj = JsonMapper.ToObject(JsonMapper.ToJson(memberData),memberInfo.type);
                    break;
                case JsonType.Array:
//                    obj = JsonMapper.ToObject(JsonMapper.ToJson(memberData), memberInfo.type)
                    break;
                case JsonType.String:
                    obj = Convert.ChangeType((string) memberData, memberInfo.type);
                    break;
                case JsonType.Int:
                    obj = Convert.ChangeType((int) memberData, memberInfo.type);
                    break;
                case JsonType.Long:
                    obj = Convert.ChangeType((long) memberData, memberInfo.type);
                    break;
                case JsonType.Double:
                    obj = Convert.ChangeType((double) memberData, memberInfo.type);
                    break;
                case JsonType.Boolean:
                    obj = Convert.ChangeType((bool) memberData, memberInfo.type);
                    break;
            }
            return new SerializableMember(memberName, obj);
        }
    }
}