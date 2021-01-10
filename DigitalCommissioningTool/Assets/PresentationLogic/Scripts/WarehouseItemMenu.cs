using ApplicationFacade.Warehouse;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using ApplicationFacade.Application;

public class WarehouseItemMenu : MonoBehaviour
{
    public GameObject ItemTemplate;
    public GameObject PanelEditItem;
    public GameObject PanelDistributeItem;
    public GameObject PanelListItem;
    private List<ItemData> Stock;
    private ItemData SelectedItem;
    private ModeHandler ModeHandler;
    private GameObject SwitchModeButton;

    private ItemData distributeItemData = null;
    private int countDistributeItem = 0;
    private StorageData storageDistributeItem = null;

    void OnDestroy()
    {
        ItemData.StockChanged -= StockHasChanged;
        SelectionManager.StorageSelected -= SelectionManagerOnStorageSelected;
        ItemData.ItemChanged -= ItemHasChanged;
    }
    void Start()
    {
        ItemData.StockChanged += StockHasChanged;
        SelectionManager.StorageSelected += SelectionManagerOnStorageSelected;
        ItemData.ItemChanged += ItemHasChanged;
        replaceResources();
        //replaceResources();
        //ItemData.AddItemToStock("M5 Schraube", 0.01);
        //ItemData.AddItemToStock("M5 Mutter", 0.002);
        //ItemData.AddItemToStock("Zündkerze", 0.05);
        //ItemData.AddItemToStock("Ölwanne", 6.2);
        //ItemData.AddItemToStock("Zahnriemen",  0.6);
        //ItemData.AddItemToStock("Zylinderkopf",  1.2);
        //ItemData.AddItemToStock("Antriebswelle", 1.7);
        //ItemData.AddItemToStock("Bremsschiebe",  1.0);
        //ItemData.AddItemToStock("Stoßdämpfer",  2.2);
        //ItemData.AddItemToStock("Luftfilter",  0.8);
        //ItemData.AddItemToStock("Radlager", 2.7);
        //ItemData.AddItemToStock("Katalysator", 5.3);
        //ItemData.AddItemToStock("Anlasser",  0.9);
        //ItemData.AddItemToStock("Turbolader", 2.4);
        //ItemData.AddItemToStock("Getriebe", 6.2);
        //ItemData.AddItemToStock("Reifen", 4);
        Stock = ItemData.GetStock.OfType<ItemData>().ToList();
        foreach (ItemData item in Stock)
        {
            AddItemToList(item);
            //item.ItemChanged += ItemHasChanged;
        }
        SwitchModeButton = GameObject.Find("SwitchModeButton");
        ModeHandler = SwitchModeButton.GetComponent<ModeHandler>();
    }

    private void OnValidate()
    {

    }

    private void AddItemToList(ItemData item)
    {
        ItemTemplate.transform.Find("NameItem/NameItemText").GetComponent<TextMeshProUGUI>().text = item.Name;
        ItemTemplate.transform.Find("CountItem/CountItemText").GetComponent<TextMeshProUGUI>().text = "x" + item.Count.ToString();
        ItemTemplate.transform.Find("AdditionalInfoItem/WeightItem/WeightItemText").GetComponent<TextMeshProUGUI>().text = weightToString(item.Weight);
        GameObject field = Instantiate(ItemTemplate, ItemTemplate.transform.parent);
        field.SetActive(true);
        updateSize();
    }
    private void ItemHasChanged(ItemData item)
    {
        StockHasChanged(item);
    }

    private void StockHasChanged(ItemData item)
    {
        int index = Stock.FindIndex(x => x.Name.Equals(item.Name));
        if (index >= 0)
        {
            Transform field = ItemTemplate.transform.parent.GetChild(index + 1);
            if (ItemData.StockContainsItem(item.Name))
            {
                Stock[index] = item;
                field.Find("NameItem/NameItemText").GetComponent<TextMeshProUGUI>().text = item.Name;
                field.Find("CountItem/CountItemText").GetComponent<TextMeshProUGUI>().text = "x" + item.Count.ToString();
                field.Find("AdditionalInfoItem/WeightItem/WeightItemText").GetComponent<TextMeshProUGUI>().text = weightToString(item.Weight);
            }
            else
            {
                Stock.RemoveAt(index);
                Destroy(field.gameObject);
                updateSize();
            }
        }
        else
        {
            Stock.Add(item);
            AddItemToList(item);
        }
    }

