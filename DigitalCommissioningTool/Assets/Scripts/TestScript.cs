using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SystemTools.ManagingRessources;

public class TestScript : MonoBehaviour
{
    private string Test { get; set; }

    // Start is called before the first frame update
    void Start()
    {
        for ( int i = 1; i <= 10; i++ )
        {
            Debug.Log( StringRessourceManager.LoadString( "test" + i ) );
        }

        Test = Test.LoadString( "TestFehler" );

        Debug.Log( Test );

        Debug.Log( StringRessourceManager.LoadString( 11 ) );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
