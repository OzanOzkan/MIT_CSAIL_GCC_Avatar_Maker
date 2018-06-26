using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Xml;
using System.Text;
using UnityEngine.UI;

public class FaceObjectController : MonoBehaviour
{
    public Dictionary<AssetType, Transform> m_transforms;

    private void Awake()
    {
        // GameObject mappings.
        m_transforms = new Dictionary<AssetType, Transform>();
        m_transforms.Add(AssetType.HeadShape, gameObject.transform.Find("fo_faceshape"));
        m_transforms.Add(AssetType.Ears, gameObject.transform.Find("fo_ears"));
        m_transforms.Add(AssetType.Hair, gameObject.transform);
        m_transforms.Add(AssetType.Eyes, gameObject.transform.Find("fo_eyes"));
        m_transforms.Add(AssetType.Eyebrows, gameObject.transform.Find("fo_eyebrows"));
        m_transforms.Add(AssetType.Glasses, gameObject.transform.Find("fo_glasses"));
        m_transforms.Add(AssetType.FaceTexture, gameObject.transform.Find("fo_facedetail"));
        m_transforms.Add(AssetType.Nose, gameObject.transform.Find("fo_nose"));
        m_transforms.Add(AssetType.Moustache, gameObject.transform.Find("fo_moustache"));
        m_transforms.Add(AssetType.Beard, gameObject.transform.Find("fo_beard"));
        m_transforms.Add(AssetType.Mouth, gameObject.transform.Find("fo_mouth"));
        m_transforms.Add(AssetType.Body, gameObject.transform.Find("fo_body"));
    }

    private void Update()
    {
        // Manage visibility of empty sprites. (Empty sprites displays as white planes)
        foreach(Transform currentTransform in m_transforms.Values)
        {
            ManageSpriteVisibility(currentTransform);
        }
    }

    public void GenerateRandomAvatar()
    {
        // Random face generation. TODO: Gender corrections.
        AssetGender randomGender = (AssetGender)Random.Range(0, 2);

        List<CBaseAsset> tempAssets = AvatarCreatorContext.GetLoadedAssetsByType(AssetType.HeadShape);
        SetFaceObjectPart(tempAssets[Random.Range(0, tempAssets.Count)], false);

        tempAssets = AvatarCreatorContext.GetLoadedAssetsByType(AssetType.Ears);
        SetFaceObjectPart(tempAssets[Random.Range(0, tempAssets.Count)], false);

        //   tempAssets = AvatarCreatorContext.GetLoadedAssetsByTypeAndGender(AssetType.Hair, randomGender);
        tempAssets = AvatarCreatorContext.GetLoadedAssetsByType(AssetType.Hair);
        SetFaceObjectPart(tempAssets[Random.Range(0, tempAssets.Count)], false);

        //tempAssets = AvatarCreatorContext.GetLoadedAssetsByType(AssetType.Eyes);
        //SetFaceObjectPart(tempAssets[Random.Range(0, tempAssets.Count)], false);

        tempAssets = AvatarCreatorContext.GetLoadedAssetsByType(AssetType.Eyebrows);
        SetFaceObjectPart(tempAssets[Random.Range(0, tempAssets.Count)], false);

        tempAssets = AvatarCreatorContext.GetLoadedAssetsByType(AssetType.Nose);
        SetFaceObjectPart(tempAssets[Random.Range(0, tempAssets.Count)], false);
    }

    private void ManageSpriteVisibility(Transform root)
    {
        if (root.childCount == 0)
        {
            if (root.GetComponent<Image>().sprite == null)
                root.gameObject.SetActive(false);
            else
                root.gameObject.SetActive(true);

            return;
        }
        else
        {
            for (int i = 0; i < root.childCount; ++i)
            {
                ManageSpriteVisibility(root.GetChild(i));
            }
        }
    }

