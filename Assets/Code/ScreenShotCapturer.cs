using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenShotCapturer : MonoBehaviour {

    public int x;
    public int y;
    public int w;
    public int h;

    /// <summary>
    /// Called once in every frame.
    /// </summary>
    private void Update()
    {
        gameObject.GetComponent<Camera>().backgroundColor = AvatarCreatorContext.gameCamera.backgroundColor;
        gameObject.GetComponent<Camera>().enabled = AvatarCreatorContext.takeScreenShot;
    }

    /// <summary>
    /// OnPostRender is called after a camera finished rendering the Scene.
    /// </summary>
    private void OnPostRender()
    {
        // If screenshot variable is set to true, screenshot will be captured on this frame.
        if (AvatarCreatorContext.takeScreenShot)
        {
            Debug.Log("ScreenShotCapturer:OnPostRender: Capturing screenshot.");

            AvatarCreatorContext.takeScreenShot = false;

            RenderTexture originalTexture = RenderTexture.active;
            RenderTexture ssrTexture = gameObject.GetComponent<Camera>().activeTexture;
            RenderTexture.active = ssrTexture;
            Texture2D tex = new Texture2D(ssrTexture.width, ssrTexture.height);
            tex.ReadPixels(new Rect(0, 0, ssrTexture.width, ssrTexture.height), 0, 0);
            tex.Apply();
            RenderTexture.active = originalTexture;

            AvatarCreatorContext.fileTransferManager.DownloadScreenshot(tex.EncodeToPNG());
            Destroy(tex);
        }
    }
}
