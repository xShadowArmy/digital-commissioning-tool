using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using ApplicationFacade;
using UnityEngine.UI;
using ApplicationFacade.Warehouse;
using ApplicationFacade.Application;
using SystemFacade;

public class QueueMenu : MonoBehaviour
{
    [SerializeField] SelectionManager selectionManager;
    public GameObject ItemTemplate;
    public GameObject PanelQueue;
    public GameObject CanvasWarehouseItems;
    public Transform WalkTargets;
    public TestPascal Avatar;
    public TextMeshProUGUI ButtonText;
    [HideInInspector]
    public DragItem ActiveItem;
    public GameObject DraggedItem;
    public ItemData SelectedItem;
    public DialogMenu dialogMenu;

    public List<DragItem> QueueItems = new List<DragItem>();

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
        dialogMenu.AmountConfirmedEvent += DialogEvent;
    }
    public void AddItemToQueue(ItemData itemData)
    {
        ItemTemplate.transform.Find("NameItemQueue/NameItemTextQueue").GetComponent<TextMeshProUGUI>().text = itemData.Name;
        ItemTemplate.transform.Find("CountItemQueue/CountItemQueueText").GetComponent<TextMeshProUGUI>().text = "x" + itemData.Count;
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
        /*
        Avatar.WalkTrajectoryPoints.Clear();
        foreach (DragItem item in QueueItems)
        { 
            Transform walkTarget = item.LinkedItem.Object.transform.Find("BoxWalkTarget");
            Avatar.WalkTrajectoryPoints.Add(walkTarget);
        }
        Avatar.UpdatePath();
        */
    }
    public void OnClick()
    {
        if (ActiveItem != null)
        {
            Destroy(ActiveItem.gameObject);
            ActiveItem.LinkedItem.ReturnItem();
            QueueItems.Remove(ActiveItem);
        }
        else
        {
            if (SelectedItem != null && !QueueItems.Exists(x => x.LinkedItem == SelectedItem))
            {
                dialogMenu.SelectedItem = SelectedItem;
                dialogMenu.gameObject.SetActive(true);
            }
        }
        ButtonText.transform.parent.gameObject.SetActive(false);
    }
    private void SelectionManager_ShelveSelected(GameObject selectedObject, bool active)
    {
        if (ActiveItem != null)
        {
            ActiveItem.transform.GetComponentInChildren<Image>().color = Color.white;
            ActiveItem = null;
            ButtonText.transform.parent.gameObject.SetActive(false);
        }
        if (active)
        {
            SelectedItem = GameManager.GameWarehouse.GetStorageRackItem(selectedObject);
            ButtonText.text = StringResourceManager.LoadString("@AddItem");
            ButtonText.transform.parent.gameObject.SetActive(true);
        }
        else
        {
            SelectedItem = null;
            ButtonText.transform.parent.gameObject.SetActive(false);
        }
    }
    private void DialogEvent(DialogMenu dialog) 
    {
        AddItemToQueue(dialog.SelectedItem.RequestCopyItem(dialog.Amount));
    }
    // Update is called once per frame
    void Update()
    {

    }
}

