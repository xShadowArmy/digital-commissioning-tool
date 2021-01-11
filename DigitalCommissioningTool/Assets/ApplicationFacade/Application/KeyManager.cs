using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SystemFacade;
using UnityEngine;
using System.Reflection;

namespace ApplicationFacade.Application
{
    /// <summary>
    /// Verwaltet die Daten ueber belegte Tastenkombinationen.
    /// </summary>
    public static class KeyManager
    {
        /// <summary>
        /// Taste fuer Objekt Rotation.
        /// </summary>
        public static KeyData Rotate { get; set; }

        /// <summary>
        /// Taste fuer Objekt Rotation nach Links.
        /// </summary>
        public static KeyData RotateLeft { get; set; }

        /// <summary>
        /// Taste fuer Objekt Rotation nach Rechts.
        /// </summary>
        public static KeyData RotateRight { get; set; }

        /// <summary>
        /// Taste um die Kamera nach oben zu Bewegen.
        /// </summary>
        public static KeyData MoveCameraUp { get; set; }

        /// <summary>
        /// Taste um die Kamera nach unten zu Bewegen.
        /// </summary>
        public static KeyData MoveCameraDown { get; set; }

        /// <summary>
        /// Taste um Objekt an der X Achse zu Bewegen.
        /// </summary>
        public static KeyData MoveXAxis { get; set; }

        /// <summary>
        /// Taste um Objekt an der Z Achse zu Bewegen.
        /// </summary>
        public static KeyData MoveZAxis { get; set; }

        /// <summary>
        /// Taste um zwischen den Modi durchzuwaechseln.
        /// </summary>
        public static KeyData ChangeMode { get; set; }

        /// <summary>
        /// Taste um ein Regal zu erstellen.
        /// </summary>
        public static KeyData InsertStorageReck { get; set; }

        /// <summary>
        /// Taste um eine Waend einzufuegen.
        /// </summary>
        public static KeyData InsertWall { get; set; }

        /// <summary>
        /// Taste um ein Fenster einzufuegen.
        /// </summary>
        public static KeyData InsertWindow { get; set; }

        /// <summary>
        /// Taste um eine Tuer einzufuegen.
        /// </summary>
        public static KeyData InsertDoor { get; set; }

        /// <summary>
        /// Taste um ein Tor einzufuegen.
        /// </summary>
        public static KeyData InsertGate { get; set; }

        /// <summary>
        /// Taste um ein Item einzufuegen.
        /// </summary>
        public static KeyData InsertItem { get; set; }

        /// <summary>
        /// Taste um das ausgewaehlte Objekt zu entfernen.
        /// </summary>
        public static KeyData RemoveSelected { get; set; }

        /// <summary>
        /// Taste um das ausgewaehlte Objekt zu bewegen.
        /// </summary>
        public static KeyData MoveSelected { get; set; }

        /// <summary>
        /// Taste um den Timer zu zu starten/stoppen.
        /// </summary>
        public static KeyData ToogleTimer { get; set; }

        /// <summary>
        /// Taste um Menü zu Öffnen/Schließen.
        /// </summary>
        public static KeyData ToogleMenu { get; set; }

        /// <summary>
        /// Laedt die standard Tastenbelegung.
        /// </summary>
        static KeyManager()
        {
            SetDefault( );
        }

        /// <summary>
        /// Gibt die Daten ueber alle verfuegbaren Tastenkombinationen zurueck.
        /// </summary>
        public static KeyData[] Keys
        {
            get
            {
                KeyData[] data = new KeyData[18];
                
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
                data[17] = ToogleMenu;

                return data;
            }
        }

        /// <summary>
        /// Laedt eine gespeicherte Tastenbelegung.
        /// </summary>
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
                man.LoadData("ToogleMenu", ToogleMenu);

                man.CloseConfigFile( );
            }
        }

        /// <summary>
        /// Setzt die standard Tastenbelegung.
        /// </summary>
        public static void SetDefault()
        {
            Rotate = new KeyData( KeyCode.R, false );
            RotateLeft = new KeyData( KeyCode.Q, false );
            RotateRight = new KeyData( KeyCode.E, false );

            MoveCameraUp = new KeyData( KeyCode.E, false );
            MoveCameraDown = new KeyData( KeyCode.Q, false );

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
            ToogleMenu = new KeyData(KeyCode.Escape, false);
        }

        /// <summary>
        /// Speichert die aktuelle Tastenbelegung.
        /// </summary>
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
                man.StoreData("ToogleMenu", ToogleMenu, true);

                man.CloseConfigFile( );
            }
        }
    }
}
