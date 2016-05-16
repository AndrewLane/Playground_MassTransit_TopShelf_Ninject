using System;

namespace Core.Messages
{
    /// <summary>
    /// Signifies that an application was launched
    /// </summary>
    public interface IAppLaunched
    {
        string WhichApp { get; set; }
        DateTime When { get; set; }
    }
}
