﻿using System.Diagnostics;
using System.Net;

namespace System.Notifications.Adpater.MessageBroker.RabbitMQ.Abstracts.Monitory;

public readonly struct ActivityBus
{
    private readonly Activity _activity;
    public ActivityBus(Activity activity) => _activity = activity;

    public void SetTag(string key, string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            return;

        _activity.SetTag(key, value);
    }

    public void AddExceptionEvent(Exception exception)
    {
        exception = exception.GetBaseException() ?? exception;

        var exceptionMessage = exception.Message;

        var tags = new ActivityTagsCollection
            {
                { ActivityConstants.ExceptionMessage, exceptionMessage },
                { ActivityConstants.ExceptionType, exception.GetType().Name },
                { ActivityConstants.ExceptionStackTrace, exception.StackTrace }
            };

        var activityEvent = new ActivityEvent("exception", DateTimeOffset.UtcNow, tags);

        _activity.AddEvent(activityEvent);
        _activity.SetStatus(ActivityStatusCode.Error, exceptionMessage);
    }

    public void Stop()
    {
        if (_activity.Status == ActivityStatusCode.Unset)
            _activity.SetStatus(ActivityStatusCode.Ok);

        _activity.Dispose();
    }

    public void Start()
    {
        _activity.Start();

        _activity.SetTag(ActivityConstants.Hostname, Dns.GetHostName());        
        _activity.SetTag(ActivityConstants.TraceId, _activity.TraceId);
        _activity.SetTag(ActivityConstants.SpanId, _activity.SpanId);
    }
}


