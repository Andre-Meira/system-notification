using RabbitMQ.Client.Events;
using System.Diagnostics;
using System.Text;

namespace System.Notifications.Adpater.MessageBroker.RabbitMQ.Abstracts.Monitory;

public static class ActivityTraceExntesions
{
    private static readonly ActivitySource activitySource = new ActivitySource(ActivityConstants.Source, "1.0.0");

    public static ActivityBus? CreatePublishActivityBus<TMessage>(this TMessage message,
        string exchange,
        string exchangeType,
        string routingKey)
        where TMessage : class
    {
        var activity = Activity.Current;
        if (activity is null) return null;

        var customActivity = activitySource.CreateActivity(ActivityConstants.NameActivity_Publish, ActivityKind.Producer, activity.Context);

        if (customActivity == null) return null;

        customActivity.SetTag(ActivityConstants.MessageName, typeof(TMessage).Name);
        customActivity.SetTag(ActivityConstants.MessageRouting, routingKey);
        customActivity.SetTag(ActivityConstants.MessageExchange, exchange);
        customActivity.SetTag(ActivityConstants.MessageExchangeType, exchangeType);

        return new ActivityBus(customActivity);
    }

    public static ActivityBus? CreateConsumerActivityBus<TConsumer>(this BasicDeliverEventArgs message)
        where TConsumer : IConsumer
    {
        var headers = message.BasicProperties.Headers;
        var value = headers[ActivityConstants.Header_Id];

        if (value == null) return null;
        string valueString = Encoding.UTF8.GetString((byte[])value);


        if (ActivityContext.TryParse(valueString, null, out var activityContext))
        {
            var parentActivityContext = new ActivityContext(activityContext.TraceId, activityContext.SpanId,
                activityContext.TraceFlags, activityContext.TraceState, true);

            var customActivity = activitySource.CreateActivity(typeof(TConsumer).Name, ActivityKind.Consumer, parentActivityContext);

            if (customActivity == null) return null;

            customActivity.SetTag(ActivityConstants.ConsumerRouting, message.RoutingKey);
            customActivity.SetTag(ActivityConstants.ConsumerExchange, message.Exchange);

            return new ActivityBus(customActivity);
        }

        return default;
    }

}
