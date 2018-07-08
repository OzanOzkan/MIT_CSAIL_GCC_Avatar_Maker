using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenShotCapturer : MonoBehaviour {

    public int x;
    public int y;
    public int w;
    public int h;

    private void Update()
    {
        gameObject.GetComponent<Camera>().backgroundColor = AvatarCreatorContext.gameCamera.backgroundColor;
        gameObject.GetComponent<Camera>().enabled = AvatarCreatorContext.takeScreenShot;
    }

    private void OnPostRender()
    {
        if (AvatarCreatorContext.takeScreenShot)
        {
            Debug.Log("ScreenShotCapturer:OnPostRender: Capturing screenshot.");
            //var tex = new Texture2D(w, h, TextureFormat.RGB24, false);

            //Rect rex = new Rect(x, y, w, h);
            //tex.ReadPixels(rex, 0, 0);
            //tex.Apply();

            ////GameObject.Find("testimg").GetComponent<Image>().sprite = Sprite.Create(tex, new Rect(0, 0, rex.width, rex.height), new Vector2(0.5f, 0.5f));

            //AvatarCreatorContext.fileTransferManager.DownloadScreenshot(tex.EncodeToPNG());

            //Destroy(tex);

            //AvatarCreatorContext.takeScreenShot = false;

            RenderTexture originalTexture = RenderTexture.active;
            RenderTexture ssrTexture = gameObject.GetComponent<Camera>().activeTexture;
            RenderTexture.active = ssrTexture;
            Texture2D tex = new Texture2D(ssrTexture.width, ssrTexture.height);
            tex.ReadPixels(new Rect(0, 0, ssrTexture.width, ssrTexture.height), 0, 0);
            tex.Apply();
            RenderTexture.active = originalTexture;

            AvatarCreatorContext.fileTransferManager.DownloadScreenshot(tex.EncodeToPNG());
            Destroy(tex);

            AvatarCreatorContext.takeScreenShot = false;

            //GameObject.Find("testimg").GetComponent<Image>().sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
        }
    }
}
