namespace Core
{
    /// <summary>
    /// Models how a particular application interacts with messages.  Are they a consumer of messages, or a producer only?
    /// </summary>
    public enum MessagingPreference
    {
        /// <summary>
        /// Only publishes messages and will never consume them
        /// </summary>
        PublisherOnly,

        /// <summary>
        /// Has at least 1 consumer
        /// </summary>
        Consumer
    }
}
