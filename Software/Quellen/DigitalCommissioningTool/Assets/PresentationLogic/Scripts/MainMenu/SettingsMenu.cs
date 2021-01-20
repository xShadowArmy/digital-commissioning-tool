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
        LoadSettings();
    }
    private void OnApplicationQuit()
    {
        StoreSettings();
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
        for (int i = 0; i < dropdownLanguage.options.Count; i++)
        {
            switch (i)
            {
                case 0:
                    dropdownLanguage.options[i].text = StringResourceManager.LoadString("@German");
                    break;
                case 1:
                    dropdownLanguage.options[i].text = StringResourceManager.LoadString("@English");
                    break;
            }
        }
    }

    public void LoadSettings()
    {
        InitResolution();
        InitLanguage();
        ResourceHandler.ReplaceResources();
        using (ConfigManager cman = new ConfigManager())
        {
            cman.OpenConfigFile("Settings.xml", true);
            string resolution = cman.LoadData("Resolution").GetValueAsString();
            dropdownResolution.value = dropdownResolution.options.FindIndex((i) => { return i.text.Equals(resolution); });
            dropdownLanguage.value = cman.LoadData("Language").GetValueAsInt();
            fullscreen.isOn = cman.LoadData("fullscreen").GetValueAsBool();
            FullscreenChanged();
            LanguageChanged();
        }
    }
    private void StoreSettings()
    {
        using (ConfigManager cman = new ConfigManager())
        {
            cman.OpenConfigFile("Settings.xml", true);
            cman.StoreData("Resolution", resolutions[resolutions.Length - 1 - dropdownResolution.value]);
            cman.StoreData("Language", dropdownLanguage.value);
            cman.StoreData("fullscreen", fullscreen.isOn);
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
        switch (dropdownLanguage.value)
        {
            case 0:
                StringResourceManager.SelectLanguage(SystemLanguage.German);
                dropdownLanguage.captionText.text = StringResourceManager.LoadString("@German");
                break;
            case 1:
                StringResourceManager.SelectLanguage(SystemLanguage.English);
                dropdownLanguage.captionText.text = StringResourceManager.LoadString("@English");
                break;
        }
        ResourceHandler.ReplaceResources();
        InitLanguage();
    }
    public void Back()
    {
        gameObject.transform.parent.Find("MainPanel").gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
    public void KeyBinds()
    {
        gameObject.transform.parent.Find("KeyBindPanel").gameObject.SetActive(true);
        gameObject.SetActive(false);
    }
}


