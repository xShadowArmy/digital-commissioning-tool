using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ApplicationFacade;

public class LoadSceneData : MonoBehaviour
{
    void Start()
    {
        //GameManager.CreateProject( "SmallWarehouse", WarehouseSize.Small );
        //GameManager.CreateProject( "MediumWarehouse", WarehouseSize.Medium );
        //GameManager.CreateProject( "LargeWarehouse", WarehouseSize.Large );

        GameManager.LoadProject( "MediumWarehouse" );
        
        //StorageData data = GameManager.GameWarehouse.CreateStorageRack( );
        
        //ItemData.AddItemToStock( "TestItem2", 300 );
        
        //ItemData test1stock = ItemData.RequestStockItem( "TestItem2" );
        
        //ItemData test1 = test1stock.RequestItem( 150 );
        
        //GameManager.GameWarehouse.AddItemToStorageRack( data, test1, 12 );
        //GameManager.GameWarehouse.AddItemToStorageRack( data, test2, 8 );
        //GameManager.GameWarehouse.AddItemToStorageRack( data, test3, 9 );



        //GameManager.SaveProject( "MediumWarehouse" );
    }
    
    void Update()
    {
        
    }
}
