using System;
using System.IO;
using Newtonsoft.Json;
using SQLite.Net;

namespace FoodByMe.Core.Services.Data
{
    public class JsonBlobSerializer : IBlobSerializer
    {
        private readonly JsonSerializer _serializer;

        public JsonBlobSerializer()
        {
            _serializer = new JsonSerializer();
        }

        public byte[] Serialize<T>(T obj)
        {
            using (var ms = new MemoryStream())
            using (var writer = new StreamWriter(ms))
            {
                _serializer.Serialize(writer, obj);
                return ms.ToArray();
            }
        }

        public object Deserialize(byte[] data, Type type)
        {
            using (var ms = new MemoryStream(data))
            using (var reader = new StreamReader(ms))
            {
                return _serializer.Deserialize(reader, type);
            }
        }

        public bool CanDeserialize(Type type)
        {
            return true;
        }
    }
}