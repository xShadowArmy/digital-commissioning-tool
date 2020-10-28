using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SystemTools.ManagingRessources;

public class TestScript : MonoBehaviour
{
    /// <summary>
    /// Ermöglicht das Schreiben von LogDateien.
    /// </summary>
    /// <value>test</value>
    private string test = "test";
    
    public string Test
    {
        get
        {
            return test;
        }
            
        set
        {
            test = value;
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        // Lesen der Ressourcen direkt über die Managerklasse
        for ( int i = 1; i <= 10; i++ )
        {
            Debug.Log( StringRessourceManager.LoadString( "@test" + i ) );
        }

        // Lesen der Ressourcen direkt über den jeweiligen String

        Test = Test.LoadString( "@ErrorText" );

        Debug.Log( Test );
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
