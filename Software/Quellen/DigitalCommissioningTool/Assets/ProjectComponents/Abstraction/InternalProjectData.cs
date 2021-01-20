using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemFacade;

namespace ProjectComponents.Abstraction
{
    /// <summary>
    /// Stellt die Interne Xml Dateistruktur für allgemeine Projektdaten dar.
    /// </summary>
    public class InternalProjectData
    {
        /// <summary>
        /// Der Name des Projekts.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Der Pfad des Projekts.
        /// </summary>
        public string FullPath { get; set; }

        /// <summary>
        /// Das Datum an dem das Projekt erstellt wurde.
        /// </summary>
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Das Datum an dem das Projekt zuletzt geändert wurde.
        /// </summary>
        public DateTime DateModified { get; set; }

        /// <summary>
        /// Die Datei Extension für Projekte (".prj").
        /// </summary>
        public static string Extension { get; set; }

        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        public InternalProjectData()
        {
            Name = StringResourceManager.LoadString( "@DefaultProjectName" );
            Extension = ".prj";
            FullPath = Paths.ProjectsPath;
            DateCreated  = DateTime.Now;
            DateModified = DateTime.Now;
        }
    }
}
