using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectComponents.Abstraction;

namespace ApplicationFacade
{
    public struct DoorData
    {
        public long ID
        {
            get
            {
                return Data.ID;
            }

            set
            {
                Data.SetID( value );
            }
        }

        public string Type
        {
            get
            {
                return Data.Type;
            }

            set
            {
                Data.SetType( value );
            }
        }
        
        public TransformationData Transformation
        {
            get
            {
                return new TransformationData( Data.Transformation.Position, Data.Transformation.Rotation, Data.Transformation.Scale );
            }

            set
            {
                Data.SetTransformation( new ProjectTransformationData( value.Position, value.Rotation, value.Scale ) );
            }
        }

        private ProjectDoorData Data { get; set; }
        
        public DoorData( long id, string type, TransformationData transformation )
        {
            Data = new ProjectDoorData( id, type, new ProjectTransformationData( transformation.Position, transformation.Rotation, transformation.Scale) );
        }
    }
}
