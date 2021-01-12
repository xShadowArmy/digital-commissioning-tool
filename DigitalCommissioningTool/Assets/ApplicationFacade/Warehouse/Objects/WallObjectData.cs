using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ApplicationFacade.Warehouse
{
    /// <summary>
    /// Abstraktion für Wände, Fenster und Türen.
    /// </summary>
    public abstract class WallObjectData : GameObjectData
    {
        /// <summary>
        /// Ausrichtung des Objekts.
        /// </summary>
        public WallFace Face { get; internal set; }

        /// <summary>
        /// Kategorie des Objekts.
        /// </summary>
        public WallClass Class { get; internal set; }

        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        internal WallObjectData(  ) : base( )
        {
        }

        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        /// <param name="id">Die ID des Objekts.</param>
        internal WallObjectData( long id ) : base( id )
        {
        }

        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        /// <param name="id">Die ID des Objekts.</param>
        /// <param name="position">Die Position des Objekts.</param>
        /// <param name="rotation">Die Rotation des Objekts.</param>
        /// <param name="scale">Die Skalierung des Objekts.</param>
        internal WallObjectData( long id, Vector3 position, Quaternion rotation, Vector3 scale ) : base( id, position, rotation, scale )
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
        internal WallObjectData( long id, Vector3 position, Quaternion rotation, Vector3 scale, GameObject obj ) : base( id, position, rotation, scale, obj )
        {
        }

        /// <summary>
        /// Ändert die Ausrichtung des Objekts.
        /// </summary>
        /// <param name="face"></param>
        public void SetFace( WallFace face )
        {
            Face = face;

            ObjectChanged( );
        }

        /// <summary>
        /// Ändert die Kategorie des Objekts.
        /// </summary>
        /// <param name="wClass"></param>
        public void SetClass( WallClass wClass )
        {
            Class = wClass;

            ObjectChanged( );
        }

        /// <summary>
        /// Gibt die Laenge des Objekts zurueck.
        /// </summary>
        /// <returns>Die Laenge des Objekts.</returns>
        protected abstract float GetLength();

        /// <summary>
        /// Wird aufgerufen wenn sich das Objekt ändert und speichert die Änderungen.
        /// </summary>
        protected abstract override void ObjectChanged();
    }
}
