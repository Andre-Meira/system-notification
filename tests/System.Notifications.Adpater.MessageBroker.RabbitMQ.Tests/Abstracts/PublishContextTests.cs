using Microsoft.Extensions.DependencyInjection;
using System.Notifications.Adpater.MessageBroker.RabbitMQ.Abstracts;
using System.Notifications.Adpater.MessageBroker.RabbitMQ.Tests.Abstracts.Samples;

namespace System.Notifications.Adpater.MessageBroker.RabbitMQ.Tests.Abstracts;

public class PublishContextTests : IClassFixture<IntegrationRabbitFixture>
{
    private readonly IntegrationRabbitFixture _integrationRabbit;

    public PublishContextTests(IntegrationRabbitFixture integrationRabbitFixture)
    {
        _integrationRabbit = integrationRabbitFixture;
    }

    [Fact]
    public async Task Valida_Publicao_de_mensagem()
    {
        ServiceCollection serviceDescriptors = new ServiceCollection();
        serviceDescriptors.AddBus(_integrationRabbit.AsyncConnectionFactory);
        var provider = serviceDescriptors.BuildServiceProvider();

        IPublishContext publishContext = provider.GetRequiredService<IPublishContext>();

        var @event = new SampleOrder("teste");
        await publishContext.PublishTopicMessage(@event, ConstantsRoutings.exchange_order, "process.order");
    }
}