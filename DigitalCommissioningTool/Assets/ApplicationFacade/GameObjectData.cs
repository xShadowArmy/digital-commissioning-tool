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
        public Vector3 Position { get; protected set; }

        public Vector3 Rotation { get; protected set; }

        public Vector3 Scale { get; protected set; }
        
        public GameObject Object { get; protected set; }

        protected long ID { get; set; }

        internal GameObjectData()
        {
            Position = new Vector3( );
            Rotation = new Vector3( );
            Scale    = new Vector3( );
            Object   = null;
            ID       = 0;
        }

        internal GameObjectData( long id )
        {
            Position = new Vector3( );
            Rotation = new Vector3( );
            Scale    = new Vector3( );
            Object   = null;
            ID       = id;
        }

        internal GameObjectData( long id, Vector3 position, Vector3 rotation, Vector3 scale )
        {
            Position = position;
            Rotation = rotation;
            Scale    = scale;
            Object   = null;
            ID       = id;
        }

        internal GameObjectData( long id, Vector3 position, Vector3 rotation, Vector3 scale, GameObject obj )
        {
            Position = position;
            Rotation = rotation;
            Scale    = scale;
            Object   = obj;
            ID       = id;
        }

        public void SetPosition( Vector3 position )
        {
            Position = position;
        }

        public void SetRotation( Vector3 rotation )
        {
            Rotation = rotation;
        }

        public void SetScale( Vector3 scale )
        {
            Scale = scale;
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
    }
}
