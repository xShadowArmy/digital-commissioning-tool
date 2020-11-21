using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectComponents.Abstraction;
using UnityEngine;

namespace ApplicationFacade
{
    public class WallData : GameObjectData
    {
        internal WallData() : base( )
        {
        }

        internal WallData( long id ) : base( id )
        {
        }

        internal WallData( long id, Vector3 position, Vector3 rotation, Vector3 scale ) : base( id, position, rotation, scale )
        {
        }

        internal WallData( long id, Vector3 position, Vector3 rotation, Vector3 scale, GameObject obj ) : base( id, position, rotation, scale, obj )
        {
        }
    }
}
