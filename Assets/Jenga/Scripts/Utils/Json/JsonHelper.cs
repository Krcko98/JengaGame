using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JengaGame.Utils.Json
{
    public class JsonHelper
    {
        public static List<T> FromJson<T>(string json)
        {
            JsonWrapper<T> wrapper = JsonUtility.FromJson<JsonWrapper<T>>(json);
            return wrapper.objs;
        }

        public static string FormatJson(string json)
        {
            char leftBracket = '{';
            char rightBracket = '}';

            string newJson = $"{leftBracket}\"objs\":{json}{rightBracket}";

            return newJson;
        }
    }

    [System.Serializable]
    internal class JsonWrapper<T>
    {
        public List<T> objs;
    }
}
