using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AppData.Warehouse
{
    public class StorageSlotCalculation : SlotCalcBase
    {
        private float XMargin { get; set; }
        private float YMargin { get; set; }
        private float ZInnerMargin { get; set; }
        private float ZOuterMargin { get; set; }

        public StorageSlotCalculation() : base()
        {
            XMargin      = 0.1f;
            ZInnerMargin = 0.1f;
            ZOuterMargin = 0.05f;
            YMargin      = 0.01f;
        }

        public StorageSlotCalculation( int slotCount, int layerCount, Vector3 layerSize, float layerDistance ) : base( slotCount, layerCount, layerSize, layerDistance )
        {
            XMargin = 0.1f;
            ZInnerMargin = 0.1f;
            ZOuterMargin = 0.05f;
            YMargin = 0.01f;
        }

        public override void StartGeneration()
        {
            SlotPositionData = new Vector3[SlotCount];
            SlotRotationData = new Vector3[SlotCount];
            SlotScaleData    = new Vector3[SlotCount];

            CalculateScale( );
            CalculatePosition();
        }

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