    public void SetFaceObjectPart(CBaseAsset asset, bool isUserAction=true)
    {
        if (asset == null)
            return;

        Transform currentTransform = m_transforms[asset.GetAssetType()];

        if (isUserAction)
        {
            //if (CheckPreviousAssetAndRemove(currentTransform, asset.GetSprites()[SpritePart.Default][0]))
            //    return;
        }

        if (asset.GetAssetType() == AssetType.Hair)
        {
            Transform hairFront = currentTransform.Find("fo_hair_front");
            Transform hairBack = currentTransform.Find("fo_hair_back");

            hairFront.GetComponent<Image>().sprite = asset.GetSprites()[SpritePart.Front][(int)AvatarCreatorContext.currentRealismLevel];

            if (asset.GetSprites().ContainsKey(SpritePart.Back))
            {
                hairBack.GetComponent<Image>().sprite = asset.GetSprites()[SpritePart.Back][(int)AvatarCreatorContext.currentRealismLevel];
                ChangeAssetColor(hairFront.GetComponent<Image>().color);
            }
            else
            {
                hairBack.GetComponent<Image>().sprite = null;
            }

        }
        else if (asset.GetAssetType() == AssetType.Eyebrows
                    || asset.GetAssetType() == AssetType.Ears)
        {
            for (int i = 0; i < currentTransform.childCount; ++i)
            {
                currentTransform.GetChild(i).GetComponent<Image>().sprite = asset.GetSprites()[SpritePart.Default][(int)AvatarCreatorContext.currentRealismLevel];
            }
        }
        else if (asset.GetAssetType() == AssetType.Eyes)
        {
            Transform eye_left = currentTransform.Find("fo_eye_left");
            Transform eye_right = currentTransform.Find("fo_eye_right");

            eye_left.Find("L1").GetComponent<Image>().sprite = asset.GetSprites()[SpritePart.Back][(int)AvatarCreatorContext.currentRealismLevel];
            eye_left.Find("L2").GetComponent<Image>().sprite = asset.GetSprites()[SpritePart.Default][(int)AvatarCreatorContext.currentRealismLevel];
            eye_left.Find("L3").GetComponent<Image>().sprite = asset.GetSprites()[SpritePart.Front][(int)AvatarCreatorContext.currentRealismLevel];

            eye_right.Find("L1").GetComponent<Image>().sprite = asset.GetSprites()[SpritePart.Back][(int)AvatarCreatorContext.currentRealismLevel];
            eye_right.Find("L2").GetComponent<Image>().sprite = asset.GetSprites()[SpritePart.Default][(int)AvatarCreatorContext.currentRealismLevel];
            eye_right.Find("L3").GetComponent<Image>().sprite = asset.GetSprites()[SpritePart.Front][(int)AvatarCreatorContext.currentRealismLevel];
        }
        else if (asset.GetAssetType() == AssetType.Mouth)
        {
            currentTransform.Find("L1").GetComponent<Image>().sprite = asset.GetSprites()[SpritePart.Back][(int)AvatarCreatorContext.currentRealismLevel];
            currentTransform.Find("L2").GetComponent<Image>().sprite = asset.GetSprites()[SpritePart.Default][(int)AvatarCreatorContext.currentRealismLevel];

            //if (currentTransform.Find("L1").GetComponent<Image>().sprite == null)
            //    currentTransform.Find("L1").gameObject.SetActive(false);
            //else
            //    currentTransform.Find("L1").gameObject.SetActive(true);

            //if (currentTransform.Find("L2").GetComponent<Image>().sprite == null)
            //    currentTransform.Find("L2").gameObject.SetActive(false);
            //else
            //    currentTransform.Find("L2").gameObject.SetActive(true);
        }
        else if(asset.GetAssetType() == AssetType.Body)
        {
            currentTransform.Find("L1").GetComponent<Image>().sprite = asset.GetSprites()[SpritePart.Back][(int)AvatarCreatorContext.currentRealismLevel];
            currentTransform.Find("L2").GetComponent<Image>().sprite = asset.GetSprites()[SpritePart.Default][(int)AvatarCreatorContext.currentRealismLevel];
        }
        else
        {
            currentTransform.GetComponent<Image>().sprite = asset.GetSprites()[SpritePart.Default][(int)AvatarCreatorContext.currentRealismLevel];
        }
    }

    private bool CheckPreviousAssetAndRemove(Transform currentObject, Sprite currentAsset)
    {
        if (currentObject.childCount > 0)
        {
            if (currentObject.GetChild(0).GetComponent<Image>().sprite == currentAsset)
            {
                for (int i = 0; i < currentObject.childCount; ++i)
                {
                    currentObject.GetChild(i).GetComponent<Image>().sprite = null;
                }

                return true;
            }
        }
        else
        {
            if (currentObject.GetComponent<Image>().sprite == currentAsset)
            {
                currentObject.GetComponent<Image>().sprite = null;
                return true;
            }
        }

        return false;
    }

