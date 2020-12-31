using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using ApplicationFacade;
using UnityEngine.UI;

public class QueueMenu : MonoBehaviour
{
    [SerializeField] SelectionManager selectionManager;
    public GameObject ItemTemplate;
    public GameObject PanelQueue;
    public GameObject CanvasWarehouseItems;
    public Transform WalkTargets;
    public TestPascal Avatar;
    [HideInInspector]
    public DragItem ActiveItem;
    public GameObject DraggedItem;
    public ItemData SelectedItem;

    private List<DragItem> QueueItems = new List<DragItem>();

    // Start is called before the first frame update

    private void updateSize()
    {
        float newHeight = System.Math.Max(161, 1 + (QueueItems.Count + 1) * 16);
        RectTransform contentBox = ItemTemplate.transform.parent.GetComponent<RectTransform>();
        contentBox.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newHeight);
    }

    void Start()
    {
        selectionManager.ShelveSelected += SelectionManager_ShelveSelected;
    }
    public void AddItemToQueue(ItemData itemData)
    {
        ItemTemplate.transform.Find("Name/NameText").GetComponent<TextMeshProUGUI>().text = itemData.Name;
        ItemTemplate.transform.Find("Count/CountText").GetComponent<TextMeshProUGUI>().text = "x" + itemData.Count;
        GameObject field = Instantiate(ItemTemplate, ItemTemplate.transform.parent);
        field.SetActive(true);
        DragItem dragItem = field.GetComponent<DragItem>();
        dragItem.LinkedItem = itemData;
        QueueItems.Add(dragItem);
        updateSize();
    }

    public void QueueCloseClick()
    {
        PanelQueue.SetActive(false);
        PanelQueue.transform.parent.gameObject.SetActive(true);
    }
    public void ChangeQueueOrder()
    {
        Avatar.WalkTrajectoryPoints.Clear();
        foreach (DragItem item in QueueItems)
        { 
            Transform walkTarget = item.LinkedItem.Object.transform.Find("BoxWalkTarget");
            Avatar.WalkTrajectoryPoints.Add(walkTarget);
        }
        Avatar.UpdatePath();
    }
    public void OnClick()
    {
        if (ActiveItem != null)
        {
            Destroy(ActiveItem.gameObject);
            QueueItems.Remove(ActiveItem);
        }
        else
        {
            if (SelectedItem != null && !QueueItems.Exists(x => x.LinkedItem == SelectedItem))
            {
                AddItemToQueue(SelectedItem);
            }
        }
    }
    private void SelectionManager_ShelveSelected(GameObject selectedObject, bool active)
    {
        if (ActiveItem != null)
        {
            ActiveItem.transform.GetComponentInChildren<Image>().color = Color.white;
            ActiveItem = null;
        }
        if (active)
        {
            SelectedItem = GameManager.GameWarehouse.GetStorageRackItem(selectedObject);
        }
        else
        {
            SelectedItem = null;
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}

