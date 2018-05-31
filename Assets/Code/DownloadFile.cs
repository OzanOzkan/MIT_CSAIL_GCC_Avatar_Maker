using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.EventSystems;
using System.Text;

public class DownloadFile : MonoBehaviour {

    [DllImport("__Internal")]
    private static extern void ImageDownloader(string str, string fn, string ctype);

    public static byte[] ssData = null;
    public static string imageFilename;

    bool grab = false;

    void DownloadScreenshot()
    {
        if (ssData != null)
        {
            Debug.Log("Downloading..." + imageFilename);
            ImageDownloader(System.Convert.ToBase64String(ssData), imageFilename, "image/png");
        }
    }

    // Use this for initialization
    void Start()
    {
        EventTrigger trigger = gameObject.AddComponent<EventTrigger>();
        var pointerDown = new EventTrigger.Entry();
        pointerDown.eventID = EventTriggerType.PointerDown;
        pointerDown.callback.AddListener((e) => OnButtonPointerDown());
        trigger.triggers.Add(pointerDown);

        imageFilename = AvatarCreatorContext.sessionguid.ToString() + ".png";
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnPostRender()
    {
        if (AvatarCreatorContext.takeScreenShot)
        {
            var width = 400;
            var height = 500;
            var startX = 100;
            var startY = 200;
            var tex = new Texture2D(width, height, TextureFormat.RGB24, false);

            tex.ReadPixels(new Rect(startX, startY, width, height), 0, 0);
            tex.Apply();

            // Encode texture into PNG
            ssData = tex.EncodeToPNG();
            Destroy(tex);

            DownloadScreenshot();

            AvatarCreatorContext.takeScreenShot = false;
        }
    }

    public void OnButtonPointerDown()
    {
        AvatarCreatorContext.takeScreenShot = true;
    }
}
