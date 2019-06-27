using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;

/// <summary>
/// A class which manages file transfer operations, such as downloading and uploading save files.
/// </summary>
public class FileTransferManager : MonoBehaviour {

    /// <summary>
    /// External library function from /Assets/Plugins/FileUploader.jslib
    /// Implementation of upload is a Javascript Library because AvatarCreator is running in the browser.
    /// Uploads file to the server.
    /// </summary>
    /// <param name="gameObjectName">Name of the current game object.</param>
    [DllImport("__Internal")]
    private static extern void FileUploader(string gameObjectName);

    /// <summary>
    /// External library function from /Assets/Plugins/FileDownloader.jslib
    /// Implementation of upload is a Javascript Library because AvatarCreator is running in the browser.
    /// Downloads file from server or running Unity web application.
    /// </summary>
    /// <param name="str">Data for serialization.</param>
    /// <param name="fn">File name.</param>
    /// <param name="ctype">File type.</param>
    [DllImport("__Internal")]
    private static extern void FileDownloader(string str, string fn, string ctype);

    /// <summary>
    /// Invokes download process of created screenshot.
    /// </summary>
    /// <param name="data">Screenshot data in byte array.</param>
    public void DownloadScreenshot(byte[] data)
    {
        if (data != null)
        {
            Debug.Log("FileTransferManager:DownloadScreenshot(): Downloading screenshot.");
            FileDownloader(System.Convert.ToBase64String(data), AvatarCreatorContext.sessionguid.ToString() + ".png", "image/png");
        }
    }

    /// <summary>
    /// Invokes download process of save file.
    /// </summary>
    /// <param name="data">Serialized save data.</param>
    public void DownloadSaveFile(string data)
    {
        Debug.Log("FileTransferManager:DownloadSaveFile(): Downloading save file.");
        FileDownloader(System.Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(data)), AvatarCreatorContext.sessionguid.ToString() + ".xml", "application/xml");
    }

    /// <summary>
    /// Invokes upload process of serialized data.
    /// </summary>
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

    /// <summary>
    /// External FileUploader function calls this from JavaScript
    /// </summary>
    /// <param name="url">URL of the uploaded file.</param>
    void FileSelected(string url)
    {
        StartCoroutine(LoadFile(url));
    }

    /// <summary>
    /// Loads file from given URL.
    /// </summary>
    /// <param name="url">File URL.</param>
    /// <returns>Return variable not used.</returns>
    IEnumerator LoadFile(string url)
    {
        WWW file = new WWW(url);
        yield return file;

        AvatarCreatorContext.LoadAvatarFromFile(file.text);
    }
}
