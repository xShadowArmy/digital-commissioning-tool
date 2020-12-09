using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Warehouse
{
    public abstract class WarehouseStrategieBase : IWarehouseStrategie
    {
        protected ObjectTransformation[] Floor { get; set; }
        protected ObjectTransformation[] Walls { get; set; }
        protected ObjectTransformation[] Windows { get; set; }
        protected ObjectTransformation[] Doors { get; set; }
        protected ObjectTransformation[] StorageRacks { get; set; }

        protected readonly float xOffset;
        protected readonly float yOffset;
        protected readonly float zOffset;

        public WarehouseStrategieBase()
        {
            xOffset = -20f;
            yOffset = 1.6f;
            zOffset = -15f;
        }

        public WarehouseStrategieBase( float xOff, float yOff, float zOff )
        {
            xOffset = xOff;
            yOffset = yOff;
            zOffset = zOff;
        }

        public abstract void StartGeneration();

        public ObjectTransformation[] GetFloor()
        {
            return Floor;
        }

        public ObjectTransformation[] GetWalls()
        {
            return Walls;
        }

        public ObjectTransformation[] GetWindows()
        {
            return Windows;
        }

        public ObjectTransformation[] GetDoors()
        {
            return Doors;
        }

        public ObjectTransformation[] GetStorageRacks()
        {
            return StorageRacks;
        }
    }
}
