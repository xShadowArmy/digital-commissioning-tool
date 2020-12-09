using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectComponents.Abstraction
{
    public struct ProjectFloorData
    {
        public long ID { get; set; }
        public ProjectTransformationData Transformation { get; private set; }

        public ProjectFloorData( long id, ProjectTransformationData transformation )
        {
            Transformation = transformation;
            ID = id;
        }
    }
}
