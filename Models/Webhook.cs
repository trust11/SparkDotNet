using Newtonsoft.Json;
using System;

namespace SparkDotNet.Models
{
    /// <summary>
    /// Webhooks allow your app to be notified via HTTP when a specific event occurs in Webex Teams. For example,
    /// your app can register a webhook to be notified when a new message is posted into a specific room.
    /// Events trigger in near real-time allowing your app and backend IT systems to stay in sync with new content and room activity.
    /// Check the Webhooks Guide and our blog regularly for announcements of additional webhook resources and event types.
    /// </summary>
    public class Webhook : WebexObject
    {
        /// <summary>
        /// A unique identifier for the webhook.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// A user-friendly name for the webhook.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The URL that receives POST requests for each event.
        /// </summary>
        public string TargetUrl { get; set; }

        /// <summary>
        /// The resource type for the webhook. Creating a webhook requires 'read' scope on the resource the webhook is for.
        /// memberships: the Memberships resource
        /// messages: the Messages resource
        /// rooms: the Rooms resource
        /// </summary>
        public string Resource { get; set; }

        /// <summary>
        /// The event type for the webhook.
        /// created: an object was created
        /// updated: an object was updated
        /// deleted: an object was deleted
        /// </summary>
        [JsonProperty("event")]
        public string Sparkevent { get; set; }

        /// <summary>
        /// The filter that defines the webhook scope.
        /// </summary>
        public string Filter { get; set; }

        /// <summary>
        /// The secret used to generate payload signature.
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// The status of the webhook. Use active to reactivate a disabled webhook.
        /// active: the webhook is active
        /// inactive: the webhook is inactive
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// The date and time the webhook was created.
        /// </summary>
        public DateTime Created { get; set; }
    }
}