    public void MoveAsset(AssetModifyFlag modifyFlag, bool isPositiveRate)
    {
        Transform currentObject = m_transforms[AvatarCreatorContext.selectedAssetType];

        float maxVertical = 9f;
        float maxHorizontal = 5f;

        float moveOffset = 0.5f;
        Vector3 tempPos = currentObject.localPosition;

        if(modifyFlag == AssetModifyFlag.MoveVertical)
        {
            if (isPositiveRate)
            {
                if (tempPos.y < maxVertical)
                    tempPos.y += moveOffset;
            }
            else
            {
                if(tempPos.y > -maxVertical)
                    tempPos.y -= moveOffset;
            }
        }
        else
        {
            if (isPositiveRate)
            {
                if (tempPos.x < maxHorizontal)
                    tempPos.x += moveOffset;
            }
            else
            {
                if (tempPos.x > -maxHorizontal)
                    tempPos.x -= moveOffset;
            }
        }

        currentObject.localPosition = tempPos;
    }

    public void ResizeAsset(AssetModifyFlag modifyFlag, bool isPositiveRate)
    {
        Transform currentObject = m_transforms[AvatarCreatorContext.selectedAssetType];

        // get current size
        // calculate new size
        // check if it is bigger or smaller than %10 of original.
        // if not, do it.

        Vector3 maxScale = new Vector3(1.2f, 1.2f, 0);
        Vector3 minScale = new Vector3(0.8f, 0.8f, 0);
        float resizeOffset = 0.05f;

        if(modifyFlag == AssetModifyFlag.Resize)
        {
            if (isPositiveRate)
            {
                if(currentObject.localScale.x < maxScale.x && currentObject.localScale.y < maxScale.y)
                    currentObject.localScale = new Vector3(currentObject.localScale.x + resizeOffset, currentObject.localScale.y + resizeOffset, 0);
            }
            else
            {
                if(currentObject.localScale.x > minScale.x && currentObject.localScale.y > minScale.y)
                    currentObject.localScale = new Vector3(currentObject.localScale.x - resizeOffset, currentObject.localScale.y - resizeOffset, 0);
            }
        }
        else if(modifyFlag == AssetModifyFlag.StretchHorizontal)
        {
            if (isPositiveRate)
            {
                if (currentObject.localScale.x < maxScale.x)
                    currentObject.localScale = new Vector3(currentObject.localScale.x + resizeOffset, currentObject.localScale.y, 0);
            }
            else
            {
                if(currentObject.localScale.x > minScale.x)
                    currentObject.localScale = new Vector3(currentObject.localScale.x - resizeOffset, currentObject.localScale.y, 0);
            }
        }
        else if(modifyFlag == AssetModifyFlag.StretchVertical)
        {
            if (isPositiveRate)
            {
                if (currentObject.localScale.y < maxScale.y)
                    currentObject.localScale = new Vector3(currentObject.localScale.x, currentObject.localScale.y + resizeOffset, 0);
            }
            else
            {
                if(currentObject.localScale.y > minScale.y)
                    currentObject.localScale = new Vector3(currentObject.localScale.x, currentObject.localScale.y - resizeOffset, 0);
            }
        }
    }

    // false: left, true: right
    public void SetDistance(bool direction)
    {
        Transform currentObject = m_transforms[AvatarCreatorContext.selectedAssetType];

        float distanceOffset = 1f;

        Transform left = currentObject.transform.GetChild(0);
        Transform right = currentObject.transform.GetChild(1);

        if (direction)
        {
            left.position = new Vector3(left.position.x - distanceOffset, left.position.y, 0);
            right.position = new Vector3(right.position.x + distanceOffset, right.position.y, 0);
        }
        else
        {
            left.position = new Vector3(left.position.x + distanceOffset, left.position.y, 0);
            right.position = new Vector3(right.position.x - distanceOffset, right.position.y, 0);
        }
    }

    public void RotateAsset(bool direction)
    {
        Transform currentObject = m_transforms[AvatarCreatorContext.selectedAssetType];

      //  float maxAngle = 30f;
        float rotateOffset = 1f;

        Transform left = currentObject.transform.GetChild(0);
        Transform right = currentObject.transform.GetChild(1);

       // Debug.Log("Left: " + left.localEulerAngles.z + " Right:" + right.localEulerAngles.z);

        if (direction) // right
        {
                right.Rotate(0, 0, -rotateOffset);
                left.rotation = Quaternion.Inverse(right.localRotation);

            //left.Rotate(0, 0, rotateOffset);
            //right.Rotate(0, 0, -rotateOffset);
        }
        else // left
        {
                left.Rotate(0, 0, -rotateOffset);
                right.rotation = Quaternion.Inverse(left.localRotation);

            //left.Rotate(0, 0, -rotateOffset);
            //right.Rotate(0, 0, rotateOffset);
        }
    }

