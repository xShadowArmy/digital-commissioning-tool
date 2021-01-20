using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AppData.Warehouse
{
    /// <summary>
    /// Strategie fuer den Lagerhaus generierungs Algorithmus.
    /// </summary>
    public interface IWarehouseStrategie 
    {
        /// <summary>
        /// Startet den Algorithmus.
        /// </summary>
        void StartGeneration();  
        
        /// <summary>
        /// Gibt die Transformations Daten der Boeden zurueck.
        /// </summary>
        /// <returns>Die Transformations Daten der Boeden.</returns>
        ObjectTransformation[] GetFloor();

        /// <summary>
        /// Gibt die Transformations Daten der äusseren Waende zurueck.
        /// </summary>
        /// <returns>Die Transformations Daten der äusseren Waende.</returns>
        ObjectTransformation[] GetOuterWalls();

        /// <summary>
        /// Gibt die Transformations Daten der inneren Waende zurueck.
        /// </summary>
        /// <returns>Die Transformations Daten der inneren Waende.</returns>
        ObjectTransformation[] GetInnerWalls();

        /// <summary>
        /// Gibt die Transformations Daten der Nordwand zurueck.
        /// </summary>
        /// <returns>Die Transformations Daten der Nordwand.</returns>
        ObjectTransformation[] GetNorthWalls();

        /// <summary>
        /// Gibt die Transformations Daten der Ostwand zurueck.
        /// </summary>
        /// <returns>Die Transformations Daten der Ostwand.</returns>
        ObjectTransformation[] GetEastWalls();

        /// <summary>
        /// Gibt die Transformations Daten der Suedwand zurueck.
        /// </summary>
        /// <returns>Die Transformations Daten der Suedwand.</returns>
        ObjectTransformation[] GetSouthWalls();

        /// <summary>
        /// Gibt die Transformations Daten der Westwand zurueck.
        /// </summary>
        /// <returns>Die Transformations Daten der Westwand.</returns>
        ObjectTransformation[] GetWestWalls();

        /// <summary>
        /// Gibt die Transformations Daten der Fenster zurueck.
        /// </summary>
        /// <returns>Die Transformations Daten der Fenster.</returns>
        ObjectTransformation[] GetWindows();

        /// <summary>
        /// Gibt die Transformations Daten der Tueren zurueck.
        /// </summary>
        /// <returns>Die Transformations Daten der Tueren.</returns>
        ObjectTransformation[] GetDoors();

        /// <summary>
        /// Gibt die Transformations Daten der Regale zurueck.
        /// </summary>
        /// <returns>Die Transformations Daten der Regale.</returns>
        ObjectTransformation[] GetStorageRacks();
    }
}