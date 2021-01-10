using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectComponents.Abstraction;

namespace ApplicationFacade.Application
{
    /// <summary>
    /// Facade Objekt mit allen Projekteinstellungen.
    /// </summary>
    public class ProjectSettings
    {
        /// <summary>
        /// Objekt das überdeckt wird.
        /// </summary>
        internal InternalProjectSettings Data { get; private set; }

        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        public ProjectSettings()
        {
            Data = new InternalProjectSettings( );
        }
    }
}
