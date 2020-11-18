using UnityEngine;
using SystemFacade;

namespace Scripts
{
    public class TestScript : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            LogManager.WriteInfo( "test" );

            Debug.Log( Paths.LogPath );
            Debug.Log( Paths.ProjectsPath );
            Debug.Log( Paths.ResourcePath );
            Debug.Log( Paths.StringResourcePath );
            Debug.Log( Paths.DataResourcePath );
            Debug.Log( Paths.TempPath );

            LogManager.Flush( );

            ArchiveManager.ArchiveDirectory( Paths.ProjectsPath, Paths.TempPath + "project.prj" );
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}

