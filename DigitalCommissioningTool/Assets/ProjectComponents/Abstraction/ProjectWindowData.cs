using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectComponents.Abstraction
{
    public struct ProjectWindowData
    {
        public long ID { get; set; }
        public ProjectTransformationData Transformation { get; set; }
        
        public ProjectWindowData( long id, ProjectTransformationData transformation )
        {
            ID = id;
            Transformation = transformation;
        }

        public void SetID( long id )
        {
            ID = id;
        }

        public void SetTransformation( ProjectTransformationData transformation )
        {
            Transformation = transformation;
        }
    }
}
