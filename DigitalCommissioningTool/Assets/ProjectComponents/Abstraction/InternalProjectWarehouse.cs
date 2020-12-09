using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;

namespace ProjectComponents.Abstraction
{
    public class InternalProjectWarehouse
    {
        public List<ProjectFloorData> Floor { get; private set; }

        public List<ProjectWallData> Walls { get; private set; }

        public List<ProjectWindowData> Windows { get; private set; }

        public List<ProjectDoorData> Doors { get; private set; }

        public List<ProjectStorageData> StorageRacks { get; private set; }

        public InternalProjectWarehouse()
        {
            Floor = new List<ProjectFloorData>( );

            Walls = new List<ProjectWallData>( );

            Windows = new List<ProjectWindowData>( );

            Doors = new List<ProjectDoorData>( );

            StorageRacks = new List<ProjectStorageData>( );
        }
    }
}
