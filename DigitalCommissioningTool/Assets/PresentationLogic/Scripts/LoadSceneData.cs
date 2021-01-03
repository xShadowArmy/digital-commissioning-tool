using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ApplicationFacade;
using ApplicationFacade.Application;
using ApplicationFacade.Warehouse;

public class LoadSceneData : MonoBehaviour
{
    void Start()
    {
        //GameManager.CreateProject( "SmallWarehouse", WarehouseSize.Small );
        //GameManager.CreateProject( "MediumWarehouse", WarehouseSize.Medium );
        //GameManager.CreateProject( "LargeWarehouse", WarehouseSize.Large );

        GameManager.LoadProject( "MediumWarehouse" );
        
        StorageData data = GameManager.GameWarehouse.CreateStorageRack( );
        
        ItemData.AddItemToStock( "M3Schrauben", 300 );
        ItemData.AddItemToStock( "Zündkerzen", 50 );
        
        ItemData test1stock = ItemData.RequestStockItem( "M3Schrauben" );
        ItemData test2stock = ItemData.RequestStockItem( "Zündkerzen" );
        
        ItemData test1 = test1stock.RequestItem( 150 );
        ItemData test2 = test2stock.RequestItem( 4 );

        data.AddItem( test1, 4 );
        data.AddItem( test2, 6 );



        //GameManager.SaveProject( "MediumWarehouse" );
    }
    
    void Update()
    {
        
    }
}
