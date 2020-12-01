using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectComponents.Abstraction;
using UnityEngine;

namespace ApplicationFacade
{
    public class GameObjectData : IDataIdentifier
    {
        public delegate void GameObjectChangedEventHandler( GameObjectData obj, GameObjectDataType type );

        public event GameObjectChangedEventHandler GameObjectDataChanged;

        public Vector3 Position { get; protected set; }

        public Vector3 Rotation { get; protected set; }

        public Vector3 Scale { get; protected set; }
        
        public GameObject Object { get; protected set; }

        protected GameObjectDataType ObjType { get; set; }

        protected long ID { get; set; }

        internal GameObjectData( GameObjectDataType type )
        {
            Position = new Vector3( );
            Rotation = new Vector3( );
            Scale    = new Vector3( );
            Object   = null;
            ID       = 0;
            ObjType = type;
        }

        internal GameObjectData( GameObjectDataType type, long id )
        {
            Position = new Vector3( );
            Rotation = new Vector3( );
            Scale    = new Vector3( );
            Object   = null;
            ID       = id;
            ObjType = type;
        }

        internal GameObjectData( GameObjectDataType type, long id, Vector3 position, Vector3 rotation, Vector3 scale )
        {
            Position = position;
            Rotation = rotation;
            Scale    = scale;
            Object   = null;
            ID       = id;
            ObjType = type;
        }

        internal GameObjectData( GameObjectDataType type, long id, Vector3 position, Vector3 rotation, Vector3 scale, GameObject obj )
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
            Position = position;

            if ( Object != null )
            {
                Object.transform.position = position;
            }

            OnChange( );
        }

        public void SetRotation( Vector3 rotation )
        {
            Rotation = rotation;

            if ( Object != null )
            {
                Object.transform.rotation = Quaternion.Euler( rotation );
            }

            OnChange( );
        }

        public void SetScale( Vector3 scale )
        {
            Scale = scale;

            if ( Object != null )
            {
                Object.transform.localScale = scale;
            }

            OnChange( );
        }

        public void SetTransform( Transform transform )
        {
            Position = transform.position;
            Rotation = transform.rotation.eulerAngles;
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
            Object = obj;
        }

        public void SetID( long id )
        {
            ID = id;
        }

        public long GetID()
        {
            return ID;
        }

        protected virtual void OnChange()
        {
            GameObjectDataChanged?.Invoke( this, ObjType );
        }
    }
}
