using ServiceStack.Text;
using SkyApm.Common;
using SkyApm.Config;
using SkyApm.Tracing;
using SkyApm.Tracing.Segments;
using System;
using ServiceStack;
using System.Data.Common;

namespace SkyApm.Diagnostics.SSRedis
{
    public class SSRedisTracingDiagnosticProcessor : ITracingDiagnosticProcessor
    {

        #region Const

        public const string ComponentName = "ServiceStack.Redis";

        // public const string FreeRedis_Notice = "FreeRedis.Notice";

        #endregion

        public string ListenerName => ServiceStack.Diagnostics.Listeners.Redis;

        private readonly ITracingContext _tracingContext;
        private readonly IExitSegmentContextAccessor _contextAccessor;
        private readonly ILocalSegmentContextAccessor _localSegmentContextAccessor;
        private readonly TracingConfig _tracingConfig;
        public SSRedisTracingDiagnosticProcessor(ITracingContext tracingContext,
            ILocalSegmentContextAccessor localSegmentContextAccessor, IExitSegmentContextAccessor contextAccessor, IConfigAccessor configAccessor)
        {
            _tracingContext = tracingContext;
            _localSegmentContextAccessor = localSegmentContextAccessor;
            _contextAccessor = contextAccessor;
            _tracingConfig = configAccessor.Get<TracingConfig>();
        }

        #region WriteCommand
        [DiagnosticName(ServiceStack.Diagnostics.Events.Redis.WriteCommandBefore)]
        public void WriteCommandBefore([Object] ServiceStack.RedisDiagnosticEvent eventData)
        {
            var context = _tracingContext.CreateExitSegmentContext("SSRedis.WriteCommand", $"{eventData.Host}:{eventData.Port}");
            context.Span.SpanLayer = SpanLayer.CACHE;
            context.Span.Component = Components.Redis; // Components.SSRedis;
            context.Span.Peer = new Common.StringOrIntValue($"{eventData.Host}:{eventData.Port}");
            context.Span.AddTag(Tags.CACHE_TYPE, ComponentName);
            context.Span.AddTag(Tags.CACHE_OP, eventData.Operation);
            context.Span.AddTag(Tags.CACHE_CMD, GetUtf8BytesString(eventData.Command));
            context.Span.AddTag("op_id", eventData.OperationId.ToString());
            context.Span.AddTag("req_time", eventData.Timestamp);
            if (eventData.Exception != null)
                context.Span.ErrorOccurred(eventData.Exception, _tracingConfig);
            //_tracingContext.Release(context);
        }


        [DiagnosticName(ServiceStack.Diagnostics.Events.Redis.WriteCommandAfter)]
        public void WriteCommandAfter([Object] ServiceStack.RedisDiagnosticEvent eventData)
        {
            //if (eventData == null)
            //    return;

            //var context = _tracingContext.CreateExitSegmentContext(eventData.EventType, $"{eventData.Host}:{eventData.Port}", startTimeMilliseconds: eventData.BegainTimestamp);
            //context.Span.SpanLayer = SpanLayer.CACHE;
            //context.Span.Component = Components.SSRedis;
            //context.Span.Peer = new Common.StringOrIntValue($"{eventData.Host}:{eventData.Port}");
            //context.Span.AddTag(Tags.CACHE_TYPE, ComponentName);
            //context.Span.AddTag(Tags.CACHE_OP, eventData.Operation);
            //context.Span.AddTag(Tags.CACHE_CMD, GetUtf8BytesString(eventData.Command));
            //context.Span.AddTag("op_id", eventData.OperationId.ToString());
            //context.Span.AddTag("req_time", eventData.BegainTimestamp);
            //context.Span.AddTag("exec_time", eventData.DurationMs);
            //if (eventData.Exception != null)
            //    context.Span.ErrorOccurred(eventData.Exception, _tracingConfig);
            //_tracingContext.Release(context);

            var context = _contextAccessor.Context;
            if (context != null)
            {
                if (eventData != null)
                {
                    context.Span.AddTag("resp_time", eventData.Timestamp);
                    context.Span.AddTag("exec_time", eventData.DurationMs);

                    //long reqTime = 0;
                    //string strReqTime = context.Span.GetTag("req_time");
                    //if (!string.IsNullOrEmpty(strReqTime) && long.TryParse(strReqTime, out reqTime))
                    //{
                    //    context.Span.AddTag("exec_time", eventData.Timestamp - reqTime);
                    //}

                    if (eventData?.Exception != null)
                        context.Span.ErrorOccurred(eventData.Exception, _tracingConfig);
                }

                _tracingContext.Release(context);
            }
        }

