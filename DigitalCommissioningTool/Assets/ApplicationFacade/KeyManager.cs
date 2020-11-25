using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemFacade;
using UnityEngine;

namespace ApplicationFacade
{
    public static class KeyManager
    {
        public static KeyData Rotate { get; set; }
        public static KeyData RotateLeft { get; set; }
        public static KeyData RotateRight { get; set; }
        public static KeyData MoveCameraUp { get; set; }
        public static KeyData MoveCameraDown { get; set; }
        public static KeyData MoveXAxis { get; set; }
        public static KeyData MoveZAxis { get; set; }
        public static KeyData ChangeMode { get; set; }
        public static KeyData InsertStorageReck { get; set; }
        public static KeyData InsertWall { get; set; }
        public static KeyData InsertWindow { get; set; }
        public static KeyData InsertDoor { get; set; }
        public static KeyData InsertGate { get; set; }
        public static KeyData InsertItem { get; set; }
        public static KeyData RemoveSelected { get; set; }
        public static KeyData MoveSelected { get; set; }
        
        static KeyManager()
        {
            SetDefault( );
        }

        public static void LoadKeyConfiguration()
        {
            using ( ConfigManager man = new ConfigManager( ) )
            {
                man.OpenConfigFile( "UserSettings" );

                man.LoadData( "Rotate", Rotate );
                man.LoadData( "RotateLeft", RotateLeft );
                man.LoadData( "RotateRight", RotateRight );
                man.LoadData( "Rotate", Rotate );
                man.LoadData( "MoveCameraUp", MoveCameraUp );
                man.LoadData( "MoveCameraDown", MoveCameraDown );
                man.LoadData( "MoveXAxis", MoveXAxis );
                man.LoadData( "MoveZAxis", MoveZAxis );
                man.LoadData( "ChangeMode", ChangeMode );
                man.LoadData( "InsertStorageReck", InsertStorageReck );
                man.LoadData( "InsertWall", InsertWall );
                man.LoadData( "InsertWindow", InsertWindow );
                man.LoadData( "InsertDoor", InsertDoor );
                man.LoadData( "InsertGate", InsertGate );
                man.LoadData( "InsertItem", InsertItem );
                man.LoadData( "RemoveSelected", RemoveSelected );
                man.LoadData( "MoveSelected", MoveSelected );
                
                man.CloseConfigFile( );
            }
        }

        public static void SetDefault()
        {
            Rotate = new KeyData( KeyCode.R, false );
            RotateLeft = new KeyData( KeyCode.Q, false );
            RotateRight = new KeyData( KeyCode.E, false );

            MoveCameraUp = new KeyData( KeyCode.Q, false );
            MoveCameraDown = new KeyData( KeyCode.E, false );

            MoveXAxis = new KeyData( KeyCode.X, false );
            MoveZAxis = new KeyData( KeyCode.Z, false );

            InsertStorageReck = new KeyData( KeyCode.Alpha1, true );
            InsertWall = new KeyData( KeyCode.Alpha2, true );
            InsertWindow = new KeyData( KeyCode.Alpha3, true );
            InsertDoor = new KeyData( KeyCode.Alpha4, true );
            InsertGate = new KeyData( KeyCode.Alpha5, true );
            InsertItem = new KeyData( KeyCode.Alpha6, true );

            ChangeMode = new KeyData( KeyCode.M, true );
            RemoveSelected = new KeyData( KeyCode.Delete, false );
            MoveSelected = new KeyData( KeyCode.M, false );
        }

        public static void SaveKeyConfiguration()
        {
            using ( ConfigManager man = new ConfigManager( ) )
            {
                man.OpenConfigFile( "UserSettings" );

                man.StoreData( "Rotate", Rotate, false );
                man.StoreData( "RotateLeft", RotateLeft, false );
                man.StoreData( "RotateRight", RotateRight, false );
                man.StoreData( "MoveCameraUp", MoveCameraUp, false );
                man.StoreData( "MoveCameraDown", MoveCameraDown, false );
                man.StoreData( "MoveXAxis", MoveXAxis, false );
                man.StoreData( "MoveZAxis", MoveZAxis, false );
                man.StoreData( "ChangeMode", ChangeMode, false );
                man.StoreData( "InsertStorageReck", InsertStorageReck, false );
                man.StoreData( "InsertWall", InsertWall, false );
                man.StoreData( "InsertWindow", InsertWindow, false );
                man.StoreData( "InsertDoor", InsertDoor, false );
                man.StoreData( "InsertGate", InsertGate, false );
                man.StoreData( "InsertItem", InsertItem, false );
                man.StoreData( "RemoveSelected", RemoveSelected, false );
                man.StoreData( "MoveSelected", MoveSelected, false );

                man.CloseConfigFile( );
            }
        }
    }
}
