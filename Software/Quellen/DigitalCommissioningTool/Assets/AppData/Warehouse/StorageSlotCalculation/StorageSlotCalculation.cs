using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AppData.Warehouse
{
    /// <summary>
    /// Standard Algorithmus fuer das einfache Berechnen von Regalslots.
    /// </summary>
    public class StorageSlotCalculation : SlotCalcBase
    {
        /// <summary>
        /// Der X Abstand der Kisten zum Rand der Ebene.
        /// </summary>
        private float XMargin { get; set; }

        /// <summary>
        /// Der Y Abstand der Kisten zu den Ebenen.
        /// </summary>
        private float YMargin { get; set; }

        /// <summary>
        /// Der Z Abstand zwischen den Kisten.
        /// </summary>
        private float ZInnerMargin { get; set; }

        /// <summary>
        /// Der Z Abstand der Kisten zum Rand der Ebene.
        /// </summary>
        private float ZOuterMargin { get; set; }

        /// <summary>
        /// Erstellt eine neue Instanz mit default Werten.
        /// </summary>
        public StorageSlotCalculation() : base()
        {
            XMargin      = 0.1f;
            ZInnerMargin = 0.1f;
            ZOuterMargin = 0.05f;
            YMargin      = 0.01f;
        }

        /// <summary>
        /// Erstellt eine neue Instanz mit custom Regal werten.
        /// </summary>
        /// <param name="slotCount">Die Anzahl der Slots - sollte ein vielfaches der Ebenen sein.</param>
        /// <param name="layerCount">Due Anzahl der Ebenen.</param>
        /// <param name="layerSize">Die Groesse einer Ebene.</param>
        /// <param name="layerDistance">Der Abstand zwischen den Ebenen.</param>
        public StorageSlotCalculation( int slotCount, int layerCount, Vector3 layerSize, float layerDistance ) : base( slotCount, layerCount, layerSize, layerDistance )
        {
            XMargin = 0.1f;
            ZInnerMargin = 0.1f;
            ZOuterMargin = 0.05f;
            YMargin = 0.01f;
        }

        /// <summary>
        /// Startet den Algorithmus.
        /// </summary>
        public override void StartGeneration()
        {
            SlotPositionData = new Vector3[SlotCount];
            SlotRotationData = new Vector3[SlotCount];
            SlotScaleData    = new Vector3[SlotCount];

            CalculateScale( );
            CalculatePosition();
        }

        /// <summary>
        /// Berechnet die Skalierung der Slots.
        /// </summary>
        private void CalculateScale()
        {
            Vector3 slotScale = new Vector3( LayerSize.x - XMargin * 2,
                                             (LayerDistance / LayerSize.y) * ( 1 - ( LayerDistance - YMargin ) ),
                                             ( LayerSize.z / 2 - ZOuterMargin * 2 - ( (SlotCount / LayerCount) - 1 ) * ZInnerMargin ) / (SlotCount / LayerCount) );

            for( int i = 0; i < SlotCount; i++ )
            {
                SlotScaleData[i] = new Vector3( slotScale.x, slotScale.y, slotScale.z );
            }
        }

        /// <summary>
        /// Berechnet die Positionen der SLots.
        /// </summary>
        private void CalculatePosition()
        {
            for( int i = 0; i < SlotCount; i++ )
            {
                Vector3 slotPosition = new Vector3( -SlotScaleData[i].x / 2,
                                                    ( LayerDistance - YMargin ),
                                                    ( -SlotScaleData[i].z / 2 * (SlotCount / LayerCount) + ZOuterMargin - ZInnerMargin * ( (SlotCount / LayerCount) / 2f ) ) + (i % (SlotCount / LayerCount)) * ZInnerMargin + (i % (SlotCount / LayerCount)) * SlotScaleData[i].z );

                SlotPositionData[i] = new Vector3( slotPosition.x, slotPosition.y, slotPosition.z );
            }
        }
    }
}
