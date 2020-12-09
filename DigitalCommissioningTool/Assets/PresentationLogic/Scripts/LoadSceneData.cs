using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ApplicationFacade;

public class LoadSceneData : MonoBehaviour
{
    void Start()
    {
        //GameManager.CreateProject( "SmallWarehouse", WarehouseSize.Small );
        //GameManager.CreateProject( "MediumWarehouse", WarehouseSize.Medium );
        //GameManager.CreateProject( "LargeWarehouse", WarehouseSize.Large );

        GameManager.LoadProject( "LargeWarehouse" );

        //GameManager.GameWarehouse.CreateWall( new Vector3( -25 + 0f, 1.6f, -17 + 1f ),  new Quaternion( 0,0,0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 0f, 1.6f, -15 + 2f ),  new Vector3( 0, 90, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 0f, 1.6f, -15 + 3f ),  new Vector3( 0, 90, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 0f, 1.6f, -15 + 4f ),  new Vector3( 0, 90, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 0f, 1.6f, -15 + 5f ),  new Vector3( 0, 90, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 0f, 1.6f, -15 + 6f ),  new Vector3( 0, 90, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 0f, 1.6f, -15 + 7f ),  new Vector3( 0, 90, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 0f, 1.6f, -15 + 8f ),  new Vector3( 0, 90, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 0f, 1.6f, -15 + 9f ),  new Vector3( 0, 90, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 0f, 1.6f, -15 + 10f ), new Vector3( 0, 90, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 0f, 1.6f, -15 + 11f ), new Vector3( 0, 90, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 0f, 1.6f, -15 + 12f ), new Vector3( 0, 90, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 0f, 1.6f, -15 + 13f ), new Vector3( 0, 90, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 0f, 1.6f, -15 + 14f ), new Vector3( 0, 90, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 0f, 1.6f, -15 + 15f ), new Vector3( 0, 90, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 0f, 1.6f, -15 + 16f ), new Vector3( 0, 90, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 0f, 1.6f, -15 + 17f ), new Vector3( 0, 90, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 0f, 1.6f, -15 + 18f ), new Vector3( 0, 90, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 0f, 1.6f, -15 + 19f ), new Vector3( 0, 90, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 0f, 1.6f, -15 + 20f ), new Vector3( 0, 90, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 0f, 1.6f, -15 + 21f ), new Vector3( 0, 90, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 0f, 1.6f, -15 + 22f ), new Vector3( 0, 90, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 0f, 1.6f, -15 + 23f ), new Vector3( 0, 90, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 0f, 1.6f, -15 + 24f ), new Vector3( 0, 90, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 0f, 1.6f, -15 + 25f ), new Vector3( 0, 90, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 0f, 1.6f, -15 + 26f ), new Vector3( 0, 90, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 0f, 1.6f, -15 + 27f ), new Vector3( 0, 90, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 0f, 1.6f, -15 + 28f ), new Vector3( 0, 90, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 0f, 1.6f, -15 + 29f ), new Vector3( 0, 90, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 0f, 1.6f, -15 + 30f ), new Vector3( 0, 90, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );

        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 0.4f,  1.6f, -15 + 30.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 1.4f,  1.6f, -15 + 30.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 2.4f,  1.6f, -15 + 30.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 3.4f,  1.6f, -15 + 30.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 4.4f,  1.6f, -15 + 30.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 5.4f,  1.6f, -15 + 30.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 6.4f,  1.6f, -15 + 30.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 7.4f,  1.6f, -15 + 30.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 8.4f,  1.6f, -15 + 30.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 9.4f,  1.6f, -15 + 30.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 10.4f, 1.6f, -15 + 30.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 11.4f, 1.6f, -15 + 30.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 12.4f, 1.6f, -15 + 30.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 13.4f, 1.6f, -15 + 30.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 14.4f, 1.6f, -15 + 30.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 15.4f, 1.6f, -15 + 30.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 16.4f, 1.6f, -15 + 30.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 17.4f, 1.6f, -15 + 30.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 18.4f, 1.6f, -15 + 30.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 19.4f, 1.6f, -15 + 30.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 20.4f, 1.6f, -15 + 30.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 21.4f, 1.6f, -15 + 30.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 22.4f, 1.6f, -15 + 30.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 23.4f, 1.6f, -15 + 30.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 24.4f, 1.6f, -15 + 30.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 25.4f, 1.6f, -15 + 30.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 26.4f, 1.6f, -15 + 30.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 27.4f, 1.6f, -15 + 30.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 28.4f, 1.6f, -15 + 30.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 29.4f, 1.6f, -15 + 30.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 30.4f, 1.6f, -15 + 30.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 31.4f, 1.6f, -15 + 30.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 32.4f, 1.6f, -15 + 30.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 33.4f, 1.6f, -15 + 30.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 34.4f, 1.6f, -15 + 30.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 35.4f, 1.6f, -15 + 30.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 36.4f, 1.6f, -15 + 30.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 37.4f, 1.6f, -15 + 30.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 38.4f, 1.6f, -15 + 30.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 39.4f, 1.6f, -15 + 30.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );

        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 40f, 1.6f, -15 + 30.2f ), new Vector3( 0, 90, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 40f, 1.6f, -15 + 29.2f ), new Vector3( 0, 90, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 40f, 1.6f, -15 + 28.2f ), new Vector3( 0, 90, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 40f, 1.6f, -15 + 27.2f ), new Vector3( 0, 90, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 40f, 1.6f, -15 + 26.2f ), new Vector3( 0, 90, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 40f, 1.6f, -15 + 25.2f ), new Vector3( 0, 90, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 40f, 1.6f, -15 + 24.2f ), new Vector3( 0, 90, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 40f, 1.6f, -15 + 23.2f ), new Vector3( 0, 90, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 40f, 1.6f, -15 + 22.2f ), new Vector3( 0, 90, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 40f, 1.6f, -15 + 21.2f ), new Vector3( 0, 90, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 40f, 1.6f, -15 + 20.2f ), new Vector3( 0, 90, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 40f, 1.6f, -15 + 19.2f ), new Vector3( 0, 90, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 40f, 1.6f, -15 + 18.2f ), new Vector3( 0, 90, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 40f, 1.6f, -15 + 17.2f ), new Vector3( 0, 90, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 40f, 1.6f, -15 + 16.2f ), new Vector3( 0, 90, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 40f, 1.6f, -15 + 15.2f ), new Vector3( 0, 90, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 40f, 1.6f, -15 + 14.2f ), new Vector3( 0, 90, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 40f, 1.6f, -15 + 13.2f ), new Vector3( 0, 90, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 40f, 1.6f, -15 + 12.2f ), new Vector3( 0, 90, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 40f, 1.6f, -15 + 11.2f ), new Vector3( 0, 90, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 40f, 1.6f, -15 + 10.2f ), new Vector3( 0, 90, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 40f, 1.6f, -15 +  9.2f ), new Vector3( 0, 90, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 40f, 1.6f, -15 +  8.2f ), new Vector3( 0, 90, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 40f, 1.6f, -15 +  7.2f ), new Vector3( 0, 90, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 40f, 1.6f, -15 +  6.2f ), new Vector3( 0, 90, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 40f, 1.6f, -15 +  5.2f ), new Vector3( 0, 90, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 40f, 1.6f, -15 +  4.2f ), new Vector3( 0, 90, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 40f, 1.6f, -15 +  3.2f ), new Vector3( 0, 90, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 40f, 1.6f, -15 +  2.2f ), new Vector3( 0, 90, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 40f, 1.6f, -15 +  1.2f ), new Vector3( 0, 90, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );

        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 39.6f, 1.6f, -15 + 0.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 38.6f, 1.6f, -15 + 0.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 37.6f, 1.6f, -15 + 0.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 36.6f, 1.6f, -15 + 0.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 35.6f, 1.6f, -15 + 0.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 34.6f, 1.6f, -15 + 0.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 33.6f, 1.6f, -15 + 0.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 32.6f, 1.6f, -15 + 0.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 31.6f, 1.6f, -15 + 0.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 30.6f, 1.6f, -15 + 0.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 29.6f, 1.6f, -15 + 0.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 28.6f, 1.6f, -15 + 0.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 27.6f, 1.6f, -15 + 0.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 26.6f, 1.6f, -15 + 0.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 25.6f, 1.6f, -15 + 0.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 24.6f, 1.6f, -15 + 0.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 23.6f, 1.6f, -15 + 0.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 22.6f, 1.6f, -15 + 0.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 21.6f, 1.6f, -15 + 0.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 20.6f, 1.6f, -15 + 0.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 19.6f, 1.6f, -15 + 0.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 18.6f, 1.6f, -15 + 0.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 17.6f, 1.6f, -15 + 0.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 16.6f, 1.6f, -15 + 0.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 15.6f, 1.6f, -15 + 0.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 14.6f, 1.6f, -15 + 0.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 13.6f, 1.6f, -15 + 0.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 12.6f, 1.6f, -15 + 0.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 11.6f, 1.6f, -15 + 0.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 10.6f, 1.6f, -15 + 0.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 9.6f,  1.6f, -15 + 0.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 8.6f,  1.6f, -15 + 0.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 7.6f,  1.6f, -15 + 0.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 6.6f,  1.6f, -15 + 0.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 5.6f,  1.6f, -15 + 0.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 4.6f,  1.6f, -15 + 0.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 3.6f,  1.6f, -15 + 0.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 2.6f,  1.6f, -15 + 0.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 1.6f,  1.6f, -15 + 0.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //GameManager.GameWarehouse.CreateWall( new Vector3( -20 + 0.6f,  1.6f, -15 + 0.6f ), new Vector3( 0, 0, 0 ), new Vector3( 1f, 3.2f, 0.2f ) );
        //
        //GameManager.GameWarehouse.CreateFloor( new Vector3( -10.0f, 0, 0.5f ), new Vector3( 0, 90, 0 ), new Vector3( 3, 1f, 2.2f ) );
        //GameManager.GameWarehouse.CreateFloor( new Vector3( 10.0f, 0, 0.5f ), new Vector3( 0, 90, 0 ), new Vector3( 3, 1f, 2.2f ) );
       //GameManager.SaveProject( "LargeWarehouse" );
    }
    
    void Update()
    {
        
    }
}
