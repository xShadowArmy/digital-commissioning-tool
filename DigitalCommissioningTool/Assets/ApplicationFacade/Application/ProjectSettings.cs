using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectComponents.Abstraction;

namespace ApplicationFacade.Application
{
    public class ProjectSettings
    {
        internal InternalProjectSettings Data { get; private set; }

        public ProjectSettings()
        {
            Data = new InternalProjectSettings( );
        }
    }
}
