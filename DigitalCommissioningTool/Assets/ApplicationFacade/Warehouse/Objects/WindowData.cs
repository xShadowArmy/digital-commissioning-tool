using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectComponents.Abstraction;
using SystemFacade;
using UnityEngine;
using ApplicationFacade.Application;

namespace ApplicationFacade.Warehouse
{
    /// <summary>
    /// Stellt ein Fenster in der Umgebung dar.
    /// </summary>
    public class WindowData : WallObjectData
    {
        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        internal WindowData() : base( )
        {
        }

        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        /// <param name="id">Die ID des Objekts.</param>
        internal WindowData( long id ) : base( id )
        {
        }

        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        /// <param name="id">Die ID des Objekts.</param>
        /// <param name="position">Die Position des Objekts.</param>
        /// <param name="rotation">Die Rotation des Objekts.</param>
        /// <param name="scale">Die Skalierung des Objekts.</param>
        internal WindowData( long id, Vector3 position, Quaternion rotation, Vector3 scale ) : base( id, position, rotation, scale )
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
        internal WindowData( long id, Vector3 position, Quaternion rotation, Vector3 scale, GameObject obj ) : base( id, position, rotation, scale, obj )
        {
        }

        /// <summary>
        /// Gibt die Laenge des Objekts zurueck.
        /// </summary>
        /// <returns>Die Laenge des Objekts.</returns>
        protected override float GetLength()
        {
            return 2f;
        }

        /// <summary>
        /// Wird aufgerufen wenn sich das Objekt ändert und speichert die Änderungen.
        /// </summary>
        protected override void ObjectChanged( )
        {
            LogManager.WriteInfo( "Aktualisiere WindowData.", "WindowData", "ObjectChanged" );

            for ( int i = 0; i < GameManager.GameWarehouse.Data.Windows.Count; i++ )
            {
                if ( GetID( ) == GameManager.GameWarehouse.Data.Windows[i].ID )
                {
                    GameManager.GameWarehouse.Data.Windows.Remove( GameManager.GameWarehouse.Data.Windows[i] );
                    GameManager.GameWarehouse.Data.Windows.Add( new ProjectWindowData( GetID( ), Face.ToString( ), Class.ToString( ), new ProjectTransformationData( Position, Rotation, Scale ) ) );

                    return;
                }
            }
        }
    }
}
