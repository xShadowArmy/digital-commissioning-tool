using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeView : MonoBehaviour
{

    public GameObject StorageRacks;
    public GameObject MoveableStorageRacks;
    public Material defaultMaterial;
    public Material selectedMaterial;
    TreeViewControl treeView;
    GameObject currentMoveableStorage;

    // Start is called before the first frame update
    void Start()
    {
        treeView = gameObject.GetComponent<TreeViewControl>();
        treeView.Expand = new System.EventHandler(ExpandHandler);
        treeView.RootItem.Items.Clear();
        treeView.RootItem.IsExpanded = true;
        TreeViewItem storageRacks = treeView.RootItem.AddItem("Storage Racks");
        AddEvents(storageRacks);
        PopulateData(storageRacks, StorageRacks.transform);
        TreeViewItem moveableStorageRacks = treeView.RootItem.AddItem("Moveable Storage Racks");
        AddEvents(moveableStorageRacks);
        PopulateData(moveableStorageRacks, MoveableStorageRacks.transform);
        float newHeight = System.Math.Min(40 + treeView.RootItem.Items.Count * 25, 450);
        newHeight = 450;
        gameObject.transform.parent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newHeight);
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
    float calcHeight(TreeViewItem root)
    {
        float height = 0;
        foreach (TreeViewItem item in root.Items)
        {
            height += 25 + calcHeight(item);
        }
        height = 450;
        return height;
    }
    public void Handler(object sender, System.EventArgs args)
    {
        Debug.Log(string.Format("{0} detected: {1}", args.GetType().Name, (sender as TreeViewItem).Header));
    }
    public void ExpandHandler(object sender, System.EventArgs args)
    {
        float newHeight;
        if (!treeView.RootItem.IsExpanded)
        {
            newHeight = System.Math.Min(40 + calcHeight(treeView.RootItem), 450);
        }
        else
        {
            newHeight = System.Math.Min(40 + treeView.RootItem.Items.Count * 25, 450);
        }
        newHeight = 450;
        gameObject.transform.parent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newHeight);
    }
    public void HoverOnHandler(object sender, System.EventArgs args)
    {
        if ((sender as TreeViewItem).Data.transform.parent.gameObject == StorageRacks)
        {
            Transform invisibleWall = (sender as TreeViewItem).Data.transform.Find("InvisibleWall").transform;
            invisibleWall.GetComponent<Renderer>().material = selectedMaterial;
        }
    }
    public void HoverOffHandler(object sender, System.EventArgs args)
    {
        Transform invisibleWall = (sender as TreeViewItem).Data.transform.Find("InvisibleWall").transform;
        invisibleWall.GetComponent<Renderer>().material = defaultMaterial;
    }
    public void ClickHandler(object sender, System.EventArgs args)
    {
        if ((sender as TreeViewItem).Data.transform.parent.gameObject == MoveableStorageRacks)
        {
            GameObject newMoveableStorage = (sender as TreeViewItem).Data;
            if (currentMoveableStorage != null && currentMoveableStorage != newMoveableStorage)
            {
                currentMoveableStorage.SetActive(false);
            }
            currentMoveableStorage = newMoveableStorage;
            newMoveableStorage.SetActive(!newMoveableStorage.activeSelf);
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
        AddHandlerEvent(out item.Expand);
        item.Click = new System.EventHandler(ClickHandler);
        item.HoverOn = new System.EventHandler(HoverOnHandler);
        item.HoverOff = new System.EventHandler(HoverOffHandler);
    }
}