﻿using ApplicationFacade.Warehouse;
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
    }
    void Start()
    {
        ItemData.StockChanged += StockHasChanged;
        SelectionManager.StorageSelected += SelectionManagerOnStorageSelected;
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
        updateSize();
        SwitchModeButton = GameObject.Find("SwitchModeButton");
        ModeHandler = SwitchModeButton.GetComponent<ModeHandler>();
    }

    private void OnValidate()
    {
        
    }

    private void AddItemToList(ItemData item)
    {
        ItemTemplate.transform.Find("Name/NameText").GetComponent<TextMeshProUGUI>().text = item.Name;
        ItemTemplate.transform.Find("Count/CountText").GetComponent<TextMeshProUGUI>().text = "x" + item.Count.ToString();
        ItemTemplate.transform.Find("AdditionalInfo/Weight/WeightText").GetComponent<TextMeshProUGUI>().text = weightToString(item.Weight);
        GameObject field = Instantiate(ItemTemplate, ItemTemplate.transform.parent);
        field.SetActive(true);
    }
    private void StockHasChanged(ItemData item)
    {
        int index = Stock.IndexOf(item);
        if (index >= 0)
        {
            Transform field = ItemTemplate.transform.parent.GetChild(index + 1);
            if (item.IsStockItem)
            {
                field.Find("Name/NameText").GetComponent<TextMeshProUGUI>().text = item.Name;
                field.Find("Count/CountText").GetComponent<TextMeshProUGUI>().text = "x" + item.Count.ToString();
                field.Find("AdditionalInfo/Weight/WeightText").GetComponent<TextMeshProUGUI>().text = weightToString(item.Weight);
            }
            else
            {
                Stock.RemoveAt(index);
                Destroy(field);
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
        TMP_InputField textName = PanelEditItem.transform.Find("Name/NameText").GetComponent<TMP_InputField>();
        TMP_InputField textWeight = PanelEditItem.transform.Find("Weight/WeightText").GetComponent<TMP_InputField>();
        TMP_InputField textCount = PanelEditItem.transform.Find("Count/CountText").GetComponent<TMP_InputField>();
        textName.text = textCount.text = textWeight.text = "";
        SelectedItem = null;

        PanelEditItem.SetActive(true);
        PanelListItem.SetActive(false);
    }
    public void EditItemClick(GameObject sender)
    {
        PanelEditItem.SetActive(true);
        PanelListItem.SetActive(false);
        TMP_InputField textName = PanelEditItem.transform.Find("Name/NameText").GetComponent<TMP_InputField>();
        TMP_InputField textWeight = PanelEditItem.transform.Find("Weight/WeightText").GetComponent<TMP_InputField>();
        TMP_InputField textCount = PanelEditItem.transform.Find("Count/CountText").GetComponent<TMP_InputField>();
        ItemData item = Stock[sender.transform.GetSiblingIndex() - 1];
        SelectedItem = item;
        textName.text = item.Name;
        textWeight.text = item.Weight.ToString();
        textCount.text = item.Count.ToString();
    }
    public void SaveItemClick()
    {
        PanelEditItem.SetActive(false);
        PanelListItem.SetActive(true);
        if (SelectedItem == null)
        {
            ItemData.AddItemToStock(".");
            SelectedItem = ItemData.RequestStockItem(".");
            Stock[Stock.Count - 1] = SelectedItem;
            Instantiate(ItemTemplate, ItemTemplate.transform.parent).SetActive(true);
            updateSize();
        }
        TMP_InputField textName = PanelEditItem.transform.Find("Name/NameText").GetComponent<TMP_InputField>();
        TMP_InputField textWeight = PanelEditItem.transform.Find("Weight/WeightText").GetComponent<TMP_InputField>();
        TMP_InputField textCount = PanelEditItem.transform.Find("Count/CountText").GetComponent<TMP_InputField>();
        SelectedItem.SetItemName(textName.text);
        try
        {
            int result = Int32.Parse(textCount.text);
            //SelectedItem.SetItemCount(result);
            SelectedItem.IncreaseItemCount(result - SelectedItem.Count);
        }
        catch (FormatException)
        {
            Debug.LogWarning("Eingabe ist nicht zulässig");
        }
        try
        {
            double result = Double.Parse(textWeight.text);
            SelectedItem.SetItemWeight(result);
        }
        catch (FormatException)
        {
            Debug.LogWarning("Eingabe ist nicht zulässig");
        }
        Transform field = ItemTemplate.transform.parent.GetChild(Stock.IndexOf(SelectedItem) + 1);
        field.Find("Name/NameText").GetComponent<TextMeshProUGUI>().text = SelectedItem.Name;
        field.Find("Count/CountText").GetComponent<TextMeshProUGUI>().text = "x" + SelectedItem.Count.ToString();
        field.Find("AdditionalInfo/Weight/WeightText").GetComponent<TextMeshProUGUI>().text = weightToString(SelectedItem.Weight);
    }
    public void AddCloseClick()
    {
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
        if (PanelDistributeItem.activeInHierarchy)
        {
            storageDistributeItem = GameManager.GameWarehouse.GetStorageRack(selectedObject.gameObject);
            PanelDistributeItem.transform.Find("SelectedStorageRackDistributeItem/SelectedStorageRackTextFieldDistributeItem/TextAreaSelectedStorageRackDistributeItem/TextSelectedStorageRackTextfieldDistributeItem").GetComponent<TextMeshProUGUI>().SetText(storageDistributeItem.Object.name);
        }
    }

    public void OnDistributeItemSaveClick()
    {
        string count = PanelDistributeItem.transform.Find("CountDistributeItem/CountTextFieldDistributeItem/TextAreaCountDistributeItem/TextCountTextfieldDistributeItem").GetComponent<TextMeshProUGUI>().text;
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
    
    //private void ItemHasChanged(ItemData item)
    //{
    //    Transform field = ItemTemplate.transform.parent.GetChild(Array.IndexOf(Stock, item)+1);
    //    field.Find("Name/NameText").GetComponent<TextMeshProUGUI>().text = item.Name;
    //    field.Find("Count/CountText").GetComponent<TextMeshProUGUI>().text = "x" + item.Count.ToString();
    //    field.Find("AdditionalInfo/Weight/WeightText").GetComponent<TextMeshProUGUI>().text = weightToString(item.Weight);
    //}
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
}
