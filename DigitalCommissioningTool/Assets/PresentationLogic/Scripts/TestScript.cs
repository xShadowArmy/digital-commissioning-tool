using UnityEngine;
using SystemFacade;
using ApplicationFacade;
using System;

namespace Scripts
{
    public class TestScript : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            TestCreate( );
            TestAddData( );
        }

        public void TestCreate()
        {
            GameManager.CreateProject( "Project123" );

            GameManager.GameWarehouse.CreateWall( new Vector3( 1f, 2f, 3f ), new Vector3( 4f, 5f, 6f ), new Vector3( 7f, 8f, 9f ) );
            GameManager.GameWarehouse.CreateDoor( new Vector3( 1f, 2f, 3f ), new Vector3( 4f, 5f, 6f ), new Vector3( 77f, 88f, 99f ), DoorType.Gate );
            GameManager.GameWarehouse.CreateStorageReck( new Vector3( 11f, 222f, 333f ), new Vector3( 4f, 5f, 6f ), new Vector3( 7f, 8f, 9f ) );
            StorageData data0 = GameManager.GameWarehouse.CreateStorageReck( new Vector3( 1f, 2f, 3f ), new Vector3( 433f, 555f, 666f ), new Vector3( 7f, 8f, 9f ) );
            GameManager.GameWarehouse.CreateStorageReckItem( new Vector3( 0f, 0f, 0f ), new Vector3( 0f, 0f, 0f ), new Vector3( 0f, 0f, 0f ), data0 );

            StorageData data = GameManager.GameContainer.CreateContainer( new Vector3( 0f, 0f, 0f ), new Vector3( 0f, 0f, 0f ), new Vector3( 0f, 0f, 0f ) );
            GameManager.GameContainer.CreateContainerItem( new Vector3( 0f, 0f, 0f ), new Vector3( 0f, 0f, 0f ), new Vector3( 0f, 0f, 0f ), data );

            GameManager.SaveProject( "Project123" );
            GameManager.CloseProject( );
        }

        public void TestAddData()
        {
            GameManager.OpenProject( "Project123" );

            GameManager.GameWarehouse.AdjustFloor( new Vector3( 1f, 2.1f, 3f ), new Vector3( 0f, 0f, 0f ), new Vector3( 100f, 1f, 100f ) );
            GameManager.GameWarehouse.CreateWindow( new Vector3( 1f, 8.345f, 3f ), new Vector3( 4f, 5f, 6f ), new Vector3( 7f, 8f, 9f ) );
            GameManager.GameWarehouse.CreateWindow( new Vector3( 1f, 8.345f, 3f ), new Vector3( 4f, 5f, 6f ), new Vector3( 7f, 8f, 9f ) );
            GameManager.GameWarehouse.CreateWindow( new Vector3( 1f, 8.345f, 3f ), new Vector3( 4f, 5f, 6f ), new Vector3( 7f, 8f, 9f ) );
            GameManager.GameWarehouse.CreateWindow( new Vector3( 1f, 8.345f, 3f ), new Vector3( 4f, 5f, 6f ), new Vector3( 7f, 8f, 9f ) );
            GameManager.GameWarehouse.CreateWindow( new Vector3( 1f, 8.345f, 3f ), new Vector3( 4f, 5f, 6f ), new Vector3( 7f, 8f, 9f ) );
            GameManager.GameWarehouse.CreateWindow( new Vector3( 1f, 8.345f, 3f ), new Vector3( 4f, 5f, 6f ), new Vector3( 7f, 8f, 9f ) );
            GameManager.GameWarehouse.CreateWindow( new Vector3( 1f, 8.345f, 3f ), new Vector3( 4f, 5f, 6f ), new Vector3( 7f, 8f, 9f ) );
            GameManager.GameWarehouse.CreateWindow( new Vector3( 1f, 8.345f, 3f ), new Vector3( 4f, 5f, 6f ), new Vector3( 7f, 8f, 9f ) );
            GameManager.GameWarehouse.CreateWindow( new Vector3( 1f, 8.345f, 3f ), new Vector3( 4f, 5f, 6f ), new Vector3( 7f, 8f, 9f ) );
            GameManager.GameWarehouse.CreateDoor( new Vector3( 1f, 2f, 3f ), new Vector3( 4f, 5f, 6f ), new Vector3( 77f, 88f, 99f ), DoorType.Door );
            GameManager.GameWarehouse.CreateStorageReck( new Vector3( 11f, 222f, 333f ), new Vector3( 4f, 0.4f, 6f ), new Vector3( 7f, 8f, 9f ) );
            GameManager.GameWarehouse.CreateStorageReck( new Vector3( 11f, 222f, 333f ), new Vector3( 4f, 0.4f, 6f ), new Vector3( 7f, 8f, 9f ) );
            GameManager.GameWarehouse.CreateStorageReck( new Vector3( 11f, 222f, 333f ), new Vector3( 4f, 0.4f, 6f ), new Vector3( 7f, 8f, 9f ) );
            GameManager.GameWarehouse.CreateStorageReck( new Vector3( 11f, 222f, 333f ), new Vector3( 4f, 0.4f, 6f ), new Vector3( 7f, 8f, 9f ) );
            GameManager.GameWarehouse.CreateStorageReck( new Vector3( 11f, 222f, 333f ), new Vector3( 4f, 0.4f, 6f ), new Vector3( 7f, 8f, 9f ) );
            GameManager.GameWarehouse.CreateStorageReck( new Vector3( 11f, 222f, 333f ), new Vector3( 4f, 0.4f, 6f ), new Vector3( 7f, 8f, 9f ) );
            GameManager.GameWarehouse.CreateStorageReck( new Vector3( 11f, 222f, 333f ), new Vector3( 4f, 0.4f, 6f ), new Vector3( 7f, 8f, 9f ) );
            GameManager.GameWarehouse.CreateWall( new Vector3( 1f, 2.1f, 3f ), new Vector3( 433f, 555f, 666f ), new Vector3( 7f, 8f, 9f ) );
            GameManager.GameWarehouse.CreateWall( new Vector3( 1f, 2.1f, 3f ), new Vector3( 433f, 555f, 666f ), new Vector3( 7f, 8f, 9f ) );
            GameManager.GameWarehouse.CreateWall( new Vector3( 1f, 2.1f, 3f ), new Vector3( 433f, 555f, 666f ), new Vector3( 7f, 8f, 9f ) );
            GameManager.GameWarehouse.CreateWall( new Vector3( 1f, 2.1f, 3f ), new Vector3( 433f, 555f, 666f ), new Vector3( 7f, 8f, 9f ) );
            GameManager.GameWarehouse.CreateWall( new Vector3( 1f, 2.1f, 3f ), new Vector3( 433f, 555f, 666f ), new Vector3( 7f, 8f, 9f ) );
            GameManager.GameWarehouse.CreateWall( new Vector3( 1f, 2.1f, 3f ), new Vector3( 433f, 555f, 666f ), new Vector3( 7f, 8f, 9f ) );
            GameManager.GameWarehouse.CreateWall( new Vector3( 1f, 2.1f, 3f ), new Vector3( 433f, 555f, 666f ), new Vector3( 7f, 8f, 9f ) );
            GameManager.GameWarehouse.CreateWall( new Vector3( 1f, 2.1f, 3f ), new Vector3( 433f, 555f, 666f ), new Vector3( 7f, 8f, 9f ) );
            GameManager.GameContainer.CreateContainer( new Vector3( 0f, 0f, 1f ), new Vector3( 0f, 0f, 0f ), new Vector3( 0f, 1f, 0f ) );
            GameManager.GameContainer.CreateContainer( new Vector3( 0f, 0f, 0f ), new Vector3( 0f, 1f, 0f ), new Vector3( 0f, 0f, 0f ) );

            GameManager.SaveProject( "Project123" );
            GameManager.CloseProject( );
        }

        public void TestLoadStore()
        {
            GameManager.OpenProject( "ExampleProject" );

            Debug.Log( "Floor Position: " + GameManager.GameWarehouse.Floor.Position );
            Debug.Log( "Floor Rotation: " + GameManager.GameWarehouse.Floor.Rotation );
            Debug.Log( "Floor Scale:    " + GameManager.GameWarehouse.Floor.Scale );

            Debug.Log( "Wall2 Position: " + GameManager.GameWarehouse.Walls[ 1 ].Position );
            Debug.Log( "Wall2 Rotation: " + GameManager.GameWarehouse.Walls[ 1 ].Rotation );
            Debug.Log( "Wall2 Scale:    " + GameManager.GameWarehouse.Walls[ 1 ].Scale );

            Debug.Log( "Door3 Position: " + "Type: " + GameManager.GameWarehouse.Doors[ 2 ].Type + " " + GameManager.GameWarehouse.Doors[ 2 ].Position );
            Debug.Log( "Door3 Rotation: " + "Type: " + GameManager.GameWarehouse.Doors[ 2 ].Type + " " + GameManager.GameWarehouse.Doors[ 2 ].Rotation );
            Debug.Log( "Door3 Scale:    " + "Type: " + GameManager.GameWarehouse.Doors[ 2 ].Type + " " + GameManager.GameWarehouse.Doors[ 2 ].Scale );

            Debug.Log( "Storage1 Itew2 Position: " + GameManager.GameWarehouse.StorageRecks[ 0 ].GetItems( )[ 1 ].Position );
            Debug.Log( "Storage1 Itew2 Rotation: " + GameManager.GameWarehouse.StorageRecks[ 0 ].GetItems( )[ 1 ].Rotation );
            Debug.Log( "Storage1 Itew2 Scale:    " + GameManager.GameWarehouse.StorageRecks[ 0 ].GetItems( )[ 1 ].Scale );
            Debug.Log( "Storage1 Itew2 IDRef: " + GameManager.GameWarehouse.StorageRecks[ 0 ].GetItems( )[ 1 ].GetID( ) );

            // man.Warehouse.Doors[ 1 ].Transformation.SetPosition( new Vector3( 30f, 1f, 59.345f ) );

            GameManager.SaveProject( "ExampleProject" );
            GameManager.CloseProject( );
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}

