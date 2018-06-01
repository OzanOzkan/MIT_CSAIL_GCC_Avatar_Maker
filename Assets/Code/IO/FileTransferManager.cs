using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

public class FileTransferManager : MonoBehaviour {

    [DllImport("__Internal")]
    private static extern void FileUploader(string gameObjectName);

    [DllImport("__Internal")]
    private static extern void FileDownloader(string str, string fn, string ctype);

    public void DownloadScreenshot(byte[] data)
    {
        if (data != null)
        {
            Debug.Log("FileTransferManager:DownloadScreenshot(): Downloading screenshot.");
            FileDownloader(System.Convert.ToBase64String(data), AvatarCreatorContext.sessionguid.ToString() + ".png", "image/png");
        }
    }

    public void DownloadSaveFile(string data)
    {
        Debug.Log("FileTransferManager:DownloadSaveFile(): Downloading save file.");
        FileDownloader(System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(data)), AvatarCreatorContext.sessionguid.ToString() + ".xml", "application/xml");
    }

    public void UploadFile()
    {
#if UNITY_EDITOR
        string path = UnityEditor.EditorUtility.OpenFilePanel("Send File", "", "xml");
        if (!System.String.IsNullOrEmpty(path))
            FileSelected("file:///" + path);
#else
        FileUploader(gameObject.name);
#endif
    }

    // External FileUploader function calls this from JavaScript.
    void FileSelected(string url)
    {
        StartCoroutine(LoadFile(url));
    }

    IEnumerator LoadFile(string url)
    {
        WWW file = new WWW(url);
        yield return file;

        AvatarCreatorContext.LoadAvatarFromFile(file.text);
    }
}
