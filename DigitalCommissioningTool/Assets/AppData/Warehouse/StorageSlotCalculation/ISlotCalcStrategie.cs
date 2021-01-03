using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AppData.Warehouse
{
    public interface ISlotCalcStrategie
    {
        void StartGeneration();

        Vector3[] GetPositionData();
        Vector3[] GetRotationData();
        Vector3[] GetScaleData();
        int GetSlotCount();
        int GetLayerCount();
    }
}
