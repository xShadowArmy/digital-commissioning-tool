using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace AppData.Warehouse
{
    public class SmallWarehouseStrategie : WarehouseStrategieBase
    {
        public SmallWarehouseStrategie()
        {
        }

        public void GenerateFloor()
        {
            int j = 0;

            Floor[j++] = new ObjectTransformation( new Vector3( -10.0f, 0, 0.5f ), Quaternion.Euler( 0, 90, 0 ), new Vector3( 3, 1f, 2.2f ) );
            Floor[j]   = new ObjectTransformation( new Vector3( 10.0f, 0, 0.5f ), Quaternion.Euler( 0, 90, 0 ), new Vector3( 3, 1f, 2.2f ) );
        }

        public void GenerateWall()
        {
            Vector3 wallScale = new Vector3( 1f, 3.2f, 0.2f );
            Quaternion nsWallRotation = Quaternion.Euler( 0f, 90f, 0f );
            Quaternion ewWallRotation = Quaternion.Euler( 0f, 0f, 0f );
            
            // NorthSouth wall
            for ( int i = 0; i < WallsNorth.Length; i++ )
            {
                WallsNorth[i] = new ObjectTransformation( new Vector3( xOffset + 0f, yOffset + 0f, zOffset + 1f + i ), nsWallRotation, wallScale );
                WallsSouth[i] = new ObjectTransformation( new Vector3( xOffset + 30f, yOffset + 0f, zOffset + 20.2f - i ), nsWallRotation, wallScale );
            }

            // EastWest wall
            for ( int i = 0; i < WallsEast.Length; i++ )
            {
                WallsEast[i] = new ObjectTransformation( new Vector3( xOffset + 0.4f + i, yOffset + 0f, zOffset + 20.6f ), ewWallRotation, wallScale );
                WallsWest[i] = new ObjectTransformation( new Vector3( xOffset + 29.6f - i, yOffset + 0f, zOffset + 0.6f ), ewWallRotation, wallScale );
            }
        }

        public void GenerateDoors()
        {
        }

        public void GenerateStorageRacks()
        {
        }
        
        public void GenerateWindows()
        {
        }

        public override void StartGeneration()
        {
            Floor = new ObjectTransformation[2];
            WallsNorth = new ObjectTransformation[20];
            WallsEast = new ObjectTransformation[30];
            WallsSouth = new ObjectTransformation[20];
            WallsWest = new ObjectTransformation[30];
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
