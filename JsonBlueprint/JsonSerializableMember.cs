using System;
using LitJson;
namespace Entitas.Serialization.Json
{
    public class JsonSerializableMember:SerializableMember
    {
        JsonData m_jsonData;

        public JsonSerializableMember(string name, JsonData data)
        {
            Name = name;
            m_jsonData = data;
        }

        public override void Parse(PublicMemberInfo memberInfo)
        {
            object obj = null;
            switch (m_jsonData.GetJsonType())
            {
                case JsonType.None:
                    break;
                case JsonType.Object:
                    obj = JsonMapper.ToObject(JsonMapper.ToJson(m_jsonData), memberInfo.type);
                    break;
                case JsonType.Array:
                    //                    obj = JsonMapper.ToObject(JsonMapper.ToJson(memberData), memberInfo.type)
                    break;
                case JsonType.String:
                    obj = Convert.ChangeType((string)m_jsonData, memberInfo.type);
                    break;
                case JsonType.Int:
                    obj = Convert.ChangeType((int)m_jsonData, memberInfo.type);
                    break;
                case JsonType.Long:
                    obj = Convert.ChangeType((long)m_jsonData, memberInfo.type);
                    break;
                case JsonType.Double:
                    obj = Convert.ChangeType((double)m_jsonData, memberInfo.type);
                    break;
                case JsonType.Boolean:
                    obj = Convert.ChangeType((bool)m_jsonData, memberInfo.type);
                    break;
            }
            Value = obj;
        }
    }
}
