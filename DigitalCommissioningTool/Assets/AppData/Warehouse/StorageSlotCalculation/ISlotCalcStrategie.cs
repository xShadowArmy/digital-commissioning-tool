using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AppData.Warehouse
{
    /// <summary>
    /// Strategie fuer den Slot algorithmus.
    /// </summary>
    public interface ISlotCalcStrategie
    {
        /// <summary>
        /// Startet den Algorithmus. 
        /// </summary>
        void StartGeneration();
        
        /// <summary>
        /// Gibt die Positions Daten aller Slots zurueck.
        /// </summary>
        /// <returns>Positionen der Slots.</returns>
        Vector3[] GetPositionData();

        /// <summary>
        /// Gibt die Rotations Daten aller Slots zurueck,
        /// </summary>
        /// <returns>Rotationen der Slots.</returns>
        Vector3[] GetRotationData();

        /// <summary>
        /// Gibt die Skalierungs Daten aller Slots zurueck.
        /// </summary>
        /// <returns>Skalierungen der Slots.</returns>
        Vector3[] GetScaleData();

        /// <summary>
        /// Gibt die Anzahl der berechneten Slots zurueck,
        /// </summary>
        /// <returns>Die Anzahl der Slots.</returns>
        int GetSlotCount();

        /// <summary>
        /// Gibt die Anzahl der Regak Ebenen zurueck.
        /// </summary>
        /// <returns>Die Anzahl der Ebenen,</returns>
        int GetLayerCount();
    }
}
