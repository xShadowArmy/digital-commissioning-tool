using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using SystemTools;

namespace ApplicationFacade.Application
{
    /// <summary>
    /// Verwaltet Daten ueber Tastenkombinationen.
    /// </summary>
    public class KeyData : ISerialConfigData
    {
        /// <summary>
        /// Der KeyCode der Tasten.
        /// </summary>
        public KeyCode Code { get; set; }

        /// <summary>
        /// Gibt an ob fuer die Tastenkombination Shift gedrueckt werden muss.
        /// </summary>
        public bool ShiftNeeded { get; set; }
        
        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        /// <param name="code">Der KeyCode der zu drueckenden Taste.</param>
        /// <param name="shiftNeeded">Gibt an ob Shift gedrueckt werden muss.</param>
        public KeyData( KeyCode code, bool shiftNeeded )
        {
            Code = code;
            ShiftNeeded = shiftNeeded;
        }

        /// <summary>
        /// Speichert Daten fuer die Serialisierung.
        /// </summary>
        /// <param name="storage">Objekt fuer die Datenspeicherung.</param>
        public void Serialize( SerialConfigData storage )
        {
            storage.AddData( Code );
            storage.AddData( ShiftNeeded );
        }

        /// <summary>
        /// Stellt die Serialisierten Daten wieder her.
        /// </summary>
        /// <param name="storage">Objekt aus dem die Daten wiederhergestellt werden koennen.</param>
        public void Restore( SerialConfigData storage )
        {
            Code = (KeyCode)Enum.Parse( typeof( KeyCode ), storage.GetValueAsString() );
            ShiftNeeded = storage.GetValueAsBool( );
        }

        /// <summary>
        /// Wandelt die Tastenkombination in einen lesbaren String um.
        /// </summary>
        /// <returns>Gibt die Tastenkombination als String zurueck.</returns>
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
