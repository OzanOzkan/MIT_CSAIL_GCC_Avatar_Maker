﻿using System.Collections;
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
        Transform mask = gameObject.transform.Find("fo_mask");
        // GameObject mappings.
        m_transforms = new Dictionary<AssetType, Transform>();
        m_transforms.Add(AssetType.HeadShape, mask.Find("fo_faceshape"));
        m_transforms.Add(AssetType.Ears, mask.Find("fo_ears"));
        m_transforms.Add(AssetType.Hair, mask);
        m_transforms.Add(AssetType.Eyes, mask.Find("fo_eyes"));
        m_transforms.Add(AssetType.Eyebrows, mask.Find("fo_eyebrows"));
        m_transforms.Add(AssetType.Glasses, mask.Find("fo_glasses"));
        m_transforms.Add(AssetType.FaceTexture, mask.Find("fo_facedetail"));
        m_transforms.Add(AssetType.Nose, mask.Find("fo_nose"));
        m_transforms.Add(AssetType.Moustache, mask.Find("fo_moustache"));
        m_transforms.Add(AssetType.Beard, gameObject.transform.Find("fo_beard"));
        m_transforms.Add(AssetType.Mouth, mask.Find("fo_mouth"));
        m_transforms.Add(AssetType.Body, gameObject.transform.Find("fo_body"));
        m_transforms.Add(AssetType.SpecialBody, gameObject.transform.Find("fo_specialbody"));
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
                root.GetComponent<Image>().enabled = false;
            else
                root.GetComponent<Image>().enabled = true;

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

        if (asset.GetAssetType() == AssetType.None)
            return;

        Transform currentTransform = m_transforms[asset.GetAssetType()];

        if (asset.GetAssetType() == AssetType.Hair)
        {
            Transform hairFront = currentTransform.Find("fo_hair_front");
            Transform hairBack = currentTransform.Find("fo_hair_back");

            if (asset.GetSprites().ContainsKey(SpritePart.Front))
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

            eye_left.Find("fo_eye_left_L1").GetComponent<Image>().sprite = asset.GetSprites()[SpritePart.Back][(int)AvatarCreatorContext.currentRealismLevel];
            eye_left.Find("fo_eye_left_L2").GetComponent<Image>().sprite = asset.GetSprites()[SpritePart.Default][(int)AvatarCreatorContext.currentRealismLevel];
            eye_left.Find("fo_eye_left_L3").GetComponent<Image>().sprite = asset.GetSprites()[SpritePart.Front][(int)AvatarCreatorContext.currentRealismLevel];

            eye_right.Find("fo_eye_right_L1").GetComponent<Image>().sprite = asset.GetSprites()[SpritePart.Back][(int)AvatarCreatorContext.currentRealismLevel];
            eye_right.Find("fo_eye_right_L2").GetComponent<Image>().sprite = asset.GetSprites()[SpritePart.Default][(int)AvatarCreatorContext.currentRealismLevel];
            eye_right.Find("fo_eye_right_L3").GetComponent<Image>().sprite = asset.GetSprites()[SpritePart.Front][(int)AvatarCreatorContext.currentRealismLevel];
        }
        else if (asset.GetAssetType() == AssetType.Mouth)
        {
            currentTransform.Find("fo_mouth_L1").GetComponent<Image>().sprite = asset.GetSprites()[SpritePart.Back][(int)AvatarCreatorContext.currentRealismLevel];
            currentTransform.Find("fo_mouth_L2").GetComponent<Image>().sprite = asset.GetSprites()[SpritePart.Default][(int)AvatarCreatorContext.currentRealismLevel];
        }
        else if(asset.GetAssetType() == AssetType.Body)
        {
            currentTransform.Find("fo_body_L1").GetComponent<Image>().sprite = asset.GetSprites()[SpritePart.Back][(int)AvatarCreatorContext.currentRealismLevel];
            currentTransform.Find("fo_body_L2").GetComponent<Image>().sprite = asset.GetSprites()[SpritePart.Default][(int)AvatarCreatorContext.currentRealismLevel];

            m_transforms[AssetType.Body].gameObject.SetActive(true);
            m_transforms[AssetType.SpecialBody].gameObject.SetActive(false);
            gameObject.transform.Find("fo_mask").gameObject.GetComponent<Mask>().enabled = false;
        }
        else if(asset.GetAssetType() == AssetType.SpecialBody)
        {
            if (asset.GetSprites().ContainsKey(SpritePart.Front))
                currentTransform.Find("fo_specialbody_L1").GetComponent<Image>().sprite = asset.GetSprites()[SpritePart.Front][(int)AvatarCreatorContext.currentRealismLevel];
            else
                currentTransform.Find("fo_specialbody_L1").GetComponent<Image>().sprite = null;

            if (asset.GetSprites().ContainsKey(SpritePart.Back))
                currentTransform.Find("fo_specialbody_L2").GetComponent<Image>().sprite = asset.GetSprites()[SpritePart.Back][(int)AvatarCreatorContext.currentRealismLevel];
            else
                currentTransform.Find("fo_specialbody_L2").GetComponent<Image>().sprite = null;

            m_transforms[AssetType.Body].gameObject.SetActive(false);
            m_transforms[AssetType.SpecialBody].gameObject.SetActive(true);
            gameObject.transform.Find("fo_mask").gameObject.GetComponent<Mask>().enabled = true;
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

        float moveOffset = 0.2f;
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

        Vector3 maxScale = new Vector3(1.2f, 1.2f, 0);
        Vector3 minScale = new Vector3(0.8f, 0.8f, 0);
        float resizeOffset = 0.02f;

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

        float distanceOffset = 0.3f;

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
        float rotateOffset = 0.3f;

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

    public void ChangeAssetColor(Color color, Transform currentObject=null)
    {
        if (currentObject == null)
            currentObject = m_transforms[AvatarCreatorContext.selectedAssetType];


        switch (AvatarCreatorContext.selectedAssetType)
        {
            case AssetType.HeadShape:
            case AssetType.Ears:
            case AssetType.Nose:
                {
                    m_transforms[AssetType.HeadShape].GetComponent<Image>().color = color;

                    m_transforms[AssetType.Ears].transform.Find("fo_ear_left").GetComponent<Image>().color = color;
                    m_transforms[AssetType.Ears].transform.Find("fo_ear_right").GetComponent<Image>().color = color;
               
                    m_transforms[AssetType.Nose].GetComponent<Image>().color = color;

                    m_transforms[AssetType.Body].transform.Find("fo_body_L2").GetComponent<Image>().color = color;

                    m_transforms[AssetType.SpecialBody].transform.Find("fo_specialbody_L1").GetComponent<Image>().color = color;
                    break;
                }
            case AssetType.Mouth:
                {
                    currentObject.Find("fo_mouth_L1").GetComponent<Image>().color = color;
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
                    currentObject.Find("fo_eye_left").Find("fo_eye_left_L2").GetComponent<Image>().color = color;
                    currentObject.Find("fo_eye_right").Find("fo_eye_right_L2").GetComponent<Image>().color = color;
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

        content.Append(SerializeSubObjects(GameObject.Find("FaceObject").transform));

        //foreach (KeyValuePair<AssetType, Transform> fObject in m_transforms)
        //{
        //    if (fObject.Value.childCount > 0)
        //    {
        //        for (int i = 0; i < fObject.Value.childCount; ++i)
        //        {
        //            content.Append(PrepareSerializeLine(fObject.Key, fObject.Value.GetChild(i)));
        //        }
        //    }
        //    else
        //    {
        //        content.Append(PrepareSerializeLine(fObject.Key, fObject.Value));
        //    }
        //}

        content.Append("</FaceObject>");

        return content.ToString();
    }

    private string SerializeSubObjects(Transform node)
    {
        string returnVal = "";

        if (node.childCount > 0)
        {
            for (int i = 0; i < node.childCount; ++i)
            {
                 returnVal += SerializeSubObjects(node.GetChild(i));
            }
        }
        else
        {
            returnVal += PrepareSerializeLine(node);
        }

        return returnVal;
    }

    private string PrepareSerializeLine(Transform transform)
    {
        string returnStr = "<Object ";
        returnStr += "name=\"" + transform.name + "\" ";

        if (transform.GetComponent<Image>() != null && transform.GetComponent<Image>().sprite != null)
            returnStr += "asset=\"" + transform.GetComponent<Image>().sprite.name + "\" ";
        else
            returnStr += "asset=\"\" ";

        //returnStr += "type=\"" + type + "\" ";
        returnStr += "posx=\"" + transform.position.x + "\" posy=\"" + transform.position.y + "\" posz=\"" + transform.position.z + "\" ";
        returnStr += "rotx=\"" + transform.rotation.x + "\" roty=\"" + transform.rotation.y + "\" rotz=\"" + transform.rotation.z + "\" rotw=\"" + transform.rotation.w + "\" ";
        returnStr += "scalex=\"" + transform.localScale.x + "\" scaley=\"" + transform.localScale.y + "\" scalez=\"" + transform.localScale.z + "\" ";

        if(transform.GetComponent<Image>() != null)
            returnStr += "color=\"" + ColorUtility.ToHtmlStringRGB(transform.GetComponent<Image>().color) + "\" ";

        returnStr += "/>\n";

        return returnStr;
    }

    public void Unserialize(string data)
    {
        Debug.Log("FaceObjectController:Unserialize()\n" + data);

        AssetType assetType = AssetType.None;
        string objectName = "";
        string assetName = "";
        Vector3 position = new Vector3();
        Quaternion rotation = new Quaternion();
        Vector3 scale = new Vector3();
        Color color = new Color();
        
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(data);

        XmlNode root = doc.DocumentElement.SelectSingleNode("/SaveFile/FaceObject");
        foreach (XmlNode node in root.ChildNodes)
        {
            foreach (XmlAttribute attr in node.Attributes)
            {
                switch (attr.Name)
                {
                    case "name":
                        {
                            objectName = attr.Value;
                            break;
                        }
                    case "asset":
                        {
                            assetName = attr.Value;
                            break;
                        }
                    case "type":
                        {
                            assetType = (AssetType)System.Enum.Parse(typeof(AssetType), attr.Value);
                            break;
                        }
                    case "posx":
                        {
                            position.x = float.Parse(attr.Value);
                            break;
                        }
                    case "posy":
                        {
                            position.y = float.Parse(attr.Value);
                            break;
                        }
                    case "posz":
                        {
                            position.z = float.Parse(attr.Value);
                            break;
                        }
                    case "rotx":
                        {
                            rotation.x = float.Parse(attr.Value);
                            break;
                        }
                    case "roty":
                        {
                            rotation.y = float.Parse(attr.Value);
                            break;
                        }
                    case "rotz":
                        {
                            rotation.z = float.Parse(attr.Value);
                            break;
                        }
                    case "rotw":
                        {
                            rotation.w = float.Parse(attr.Value);
                            break;
                        }
                    case "scalex":
                        {
                            scale.x = float.Parse(attr.Value);
                            break;
                        }
                    case "scaley":
                        {
                            scale.y = float.Parse(attr.Value);
                            break;
                        }
                    case "scalez":
                        {
                            scale.z = float.Parse(attr.Value);
                            break;
                        }
                    case "color":
                        {
                            ColorUtility.TryParseHtmlString("#"+attr.Value, out color);
                            break;
                        }
                }
            }

            Debug.Log(objectName + " " + assetName);

            if (assetName == "")
                continue;

            CBaseAsset asset = AvatarCreatorContext.FindAssetByName(assetName);

            if (asset == null)
                asset = new CBaseAsset(AssetGender.NoGender, assetType, 0, "");

            //AvatarCreatorContext.selectedAssetType = asset.GetAssetType();

            GameObject gObject = GameObject.Find(objectName);
            if (gObject)
            {
                gObject.transform.position = position;
                gObject.transform.rotation = rotation;
                gObject.transform.localScale = scale;

                if (gObject.GetComponent<Image>() != null)
                    gObject.GetComponent<Image>().color = color;
            }

            SetFaceObjectPart(asset, false);

            //Transform selectedAsset = m_transforms[assetType];

           // ChangeAssetColor(color, gameObject.transform);
        }
    }
}
