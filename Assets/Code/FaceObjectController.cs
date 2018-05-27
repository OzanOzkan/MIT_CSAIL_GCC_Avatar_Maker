using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class FaceObjectController : MonoBehaviour
{
    public Dictionary<AssetType, Transform> m_transforms;

    private void Start()
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

        // Random face generation. TODO: Gender corrections.
        AssetGender randomGender = (AssetGender)Random.Range(0, 1);

        List<CBaseAsset> tempAssets = AvatarCreatorContext.GetLoadedAssetsByType(AssetType.HeadShape);
        SetFaceObjectPart(tempAssets[Random.Range(0, tempAssets.Count - 1)]);

        tempAssets = AvatarCreatorContext.GetLoadedAssetsByType(AssetType.Ears);
        SetFaceObjectPart(tempAssets[Random.Range(0, tempAssets.Count - 1)]);

        tempAssets = AvatarCreatorContext.GetLoadedAssetsByTypeAndGender(AssetType.Hair, randomGender);
        SetFaceObjectPart(tempAssets[Random.Range(0, tempAssets.Count - 1)]);

        tempAssets = AvatarCreatorContext.GetLoadedAssetsByType(AssetType.Eyes);
        SetFaceObjectPart(tempAssets[Random.Range(0, tempAssets.Count - 1)]);

        tempAssets = AvatarCreatorContext.GetLoadedAssetsByType(AssetType.Eyebrows);
        SetFaceObjectPart(tempAssets[Random.Range(0, tempAssets.Count - 1)]);

        tempAssets = AvatarCreatorContext.GetLoadedAssetsByType(AssetType.Nose);
        SetFaceObjectPart(tempAssets[Random.Range(0, tempAssets.Count - 1)]);
    }

    public void SetFaceObjectPart(CBaseAsset asset)
    {
        Transform currentTransform = m_transforms[asset.GetAssetType()];

        if (CheckPreviousAssetAndRemove(currentTransform, asset.GetSprites()[SpritePart.Default]))
            return;

        if (asset.GetAssetType() == AssetType.Hair)
        {
            currentTransform.transform.Find("fo_hair_back").GetComponent<SpriteRenderer>().sprite = asset.GetSprites()[SpritePart.Back];
            currentTransform.transform.Find("fo_hair_front").GetComponent<SpriteRenderer>().sprite = asset.GetSprites()[SpritePart.Front];
        }
        else if (asset.GetAssetType() == AssetType.Eyebrows
                    || asset.GetAssetType() == AssetType.Eyes
                    || asset.GetAssetType() == AssetType.Ears)
        {
            for (int i = 0; i < currentTransform.childCount; ++i)
            {
                currentTransform.GetChild(i).GetComponent<SpriteRenderer>().sprite = asset.GetSprites()[SpritePart.Default];
            }
        }
        else
        {
            currentTransform.GetComponent<SpriteRenderer>().sprite = asset.GetSprites()[SpritePart.Default];
        }
    }

    private bool CheckPreviousAssetAndRemove(Transform currentObject, Sprite currentAsset)
    {
        if (currentObject.childCount > 0)
        {
            if (currentObject.GetChild(0).GetComponent<SpriteRenderer>().sprite == currentAsset)
            {
                for (int i = 0; i < currentObject.childCount; ++i)
                {
                    currentObject.GetChild(i).GetComponent<SpriteRenderer>().sprite = null;
                }

                return true;
            }
        }
        else
        {
            if (currentObject.GetComponent<SpriteRenderer>().sprite == currentAsset)
            {
                currentObject.GetComponent<SpriteRenderer>().sprite = null;
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
                currentObject.GetChild(i).GetComponent<SpriteRenderer>().color = color;
            }
        }
        else
        {
            currentObject.GetComponent<SpriteRenderer>().color = color;
        }
    }

    private void SetSkinColor(Color color)
    {
        m_transforms[AssetType.HeadShape].GetComponent<SpriteRenderer>().color = color;

        m_transforms[AssetType.Ears].transform.Find("fo_ear_left").GetComponent<SpriteRenderer>().color = color;
        m_transforms[AssetType.Ears].transform.Find("fo_ear_right").GetComponent<SpriteRenderer>().color = color;

        m_transforms[AssetType.Nose].GetComponent<SpriteRenderer>().color = color;
    }
}
