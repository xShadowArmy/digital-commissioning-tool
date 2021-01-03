using ApplicationFacade;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using ApplicationFacade.Warehouse;
public class DragItem : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler,IPointerEnterHandler
{
    public QueueMenu QueueMenu;
    [HideInInspector]
    public ItemData LinkedItem;
    Vector3 Offset;
    public void OnBeginDrag(PointerEventData eventData)
    {
        transform.GetComponentInChildren<Image>().color = Color.green;
        QueueMenu.DraggedItem = gameObject;
    }

    public void OnDrag(PointerEventData eventData)
    {
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        LinkedItem.Object.transform.SetSiblingIndex(transform.GetSiblingIndex()-1);
        QueueMenu.ChangeQueueOrder();
        transform.GetComponentInChildren<Image>().color = Color.white;
        QueueMenu.DraggedItem = null;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (QueueMenu.DraggedItem != null && QueueMenu.DraggedItem != gameObject)
        {
            QueueMenu.DraggedItem.transform.SetSiblingIndex(transform.GetSiblingIndex());
        }
    }
    public void OnClick()
    {
        if (QueueMenu.ActiveItem != null)
        {
            QueueMenu.ActiveItem.transform.GetComponentInChildren<Image>().color = Color.white;
        }
        QueueMenu.ActiveItem = this;
        transform.GetComponentInChildren<Image>().color = Color.gray;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
