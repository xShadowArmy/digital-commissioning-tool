using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectComponents.Abstraction
{
    public struct ProjectItemData
    {
        public long IDRef { get; set; }
        public ProjectTransformationData Transformation { get; set; }

        public ProjectItemData( long idRef, ProjectTransformationData transformation )
        {
            IDRef = idRef;
            Transformation = transformation;
        }

        public void SetIDRef( long idRef )
        {
            IDRef = idRef;
        }

        public void SetTransformation( ProjectTransformationData transformation )
        {
            Transformation = transformation;
        }
    }
}
