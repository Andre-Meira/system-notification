using System.Text.Json.Serialization;

namespace System.Notifications.Core.Domain.Tests.Events;

public record SampleEvent
{
    public record SampleOrder
    {
        public SampleOrder(string name)
        {
            Name = name;   
        }

        [JsonRequired]
        [JsonPropertyName("name_text")]
        public string Name { get; set; }
    }


    public record SampleOrder2
    {
        public SampleOrder2(int value)
        {
            Value = value;
        }

        [JsonRequired]
        [JsonPropertyName("value_int")]        
        public int Value { get; set; }  
    }
}
