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
        public string Name { get; set; }
        public string FullPath { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateModified { get; set; }
        public static string Extension { get; set; }

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
    }
}
