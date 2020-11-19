using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectComponents.Abstraction
{
    public struct ProjectDoorData
    {
        public long ID { get; private set; }
        public string Type { get; private set; }
        public ProjectTransformationData Transformation { get; private set; }

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
