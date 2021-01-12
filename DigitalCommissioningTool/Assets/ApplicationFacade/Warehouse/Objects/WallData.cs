using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectComponents.Abstraction;
using SystemFacade;
using ApplicationFacade.Application;
using UnityEngine;

namespace ApplicationFacade.Warehouse
{
    public class WallData : WallObjectData
    {              
        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        internal WallData() : base( )
        {
        }
        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        /// <param name="id">Die ID des Objekts.</param>
        internal WallData( long id ) : base( id )
        {
        }
        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        /// <param name="id">Die ID des Objekts.</param>
        /// <param name="position">Die Position des Objekts.</param>
        /// <param name="rotation">Die Rotation des Objekts.</param>
        /// <param name="scale">Die Skalierung des Objekts.</param>
        internal WallData( long id, Vector3 position, Quaternion rotation, Vector3 scale ) : base( id, position, rotation, scale )
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
        internal WallData( long id, Vector3 position, Quaternion rotation, Vector3 scale, GameObject obj ) : base( id, position, rotation, scale, obj )
        {
        }

        /// <summary>
        /// Gibt die Laenge des Objekts zurueck.
        /// </summary>
        /// <returns>Die Laenge des Objekts.</returns>
        protected override float GetLength()
        {
            return 1f;
        }

        /// <summary>
        /// Wird aufgerufen wenn sich das Objekt ändert und speichert die Änderungen.
        /// </summary>
        protected override void ObjectChanged()
        {
            LogManager.WriteInfo( "Aktualisiere WallData.", "WallData", "ObjectChanged" );

            for ( int i = 0; i < GameManager.GameWarehouse.Data.Walls.Count; i++ )
            {
                if ( GetID( ) == GameManager.GameWarehouse.Data.Walls[i].ID )
                {
                    GameManager.GameWarehouse.Data.Walls.Remove( GameManager.GameWarehouse.Data.Walls[i] );
                    GameManager.GameWarehouse.Data.Walls.Insert( i, new ProjectWallData( GetID( ), Object.tag, Face.ToString( ), Class.ToString( ), new ProjectTransformationData( Position, Rotation, Scale ) ) );

                    return;
                }
            }
        }
    }
}
