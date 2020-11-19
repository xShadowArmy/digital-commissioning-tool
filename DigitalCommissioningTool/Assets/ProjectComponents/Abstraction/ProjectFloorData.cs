using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectComponents.Abstraction
{
    public struct ProjectFloorData
    {
        public ProjectTransformationData Transformation { get; set; }

        public ProjectFloorData( ProjectTransformationData transformation )
        {
            Transformation = transformation;
        }

        public void SetTransformation( ProjectTransformationData transformation )
        {
            Transformation = transformation;
        }
    }
}
