using System.Collections;
using System.Collections.Generic;
using SystemFacade;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public Dropdown dropdownResolution;
    public Dropdown dropdownLanguage;
    public Toggle fullscreen;
    Resolution[] resolutions;
    // Start is called before the first frame update
    void Start()
    {
        InitResolution();
        InitLanguage();
        ReplaceResources();
        fullscreen.isOn = Screen.fullScreen;
    }
    void InitResolution()
    {
        List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
        resolutions = Screen.resolutions;
        foreach (Resolution res in resolutions)
        {
            options.Add(new Dropdown.OptionData(res.ToString()));
        }
        options.Reverse();
        dropdownResolution.AddOptions(options);
    }
    void InitLanguage()
    {
        foreach (Dropdown.OptionData data in dropdownLanguage.options)
        {
            string key = data.text.TrimStart('<').TrimEnd('>');
            string localizedText = StringResourceManager.LoadString("@" + key);
            data.text = localizedText;
        }
    }
    void ReplaceResources()
    {
        GameObject[] labels = GameObject.FindGameObjectsWithTag("Resource");

        foreach (GameObject label in labels)
        {
            string key = label.GetComponent<UnityEngine.UI.Text>().text.TrimStart('<').TrimEnd('>');
            string localizedText = StringResourceManager.LoadString("@" + key);
            label.GetComponent<UnityEngine.UI.Text>().text = localizedText;
        }
    }
    public void FullscreenChanged()
    {
        Screen.fullScreen = fullscreen.isOn;
        ResolutionChanged();
    }
    public void ResolutionChanged()
    {
        Resolution newReolution = resolutions[resolutions.Length - 1 - dropdownResolution.value];
        Screen.SetResolution(newReolution.width, newReolution.height, fullscreen.isOn, newReolution.refreshRate);
    }
    public void LanguageChanged()
    {
        switch (dropdownLanguage.itemText.text)
        {
            case "<German>":
                StringResourceManager.SelectLanguage(SystemLanguage.German);
                break;
            case "<English>":
                StringResourceManager.SelectLanguage(SystemLanguage.English);
                break;
        }
    }
    public void Back()
    {
        gameObject.transform.parent.Find("MainPanel").gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}


