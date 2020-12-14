using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ApplicationFacade;
using System;
using System.Reflection;

public class KeyBindMenu : MonoBehaviour
{
    public GameObject template;
    public GameObject Function;
    public GameObject Key;
    private List<PropertyInfo> Keys = new List<PropertyInfo>();
    private int activeKey = -1;
    // Start is called before the first frame update
    void Start()
    {
        Type t = typeof(KeyManager);
        PropertyInfo[] propertys = t.GetProperties(BindingFlags.Public | BindingFlags.Static);
        foreach (PropertyInfo property in propertys)
        {
            if (property.PropertyType == typeof(KeyData))
            {
                Keys.Add(property);
                KeyData key = (KeyData)property.GetValue(null);
                Function.GetComponent<UnityEngine.UI.Text>().text = property.Name;
                Key.GetComponent<UnityEngine.UI.Text>().text = key.ToString();
                GameObject item = Instantiate(template);
                item.transform.SetParent(template.transform.parent);
                item.SetActive(true);
            }
        }
        float newHeight = System.Math.Max(420, Keys.Count * 55);
        RectTransform contentBox = template.transform.parent.GetComponent<RectTransform>();
        contentBox.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, newHeight);
        ResourceHandler.ReplaceResources();
    }
    public void OnClick(GameObject sender)
    {
        int keyIndex = sender.transform.GetSiblingIndex() - 1;
        KeyData key = (KeyData)Keys[keyIndex].GetValue(null);
        UnityEngine.UI.Text keyText = sender.transform.Find("Key").GetComponent<UnityEngine.UI.Text>();
        if (activeKey == keyIndex)
        {
            keyText.text = key.ToString();
            activeKey = -1;
        }
        else
        {
            keyText.text = "Press any key";
            if (activeKey != -1)
            {
                KeyData oldKey = (KeyData)Keys[activeKey].GetValue(null);
                Transform oldKeyText = template.transform.parent.GetChild(activeKey + 1).transform.Find("Key");
                oldKeyText.GetComponent<UnityEngine.UI.Text>().text = oldKey.ToString();
            }
            activeKey = keyIndex;
        }

        Debug.Log(activeKey);
    }
    void OnGUI()
    {
        Event e = Event.current;
        if (e.type == EventType.KeyDown && activeKey != -1 && e.keyCode != KeyCode.LeftShift)
        {
            Keys[activeKey].SetValue(null, new KeyData(e.keyCode, e.shift));
            KeyManager.SaveKeyConfiguration();
            KeyData Key = (KeyData)Keys[activeKey].GetValue(null);
            Transform KeyText = template.transform.parent.GetChild(activeKey + 1).transform.Find("Key");
            KeyText.GetComponent<UnityEngine.UI.Text>().text = Key.ToString();
            activeKey = -1;
        }
    }
    public void Back()
    {
        gameObject.transform.parent.Find("SettingsPanel").gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}
