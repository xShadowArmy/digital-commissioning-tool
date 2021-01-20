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
    /// Stellt einen Boden in der Umgebung dar.
    /// </summary>
    public class FloorData : GameObjectData
    {
        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        internal FloorData() : base( )
        {
        }

        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        /// <param name="id">Die ID des Objekts.</param>
        internal FloorData( long id ) : base( id )
        {
        }

        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        /// <param name="id">Die ID des Objekts.</param>
        /// <param name="position">Die Position des Objekts.</param>
        /// <param name="rotation">Die Rotation des Objekts.</param>
        /// <param name="scale">Die Skalierung des Objekts.</param>
        internal FloorData( long id, Vector3 position, Quaternion rotation, Vector3 scale ) : base( id, position, rotation, scale )
        {
        }

        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        /// <param name="id">Die ID des Objekts.</param>
        /// <param name="position">Die Position des Objekts.</param>
        /// <param name="rotation">Die Rotation des Objekts.</param>
        /// <param name="scale">Die Skalierung des Objekts.</param>
        /// <param name="obj">Das GameObjekt das repräsentiert wird.</param>
        internal FloorData( long id, Vector3 position, Quaternion rotation, Vector3 scale, GameObject obj ) : base( id, position, rotation, scale, obj )
        {
        }

        /// <summary>
        /// Wird aufgerufen wenn sich das Objekt ändert und speichert die Änderungen.
        /// </summary>
        protected override void ObjectChanged( )
        {
            LogManager.WriteInfo( "[Event]Aktualisiere FloorData.", "Warehouse", "FloorHasChanged" );

            for ( int i = 0; i < GameManager.GameWarehouse.Data.Floor.Count; i++ )
            {
                if ( GetID( ) == GameManager.GameWarehouse.Data.Floor[i].ID )
                {
                    GameManager.GameWarehouse.Data.Floor.Remove( GameManager.GameWarehouse.Data.Floor[i] );
                    GameManager.GameWarehouse.Data.Floor.Insert( i, new ProjectFloorData( GetID( ), new ProjectTransformationData( Position, Rotation, Scale ) ) );

                    return;
                }
            }
        }
    }
}
