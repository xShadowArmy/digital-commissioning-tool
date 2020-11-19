using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectComponents.Abstraction;

namespace ApplicationFacade
{
    public class FloorData
    {
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

        private ProjectFloorData Data { get; set; }

        public FloorData( TransformationData transformation )
        {
            Data = new ProjectFloorData( new ProjectTransformationData( transformation.Position, transformation.Rotation, transformation.Scale ) );
        }
    }
}
