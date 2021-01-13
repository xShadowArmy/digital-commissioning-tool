using ApplicationFacade.Application;
using ApplicationFacade.Warehouse;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeView : MonoBehaviour
{

    public GameObject StorageRacks;
    public GameObject MovableStorageRacks;
    public Material defaultMaterial;
    public Material selectedMaterial;
    public GameObject avatar;
    public GameObject currentMovableStorage;
    TreeViewItem TVStorageRacks;
    TreeViewItem TVMovableStorageRacks;
    TreeViewControl treeView;
    
    void OnDestroy()
    {
        GameManager.GameContainer.ContainerCreated -= GameContainer_ContainerCreated;
        GameManager.GameContainer.ContainerDeleted -= GameContainer_ContainerDeleted;
        GameManager.GameWarehouse.StorageRackCreated -= GameWarehouse_StorageRackCreated;
        GameManager.GameWarehouse.StorageRackDeleted -= GameWarehouse_StorageRackDeleted;
    }
    // Start is called before the first frame update
    void Start()
    {
        GameManager.GameContainer.ContainerCreated += GameContainer_ContainerCreated;
        GameManager.GameContainer.ContainerDeleted += GameContainer_ContainerDeleted;
        GameManager.GameWarehouse.StorageRackCreated += GameWarehouse_StorageRackCreated;
        GameManager.GameWarehouse.StorageRackDeleted += GameWarehouse_StorageRackDeleted;

        treeView = gameObject.GetComponent<TreeViewControl>();
        AddEvents(treeView.RootItem);
        treeView.RootItem.Items.Clear();
        treeView.RootItem.IsExpanded = true;
        TVStorageRacks = treeView.RootItem.AddItem("Storage Racks");
        AddEvents(TVStorageRacks);
        PopulateData(TVStorageRacks, StorageRacks.transform);
        TVMovableStorageRacks = treeView.RootItem.AddItem("Movable Storage Racks");
        AddEvents(TVMovableStorageRacks);
        PopulateData(TVMovableStorageRacks, MovableStorageRacks.transform);
        if (MovableStorageRacks.transform.GetChild(0) != null)
        {
            currentMovableStorage = MovableStorageRacks.transform.GetChild(0).gameObject;
            ShowContainer(currentMovableStorage);
        }
        updateSize();
    }

    private void GameWarehouse_StorageRackCreated(StorageData data)
    {
        GameObject rack = data.Object;
        TreeViewItem item = TVStorageRacks.AddItem(rack.name);
        item.Data = rack;
        AddEvents(item);
        updateSize();
    }

    private void GameWarehouse_StorageRackDeleted(StorageData data)
    {
        TreeViewItem itemToRemove = null;
        foreach (TreeViewItem item in TVStorageRacks.Items)
        {
            if (data.Object.name.Equals(item.Header))
            {
                itemToRemove = item;
            }
        }
        if (itemToRemove != null)
        {
            TVStorageRacks.Items.Remove(itemToRemove);
            updateSize();
        }
    }

    private void GameContainer_ContainerCreated(StorageData storage)
    {
        GameObject container = storage.Object;
        TreeViewItem item = TVMovableStorageRacks.AddItem(container.name);
        item.Data = container;
        AddEvents(item);
        if (currentMovableStorage == null)
        {
            currentMovableStorage = container;
        }
        updateSize();
    }

    private void GameContainer_ContainerDeleted(StorageData storage)
    {
        TreeViewItem itemToRemove = null;
        foreach (TreeViewItem item in TVMovableStorageRacks.Items)
        {
            if (storage.Object.name.Equals(item.Header))
            {
                itemToRemove = item;
            }
        }
        if (itemToRemove != null)
        {
            TVMovableStorageRacks.Items.Remove(itemToRemove);
            updateSize();
        }
    }

    void PopulateData(TreeViewItem root, Transform t)
    {
        root.IsExpanded = false;
        foreach (Transform child in t)
        {
            TreeViewItem item = root.AddItem(child.name);
            item.Data = child.gameObject;
            AddEvents(item);
        }
    }
    void ShowContainer(GameObject container) 
    {
        foreach (TreeViewItem item in TVMovableStorageRacks.Items)
        {
            if (item.Data == container)
            {
                item.Data.SetActive(true);
            }
            else
            {
                 item.Data .SetActive(false);
            }
        }
    }
    float calcHeight(TreeViewItem root)
    {
        float height = 13;
        foreach (TreeViewItem item in root.Items)
        {
            if (item.IsExpanded)
            {
                height += calcHeight(item);
            }
            else
            {
                height += 20;
            }
        }
        return height;
    }
    private void updateSize()
    {
        float newHeight = System.Math.Min(25+calcHeight(treeView.RootItem), 250);
        gameObject.transform.parent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newHeight);
    }
    public void Handler(object sender, System.EventArgs args)
    {
        //Debug.Log(string.Format("{0} detected: {1}", args.GetType().Name, (sender as TreeViewItem).Header));
    }
    public void ExpandHandler(object sender, System.EventArgs args)
    {
        float newHeight = System.Math.Min(45 + calcHeight(treeView.RootItem), 450);
        if ((sender as TreeViewItem) == treeView.RootItem && !treeView.RootItem.IsExpanded)
        {
            newHeight = 75;
        }
        gameObject.transform.parent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newHeight);
    }
    public void HoverOnHandler(object sender, System.EventArgs args)
    {
        GameObject senderGameobject = (sender as TreeViewItem).Data;
        if (senderGameobject != null && senderGameobject.transform.parent.gameObject == StorageRacks)
        {
            Transform invisibleWall = (sender as TreeViewItem).Data.transform.Find("InvisibleWall").transform;
            invisibleWall.GetComponent<Renderer>().material = selectedMaterial;
        }
    }
    public void HoverOffHandler(object sender, System.EventArgs args)
    {
        GameObject senderGameobject = (sender as TreeViewItem).Data;
        if (senderGameobject != null && senderGameobject.transform.parent.gameObject == StorageRacks)
        {
            Transform invisibleWall = (sender as TreeViewItem).Data.transform.Find("InvisibleWall").transform;
            invisibleWall.GetComponent<Renderer>().material = defaultMaterial;
        }
    }
    public void ClickHandler(object sender, System.EventArgs args)
    {
        TreeViewItem TVItem = (sender as TreeViewItem);
        GameObject senderGameobject = TVItem.Data;
        if (TVItem.Header.Equals("Storage Racks"))
        {
            GameManager.GameWarehouse.CreateStorageRack();
        }
        else if (TVItem.Header.Equals("Movable Storage Racks"))
        {
            StorageData container = GameManager.GameContainer.CreateContainer();
            ShowContainer(container.Object);
        }
        else if (senderGameobject != null && senderGameobject.transform.parent.gameObject == MovableStorageRacks)
        {
            GameObject newMovableStorage = (sender as TreeViewItem).Data;
            if (currentMovableStorage != null && currentMovableStorage != newMovableStorage)
            {
                ShowContainer(newMovableStorage);
            }
            currentMovableStorage = newMovableStorage;
            avatar.GetComponent<TestPascal>().StorageRack = newMovableStorage;            
        }
    }
    void AddHandlerEvent(out System.EventHandler handler)
    {
        handler = new System.EventHandler(Handler);
    }

    void AddEvents(TreeViewItem item)
    {
        AddHandlerEvent(out item.Checked);
        AddHandlerEvent(out item.Unchecked);
        AddHandlerEvent(out item.Selected);
        AddHandlerEvent(out item.Unselected);
        item.Expand = new System.EventHandler(ExpandHandler);
        item.Click = new System.EventHandler(ClickHandler);
        item.HoverOn = new System.EventHandler(HoverOnHandler);
        item.HoverOff = new System.EventHandler(HoverOffHandler);
    }
}