    private string weightToString(double weight)
    {
        if (weight >= 1)
        {
            return Math.Round(weight, 2).ToString() + "Kg";
        }
        else
        {
            return Math.Round(weight * 1000, 0).ToString() + "g";
        }
    }
    private void updateSize()
    {
        float newHeight = System.Math.Max(161, 1 + (Stock.Count + 1) * 16);
        RectTransform contentBox = ItemTemplate.transform.parent.GetComponent<RectTransform>();
        contentBox.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newHeight);
    }
    private void replaceResources()
    {
        PanelListItem.SetActive(true);
        PanelEditItem.SetActive(true);
        PanelDistributeItem.SetActive(true);
        ItemTemplate.SetActive(true);
        ItemTemplate.transform.Find("AdditionalInfoItem").gameObject.SetActive(true);
        ResourceHandler.ReplaceResources();
        ItemTemplate.transform.Find("AdditionalInfoItem").gameObject.SetActive(false);
        ItemTemplate.SetActive(false);
        PanelEditItem.SetActive(false);
        PanelDistributeItem.SetActive(false);
    }
    public void OnClick(GameObject sender)
    {
        GameObject additionalInfo = sender.transform.Find("AdditionalInfoItem").gameObject;
        RectTransform rectTransform = sender.GetComponent<RectTransform>();
        additionalInfo.SetActive(!additionalInfo.activeSelf);
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 45 - rectTransform.rect.height);
        LayoutRebuilder.ForceRebuildLayoutImmediate(sender.transform.parent.GetComponent<RectTransform>());
    }
    public void AddItemClick()
    {
        TMP_InputField textName = PanelEditItem.transform.Find("NameEditItem/NameTextFieldEditItem").GetComponent<TMP_InputField>();
        TMP_InputField textWeight = PanelEditItem.transform.Find("WeightEditItem/WeightTextFieldEditItem").GetComponent<TMP_InputField>();
        TMP_InputField textCount = PanelEditItem.transform.Find("CountEditItem/CountTextFieldEditItem").GetComponent<TMP_InputField>();
        textName.text = textCount.text = textWeight.text = "";
        SelectedItem = null;

        PanelEditItem.SetActive(true);
        PanelListItem.SetActive(false);
    }
    public void EditItemClick(GameObject sender)
    {
        PanelEditItem.SetActive(true);
        PanelListItem.SetActive(false);
        TMP_InputField textName = PanelEditItem.transform.Find("NameEditItem/NameTextFieldEditItem").GetComponent<TMP_InputField>();
        TMP_InputField textWeight = PanelEditItem.transform.Find("WeightEditItem/WeightTextFieldEditItem").GetComponent<TMP_InputField>();
        TMP_InputField textCount = PanelEditItem.transform.Find("CountEditItem/CountTextFieldEditItem").GetComponent<TMP_InputField>();
        ItemData item = Stock[sender.transform.GetSiblingIndex() - 1];
        SelectedItem = item;
        textName.text = item.Name;
        textWeight.text = item.Weight.ToString();
        textCount.text = item.Count.ToString();
    }
    public void SaveItemClick()
    {
        bool failed = false;
        bool newItem = false;
        PanelEditItem.SetActive(false);
        PanelListItem.SetActive(true);
        if (SelectedItem == null)
        {
            newItem = true;
            ItemData.AddItemToStock(".");
            SelectedItem = ItemData.RequestStockItem(".");
        }
        TMP_InputField textName = PanelEditItem.transform.Find("NameEditItem/NameTextFieldEditItem").GetComponent<TMP_InputField>();
        TMP_InputField textWeight = PanelEditItem.transform.Find("WeightEditItem/WeightTextFieldEditItem").GetComponent<TMP_InputField>();
        TMP_InputField textCount = PanelEditItem.transform.Find("CountEditItem/CountTextFieldEditItem").GetComponent<TMP_InputField>();
        if (!textName.text.Equals("") && !(ItemData.StockContainsItem(textName.text) && newItem))
        {
            SelectedItem.SetItemName(textName.text);
        }
        else
        {
            Debug.LogWarning("Eingabe ist nicht zulässig");
            failed = true;
        }
        try
        {
            int result = Int32.Parse(textCount.text);
            //SelectedItem.SetItemCount(result);
            SelectedItem.IncreaseItemCount(result - SelectedItem.Count);
        }
        catch (FormatException)
        {
            Debug.LogWarning("Eingabe ist nicht zulässig");
            failed = true;
        }
        try
        {
            double result = Double.Parse(textWeight.text);
            SelectedItem.SetItemWeight(result);
        }
        catch (FormatException)
        {
            Debug.LogWarning("Eingabe ist nicht zulässig");
            failed = true;
        }
        if (failed && newItem)
        {
            ItemData.RemoveItemFromStock(SelectedItem);
        }
    }
    public void AddCloseClick()
    {
        ResetTextFields();
        distributeItemData = null;
        countDistributeItem = 0;
        storageDistributeItem = null;
        PanelEditItem.SetActive(false);
        PanelDistributeItem.SetActive(false);
        PanelListItem.SetActive(true);
    }
    public void ListCloseClick()
    {
        PanelListItem.SetActive(false);
        PanelListItem.transform.parent.gameObject.SetActive(true);
    }

    public void DistributeItemClick(GameObject sender)
    {
        distributeItemData = ItemData.RequestStockItem(sender.transform.Find("NameItem/NameItemText").GetComponent<TextMeshProUGUI>().text);
        PanelListItem.SetActive(false);
        PanelDistributeItem.SetActive(true);
        PanelDistributeItem.transform.Find("NameDistributeItem/NameTextFieldDistributeItem/TextAreaNameDistributeItem/TextNameTextfieldDistributeItem").GetComponent<TextMeshProUGUI>().SetText(distributeItemData.Name);

    }

    private void SelectionManagerOnStorageSelected(Transform selectedObject)
    {
        if (PanelDistributeItem.activeInHierarchy && selectedObject != null)
        {
            storageDistributeItem = GameManager.GameWarehouse.GetStorageRack(selectedObject.gameObject);
            PanelDistributeItem.transform.Find("SelectedStorageRackDistributeItem/SelectedStorageRackTextFieldDistributeItem/TextAreaSelectedStorageRackDistributeItem/TextSelectedStorageRackTextfieldDistributeItem").GetComponent<TextMeshProUGUI>().SetText(storageDistributeItem.Object.name);
        }
    }

    public void OnDistributeItemSaveClick()
    {
        string count = PanelDistributeItem.transform.Find("CountDistributeItem/CountTextFieldDistributeItem/TextAreaCountDistributeItem/TextCountTextfieldDistributeItem").GetComponent<TextMeshProUGUI>().text;
        count = count.Replace("\u200B", "");
        bool isNumeric = int.TryParse(count, out countDistributeItem);
        if (distributeItemData != null && isNumeric && countDistributeItem > 0 && storageDistributeItem != null)
        {
            ItemData itemData = distributeItemData.RequestItem(countDistributeItem);
            if (storageDistributeItem.AddItem(itemData))
            {
                AddCloseClick();
            }

        }
    }

    public ItemData GetStockItem(int index)
    {
        return Stock[index];
    }

    public void ChangeMode()
    {
        if (ModeHandler.Mode.Equals("EditorMode"))
        {
            PanelDistributeItem.SetActive(false);
            PanelEditItem.SetActive(false);
            PanelListItem.SetActive(false);
        }
        else
        {
            PanelListItem.transform.parent.gameObject.SetActive(true);
            PanelListItem.SetActive(true);
        }
    }

    private void ResetTextFields()
    {
        PanelDistributeItem.transform.Find("NameDistributeItem/NameTextFieldDistributeItem/TextAreaNameDistributeItem/TextNameTextfieldDistributeItem").GetComponent<TextMeshProUGUI>().SetText("");
        PanelDistributeItem.transform.Find("CountDistributeItem/CountTextFieldDistributeItem/TextAreaCountDistributeItem/TextCountTextfieldDistributeItem").GetComponent<TextMeshProUGUI>().SetText("");
        PanelDistributeItem.transform.Find("SelectedStorageRackDistributeItem/SelectedStorageRackTextFieldDistributeItem/TextAreaSelectedStorageRackDistributeItem/TextSelectedStorageRackTextfieldDistributeItem").GetComponent<TextMeshProUGUI>().SetText("");
    }
}
