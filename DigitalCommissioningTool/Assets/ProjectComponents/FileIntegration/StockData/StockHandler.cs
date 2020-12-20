using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectComponents.Abstraction;
using SystemFacade;

namespace ProjectComponents.FileIntegration
{
    public class StockHandler
    {
        public StockHandler()
        {
        }
        
        public ProjectItemData[] LoadFile()
        {
            LogManager.WriteInfo( "ProjektStock Datei wird gelesen.", "StockHandler", "LoadFile" );

            List<ProjectItemData> data = new List<ProjectItemData>();
            
            using( ConfigManager cman = new ConfigManager() )
            {
                cman.OpenConfigFile( Paths.TempPath, "ItemStock", true );

                int cnt = cman.LoadData( "itemCount" ).GetValueAsInt();

                for( int i = 0; i < cnt; i++ )
                {
                    ProjectItemData item = new ProjectItemData( );

                    cman.LoadData( "Item" + i, item );

                    data.Add( item );
                }

                cman.CloseConfigFile( );
            }

            return data.ToArray();
        }

        public void SaveFile( ProjectItemData[] data )
        {
            LogManager.WriteInfo( "ProjektStock Datei wird geschrieben.", "StockHandler", "SaveFile" );

            using ( ConfigManager cman = new ConfigManager( ) )
            {
                cman.OpenConfigFile( Paths.TempPath, "ItemStock", true );

                cman.StoreData( "itemCount", data.Length );

                for ( int i = 0; i < data.Length; i++ )
                {
                    cman.StoreData( "Item" + i, data[i] );
                }

                cman.CloseConfigFile( );
            }
        }
    }
}
