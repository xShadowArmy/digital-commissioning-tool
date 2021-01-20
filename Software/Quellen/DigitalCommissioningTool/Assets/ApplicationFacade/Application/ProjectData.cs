using System;
using ProjectComponents.Abstraction;

namespace ApplicationFacade.Application
{
    /// <summary>
    /// Enthaelt allgemeine Daten ueber ein Projekt.
    /// </summary>
    public class ProjectData
    {
        /// <summary>
        /// Der Name des Projekts.
        /// </summary>
        public string ProjectName
        {
            get
            {
                return Data.Name;
            }

            internal set
            {
                Data.Name = value;
            }
        }

        /// <summary>
        /// Der Pfad des Projekts.
        /// </summary>
        public string ProjectPath
        {
            get
            {
                return Data.FullPath;
            }

            internal set
            {
                Data.FullPath = value;
            }
        }

        /// <summary>
        /// Das Datum an dem das Projekt erstellt wurde.
        /// </summary>
        public DateTime DateCreated
        {
            get
            {
                return Data.DateCreated;
            }

            internal set
            {
                Data.DateCreated = value;
            }
        }

        /// <summary>
        /// Das Datum an dem das Projekt zuletzt geaendert wurde.
        /// </summary>
        public DateTime DateModified
        {
            get
            {
                return Data.DateModified;
            }

            internal set
            {
                Data.DateModified = value;
            }
        }

        /// <summary>
        /// Objekt das von der Facade ueberdeckt wird.
        /// </summary>
        internal InternalProjectData Data { get; private set; }
             
        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        internal ProjectData()
        {
            Data = new InternalProjectData( );
        }
    }
}
