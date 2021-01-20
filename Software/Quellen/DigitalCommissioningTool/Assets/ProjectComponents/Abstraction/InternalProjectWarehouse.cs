using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;

namespace ProjectComponents.Abstraction
{
    /// <summary>
    /// Stellt das Lagerhaus in der internen Projekt Dateistruktur dar.
    /// </summary>
    public class InternalProjectWarehouse
    {
        /// <summary>
        /// Liste mit allen Böden aus dem Lagerhaus.
        /// </summary>
        public List<ProjectFloorData> Floor { get; private set; }

        /// <summary>
        /// Liste mit allen Wänden aus dem Lagerhaus.
        /// </summary>
        public List<ProjectWallData> Walls { get; private set; }

        /// <summary>
        /// Liste mit allen Fenstern aus dem Lagerhaus.
        /// </summary>
        public List<ProjectWindowData> Windows { get; private set; }

        /// <summary>
        /// Liste mit allen Türen aus dem Lagerhaus.
        /// </summary>
        public List<ProjectDoorData> Doors { get; private set; }

        /// <summary>
        /// Liste mit allen Regalen aus dem Lagerhaus.
        /// </summary>
        public List<ProjectStorageData> StorageRacks { get; private set; }

        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
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
