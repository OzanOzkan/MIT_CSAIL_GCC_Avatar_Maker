using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Xml;

public class UploadFile : MonoBehaviour {

    [DllImport("__Internal")]
    private static extern void onPointerDown();

    private void Start()
    {
        EventTrigger trigger = gameObject.AddComponent<EventTrigger>();
        var pointerDown = new EventTrigger.Entry();
        pointerDown.eventID = EventTriggerType.PointerDown;
        pointerDown.callback.AddListener((e) => OnButtonPointerDown());
        trigger.triggers.Add(pointerDown);
    }

    IEnumerator LoadTexture(string url)
    {
        WWW xmlfile = new WWW(url);
        yield return xmlfile;

        Debug.Log(xmlfile.text);

        XmlDocument doc = new XmlDocument();
        doc.LoadXml(xmlfile.text);

        XmlNode root = doc.DocumentElement.SelectSingleNode("/Actions");
        foreach (XmlNode node in root.ChildNodes)
        {
            foreach (XmlAttribute attr in node.Attributes)
            {
                Debug.Log(attr.Name + " " + attr.Value);
            }
        }
    }

    void FileSelected(string url)
    {
        StartCoroutine(LoadTexture(url));
    }

    public void OnButtonPointerDown()
    {
#if UNITY_EDITOR
        string path = UnityEditor.EditorUtility.OpenFilePanel("Send File", "", "xml");
        if (!System.String.IsNullOrEmpty(path))
            FileSelected("file:///" + path);
#else
        onPointerDown ();
#endif
    }
}
