using ApplicationFacade;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WarehouseItemMenu : MonoBehaviour
{
    public GameObject ItemTemplate;
    public GameObject PanelEditItem;
    public GameObject PanelListItem;
    private ItemData[] Stock;
    private ItemData SelectedItem; 
    private ModeHandler ModeHandler;
    private GameObject SwitchModeButton;
    void Start()
    {
        replaceResources();
        ItemData.AddItemToStock("M5 Schraube", 2000, 0.01);
        ItemData.AddItemToStock("M5 Mutter", 1500, 0.002);
        ItemData.AddItemToStock("Zündkerze", 50, 0.05);
        ItemData.AddItemToStock("Ölwanne", 2, 6.2);
        ItemData.AddItemToStock("Zahnriemen", 12, 0.6);
        ItemData.AddItemToStock("Zylinderkopf", 36, 1.2);
        ItemData.AddItemToStock("Antriebswelle", 1, 1.7);
        ItemData.AddItemToStock("Bremsschiebe", 87, 1.0);
        ItemData.AddItemToStock("Stoßdämpfer", 63, 2.2);
        ItemData.AddItemToStock("Luftfilter", 134, 0.8);
        ItemData.AddItemToStock("Radlager", 21, 2.7);
        ItemData.AddItemToStock("Katalysator", 6, 5.3);
        ItemData.AddItemToStock("Anlasser", 15, 0.9);
        ItemData.AddItemToStock("Turbolader", 2, 2.4);
        ItemData.AddItemToStock("Getriebe", 8, 6.2);
        ItemData.AddItemToStock("Reifen", 24, 4);
        Stock = ItemData.GetStock;
        foreach (ItemData item in Stock)
        {
            ItemTemplate.transform.Find("Name/NameText").GetComponent<TextMeshProUGUI>().text = item.Name;
            ItemTemplate.transform.Find("Count/CountText").GetComponent<TextMeshProUGUI>().text = "x" + item.Count.ToString();
            ItemTemplate.transform.Find("AdditionalInfo/Weight/WeightText").GetComponent<TextMeshProUGUI>().text = weightToString(item.Weight);
            GameObject field = Instantiate(ItemTemplate, ItemTemplate.transform.parent);
            field.SetActive(true);
            //item.ItemChanged += ItemHasChanged;
        }
        updateSize();
        SwitchModeButton = GameObject.Find("SwitchModeButton");
        ModeHandler = SwitchModeButton.GetComponent<ModeHandler>();
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
        float newHeight = System.Math.Max(161, 1+(Stock.Length+1) * 16);
        RectTransform contentBox = ItemTemplate.transform.parent.GetComponent<RectTransform>();
        contentBox.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newHeight);
    }
    private void replaceResources()
    {
        PanelEditItem.SetActive(true);
        ItemTemplate.SetActive(true);
        ItemTemplate.transform.Find("AdditionalInfo").gameObject.SetActive(true);
        ResourceHandler.ReplaceResources();
        ItemTemplate.transform.Find("AdditionalInfo").gameObject.SetActive(false);
        ItemTemplate.SetActive(false);
        PanelEditItem.SetActive(false);
    }
    public void OnClick(GameObject sender)
    {
        GameObject additionalInfo = sender.transform.Find("AdditionalInfo").gameObject;
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
            SelectedItem = ItemData.AddItemToStock("");
            Array.Resize(ref Stock, Stock.Length + 1);
            Stock[Stock.Length - 1] = SelectedItem;
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
        Transform field = ItemTemplate.transform.parent.GetChild(Array.IndexOf(Stock, SelectedItem) + 1);
        field.Find("Name/NameText").GetComponent<TextMeshProUGUI>().text = SelectedItem.Name;
        field.Find("Count/CountText").GetComponent<TextMeshProUGUI>().text = "x" + SelectedItem.Count.ToString();
        field.Find("AdditionalInfo/Weight/WeightText").GetComponent<TextMeshProUGUI>().text = weightToString(SelectedItem.Weight);
    }
    public void AddCloseClick()
    {
        PanelEditItem.SetActive(false);
        PanelListItem.SetActive(true);
    }
    public void ListCloseClick()
    {
        PanelListItem.SetActive(false);
        PanelListItem.transform.parent.gameObject.SetActive(true);
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
