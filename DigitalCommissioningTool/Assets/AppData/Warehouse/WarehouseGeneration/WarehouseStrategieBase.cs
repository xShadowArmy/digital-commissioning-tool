using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AppData.Warehouse
{
    /// <summary>
    /// Basisklasse fuer die Erstellung der Lagerhaueser.
    /// </summary>
    public abstract class WarehouseStrategieBase : IWarehouseStrategie
    {
        /// <summary>
        /// Die Transformations Daten der Boeden.
        /// </summary>
        protected ObjectTransformation[] Floor { get; set; }

        /// <summary>
        /// Die Transformations Daten der Nordwaende.
        /// </summary>
        protected ObjectTransformation[] WallsNorth { get; set; }

        /// <summary>
        /// Die Transformations Daten der Ostwaende.
        /// </summary>
        protected ObjectTransformation[] WallsEast { get; set; }

        /// <summary>
        /// Die Transformations Daten der Suedwande.
        /// </summary>
        protected ObjectTransformation[] WallsSouth { get; set; }

        /// <summary>
        /// Die Transformations Daten der Westwaende.
        /// </summary>
        protected ObjectTransformation[] WallsWest { get; set; }

        /// <summary>
        /// Die Transformations Daten der inneren Waende.
        /// </summary>
        protected ObjectTransformation[] WallsInner { get; set; }

        /// <summary>
        /// Die Transformations Daten der Fenster.
        /// </summary>
        protected ObjectTransformation[] Windows { get; set; }

        /// <summary>
        /// Die Transformations Daten der Tueren.
        /// </summary>
        protected ObjectTransformation[] Doors { get; set; }

        /// <summary>
        /// Die Transformations Daten der Regale.
        /// </summary>
        protected ObjectTransformation[] StorageRacks { get; set; }

        /// <summary>
        /// X Offset fuer die Positionierung des Lagerhauses.
        /// </summary>
        protected readonly float xOffset;

        /// <summary>
        /// Y Offset fuer die Positionierung des Lagerhauses.
        /// </summary>
        protected readonly float yOffset;

        /// <summary>
        /// Z Offset fuer die Positionierung des Lagerhauses.
        /// </summary>
        protected readonly float zOffset;

        /// <summary>
        /// Erstellt eine neue Instanz mit default Werten.
        /// </summary>
        public WarehouseStrategieBase()
        {
            xOffset = -20f;
            yOffset = 1.6f;
            zOffset = -15f;
        }

        /// <summary>
        /// Erstellt eine neue Instanz mit custom Offset.
        /// </summary>
        /// <param name="xOff">X Offset fuer die Positionierung.</param>
        /// <param name="yOff">Y Offset fuer die Positionierung.</param>
        /// <param name="zOff">Z Offset fuer die Positionierung.</param>
        public WarehouseStrategieBase( float xOff, float yOff, float zOff )
        {
            xOffset = xOff;
            yOffset = yOff;
            zOffset = zOff;
        }

        /// <summary>
        /// Startet den Algorithmus.
        /// </summary>
        public abstract void StartGeneration();

        /// <summary>
        /// Gibt die Transformations Daten der Boeden zurueck.
        /// </summary>
        /// <returns>Die Transformations Daten der Boeden.</returns>
        public ObjectTransformation[] GetFloor()
        {
            return Floor;
        }

        /// <summary>
        /// Gibt die Transformations Daten der äusseren Wände zurueck.
        /// </summary>
        /// <returns>Die Transformations Daten der äusseren Wände.</returns>
        public ObjectTransformation[] GetOuterWalls()
        {
            ObjectTransformation[] tmp = new ObjectTransformation[WallsNorth.Length + WallsEast.Length + WallsSouth.Length + WallsWest.Length];

            int j = 0;

            for( int i = 0; i < WallsNorth.Length; i++ )
            {
                tmp[j++] = WallsNorth[i];
            }

            for ( int i = 0; i < WallsEast.Length; i++ )
            {
                tmp[j++] = WallsEast[i];
            }

            for ( int i = 0; i < WallsSouth.Length; i++ )
            {
                tmp[j++] = WallsSouth[i];
            }

            for ( int i = 0; i < WallsWest.Length; i++ )
            {
                tmp[j++] = WallsWest[i];
            }

            return tmp;
        }

        /// <summary>
        /// Gibt die Transformations Daten der inneren Waende zurueck.
        /// </summary>
        /// <returns>Die Transformations Daten der inneren Waende.</returns>
        public ObjectTransformation[] GetInnerWalls()
        {
            return WallsInner;
        }

        /// <summary>
        /// Gibt die Transformations Daten der Nordwaende zurueck.
        /// </summary>
        /// <returns>Die Transformations Daten der Nordwaende.</returns>
        public ObjectTransformation[] GetNorthWalls()
        {
            return WallsNorth;
        }

        /// <summary>
        /// Gibt die Transformations Daten der Ostwaende zurueck.
        /// </summary>
        /// <returns>Die Transformations Daten der Ostwaende.</returns>
        public ObjectTransformation[] GetEastWalls()
        {
            return WallsEast;
        }

        /// <summary>
        /// Gibt die Transformations Daten der Suedwaende zurueck.
        /// </summary>
        /// <returns>Die Transformations Daten der Suedwaende.</returns>
        public ObjectTransformation[] GetSouthWalls()
        {
            return WallsSouth;
        }

        /// <summary>
        /// Gibt die Transformations Daten der Westwaende zurueck.
        /// </summary>
        /// <returns>Die Transformations Daten der Westwaende.</returns>
        public ObjectTransformation[] GetWestWalls()
        {
            return WallsWest;
        }

        /// <summary>
        /// Gibt die Transformations Daten der Fenster zurueck.
        /// </summary>
        /// <returns>Die Transformations Daten der Fenster.</returns>
        public ObjectTransformation[] GetWindows()
        {
            return Windows;
        }

        /// <summary>
        /// Gibt die Transformations Daten der Tueren zurueck.
        /// </summary>
        /// <returns>Die Transformations Daten der Tueren.</returns>
        public ObjectTransformation[] GetDoors()
        {
            return Doors;
        }

        /// <summary>
        /// Gibt die Transformations Daten der Regale zurueck.
        /// </summary>
        /// <returns>Die Transformations Daten der Regale.</returns>
        public ObjectTransformation[] GetStorageRacks()
        {
            return StorageRacks;
        }
    }
}
