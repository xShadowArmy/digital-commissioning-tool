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

            set
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

            set
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

            set
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

            set
            {
                Data.ChangeDateModified( value );
            }
        }

        private InternalProjectData Data { get; set; }
               
        public ProjectData()
        {
            Data = new InternalProjectData( );
        }
    }
}
