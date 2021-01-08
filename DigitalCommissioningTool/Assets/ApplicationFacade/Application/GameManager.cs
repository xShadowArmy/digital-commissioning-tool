using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ApplicationFacade.Warehouse;

namespace ApplicationFacade.Application
{
    public static class GameManager
    {
        public static ProjectData OpenProjectData
        {
            get
            {
                return PManager.Data;
            }
        }
        
        public static Warehouse.Warehouse GameWarehouse { get; private set; }

        public static Container GameContainer { get; private set; }

        public static ProjectManager PManager { get; set; }

        static GameManager()
        {
            GameWarehouse = new Warehouse.Warehouse( );
            PManager = new ProjectManager( );
        }

        public static bool LoadProject( string name )
        {
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

        public static void SaveProject( string name )
        {
            PManager.SaveProject( name, GameWarehouse, GameContainer );
        }
        
        public static void CloseProject()
        {
            PManager.CloseProject( );
        }

        public static bool CreateProject( string name, WarehouseSize size = WarehouseSize.Medium )
        {
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

        public static void DeleteProject( string name )
        {
            PManager.DeleteProject( name );
        }
    }
}
