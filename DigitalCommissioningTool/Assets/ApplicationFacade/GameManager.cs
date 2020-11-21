using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationFacade
{
    public static class GameManager
    {
        public static Warehouse GameWarehouse { get; private set; }

        public static Container GameContainer { get; private set; }

        private static ProjectManager PManager { get; set; }

        static GameManager()
        {
            PManager = new ProjectManager( );
        }

        public static bool OpenProject( string name )
        {
            Warehouse wtmp = new Warehouse( );
            Container ctmp = new Container( );

            PManager.OpenProject( name, ref wtmp, ref ctmp );

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

        public static bool CreateProject( string name )
        {
            Warehouse wtmp = new Warehouse( );
            Container ctmp = new Container( );

            PManager.CreateProject( name, ref wtmp, ref ctmp );

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
