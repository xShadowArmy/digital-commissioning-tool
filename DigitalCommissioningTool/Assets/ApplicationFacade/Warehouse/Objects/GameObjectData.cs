using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectComponents.Abstraction;
using SystemFacade;
using UnityEngine;

namespace ApplicationFacade.Warehouse
{
    /// <summary>
    /// Stellt ein Objekt in der Umgebung dar.
    /// </summary>
    public abstract class GameObjectData : IDataIdentifier
    {
        /// <summary>
        /// Die Position des Objekts.
        /// </summary>
        public Vector3 Position { get; internal set; }

        /// <summary>
        /// Die Rotation des Objekts.
        /// </summary>
        public Quaternion Rotation { get; internal set; }

        /// <summary>
        /// Die Skalierung des Objekts.
        /// </summary>
        public Vector3 Scale { get; internal set; }
        
        /// <summary>
        /// Das GameObjekt das repräsentiert wird.
        /// </summary>
        public GameObject Object { get; internal set; }

        /// <summary>
        /// Gibt an ob das Objekt zerstört wurde.
        /// </summary>
        public bool Destroyed { get; private set; }

        /// <summary>
        /// Gibt an ob das Objekt Schreibgeschützt ist.
        /// </summary>
        public bool Readonly { get; internal set; }

        /// <summary>
        /// Die ID des Objekts.
        /// </summary>
        protected long ID { get; set; }

        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        internal GameObjectData( )
        {
            Position = new Vector3( );
            Rotation = new Quaternion( );
            Scale    = new Vector3( );
            Object   = null;
            ID       = 0;
        }

        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        /// <param name="id">Die ID des Objekts.</param>
        internal GameObjectData( long id )
        {
            Position = new Vector3( );
            Rotation = new Quaternion( );
            Scale    = new Vector3( );
            Object   = null;
            ID       = id;
        }

        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        /// <param name="id">Die ID des Objekts.</param>
        /// <param name="position">Die Position des Objekts.</param>
        /// <param name="rotation">Die Rotation des Objekts.</param>
        /// <param name="scale">Die Skalierung des Objekts.</param>
        internal GameObjectData( long id, Vector3 position, Quaternion rotation, Vector3 scale )
        {
            Position = position;
            Rotation = rotation;
            Scale    = scale;
            Object   = null;
            ID       = id;
        }

        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        /// <param name="id">Die ID des Objekts.</param>
        /// <param name="position">Die Position des Objekts.</param>
        /// <param name="rotation">Die Rotation des Objekts.</param>
        /// <param name="scale">Die Skalierung des Objekts.</param>
        /// <param name="obj">Das GameObjekt das repräsentiert wird.</param>
        internal GameObjectData( long id, Vector3 position, Quaternion rotation, Vector3 scale, GameObject obj )
        {
            Position = position;
            Rotation = rotation;
            Scale    = scale;
            Object   = obj;
            ID       = id;
        }

        ~GameObjectData()
        {
            Destroy( );
        }

        /// <summary>
        /// Ändert den Tag des GameObjects.
        /// </summary>
        /// <param name="tag">Der neue Tag.</param>
        public void SetTag( string tag )
        {
            if ( IsDestroyed( ) )
            {
                return;
            }

            if ( IsReadonly( ) )
            {
                return;
            }

            if ( Object != null )
            {
                Object.tag = tag;
            }

            ObjectChanged( );
        }

        /// <summary>
        /// Ändert die Position des GameObjects.
        /// </summary>
        /// <param name="position">Die neue Position.</param>
        public void SetPosition( Vector3 position )
        {
            if ( IsDestroyed( ) )
            {
                return;
            }

            if ( IsReadonly() )
            {
                return;
            }

            Position = position;

            if ( Object != null )
            {
                Object.transform.position = position;
            }

            ObjectChanged( );
        }

