﻿using System.Notifications.Adpater.MessageBroker.RabbitMQ.Abstracts;

namespace System.Notifications.Adpater.MessageBroker.RabbitMQ.Tests.Abstracts.Samples;

public record SampleOrder(string Name);

internal class ConsumerSample : IConsumerHandler<SampleOrder>
{
    public Task Consumer(IConsumerContext<SampleOrder> context)
    {
        return Task.CompletedTask;
    }
}
