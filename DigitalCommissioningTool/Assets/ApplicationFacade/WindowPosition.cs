using System.Collections;
using System.Collections.Generic;
using SystemTools;
using UnityEngine;

namespace ApplicationFacade
{
    [System.Serializable]
    public class WindowPosition : ISerialConfigData
    {
        public Vector3 Position { get; set; }
        public WindowPosition() 
        {
            Position = new Vector3();
        }
        public WindowPosition(Vector3 position)
        {
            this.Position = position;
        }
        public void Restore(SerialConfigData storage)
        {
            Position = new Vector3(storage.GetValueAsFloat(), storage.GetValueAsFloat(), storage.GetValueAsFloat());
        }

        public void Serialize(SerialConfigData storage)
        {
            storage.AddData(Position.x);
            storage.AddData(Position.y);
            storage.AddData(Position.z);
        }
    }
}
