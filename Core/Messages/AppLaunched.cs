using System;

namespace Core.Messages
{
    /// <summary>
    /// Signifies that an application was launched
    /// </summary>
    public class AppLaunched : IAppLaunched
    {
        public string WhichApp { get; set; }
        public DateTime When { get; set; }
    }
}
