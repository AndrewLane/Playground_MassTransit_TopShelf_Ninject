using System;

namespace Core.Messages
{
    /// <summary>
    /// Models some event taking place that the system needs to know about
    /// </summary>
    public class SomeEvent : ISomeEvent
    {
        public string What { get; set; }
        public DateTime When { get; set; }
    }
}
