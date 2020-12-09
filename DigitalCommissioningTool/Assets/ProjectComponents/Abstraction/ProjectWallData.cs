using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectComponents.Abstraction
{
    public struct ProjectWallData
    {
        public long ID { get; set; }
        public ProjectTransformationData Transformation { get; set; }

        public ProjectWallData( long id, ProjectTransformationData transformation )
        {
            ID = id;
            Transformation = transformation;
        }        
    }
}
