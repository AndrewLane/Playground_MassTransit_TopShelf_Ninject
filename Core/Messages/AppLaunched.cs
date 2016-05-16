using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
