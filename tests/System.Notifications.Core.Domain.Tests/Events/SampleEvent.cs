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
}
