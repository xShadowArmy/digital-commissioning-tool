using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeView : MonoBehaviour
{

    public GameObject StorageRacks;
    public Material defaultMaterial;
    public Material selectedMaterial;
    TreeViewControl treeView;
    
    // Start is called before the first frame update
    void Start()
    {
        treeView = gameObject.GetComponent<TreeViewControl>();
        treeView.Expand = new System.EventHandler(ExpandHandler);
        treeView.RootItem.Items.Clear();
        PopulateData(StorageRacks.transform); 
    }

    void PopulateData(Transform t)
    {
        treeView.RootItem.IsExpanded = true;
        foreach (Transform child in t)
        {
            TreeViewItem item = treeView.RootItem.AddItem(child.name);
            item.Data = child.gameObject;
            AddEvents(item);
        }
        float newHeight = 40 + treeView.RootItem.Items.Count * 25;//System.Math.Max(420, paths.Length * 105);
        gameObject.transform.parent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newHeight);
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
            newHeight = System.Math.Min(40 + treeView.RootItem.Items.Count * 25, 500);
        }
        else
        {
            newHeight = 80;
        }      
        gameObject.transform.parent.GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newHeight);       
    }
    public void HoverOnHandler(object sender, System.EventArgs args)
    {
        Transform invisibleWall = (sender as TreeViewItem).Data.transform.Find("InvisibleWall").transform;
        invisibleWall.GetComponent<Renderer>().material = selectedMaterial;
    }
    public void HoverOffHandler(object sender, System.EventArgs args)
    {
        Transform invisibleWall = (sender as TreeViewItem).Data.transform.Find("InvisibleWall").transform;
        invisibleWall.GetComponent<Renderer>().material = defaultMaterial;
    }
    void AddHandlerEvent(out System.EventHandler handler)
    {
        handler = new System.EventHandler(Handler);
    }

     void AddEvents(TreeViewItem item)
    {
        AddHandlerEvent(out item.Click);
        AddHandlerEvent(out item.Checked);
        AddHandlerEvent(out item.Unchecked);
        AddHandlerEvent(out item.Selected);
        AddHandlerEvent(out item.Unselected);
        item.HoverOn = new System.EventHandler(HoverOnHandler);
        item.HoverOff = new System.EventHandler(HoverOffHandler);
    }
}