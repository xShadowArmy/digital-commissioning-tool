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
        public ProjectTransformationData Size { get; private set; }

        public List<ProjectWallData> Walls { get; private set; }

        public List<ProjectWindowData> Windows { get; private set; }

        public List<ProjectDoorData> Doors { get; private set; }

        public List<ProjectStorageData> StorageRecks { get; private set; }

        public InternalProjectWarehouse()
        {
            Size = new ProjectTransformationData( );

            Walls = new List<ProjectWallData>( );

            Windows = new List<ProjectWindowData>( );

            Doors = new List<ProjectDoorData>( );

            StorageRecks = new List<ProjectStorageData>( );
        }

        public void SetDefaultValues()
        {

        }
    }
}
