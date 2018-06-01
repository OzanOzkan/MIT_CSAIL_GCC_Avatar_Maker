using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShotCapturer : MonoBehaviour {

    private void OnPostRender()
    {
        if (AvatarCreatorContext.takeScreenShot)
        {
            Debug.Log("ScreenShotCapturer:OnPostRender: Capturing screenshot.");

            var width = 400;
            var height = 500;
            var startX = 100;
            var startY = 200;
            var tex = new Texture2D(width, height, TextureFormat.RGB24, false);

            tex.ReadPixels(new Rect(startX, startY, width, height), 0, 0);
            tex.Apply();

            // Encode texture into PNG
            AvatarCreatorContext.fileTransferManager.DownloadScreenshot(tex.EncodeToPNG());

            Destroy(tex);

            AvatarCreatorContext.takeScreenShot = false;
        }
    }
}
