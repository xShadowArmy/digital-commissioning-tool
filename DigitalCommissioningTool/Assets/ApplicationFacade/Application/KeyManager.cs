using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemFacade;
using UnityEngine;
using System.Reflection;

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
        public static KeyData ToogleTimer { get; set; }

        static KeyManager()
        {
            SetDefault( );
        }

        public static KeyData[] Keys
        {
            get
            {
                KeyData[] data = new KeyData[17];
                
                data[0]  = Rotate;
                data[1]  = RotateLeft;
                data[2]  = RotateRight;
                data[3]  = MoveCameraUp;
                data[4]  = MoveCameraDown;
                data[5]  = MoveXAxis;
                data[6]  = MoveZAxis;
                data[7]  = ChangeMode;
                data[8]  = InsertStorageReck;
                data[9]  = InsertWall;
                data[10] = InsertWindow;
                data[11] = InsertDoor;
                data[12] = InsertGate;
                data[13] = InsertItem;
                data[14] = RemoveSelected;
                data[15] = MoveSelected;
                data[16] = ToogleTimer;

                return data;
            }
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
                man.LoadData( "ToogleTimer", ToogleTimer );

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

            ToogleTimer = new KeyData( KeyCode.P, false );

            ChangeMode = new KeyData( KeyCode.M, true );
            RemoveSelected = new KeyData( KeyCode.Delete, false );
            MoveSelected = new KeyData( KeyCode.M, false );
        }

        public static void SaveKeyConfiguration()
        {
            using ( ConfigManager man = new ConfigManager( ) )
            {
                man.OpenConfigFile( "UserSettings" );

                man.StoreData( "Rotate", Rotate, true );
                man.StoreData( "RotateLeft", RotateLeft, true);
                man.StoreData( "RotateRight", RotateRight, true);
                man.StoreData( "MoveCameraUp", MoveCameraUp, true);
                man.StoreData( "MoveCameraDown", MoveCameraDown, true);
                man.StoreData( "MoveXAxis", MoveXAxis, true);
                man.StoreData( "MoveZAxis", MoveZAxis, true);
                man.StoreData( "ChangeMode", ChangeMode, true);
                man.StoreData( "InsertStorageReck", InsertStorageReck, true);
                man.StoreData( "InsertWall", InsertWall, true);
                man.StoreData( "InsertWindow", InsertWindow, true);
                man.StoreData( "InsertDoor", InsertDoor, true);
                man.StoreData( "InsertGate", InsertGate, true);
                man.StoreData( "InsertItem", InsertItem, true);
                man.StoreData( "RemoveSelected", RemoveSelected, true);
                man.StoreData( "MoveSelected", MoveSelected, true);
                man.StoreData( "ToggleTimer", ToogleTimer, true);

                man.CloseConfigFile( );
            }
        }
    }
}
