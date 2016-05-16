using System;

namespace Core.Messages
{
    /// <summary>
    /// Models some event taking place that the system needs to know about
    /// </summary>
    public interface ISomeEvent
    {
        string What { get; set; }
        DateTime When { get; set; }
    }
}
