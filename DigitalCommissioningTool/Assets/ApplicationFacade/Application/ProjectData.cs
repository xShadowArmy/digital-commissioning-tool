using System;
using ProjectComponents.Abstraction;

namespace ApplicationFacade.Application
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
                Data.Name = value;
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
                Data.FullPath = value;
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
                Data.DateCreated = value;
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
                Data.DateModified = value;
            }
        }

        internal InternalProjectData Data { get; private set; }
               
        internal ProjectData()
        {
            Data = new InternalProjectData( );
        }
    }
}
