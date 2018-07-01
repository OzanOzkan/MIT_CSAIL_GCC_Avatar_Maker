using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenShotCapturer : MonoBehaviour {

    public int x;
    public int y;
    public int w;
    public int h;

    private void OnPostRender()
    {
        if (AvatarCreatorContext.takeScreenShot)
        {
            Debug.Log("ScreenShotCapturer:OnPostRender: Capturing screenshot.");
            var tex = new Texture2D(w, h, TextureFormat.RGB24, false);

            Rect rex = new Rect(x, y, w, h);
            tex.ReadPixels(rex, 0, 0);
            tex.Apply();

            //GameObject.Find("testimg").GetComponent<Image>().sprite = Sprite.Create(tex, new Rect(0, 0, rex.width, rex.height), new Vector2(0.5f, 0.5f));

            AvatarCreatorContext.fileTransferManager.DownloadScreenshot(tex.EncodeToPNG());

            Destroy(tex);

            AvatarCreatorContext.takeScreenShot = false;
        }
    }
}
