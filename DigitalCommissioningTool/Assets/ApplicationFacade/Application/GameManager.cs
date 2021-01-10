using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationFacade.Warehouse;

namespace ApplicationFacade.Application
{
    /// <summary>
    /// Zentrale Klasse fuer die Verwaltung und Anpassung aller Elemente.
    /// </summary>
    public static class GameManager
    {
        /// <summary>
        /// Die Projekt Daten des geoffneten Projekts.
        /// </summary>
        public static ProjectData OpenProjectData
        {
            get
            {
                return PManager.Data;
            }
        }
        
        /// <summary>
        /// Das Lagerhaus der aktuellen Umgebung.
        /// </summary>
        public static Warehouse.Warehouse GameWarehouse { get; private set; }

        /// <summary>
        /// Die Containerklasse fuer die Handhabung der mobilen Regale.
        /// </summary>
        public static Container GameContainer { get; private set; }

        /// <summary>
        /// Der Projektmanager zum Erstellen, speichern und Laden von Projekten.
        /// </summary>
        public static ProjectManager PManager { get; set; }

        /// <summary>
        /// Initiallisiert den GameManager.
        /// </summary>
        static GameManager()
        {
            GameWarehouse = new Warehouse.Warehouse( );
            GameContainer = new Container( );
            PManager = new ProjectManager( );
        }

        /// <summary>
        /// Laedt ein Projekt in die Umgebung.
        /// </summary>
        /// <param name="name">Der Name des Projekts das geladen werden soll.</param>
        /// <returns>Gibt true zurueck wenn Erfolgreich.</returns>
        public static bool LoadProject( string name )
        {
            if ( GameWarehouse != null )
            {
                GameWarehouse.DestroyWarehouse( );
            }

            Warehouse.Warehouse wtmp = new Warehouse.Warehouse( );
            Container ctmp = new Container( );

            PManager.LoadProject( name, ref wtmp, ref ctmp );

            GameWarehouse = wtmp;
            GameContainer = ctmp;

            if ( GameWarehouse == null || GameContainer == null )
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Speichert das aktuell geoffnete Projekt.
        /// </summary>
        /// <param name="name">Der Name unter dem das Projekt gespeichert werden soll.</param>
        public static void SaveProject( string name )
        {
            PManager.SaveProject( name, GameWarehouse, GameContainer );
        }
        
        /// <summary>
        /// Schliesst das aktuell Geoffnete Projekt.
        /// </summary>
        public static void CloseProject()
        {
            if ( GameWarehouse != null )
            {
                GameWarehouse.DestroyWarehouse( );
            }

            PManager.CloseProject( );
        }

        /// <summary>
        /// Erstellt ein neuens Projekt mit der angegebenen Lagerhaus groesse.
        /// </summary>
        /// <param name="name">Der Name des neuen Projekts.</param>
        /// <param name="size">Die Groesse des Lagerhauses.</param>
        /// <returns>Gibt true zurueck wenn Erfolgreich.</returns>
        public static bool CreateProject( string name, WarehouseSize size = WarehouseSize.Medium )
        {
            if ( GameWarehouse != null )
            {
                GameWarehouse.DestroyWarehouse( );
            }

            Warehouse.Warehouse wtmp = new Warehouse.Warehouse( );
            Container ctmp = new Container( );

            PManager.CreateProject( name, size, ref wtmp, ref ctmp );

            GameWarehouse = wtmp;
            GameContainer = ctmp;

            if ( GameWarehouse == null || GameContainer == null )
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Loescht das Projekt mit dem angegebenen Namen.
        /// </summary>
        /// <param name="name">Der Name des Projekts das Geloescht werden soll.</param>
        public static void DeleteProject( string name )
        {
            PManager.DeleteProject( name );
        }
    }
}
