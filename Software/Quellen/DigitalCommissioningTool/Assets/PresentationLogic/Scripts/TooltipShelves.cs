﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
using ApplicationFacade.Application;
using ApplicationFacade.Warehouse;
using SystemFacade;

public class TooltipShelves : MonoBehaviour
{
    [SerializeField] SelectionManager selectionManager;
    [SerializeField] Image current;
    public event EventHandler ShelveSelected;
    public TextMeshProUGUI header;
    public TextMeshProUGUI content;
    public LayoutElement layoutElement;
    public int characterWrapLimit;
    private Tooltip tooltip;
    private string headerMessage = "New Text";
    private string contentMessage = "New Text";
    private ItemData item;


    void Start()
    {
        LogManager.WriteError("ToolTipShelves Called");
        selectionManager.ShelveSelected += SelectionManager_ShelveSelected;
        tooltip = current.GetComponent<Tooltip>();
    }

    /// <summary>
    /// 
    /// 
    /// </summary>
    /// <param name="selectedObject">Referenz auf das Object, dass das Tooltip event ausgelöst hat</param>
    /// <param name="active">Parameter zum aktivieren oder deaktivieren von dem Tooltip</param>
    private void SelectionManager_ShelveSelected(GameObject selectedObject, bool active)
    {
        if (active)
        {
            item = GameManager.GameWarehouse.GetStorageRackItem(selectedObject);

            if (item == null)
            {
                item = GameManager.GameContainer.GetContainerItem(selectedObject);
            }

            headerMessage = item.Name;
            getMessages();
            activate();

            if (item.ParentStorage.IsContainer || item.Count == 0)
            {
                GameObject.FindWithTag("AddItemsButton").GetComponent<Button>().interactable = false;
                GameObject.FindWithTag("RemoveItemButton").GetComponent<Button>().interactable = false;
            }

            else
            {
                GameObject.FindWithTag("AddItemsButton").GetComponent<Button>().interactable = true;
                GameObject.FindWithTag("RemoveItemButton").GetComponent<Button>().interactable = true;
            }
        }
        else
        {
            item = null;
            deactivate();
        }
    }


    void Update()
    {
        if (item != null)
        {
            if (Input.GetKeyDown(KeyManager.RemoveSelected.Code) && ModeHandler.Mode.Equals( "MosimMode" ) )
            {
                item.ParentStorage.RemoveItem( item );
                item.ReturnItem( );
            }
        }

        if (Application.isEditor)
        {
            int headerLenght = header.text.Length;
            int contentLength = content.text.Length;

            if ((headerLenght > characterWrapLimit || contentLength > characterWrapLimit))

                layoutElement.enabled = true;
            else
                layoutElement.enabled = false;
        }

        if (Input.GetMouseButtonDown(1))
        {
            deactivate();
        }
    }

    /// <summary>
    /// Ruft Set Tooltip auf
    /// </summary>
    public void activate()
    {
        tooltip.SetTooltip(contentMessage, headerMessage, false);
    }


    /// <summary>
    /// Ruft RemoveTooltip auf
    /// </summary>
    public void deactivate()
    {
        tooltip.RemoveTooltip();
    }

    /// <summary>
    /// Weist der Variable Content Massage die Parameter des selectedObjects, die in IdemData gespeichert sind zu
    /// </summary>
    private void getMessages()
    {
        contentMessage = "Anzahl: " + item.Count + "\n";
        contentMessage += "Gewicht: " + item.Weight + " kg";
    }

    /// <summary>
    /// Aktualisiert den Inhalt des Tooltips
    /// </summary>
    private void updateTooltip()
    {
        getMessages();
        tooltip.SetTooltip(contentMessage, headerMessage, true);
    }

    /// <summary>
    /// Vermindert die Anzahl an Items in den Kisten um 1 oder löscht sie ggf.
    /// </summary>
    public void removeItems()
    {
        Debug.Log("clickedremoved");
        if (item.Count > 1)
        {
            item.DecreaseItemCount(1);
            updateTooltip();
        }
        else
        {
            item.ParentStorage.RemoveItem(item);
            deactivate();
        }
    }

    /// <summary>
    /// Erhöht die Anzahl an Items in den Kisten um 1
    /// </summary>
    public void addItems()
    {
        Debug.Log("clickedadd");
        item.IncreaseItemCount(1);
        updateTooltip();
    }
}