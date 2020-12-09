using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ProjectComponents.Abstraction
{
    public class InternalProjectContainer
    {
        public List<ProjectStorageData> Container { get; private set; }

        public InternalProjectContainer()
        {
            Container = new List<ProjectStorageData>( );
        }
    }
}
