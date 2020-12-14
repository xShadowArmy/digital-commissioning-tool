using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppData.Warehouse
{
    public interface IWarehouseStrategie 
    {
        void StartGeneration();
         
        ObjectTransformation[] GetFloor();
        ObjectTransformation[] GetOuterWalls();
        ObjectTransformation[] GetInnerWalls();
        ObjectTransformation[] GetNorthWalls();
        ObjectTransformation[] GetEastWalls();
        ObjectTransformation[] GetSouthWalls();
        ObjectTransformation[] GetWestWalls();
        ObjectTransformation[] GetWindows();
        ObjectTransformation[] GetDoors();
        ObjectTransformation[] GetStorageRacks();
    }
}