        /// <summary>
        /// Ändert die Rotation des GameObjects.
        /// </summary>
        /// <param name="rotation">Die Rotation des Objekts.</param>
        public void SetRotation( Quaternion rotation )
        {
            if ( IsDestroyed( ) )
            {
                return;
            }

            if ( IsReadonly( ) )
            {
                return;
            }

            Rotation = rotation;

            if ( Object != null )
            {
                Object.transform.rotation = rotation;
            }

            ObjectChanged( );
        }

        /// <summary>
        /// Ändert die Skalierung des GameObjekcs.
        /// </summary>
        /// <param name="scale">Die neue Skalierung.</param>
        public void SetScale( Vector3 scale )
        {
            if ( IsDestroyed( ) )
            {
                return;
            }

            if ( IsReadonly( ) )
            {
                return;
            }

            Scale = scale;

            if ( Object != null )
            {
                Object.transform.localScale = scale;
            }

            ObjectChanged( );
        }

        /// <summary>
        /// Ändert die Transformationsdaten des GameObjects.
        /// </summary>
        /// <param name="transform">Die neuen Transformationsdaten.</param>
        public void SetTransform( Transform transform )
        {
            if ( IsDestroyed( ) )
            {
                return;
            }

            if ( IsReadonly( ) )
            {
                return;
            }

            Position = transform.position;
            Rotation = transform.rotation;
            Scale    = transform.localScale;

            if ( Object != null )
            {
                Object.transform.rotation   = transform.rotation;
                Object.transform.position   = transform.position;
                Object.transform.localScale = transform.localScale;
            }

            ObjectChanged( );
        }

        /// <summary>
        /// Änder das GameObject.
        /// </summary>
        /// <param name="obj">Das neue GameObject.</param>
        public void ChangeGameObject( GameObject obj )
        {
            if ( IsDestroyed( ) )
            {
                return;
            }

            if ( IsReadonly( ) )
            {
                return;
            }

            Object = obj;
        }
        
        /// <summary>
        /// Zerstört das GameObject.
        /// </summary>
        public void Destroy()
        {
            if ( Destroyed )
            {
                return;
            }

            Destroyed = true;

            if ( Object != null )
            {
                GameObject.Destroy( Object );
                Object = null;
            }
        }

        /// <summary>
        /// Speichert eine neue ID.
        /// </summary>
        /// <param name="id">Die neue ID.</param>
        public void SetID( long id )
        {
            if ( IsDestroyed( ) )
            {
                return;
            }

            ID = id;
        }

        /// <summary>
        /// Gibt die gespeicherte ID zurück.
        /// </summary>
        /// <returns>Die gespeicherte ID.</returns>
        public long GetID()
        {
            if ( IsDestroyed( ) )
            {
                return 0;
            }

            return ID;
        }

        /// <summary>
        /// Wird aufgerufen wenn das Objekt verändert wurde.
        /// </summary>
        protected abstract void ObjectChanged();

        /// <summary>
        /// Überprüft ob das Objekt schreibgeschützt ist.
        /// </summary>
        /// <returns>True wenn Schreibgeschützt.</returns>
        protected bool IsReadonly()
        {
            if ( Destroyed )
            {
                LogManager.WriteWarning( "Es soll ein Objekt geändert werden das Readonly ist!", "GameObjectData", "IsReadonly" );
                Debug.LogWarning( "Es soll ein Objekt geändert werden das Readonly ist!" );

                return true;
            }

            return false;
        }

        /// <summary>
        /// Überprüft ob das Objekt bereits Zerstört wurde.
        /// </summary>
        /// <returns>True wenn das Objekt bereits Zerstört ist.</returns>
        protected bool IsDestroyed()
        {
            if ( Destroyed )
            {
                LogManager.WriteWarning( "Es wird auf ein Objekt zugegriffen das bereits Zerstört ist!", "GameObjectData", "IsDestroyed" );
                Debug.LogWarning( "Es wird auf ein Objekt zugegriffen das bereits Zerstört ist!" );

                return true;
            }

            return false;
        }
    }
}
