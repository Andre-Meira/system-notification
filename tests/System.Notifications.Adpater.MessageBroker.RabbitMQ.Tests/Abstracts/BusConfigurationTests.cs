using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;
using System.Notifications.Adpater.MessageBroker.RabbitMQ.Abstracts;
using System.Notifications.Adpater.MessageBroker.RabbitMQ.Tests.Abstracts.Samples;

namespace System.Notifications.Adpater.MessageBroker.RabbitMQ.Tests.Abstracts;

public class BusConfigurationTests : IClassFixture<IntegrationRabbitFixture>
{
    private readonly IntegrationRabbitFixture _integrationRabbit;

    public BusConfigurationTests(IntegrationRabbitFixture integrationRabbitFixture)
    {
        _integrationRabbit = integrationRabbitFixture;
    }

    [Fact(DisplayName = "Valida inicialiazação do bus")]
    public void Valida_Inialializacao_do_Bus()
    {
        ServiceCollection serviceDescriptors = new ServiceCollection();
        serviceDescriptors.AddBus(_integrationRabbit.AsyncConnectionFactory);
        var provider = serviceDescriptors.BuildServiceProvider();
        provider.GetRequiredService<IPublishContext>();
    }

    [Fact(DisplayName = "Valida criação de um consumer")]
    public void AddConsumer()
    {
        ServiceCollection serviceDescriptors = new ServiceCollection();
        serviceDescriptors.AddBus(_integrationRabbit.AsyncConnectionFactory);

        serviceDescriptors.AddConsumer<ConsumerSample, SampleOrder>(e =>
        {
            e.Configure("consumer-sample", ExchangeType.Topic, "*.order");
            e.ConfigureExchangeConsumer(ConstantsRoutings.exchange_order, ConstantsRoutings.exchange_type_order);
            e.Validate();
        });

        var provider = serviceDescriptors.BuildServiceProvider();
        provider.GetRequiredService<ConsumerSample>();
    }
}
