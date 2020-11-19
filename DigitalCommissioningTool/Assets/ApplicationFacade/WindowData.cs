using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectComponents.Abstraction;

namespace ApplicationFacade
{
    public struct WindowData
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

        private ProjectWindowData Data { get; set; }

        public WindowData( long id, TransformationData transformation )
        {
            Data = new ProjectWindowData( id, new ProjectTransformationData( transformation.Position, transformation.Rotation, transformation.Scale ) );
        }
    }
}
