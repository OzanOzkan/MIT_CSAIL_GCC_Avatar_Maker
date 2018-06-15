using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenShotCapturer : MonoBehaviour {

    private void OnPostRender()
    {
        if (AvatarCreatorContext.takeScreenShot)
        {
            Debug.Log("ScreenShotCapturer:OnPostRender: Capturing screenshot.");

            GameObject ssareagameobject = GameObject.Find("frame_screenshot");
            Sprite ssarea = ssareagameobject.GetComponent<Image>().sprite;

            Debug.Log("Width: " + ssareagameobject.GetComponent<RectTransform>().sizeDelta.x + " Height: " + ssareagameobject.GetComponent<RectTransform>().sizeDelta.y);

            float minX = ssareagameobject.GetComponent<RectTransform>().position.x + ssareagameobject.GetComponent<RectTransform>().rect.xMin;
            float maxY = ssareagameobject.GetComponent<RectTransform>().position.y + ssareagameobject.GetComponent<RectTransform>().rect.yMax;
            float z = ssareagameobject.GetComponent<RectTransform>().position.z;
            Vector3 topLeft = new Vector3(minX, maxY, z);

            Camera cam = GameObject.Find("Main Camera").GetComponent<Camera>();
            

            Matrix4x4 a = ssareagameobject.GetComponent<RectTransform>().localToWorldMatrix;


            //Debug.Log(System.Convert.ToInt32(ssarea.bounds.extents.x * 2) + " " + System.Convert.ToInt32(ssarea.bounds.extents.y * 2));
            //Debug.Log(ssareagameobject.transform.position.x - ssarea.bounds.extents.x + " " + ssareagameobject.transform.position.y + ssarea.bounds.extents.y);

            var width = System.Convert.ToInt32(ssareagameobject.GetComponent<RectTransform>().sizeDelta.x); //System.Convert.ToInt32(ssarea.bounds.extents.x * 2);
            var height = System.Convert.ToInt32(ssareagameobject.GetComponent<RectTransform>().sizeDelta.y); //System.Convert.ToInt32(ssarea.bounds.extents.y * 2);
            var startX = -7.8f; //System.Convert.ToInt32(ssareagameobject.transform.position.x - ssarea.bounds.extents.x);
            var startY = 11.3f; //System.Convert.ToInt32(ssareagameobject.transform.position.y + ssarea.bounds.extents.y);
            var tex = new Texture2D(width, height, TextureFormat.RGB24, false);

            Vector3 asd = new Vector3(startX, startY, 0);
            Debug.Log("Topleft: " + cam.WorldToScreenPoint(asd));


            tex.ReadPixels(new Rect(cam.WorldToScreenPoint(asd).x, cam.WorldToScreenPoint(asd).y, width, height), 0, 0);
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
