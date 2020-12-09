using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectComponents.Abstraction;
using SystemFacade;
using UnityEngine;

namespace ApplicationFacade
{
    public class GameObjectData : IDataIdentifier
    {
        public delegate void GameObjectChangedEventHandler( GameObjectData obj, GameObjectDataType type );

        public event GameObjectChangedEventHandler GameObjectDataChanged;

        public Vector3 Position { get; protected set; }

        public Quaternion Rotation { get; protected set; }

        public Vector3 Scale { get; protected set; }
        
        public GameObject Object { get; protected set; }

        internal bool Destroyed { get; set; }

        protected GameObjectDataType ObjType { get; set; }

        protected long ID { get; set; }

        internal GameObjectData( GameObjectDataType type )
        {
            Position = new Vector3( );
            Rotation = new Quaternion( );
            Scale    = new Vector3( );
            Object   = null;
            ID       = 0;
            ObjType = type;
        }

        internal GameObjectData( GameObjectDataType type, long id )
        {
            Position = new Vector3( );
            Rotation = new Quaternion( );
            Scale    = new Vector3( );
            Object   = null;
            ID       = id;
            ObjType = type;
        }

        internal GameObjectData( GameObjectDataType type, long id, Vector3 position, Quaternion rotation, Vector3 scale )
        {
            Position = position;
            Rotation = rotation;
            Scale    = scale;
            Object   = null;
            ID       = id;
            ObjType = type;
        }

        internal GameObjectData( GameObjectDataType type, long id, Vector3 position, Quaternion rotation, Vector3 scale, GameObject obj )
        {
            Position = position;
            Rotation = rotation;
            Scale    = scale;
            Object   = obj;
            ID       = id;
            ObjType = type;
        }

        public void SetPosition( Vector3 position )
        {
            if ( Destroyed )
            {
                LogManager.WriteWarning( "Es wird auf ein Objekt zugegriffen das bereits Zerstört ist!", "GameObjectData", "SetPosition" );
                Debug.LogWarning( "Es wird auf ein Objekt zugegriffen das bereits Zerstört ist!" );

                return;
            }

            Position = position;

            if ( Object != null )
            {
                Object.transform.position = position;
            }

            OnChange( );
        }

        public void SetRotation( Quaternion rotation )
        {
            if ( Destroyed )
            {
                LogManager.WriteWarning( "Es wird auf ein Objekt zugegriffen das bereits Zerstört ist!", "GameObjectData", "SetRotation" );
                Debug.LogWarning( "Es wird auf ein Objekt zugegriffen das bereits Zerstört ist!" );

                return;
            }

            Rotation = rotation;

            if ( Object != null )
            {
                Object.transform.rotation = rotation;
            }

            OnChange( );
        }

        public void SetScale( Vector3 scale )
        {
            if ( Destroyed )
            {
                LogManager.WriteWarning( "Es wird auf ein Objekt zugegriffen das bereits Zerstört ist!", "GameObjectData", "SetScale" );
                Debug.LogWarning( "Es wird auf ein Objekt zugegriffen das bereits Zerstört ist!" );

                return;
            }

            Scale = scale;

            if ( Object != null )
            {
                Object.transform.localScale = scale;
            }

            OnChange( );
        }

        public void SetTransform( Transform transform )
        {
            if ( Destroyed )
            {
                LogManager.WriteWarning( "Es wird auf ein Objekt zugegriffen das bereits Zerstört ist!", "GameObjectData", "SetTransform" );
                Debug.LogWarning( "Es wird auf ein Objekt zugegriffen das bereits Zerstört ist!" );

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

            OnChange( );
        }

        public void ChangeGameObject( GameObject obj )
        {
            if ( Destroyed )
            {
                LogManager.WriteWarning( "Es wird auf ein Objekt zugegriffen das bereits Zerstört ist!", "GameObjectData", "ChangeGameObject" );
                Debug.LogWarning( "Es wird auf ein Objekt zugegriffen das bereits Zerstört ist!" );

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
            if ( Destroyed )
            {
                LogManager.WriteWarning( "Es wird auf ein Objekt zugegriffen das bereits Zerstört ist!", "GameObjectData", "SetID" );
                Debug.LogWarning( "Es wird auf ein Objekt zugegriffen das bereits Zerstört ist!" );

                return;
            }

            ID = id;
        }

        public long GetID()
        {
            if ( Destroyed )
            {
                LogManager.WriteWarning( "Es wird auf ein Objekt zugegriffen das bereits Zerstört ist!", "GameObjectData", "GetID" );
                Debug.LogWarning( "Es wird auf ein Objekt zugegriffen das bereits Zerstört ist!" );

                return 0;
            }

            return ID;
        }

        protected virtual void OnChange()
        {
            GameObjectDataChanged?.Invoke( this, ObjType );
        }
    }
}