    public void ChangeAssetColor(Color color)
    {
        Transform currentObject = m_transforms[AvatarCreatorContext.selectedAssetType];

        switch (AvatarCreatorContext.selectedAssetType)
        {
            case AssetType.HeadShape:
                {
                    m_transforms[AssetType.HeadShape].GetComponent<Image>().color = color;

                    m_transforms[AssetType.Ears].transform.Find("fo_ear_left").GetComponent<Image>().color = color;
                    m_transforms[AssetType.Ears].transform.Find("fo_ear_right").GetComponent<Image>().color = color;
               
                    m_transforms[AssetType.Nose].GetComponent<Image>().color = color;
                    break;
                }
            case AssetType.Mouth:
                {
                    currentObject.Find("L1").GetComponent<Image>().color = color;
                    break;
                }
            case AssetType.Hair:
                {
                    currentObject.Find("fo_hair_front").GetComponent<Image>().color = color;
                    currentObject.Find("fo_hair_back").GetComponent<Image>().color = color;
                    break;
                }
            case AssetType.Moustache:
            case AssetType.Beard:
                {
                    m_transforms[AssetType.Moustache].GetComponent<Image>().color = color;
                    m_transforms[AssetType.Beard].GetComponent<Image>().color = color;
                    break;
                }
            case AssetType.Eyes:
                {
                    currentObject.Find("fo_eye_left").Find("L2").GetComponent<Image>().color = color;
                    currentObject.Find("fo_eye_right").Find("L2").GetComponent<Image>().color = color;
                    break;
                }
            case AssetType.Eyebrows:
                {
                    currentObject.Find("fo_eyebrow_left").GetComponent<Image>().color = color;
                    currentObject.Find("fo_eyebrow_right").GetComponent<Image>().color = color;
                    break;
                }
        }        

        //if (currentObject.childCount > 0)
        //{
        //    for (int i = 0; i < currentObject.childCount; ++i)
        //    {
        //        currentObject.GetChild(i).GetComponent<Image>().color = color;
        //    }
        //}
        //else
        //{
        //    currentObject.GetComponent<Image>().color = color;
        //}
    }

    public void SetRealismLevel(int level)
    {

    }

    public string Serialize()
    {
        Debug.Log("FaceObjectController:Serialize()");

        StringBuilder content = new StringBuilder();
        content.Append("<FaceObject>\n");

        foreach (KeyValuePair<AssetType, Transform> fObject in m_transforms)
        {
            if (fObject.Value.childCount > 0)
            {
                for (int i = 0; i < fObject.Value.childCount; ++i)
                {
                    content.Append(PrepareSerializeLine(fObject.Key, fObject.Value.GetChild(i)));
                }
            }
            else
            {
                content.Append(PrepareSerializeLine(fObject.Key, fObject.Value));
            }
        }

        content.Append("</FaceObject>");

        return content.ToString();
    }

    private string PrepareSerializeLine(AssetType type, Transform transform)
    {
        string returnStr = "<Object ";
        returnStr += "name=\"" + transform.name + "\" ";

        if (transform.GetComponent<Image>().sprite != null)
            returnStr += "asset=\"" + transform.GetComponent<Image>().sprite.name + "\" ";
        else
            returnStr += "asset=\"\" ";

        returnStr += "type=\"" + type + "\" ";
        returnStr += "/>\n";

        return returnStr;
    }

    public void Unserialize(string data)
    {
        Debug.Log("FaceObjectController:Unserialize()\n" + data);

        AssetType assetType = AssetType.None;
        string assetName = "";

        XmlDocument doc = new XmlDocument();
        doc.LoadXml(data);

        XmlNode root = doc.DocumentElement.SelectSingleNode("/SaveFile/FaceObject");
        foreach (XmlNode node in root.ChildNodes)
        {
            foreach (XmlAttribute attr in node.Attributes)
            {
                if (attr.Name == "asset")
                    assetName = attr.Value;
                else if (attr.Name == "type")
                    assetType = (AssetType)System.Enum.Parse(typeof(AssetType), attr.Value);
            }

            CBaseAsset asset = AvatarCreatorContext.FindAssetByName(assetType, assetName);

            if (asset == null)
                asset = new CBaseAsset(AssetGender.NoGender, assetType, 0, "");

            SetFaceObjectPart(asset, false);
        }
    }
}
