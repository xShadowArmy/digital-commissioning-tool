using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AppData.Warehouse
{
    /// <summary>
    /// Basisklasse fuer die Berechnung der Regal Slots.
    /// </summary>
    public abstract class SlotCalcBase : ISlotCalcStrategie
    {
        /// <summary>
        /// Die Positionen der einzelnen Slots.
        /// </summary>
        protected Vector3[] SlotPositionData { get; set; }

        /// <summary>
        /// Die Rotationen der einzelnen Slots.
        /// </summary>
        protected Vector3[] SlotRotationData { get; set; }

        /// <summary>
        /// Die Skalierung der einzelnen Slots.
        /// </summary>
        protected Vector3[] SlotScaleData { get; set; }

        /// <summary>
        /// Die Anzahl der Slots.
        /// </summary>
        public readonly int SlotCount;

        /// <summary>
        /// Die Anzahl der Ebenen.
        /// </summary>
        public readonly int LayerCount;

        /// <summary>
        /// Die Groesse einer Ebene.
        /// </summary>
        public readonly Vector3 LayerSize;

        /// <summary>
        /// Der Abstand zwischen zwei Ebenen.
        /// </summary>
        public readonly float LayerDistance;

        /// <summary>
        /// Erstellt eine neue Instanz mit den default Werten.
        /// </summary>
        public SlotCalcBase()
        {
            SlotCount  = 20;
            LayerCount = 4;
            LayerSize  = new Vector3( 0.8f, 0.04f, 2f );
            LayerDistance = 0.5f;
        }

        /// <summary>
        /// Erstellt eine neue Instanz mit custom Regalwerten
        /// </summary>
        /// <param name="slotCount">Die Anzahl der zu berechnenden Slots. Sollte ein vielfaches der Ebenen sein</param>
        /// <param name="layerCount">Die Anzahl der Ebenen.</param>
        /// <param name="layerSize">Die Groesse einer Ebene.</param>
        /// <param name="layerDistance">Der Abstand zwischen zwei Ebenen.</param>
        public SlotCalcBase( int slotCount, int layerCount, Vector3 layerSize, float layerDistance )
        {
            SlotCount  = slotCount;
            LayerCount = layerCount;
            LayerSize  = layerSize;
            LayerDistance = layerDistance;
        }

        /// <summary>
        /// Gibt die Positions Daten aller Slots zurueck.
        /// </summary>
        /// <returns>Positionen der Slots.</returns>
        public Vector3[] GetPositionData()
        {
            return SlotPositionData;
        }

        /// <summary>
        /// Gibt die Rotations Daten aller Slots zurueck,
        /// </summary>
        /// <returns>Rotationen der Slots.</returns>
        public Vector3[] GetRotationData()
        {
            return SlotRotationData;
        }

        /// <summary>
        /// Gibt die Skalierungs Daten aller Slots zurueck.
        /// </summary>
        /// <returns>Skalierungen der Slots.</returns>
        public Vector3[] GetScaleData()
        {
            return SlotScaleData;
        }

        /// <summary>
        /// Gibt die Anzahl der berechneten Slots zurueck,
        /// </summary>
        /// <returns>Die Anzahl der Slots.</returns>
        public int GetSlotCount()
        {
            return SlotCount;
        }

        /// <summary>
        /// Gibt die Anzahl der Regak Ebenen zurueck.
        /// </summary>
        /// <returns>Die Anzahl der Ebenen,</returns>
        public int GetLayerCount()
        {
            return LayerCount;
        }

        /// <summary>
        /// Startet den Algorithmus. 
        /// </summary>
        public abstract void StartGeneration();
    }
}
