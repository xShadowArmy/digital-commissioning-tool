using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectComponents.Abstraction
{
    public struct ProjectDoorData
    {
        public long ID { get; set; }
        public string Type { get; set; }
        public string Face { get; set; }
        public string Class { get; set; }

        public ProjectTransformationData Transformation { get; private set; }

        public ProjectDoorData( long id, string face, string wClass, string type, ProjectTransformationData transformation )
        {
            ID    = id;
            Face  = face;
            Class = wClass;
            Type  = type;
            Transformation = transformation;
        }
    }
}
