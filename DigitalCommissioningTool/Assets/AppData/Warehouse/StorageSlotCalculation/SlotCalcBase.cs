using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AppData.Warehouse
{
    public abstract class SlotCalcBase : ISlotCalcStrategie
    {
        protected Vector3[] SlotPositionData { get; set; }

        protected Vector3[] SlotRotationData { get; set; }

        protected Vector3[] SlotScaleData { get; set; }

        public readonly int SlotCount;

        public readonly int LayerCount;

        public readonly Vector3 LayerSize;

        public readonly float LayerDistance;

        public SlotCalcBase()
        {
            SlotCount  = 20;
            LayerCount = 4;
            LayerSize  = new Vector3( 0.8f, 0.04f, 2f );
            LayerDistance = 0.5f;
        }

        public SlotCalcBase( int slotCount, int layerCount, Vector3 layerSize, float layerDistance )
        {
            SlotCount  = slotCount;
            LayerCount = layerCount;
            LayerSize  = layerSize;
            LayerDistance = layerDistance;
        }

        public Vector3[] GetPositionData()
        {
            return SlotPositionData;
        }

        public Vector3[] GetRotationData()
        {
            return SlotRotationData;
        }

        public Vector3[] GetScaleData()
        {
            return SlotScaleData;
        }

        public int GetSlotCount()
        {
            return SlotCount;
        }

        public int GetLayerCount()
        {
            return LayerCount;
        }

        public abstract void StartGeneration();
    }
}
