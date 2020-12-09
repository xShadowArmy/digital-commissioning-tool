using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppData.Warehouse
{
    public interface IWarehouseStrategie 
    {
        void StartGeneration();

        ObjectTransformation[] GetFloor();
        ObjectTransformation[] GetWalls();
        ObjectTransformation[] GetWindows();
        ObjectTransformation[] GetDoors();
        ObjectTransformation[] GetStorageRacks();
    }
}