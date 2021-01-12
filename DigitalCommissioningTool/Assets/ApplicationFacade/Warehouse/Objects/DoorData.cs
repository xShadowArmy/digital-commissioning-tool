using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationFacade.Application;
using ProjectComponents.Abstraction;
using SystemFacade;
using UnityEngine;

namespace ApplicationFacade.Warehouse
{
    /// <summary>
    /// Stellt eine Tür in der Umgebung dar.
    /// </summary>
    public class DoorData : WallObjectData
    {
        /// <summary>
        /// Die Türart.
        /// </summary>
        public DoorType Type { get; internal set; }

        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        internal DoorData() : base( )
        {
            Type = DoorType.Door;
        }

        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        /// <param name="id">Die ID des Objekts.</param>
        internal DoorData( long id ) : base( id )
        {
            Type = DoorType.Door;
        }

        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        /// <param name="id">Die ID des Objekts.</param>
        /// <param name="position">Die Position des Objekts.</param>
        /// <param name="rotation">Die Rotation des Objekts.</param>
        /// <param name="scale">Die Skalierung des Objekts.</param>
        internal DoorData( long id, Vector3 position, Quaternion rotation, Vector3 scale ) : base( id, position, rotation, scale )
        {
            Type = DoorType.Door;
        }

        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        /// <param name="id">Die ID des Objekts.</param>
        /// <param name="position">Die Position des Objekts.</param>
        /// <param name="rotation">Die Rotation des Objekts.</param>
        /// <param name="scale">Die Skalierung des Objekts.</param>
        /// <param name="obj">Das GameObjekt das repräsentiert wird.</param>
        internal DoorData( long id, Vector3 position, Quaternion rotation, Vector3 scale, GameObject obj ) : base( id, position, rotation, scale, obj )
        {
            Type = DoorType.Door;
        }

        /// <summary>
        /// Wird aufgerufen wenn sich das Objekt ändert und speichert die Änderungen.
        /// </summary>
        protected override void ObjectChanged( )
        {
            LogManager.WriteInfo( "Aktualisiere DoorData.", "DoorData", "ObjectChanged" );

            for ( int i = 0; i < GameManager.GameWarehouse.Data.Doors.Count; i++ )
            {
                if ( GetID( ) == GameManager.GameWarehouse.Data.Doors[i].ID )
                {
                    GameManager.GameWarehouse.Data.Doors.Remove( GameManager.GameWarehouse.Data.Doors[i] );
                    GameManager.GameWarehouse.Data.Doors.Insert( i, new ProjectDoorData( GetID( ), Face.ToString(), Class.ToString(), Type.ToString( ), new ProjectTransformationData( Position, Rotation, Scale ) ) );

                    return;
                }
            }
        }
    }
}
