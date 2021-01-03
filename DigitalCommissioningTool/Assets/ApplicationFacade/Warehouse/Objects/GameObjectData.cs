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
    public abstract class GameObjectData : IDataIdentifier
    {
        public Vector3 Position { get; internal set; }

        public Quaternion Rotation { get; internal set; }

        public Vector3 Scale { get; internal set; }
        
        public GameObject Object { get; internal set; }

        public bool Destroyed { get; private set; }

        public bool Readonly { get; internal set; }

        protected long ID { get; set; }

        internal GameObjectData( )
        {
            Position = new Vector3( );
            Rotation = new Quaternion( );
            Scale    = new Vector3( );
            Object   = null;
            ID       = 0;
        }

        internal GameObjectData( long id )
        {
            Position = new Vector3( );
            Rotation = new Quaternion( );
            Scale    = new Vector3( );
            Object   = null;
            ID       = id;
        }

        internal GameObjectData( long id, Vector3 position, Quaternion rotation, Vector3 scale )
        {
            Position = position;
            Rotation = rotation;
            Scale    = scale;
            Object   = null;
            ID       = id;
        }

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
            }
        }

        public void SetID( long id )
        {
            if ( IsDestroyed( ) )
            {
                return;
            }

            ID = id;
        }

        public long GetID()
        {
            if ( IsDestroyed( ) )
            {
                return 0;
            }

            return ID;
        }

        protected abstract void ObjectChanged();

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
