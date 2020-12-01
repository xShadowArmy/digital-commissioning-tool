using System;
using ProjectComponents.Abstraction;

namespace ApplicationFacade
{
    public class ProjectData
    {
        public string ProjectName
        {
            get
            {
                return Data.Name;
            }

            internal set
            {
                Data.ChangeProjectName( value );
            }
        }

        public string ProjectPath
        {
            get
            {
                return Data.FullPath;
            }

            internal set
            {
                Data.ChangeProjectPath( value );
            }
        }

        public DateTime DateCreated
        {
            get
            {
                return Data.DateCreated;
            }

            internal set
            {
                Data.ChangeDateCreated( value );
            }
        }

        public DateTime DateModified
        {
            get
            {
                return Data.DateModified;
            }

            internal set
            {
                Data.ChangeDateModified( value );
            }
        }

        internal InternalProjectData Data { get; private set; }
               
        internal ProjectData()
        {
            Data = new InternalProjectData( );
        }
    }
}
