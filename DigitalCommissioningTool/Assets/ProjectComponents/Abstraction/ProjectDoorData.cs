using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectComponents.Abstraction
{
    public class ProjectDoorData
    {
        public long ID { get; set; }
        public string Type { get; set; }
        public ProjectTransformationData Transformation { get; set; }

        public ProjectDoorData( long id, string type, ProjectTransformationData transformation )
        {
            ID = id;
            Type = type;
            Transformation = transformation;
        }

        public void SetID( long id )
        {
            ID = id;
        }

        public void SetType( string type )
        {
            if ( !type.Equals( "door" ) && !type.Equals( "gate" ) )
            {
                return;
            }

            Type = type;
        }

        public void SetTransformation( ProjectTransformationData transformation )
        {
            Transformation = transformation;
        }
    }
}
