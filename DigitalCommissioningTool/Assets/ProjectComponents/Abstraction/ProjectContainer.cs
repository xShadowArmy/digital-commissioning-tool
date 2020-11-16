using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ProjectComponents.Abstraction
{
    public class ProjectContainer
    {
        public List<ProjectStorageData> StorageRecks { get; private set; }

        public ProjectContainer()
        {
            StorageRecks = new List<ProjectStorageData>( );
        }
    }
}
