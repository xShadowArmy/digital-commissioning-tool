using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemFacade
{
    /// <summary>
    /// Definiert die Priorität einer Log Nachricht.
    /// </summary>
    public enum LogLevel : int
    {
        Info    = 1,
        Warning = 2,
        Error   = 3
    }
}
