using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShotCapturer : MonoBehaviour {

    private void OnPostRender()
    {
        if (AvatarCreatorContext.takeScreenShot)
        {
            Debug.Log("ScreenShotCapturer:OnPostRender: Capturing screenshot.");

            GameObject ssareagameobject = GameObject.Find("screenshot_area");
            Sprite ssarea = ssareagameobject.GetComponent<SpriteRenderer>().sprite;

            var width = System.Convert.ToInt32(ssarea.bounds.extents.x * 2);
            var height = System.Convert.ToInt32(ssarea.bounds.extents.y * 2);
            var startX = System.Convert.ToInt32(ssareagameobject.transform.position.x - ssarea.bounds.extents.x);
            var startY = System.Convert.ToInt32(ssareagameobject.transform.position.y + ssarea.bounds.extents.y);
            var tex = new Texture2D(width, height, TextureFormat.RGB24, false);

            tex.ReadPixels(new Rect(startX, startY, width, height), 0, 0);
            tex.Apply();

            // Encode texture into PNG
            AvatarCreatorContext.fileTransferManager.DownloadScreenshot(tex.EncodeToPNG());

            Destroy(tex);

            AvatarCreatorContext.takeScreenShot = false;
        }
    }

    private void OnGUI()
    {

        //GameObject ssareagameobject = GameObject.Find("screenshot_area");
        //Sprite ssarea = ssareagameobject.GetComponent<SpriteRenderer>().sprite;

        //var width = System.Convert.ToInt32(ssarea.bounds.extents.x * 2);
        //var height = System.Convert.ToInt32(ssarea.bounds.extents.y * 2);
        //var startX = System.Convert.ToInt32(ssareagameobject.transform.position.x - ssarea.bounds.extents.x);
        //var startY = System.Convert.ToInt32(ssareagameobject.transform.position.y + ssarea.bounds.extents.y);
        //var tex = new Texture2D(width, height, TextureFormat.RGB24, false);

       
    }
}
