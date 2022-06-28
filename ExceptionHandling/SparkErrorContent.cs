using System;
using System.Collections.Generic;

namespace SparkDotNet.ExceptionHandling
{
    public class SparkErrorContent
    {
        public string Message { get; set; }
        public List<SparkErrorMessage> Errors { get; set; }
        public string TrackingId { get; set; }
        public Uri RequestUrl { get; internal set; }
        public string Body { get; set; }
        public string Method { get; set; }
        public override string ToString() => $"Message:{Message}\nTrackingId:{TrackingId}\nMethod:{Method}\nRequestURL:{RequestUrl}\nBody:{Body}";
    }
}