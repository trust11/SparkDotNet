using System.Collections.Generic;

namespace SparkDotNet.ExceptionHandling
{
    public class SparkErrorContent
    {
        public string Message { get; set; }

        public List<SparkErrorMessage> Errors { get; set; }

        public string TrackingId { get; set; }

        public override string ToString()
        {
            return $"TrackingId:{TrackingId}\nMessage:{Message}\nErrors:{string.Join<SparkErrorMessage>("\n", Errors.ToArray())}";
        }
    }
}