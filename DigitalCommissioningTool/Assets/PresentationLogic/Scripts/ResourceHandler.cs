using System.Collections;
using System.Collections.Generic;
using SystemFacade;
using UnityEngine;

public class ResourceHandler : MonoBehaviour
{
    static Dictionary<GameObject, string> resources = new Dictionary<GameObject, string>();

    public static void LoadResources()
    {
        GameObject[] labels = GameObject.FindGameObjectsWithTag("Resource");
        foreach (GameObject g in labels)
        {
            string text = g.GetComponent<UnityEngine.UI.Text>().text;
            if (!resources.ContainsKey(g) || text.StartsWith("<") && text.EndsWith(">"))
            {
                resources[g] = text.TrimStart('<').TrimEnd('>');
            }
        }
    }
    public static void ReplaceResources()
    {
        LoadResources();
        List<GameObject> deletedResources = new List<GameObject>();
        foreach (KeyValuePair<GameObject, string> item in resources)
        {
            if (item.Key != null)
            {
                item.Key.GetComponent<UnityEngine.UI.Text>().text = StringResourceManager.LoadString("@" + item.Value);
            }
            else
            {
                deletedResources.Add(item.Key);
            }
            
        }
        foreach (GameObject g in deletedResources)
        {
            resources.Remove(g);
        }
    }
}
