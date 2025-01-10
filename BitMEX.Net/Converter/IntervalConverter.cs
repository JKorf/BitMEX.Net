using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BitMEX.Net.Converter
{
    internal class IntervalConverter : JsonConverter<TimeSpan>
    {
        public override bool HandleNull => true;

        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var time = reader.GetDateTime();
            return new TimeSpan(time.Hour, time.Minute, time.Second);
        }

        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
            => writer.WriteStringValue($"2000-01-01T{value.Hours}:{value.Minutes}:{value.Seconds}.{value.Milliseconds}Z");
    }
}
