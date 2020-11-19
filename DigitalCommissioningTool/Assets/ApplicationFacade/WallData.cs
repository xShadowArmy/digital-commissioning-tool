using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectComponents.Abstraction;

namespace ApplicationFacade
{
    public class WallData
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


        private ProjectWallData Data { get; set; }

        public WallData( long id, ProjectTransformationData transformation )
        {
        }
    }
}
