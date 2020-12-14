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
        protected ObjectTransformation[] WallsNorth { get; set; }
        protected ObjectTransformation[] WallsEast { get; set; }
        protected ObjectTransformation[] WallsSouth { get; set; }
        protected ObjectTransformation[] WallsWest { get; set; }
        protected ObjectTransformation[] WallsInner { get; set; }
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

        public ObjectTransformation[] GetOuterWalls()
        {
            ObjectTransformation[] tmp = new ObjectTransformation[WallsNorth.Length + WallsEast.Length + WallsSouth.Length + WallsWest.Length];

            int j = 0;

            for( int i = 0; i < WallsNorth.Length; i++ )
            {
                tmp[j++] = WallsNorth[i];
            }

            for ( int i = 0; i < WallsEast.Length; i++ )
            {
                tmp[j++] = WallsEast[i];
            }

            for ( int i = 0; i < WallsSouth.Length; i++ )
            {
                tmp[j++] = WallsSouth[i];
            }

            for ( int i = 0; i < WallsWest.Length; i++ )
            {
                tmp[j++] = WallsWest[i];
            }

            return tmp;
        }
         
        public ObjectTransformation[] GetInnerWalls()
        {
            return WallsInner;
        }

        public ObjectTransformation[] GetNorthWalls()
        {
            return WallsNorth;
        }

        public ObjectTransformation[] GetEastWalls()
        {
            return WallsEast;
        }

        public ObjectTransformation[] GetSouthWalls()
        {
            return WallsSouth;
        }

        public ObjectTransformation[] GetWestWalls()
        {
            return WallsWest;
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