        [DiagnosticName(ServiceStack.Diagnostics.Events.Redis.WriteCommandError)]
        public void WriteCommandError([Object] ServiceStack.RedisDiagnosticEvent eventData)
        {
            //if (eventData == null)
            //    return;

            //var context = _tracingContext.CreateExitSegmentContext(eventData.EventType, $"{eventData.Host}:{eventData.Port}", startTimeMilliseconds: eventData.BegainTimestamp);
            //context.Span.SpanLayer = SpanLayer.CACHE;
            //context.Span.Component = Components.SSRedis;
            //context.Span.Peer = new Common.StringOrIntValue($"{eventData.Host}:{eventData.Port}");
            //context.Span.AddTag(Tags.CACHE_TYPE, ComponentName);
            //context.Span.AddTag(Tags.CACHE_OP, eventData.Operation);
            //context.Span.AddTag(Tags.CACHE_CMD, GetUtf8BytesString(eventData.Command));
            //context.Span.AddTag("op_id", eventData.OperationId.ToString());
            //context.Span.AddTag("req_time", eventData.BegainTimestamp);
            //context.Span.AddTag("exec_time", eventData.DurationMs);
            //if (eventData.Exception != null)
            //    context.Span.ErrorOccurred(eventData.Exception, _tracingConfig);
            //_tracingContext.Release(context);
            var context = _contextAccessor.Context;
            if (context != null)
            {
                if (eventData != null)
                {
                    context.Span.AddTag("resp_time", eventData.Timestamp);
                    context.Span.AddTag("exec_time", eventData.DurationMs);

                    //long reqTime = 0;
                    //string strReqTime = context.Span.GetTag("req_time");
                    //if (!string.IsNullOrEmpty(strReqTime) && long.TryParse(strReqTime, out reqTime))
                    //{
                    //    context.Span.AddTag("exec_time", eventData.Timestamp - reqTime);
                    //}

                    if (eventData?.Exception != null)
                        context.Span.ErrorOccurred(eventData.Exception, _tracingConfig);
                }

                _tracingContext.Release(context);
            }
        }

        [DiagnosticName(ServiceStack.Diagnostics.Events.Redis.WriteCommandRetry)]
        public void WriteCommandRetry([Object] ServiceStack.RedisDiagnosticEvent eventData)
        {
            if (eventData == null)
                return;

            var context = _tracingContext.CreateExitSegmentContext("SSRedis.RetryCommand", $"{eventData.Host}:{eventData.Port}", startTimeMilliseconds: eventData.BegainTimestamp);
            context.Span.SpanLayer = SpanLayer.CACHE;
            context.Span.Component = Components.Redis; // Components.SSRedis;
            context.Span.Peer = new Common.StringOrIntValue($"{eventData.Host}:{eventData.Port}");
            context.Span.AddTag(Tags.CACHE_TYPE, ComponentName);
            context.Span.AddTag(Tags.CACHE_OP, eventData.Operation);
            context.Span.AddTag(Tags.CACHE_CMD, GetUtf8BytesString(eventData.Command));
            context.Span.AddTag("op_id", eventData.OperationId.ToString());
            context.Span.AddTag("req_time", eventData.BegainTimestamp);
            // context.Span.AddTag("exec_time", eventData.DurationMs);
            if (eventData.Exception != null)
                context.Span.ErrorOccurred(eventData.Exception, _tracingConfig);
            //_tracingContext.Release(context);
            //var context = _contextAccessor.Context;
            //if (context != null)
            //{
            //    if (eventData != null)
            //    {
            //        //context.Span.AddTag("retry_time", eventData.Timestamp);
            //        int retryTimes = 1;
            //        string strRetryTimes = context.Span.GetTag("retry_times");
            //        if (!string.IsNullOrEmpty(strRetryTimes) && int.TryParse(strRetryTimes, out retryTimes))
            //        {
            //            retryTimes++;
            //        }
            //        context.Span.AddTag("retry_times", retryTimes);

            //        if (eventData?.Exception != null)
            //            context.Span.ErrorOccurred(eventData.Exception, _tracingConfig);
            //    }
            //}
        }

        #endregion

        #region ConnectionOpen

        [DiagnosticName(ServiceStack.Diagnostics.Events.Redis.WriteConnectionOpenBefore)]
        public void WriteConnectionOpenBefore([Object] ServiceStack.RedisDiagnosticEvent eventData)
        {
            var context = _tracingContext.CreateExitSegmentContext("SSRedis.ConnectionOpen", $"{eventData.Host}:{eventData.Port}");
            context.Span.SpanLayer = SpanLayer.CACHE;
            context.Span.Component = Components.Redis; // Components.SSRedis;
            context.Span.Peer = new Common.StringOrIntValue($"{eventData.Host}:{eventData.Port}");
            context.Span.AddTag(Tags.CACHE_TYPE, ComponentName);
            context.Span.AddTag(Tags.CACHE_OP, eventData.Operation);
            //context.Span.AddTag(Tags.CACHE_CMD, GetUtf8BytesString(eventData.Command));
            context.Span.AddTag("op_id", eventData.OperationId.ToString());
            context.Span.AddTag("req_time", eventData.Timestamp);
            if (eventData.Exception != null)
                context.Span.ErrorOccurred(eventData.Exception, _tracingConfig);
            //_tracingContext.Release(context);
        }


