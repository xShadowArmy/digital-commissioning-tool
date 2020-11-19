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
        public ProjectFloorData Floor { get; private set; }

        public List<ProjectWallData> Walls { get; private set; }

        public List<ProjectWindowData> Windows { get; private set; }

        public List<ProjectDoorData> Doors { get; private set; }

        public List<ProjectStorageData> StorageRecks { get; private set; }

        public InternalProjectWarehouse()
        {
            Floor = new ProjectFloorData( );

            Walls = new List<ProjectWallData>( );

            Windows = new List<ProjectWindowData>( );

            Doors = new List<ProjectDoorData>( );

            StorageRecks = new List<ProjectStorageData>( );
        }

        public void SetDefaultValues()
        {

        }

        public void UpdateFloor( ProjectFloorData data )
        {
            Floor = data;
        }

        public void AddWall( ProjectWallData data )
        {
            Walls.Add( data );
        }

        public bool RemoveWall( ProjectWallData data )
        {
            return Walls.Remove( data );
        }

        public void AddWindow( ProjectWindowData data )
        {
            Windows.Add( data );
        }

        public bool RemoveWindow( ProjectWindowData data )
        {
            return Windows.Remove( data );
        }

        public void AddDoor( ProjectDoorData data )
        {
            Doors.Add( data );
        }

        public bool RemoveDoor( ProjectDoorData data )
        {
            return Doors.Remove( data );
        }

        public void AddStorageReck( ProjectStorageData data )
        {
            StorageRecks.Add( data );
        }

        public bool RemoveStorageReck( ProjectStorageData data )
        {
            return StorageRecks.Remove( data );
        }
    }
}
