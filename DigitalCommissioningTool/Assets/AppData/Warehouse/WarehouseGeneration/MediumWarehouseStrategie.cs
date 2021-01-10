using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AppData.Warehouse
{
    /// <summary>
    /// Algorithmus fuer die Generierung eines mittelgroessen Lagerhauses.
    /// </summary>
    public class MediumWarehouseStrategie : WarehouseStrategieBase
    {
        /// <summary>
        /// Erstellt eine neue Instanz.
        /// </summary>
        public MediumWarehouseStrategie()
        {
        }

        /// <summary>
        /// Berechnet die Boeden.
        /// </summary>
        public void GenerateFloor()
        {
            int j = 0;

            Floor[j++] = new ObjectTransformation( new Vector3( -10.0f, 0, 0.5f ), Quaternion.Euler( 0, 90, 0 ), new Vector3( 3, 1f, 2.2f ) );
            Floor[j]   = new ObjectTransformation( new Vector3( 10.0f, 0, 0.5f ), Quaternion.Euler( 0, 90, 0 ), new Vector3( 3, 1f, 2.2f ) );
        }

        /// <summary>
        /// Berechnet die Waende.
        /// </summary>
        public void GenerateWall()
        {
            Vector3 wallScale = new Vector3( 1f, 3.2f, 0.2f );
            Quaternion nsWallRotation = Quaternion.Euler( 0f, 90f, 0f );
            Quaternion ewWallRotation = Quaternion.Euler( 0f, 0f, 0f );
            
            // NorthSouth wall
            for ( int i = 0; i < WallsNorth.Length; i++ )
            {
                WallsNorth[i] = new ObjectTransformation( new Vector3( xOffset + 0f, yOffset + 0f, zOffset + 1.2f + i ), nsWallRotation, wallScale );
                WallsSouth[i] = new ObjectTransformation( new Vector3( xOffset + 40.2f, yOffset + 0f, zOffset + 30.2f - i ), nsWallRotation, wallScale );
            }

            // EastWest wall
            for ( int i = 0; i < WallsEast.Length; i++ )
            {
                WallsEast[i] = new ObjectTransformation( new Vector3( xOffset + 0.6f + i, yOffset + 0f, zOffset + 30.8f ), ewWallRotation, wallScale );
                WallsWest[i] = new ObjectTransformation( new Vector3( xOffset + 39.6f - i, yOffset + 0f, zOffset + 0.6f ), ewWallRotation, wallScale );
            }
        }

        /// <summary>
        /// Berechnet die Tueren.
        /// </summary>
        public void GenerateDoors()
        {
        }

        /// <summary>
        /// Berechnet die Regale.
        /// </summary>
        public void GenerateStorageRacks()
        {
        }

        /// <summary>
        /// Berechnet die Fenster.
        /// </summary>
        public void GenerateWindows()
        {
        }

        /// <summary>
        /// Startet den Algorithmus.
        /// </summary>
        public override void StartGeneration()
        {
            Floor = new ObjectTransformation[2];
            WallsNorth = new ObjectTransformation[30];
            WallsEast = new ObjectTransformation[40];
            WallsSouth = new ObjectTransformation[30];
            WallsWest = new ObjectTransformation[40];
            WallsInner = new ObjectTransformation[0];
            Windows = new ObjectTransformation[0];
            Doors = new ObjectTransformation[0];
            StorageRacks = new ObjectTransformation[0];

            GenerateFloor( );
            GenerateWall( );
            GenerateWindows( );
            GenerateDoors( );
            GenerateStorageRacks( );
        }
    }
}
