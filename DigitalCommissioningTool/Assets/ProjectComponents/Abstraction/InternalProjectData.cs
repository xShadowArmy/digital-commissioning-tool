using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemFacade;

namespace ProjectComponents.Abstraction
{
    public class InternalProjectData
    {
        public string Name { get; private set; }
        public string FullPath { get; private set; }
        public DateTime DateCreated { get; private set; }
        public DateTime DateModified { get; private set; }
        public static string Extension { get; private set; }

        public InternalProjectData()
        {
            if ( !StringResourceManager.Exists( "@DefaultProjectName" ) )
            {
                StringResourceManager.StoreString( "DefaultProjectName", "NeuesProject" );
            }

            Name = Name.LoadString( "@DefaultProjectName" );
            Extension = ".prj";
            FullPath = Paths.ProjectsPath;
            DateCreated  = DateTime.Now;
            DateModified = DateTime.Now;
        }

        public void ChangeProjectName( string newName )
        {
            LogManager.WriteInfo( "Aendere Projektname von \"" + Name + "\" zu \"" + newName + "\"", "InternalProjectData", "ChangeProjectName" );

            if ( newName == null || newName == string.Empty )
            {
                LogManager.WriteWarning( "Ungueltiger Projektname!", "InternalProjectData", "ChangeProjectName" );

                return;
            }

            Name = newName;
        }

        public void ChangeProjectPath( string newPath )
        {
            LogManager.WriteInfo( "Aendere Projektpfad von \"" + FullPath + "\" zu \"" + newPath + Name + Extension + "\"", "InternalProjectData", "ChangeProjectPath" );

            if ( newPath == null || newPath == string.Empty )
            {
                LogManager.WriteWarning( "Ungueltiger Projektpfad!", "InternalProjectData", "ChangeProjectPath" );

                return;
            }

            FullPath = newPath;
        }

        public void ChangeDateCreated( DateTime date )
        {
            LogManager.WriteInfo( "Aendere Projekterstellungsdatum von \"" + DateCreated + "\" zu \"" + date.ToShortDateString() + "\"", "InternalProjectData", "ChangeDateCreated" );

            if ( date == null )
            {
                LogManager.WriteWarning( "Ungueltiges Projekterstellungsdatum!", "InternalProjectData", "ChangeDateCreated" );

                return;
            }

            DateCreated = date;
        }

        public void ChangeDateModified( DateTime date )
        {
            LogManager.WriteInfo( "Aendere Projektaenderungsdatum von \"" + DateCreated + "\" zu \"" + date.ToShortDateString( ) + "\"", "InternalProjectData", "ChangeDateModified" );

            if ( date == null )
            {
                LogManager.WriteWarning( "Ungueltiges Projektaenderungsdatum!", "InternalProjectData", "ChangeDateModified" );

                return;
            }

            DateModified = date;
        }
    }
}
