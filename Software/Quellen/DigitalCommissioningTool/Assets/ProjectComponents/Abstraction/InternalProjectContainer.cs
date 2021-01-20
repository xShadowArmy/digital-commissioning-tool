using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ProjectComponents.Abstraction
{
    /// <summary>
    /// Stellt die Xml Dateistruktur für die Container eines Projekts dar.
    /// </summary>
    public class InternalProjectContainer
    {
        /// <summary>
        /// Eine Liste mit allen gespeicherten Containern.
        /// </summary>
        public List<ProjectStorageData> Container { get; private set; }

        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        public InternalProjectContainer()
        {
            Container = new List<ProjectStorageData>( );
        }
    }
}
