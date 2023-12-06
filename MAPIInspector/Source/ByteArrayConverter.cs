using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace MAPIInspector
{
    public class ByteArrayConverter : JsonConverter
    {
        public override void WriteJson(
            JsonWriter writer,
            object value,
            JsonSerializer serializer)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            byte[] data = (byte[])value;

            writer.WriteValue(BitConverter.ToString(data).Replace("-", ""));
        }

        public override object ReadJson(
            JsonReader reader,
            Type objectType,
            object existingValue,
            JsonSerializer serializer)
        {
            if (reader.TokenType == JsonToken.String)
            {
                // Parse the hex string into a byte array
                string hexString = (string)reader.Value;
                byte[] byteData = new byte[hexString.Length / 2];

                for (int i = 0; i < byteData.Length; i++)
                {
                    byteData[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
                }

                return byteData;
            }
            else
            {
                throw new Exception(
                    string.Format(
                        "Unexpected token parsing binary. "
                        + "Expected String, got {0}.",
                        reader.TokenType));
            }
        }

        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(byte[]);
        }
    }
}
