using ApplicationFacade;
using System.Collections;
using System.Collections.Generic;
using SystemFacade;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragWindow : MonoBehaviour, IDragHandler, IBeginDragHandler
{
    public string WindowName;
    Vector3 DefualtPosition;
    Vector3 Offset;
    bool dirty = false;
    void Start()
    {
        DefualtPosition = transform.position;
        Rect PanelRect = transform.GetComponent<RectTransform>().rect;
        Rect ScreenRect = new Rect(0, 0, Screen.width - PanelRect.width, Screen.height - PanelRect.height);
        WindowPosition LoadedPosition = new WindowPosition();
        using (ConfigManager cman = new ConfigManager())
        {
            cman.OpenConfigFile("Preferences.xml", true);
            cman.LoadData(WindowName + "Position", LoadedPosition);
        }
        if (LoadedPosition.Position != Vector3.zero)
        {
            if (ScreenRect.Contains(LoadedPosition.Position))
            {
                transform.position = LoadedPosition.Position;
            }
        }       
    }
    void OnApplicationQuit()
    {
        if (dirty)
        {
            using (ConfigManager cman = new ConfigManager())
            {
                cman.OpenConfigFile("Preferences.xml", true);
                cman.StoreData(WindowName + "Position", new WindowPosition(transform.position));
            }
        }
    }//TODO Add save on ProjectCloseEvent
    public void OnBeginDrag(PointerEventData eventData)
    {
        Offset = Input.mousePosition - transform.position;
        dirty = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition - Offset;
    }
    
}