        [DiagnosticName(ServiceStack.Diagnostics.Events.Redis.WriteConnectionOpenAfter)]
        public void WriteConnectionOpenAfter([Object] ServiceStack.RedisDiagnosticEvent eventData)
        {
            //if (eventData == null)
            //    return;

            //var context = _tracingContext.CreateExitSegmentContext(eventData.EventType, $"{eventData.Host}:{eventData.Port}", startTimeMilliseconds: eventData.BegainTimestamp);
            //context.Span.SpanLayer = SpanLayer.CACHE;
            //context.Span.Component = Components.SSRedis;
            //context.Span.Peer = new Common.StringOrIntValue($"{eventData.Host}:{eventData.Port}");
            //context.Span.AddTag(Tags.CACHE_TYPE, ComponentName);
            //context.Span.AddTag(Tags.CACHE_OP, eventData.Operation);
            //context.Span.AddTag(Tags.CACHE_CMD, GetUtf8BytesString(eventData.Command));
            //context.Span.AddTag("op_id", eventData.OperationId.ToString());
            //context.Span.AddTag("req_time", eventData.BegainTimestamp);
            //context.Span.AddTag("exec_time", eventData.DurationMs);
            //if (eventData.Exception != null)
            //    context.Span.ErrorOccurred(eventData.Exception, _tracingConfig);
            //_tracingContext.Release(context);

            var context = _contextAccessor.Context;
            if (context != null)
            {
                if (eventData != null)
                {
                    context.Span.AddTag("resp_time", eventData.Timestamp);
                    context.Span.AddTag("exec_time", eventData.DurationMs);

                    //long reqTime = 0;
                    //string strReqTime = context.Span.GetTag("req_time");
                    //if (!string.IsNullOrEmpty(strReqTime) && long.TryParse(strReqTime, out reqTime))
                    //{
                    //    context.Span.AddTag("exec_time", eventData.Timestamp - reqTime);
                    //}

                    if (eventData?.Exception != null)
                        context.Span.ErrorOccurred(eventData.Exception, _tracingConfig);
                }

                _tracingContext.Release(context);
            }
        }

        [DiagnosticName(ServiceStack.Diagnostics.Events.Redis.WriteConnectionOpenError)]
        public void WriteConnectionOpenError([Object] ServiceStack.RedisDiagnosticEvent eventData)
        {
            //if (eventData == null)
            //    return;

            //var context = _tracingContext.CreateExitSegmentContext(eventData.EventType, $"{eventData.Host}:{eventData.Port}", startTimeMilliseconds: eventData.BegainTimestamp);
            //context.Span.SpanLayer = SpanLayer.CACHE;
            //context.Span.Component = Components.SSRedis;
            //context.Span.Peer = new Common.StringOrIntValue($"{eventData.Host}:{eventData.Port}");
            //context.Span.AddTag(Tags.CACHE_TYPE, ComponentName);
            //context.Span.AddTag(Tags.CACHE_OP, eventData.Operation);
            //context.Span.AddTag(Tags.CACHE_CMD, GetUtf8BytesString(eventData.Command));
            //context.Span.AddTag("op_id", eventData.OperationId.ToString());
            //context.Span.AddTag("req_time", eventData.BegainTimestamp);
            //context.Span.AddTag("exec_time", eventData.DurationMs);
            //if (eventData.Exception != null)
            //    context.Span.ErrorOccurred(eventData.Exception, _tracingConfig);
            //_tracingContext.Release(context);
            var context = _contextAccessor.Context;
            if (context != null)
            {
                if (eventData != null)
                {
                    context.Span.AddTag("resp_time", eventData.Timestamp);
                    context.Span.AddTag("exec_time", eventData.DurationMs);

                    //long reqTime = 0;
                    //string strReqTime = context.Span.GetTag("req_time");
                    //if (!string.IsNullOrEmpty(strReqTime) && long.TryParse(strReqTime, out reqTime))
                    //{
                    //    context.Span.AddTag("exec_time", eventData.Timestamp - reqTime);
                    //}

                    if (eventData?.Exception != null)
                        context.Span.ErrorOccurred(eventData.Exception, _tracingConfig);
                }

                _tracingContext.Release(context);
            }
        }
        
        #endregion

        #region Utils

        public static string GetUtf8BytesString(byte[][] bytes)
        {
            var sb = StringBuilderCache.Allocate();
            bool skipNext = false;
            foreach (var arg in bytes)
            {
                if (skipNext)
                {
                    skipNext = false;
                    continue;
                }

                var strArg = arg.FromUtf8Bytes();
                if (strArg == "AUTH")
                    skipNext = true;

                if (sb.Length > 0)
                    sb.Append(" ");

                sb.Append(strArg);

                if (sb.Length > 100)
                {
                    sb.Remove(100,sb.Length- 100);
                    sb.Append("...");
                    break;
                }
            }
            return StringBuilderCache.ReturnAndFree(sb);
        }

        #endregion
    }
}
