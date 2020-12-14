using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using SystemTools;

namespace ApplicationFacade
{
    public struct KeyData : ISerialConfigData
    {
        public KeyCode Code { get; set; }
        public bool ShiftNeeded { get; set; }

        public KeyData( KeyCode code, bool shiftNeeded )
        {
            Code = code;
            ShiftNeeded = shiftNeeded;
        }

        public void Serialize( SerialConfigData storage )
        {
            storage.AddData( Code );
            storage.AddData( ShiftNeeded );
        }

        public void Restore( SerialConfigData storage )
        {
            Code = (KeyCode)Enum.Parse( typeof( KeyCode ), storage.GetValueAsString() );
            ShiftNeeded = storage.GetValueAsBool( );
        }

        public override string ToString()
        {
            string keyString = "";
            if (this.ShiftNeeded)
            {
                keyString = "Shift + ";
            }
            return keyString += this.Code.ToString();
        }
    }
}
