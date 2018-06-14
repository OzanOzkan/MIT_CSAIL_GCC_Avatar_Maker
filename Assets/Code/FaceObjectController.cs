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
        m_transforms.Add(AssetType.Hair, gameObject.transform.Find("fo_hair"));
        m_transforms.Add(AssetType.Eyes, gameObject.transform.Find("fo_eyes"));
        m_transforms.Add(AssetType.Eyebrows, gameObject.transform.Find("fo_eyebrows"));
        m_transforms.Add(AssetType.Glasses, gameObject.transform.Find("fo_glasses"));
        m_transforms.Add(AssetType.FaceTexture, gameObject.transform.Find("fo_facedetail"));
        m_transforms.Add(AssetType.Nose, gameObject.transform.Find("fo_nose"));
        m_transforms.Add(AssetType.Moustache, gameObject.transform.Find("fo_moustache"));
        m_transforms.Add(AssetType.Beard, gameObject.transform.Find("fo_beard"));
        //m_transforms.Add(AssetType.Mouth, gameObject.transform.Find("fo_mouth"));
        // TODO: Body depends to provided assets.
    }

    public void GenerateRandomAvatar()
    {
        // Random face generation. TODO: Gender corrections.
        AssetGender randomGender = (AssetGender)Random.Range(0, 2);

        List<CBaseAsset> tempAssets = AvatarCreatorContext.GetLoadedAssetsByType(AssetType.HeadShape);
        SetFaceObjectPart(tempAssets[Random.Range(0, tempAssets.Count)], false);

        tempAssets = AvatarCreatorContext.GetLoadedAssetsByType(AssetType.Ears);
        SetFaceObjectPart(tempAssets[Random.Range(0, tempAssets.Count)], false);

        tempAssets = AvatarCreatorContext.GetLoadedAssetsByTypeAndGender(AssetType.Hair, randomGender);
        SetFaceObjectPart(tempAssets[Random.Range(0, tempAssets.Count)], false);

        tempAssets = AvatarCreatorContext.GetLoadedAssetsByType(AssetType.Eyes);
        SetFaceObjectPart(tempAssets[Random.Range(0, tempAssets.Count)], false);

        tempAssets = AvatarCreatorContext.GetLoadedAssetsByType(AssetType.Eyebrows);
        SetFaceObjectPart(tempAssets[Random.Range(0, tempAssets.Count)], false);

        tempAssets = AvatarCreatorContext.GetLoadedAssetsByType(AssetType.Nose);
        SetFaceObjectPart(tempAssets[Random.Range(0, tempAssets.Count)], false);
    }

    public void SetFaceObjectPart(CBaseAsset asset, bool isUserAction=true)
    {
        if (asset == null)
            return;

        Transform currentTransform = m_transforms[asset.GetAssetType()];

        if (isUserAction)
        {
            if (CheckPreviousAssetAndRemove(currentTransform, asset.GetSprites()[SpritePart.Default]))
                return;
        }

        if (asset.GetAssetType() == AssetType.Hair)
        {
            currentTransform.transform.Find("fo_hair_back").GetComponent<Image>().sprite = asset.GetSprites()[SpritePart.Back];
            currentTransform.transform.Find("fo_hair_front").GetComponent<Image>().sprite = asset.GetSprites()[SpritePart.Front];
        }
        else if (asset.GetAssetType() == AssetType.Eyebrows
                    || asset.GetAssetType() == AssetType.Eyes
                    || asset.GetAssetType() == AssetType.Ears)
        {
            for (int i = 0; i < currentTransform.childCount; ++i)
            {
                currentTransform.GetChild(i).GetComponent<Image>().sprite = asset.GetSprites()[SpritePart.Default];
            }
        }
        else
        {
            currentTransform.GetComponent<Image>().sprite = asset.GetSprites()[SpritePart.Default];
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

        float moveOffset = 0.5f;
        Vector3 tempPos = currentObject.position;

        if(modifyFlag == AssetModifyFlag.MoveVertical)
        {
            if (isPositiveRate)
                tempPos.y += moveOffset;
            else
                tempPos.y -= moveOffset;
        }
        else
        {
            if (isPositiveRate)
                tempPos.x += moveOffset;
            else
                tempPos.x -= moveOffset;
        }

        currentObject.position = tempPos;
    }

    public void ResizeAsset(AssetModifyFlag modifyFlag, bool isPositiveRate)
    {
        Transform currentObject = m_transforms[AvatarCreatorContext.selectedAssetType];

        float resizeOffset = 0.1f;

        if(modifyFlag == AssetModifyFlag.Resize)
        {
            if (isPositiveRate)
                currentObject.localScale = new Vector3(currentObject.localScale.x + resizeOffset, currentObject.localScale.y + resizeOffset, 0);
            else
                currentObject.localScale = new Vector3(currentObject.localScale.x - resizeOffset, currentObject.localScale.y - resizeOffset, 0);
        }
        else if(modifyFlag == AssetModifyFlag.StretchHorizontal)
        {
            if(isPositiveRate)
                currentObject.localScale = new Vector3(currentObject.localScale.x + resizeOffset, currentObject.localScale.y, 0);
            else
                currentObject.localScale = new Vector3(currentObject.localScale.x - resizeOffset, currentObject.localScale.y, 0);
        }
        else if(modifyFlag == AssetModifyFlag.StretchVertical)
        {
            if(isPositiveRate)
                currentObject.localScale = new Vector3(currentObject.localScale.x, currentObject.localScale.y + resizeOffset, 0);
            else
                currentObject.localScale = new Vector3(currentObject.localScale.x, currentObject.localScale.y - resizeOffset, 0);
        }
    }

    public void ChangeAssetColor(Color color)
    {
        if (AvatarCreatorContext.selectedAssetType == AssetType.HeadShape)
        {
            SetSkinColor(color);
            return;
        }

        Transform currentObject = m_transforms[AvatarCreatorContext.selectedAssetType];

        if (currentObject.childCount > 0)
        {
            for (int i = 0; i < currentObject.childCount; ++i)
            {
                currentObject.GetChild(i).GetComponent<Image>().color = color;
            }
        }
        else
        {
            currentObject.GetComponent<Image>().color = color;
        }
    }

    private void SetSkinColor(Color color)
    {
        m_transforms[AssetType.HeadShape].GetComponent<Image>().color = color;

        m_transforms[AssetType.Ears].transform.Find("fo_ear_left").GetComponent<Image>().color = color;
        m_transforms[AssetType.Ears].transform.Find("fo_ear_right").GetComponent<Image>().color = color;

        m_transforms[AssetType.Nose].GetComponent<Image>().color = color;
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
