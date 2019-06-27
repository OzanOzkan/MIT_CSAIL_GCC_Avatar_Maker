using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Xml;
using System.Text;
using UnityEngine.UI;

/// <summary>
/// A class for FaceObject.
/// Implements the functionality of FaceObject. Attached to FaceObject game object in the scene.
/// </summary>
public class FaceObjectController : MonoBehaviour
{
    /* Transforms are Unity components instantiated in scene.
     * The Transform component determines the Position, Rotation, and Scale of each object in the scene. Every GameObject has a Transform.
     * FaceObject consists of several Transform objects, dedicated for face parts such as head shape, ears, hair, eyes, eyebrows etc.
     * Transform objects responsible of rendering assets using their Image component.
     * Loaded asset objects stores their sprite objects. Those objects will be passed to respective transform's Image component.
     * Position, rotation and scale manupilation also applied to Transform object directly.
    */

    // Asset objects of the face.
    public Dictionary<AssetType, Transform> m_transforms;

    // Current selected asset for manipulation.
    private CBaseAsset m_currentAsset;

    /// <summary>
    /// Used to initialize variables. Called once by Unity before the scene starts.
    /// </summary>
    private void Awake()
    {
        Transform mask = gameObject.transform.Find("fo_mask");

        // GameObject mappings: Asset types and respective transforms.
        m_transforms = new Dictionary<AssetType, Transform>();
        m_transforms.Add(AssetType.HeadShape, mask.Find("fo_faceshape"));
        m_transforms.Add(AssetType.Ears, mask.Find("fo_ears"));
        m_transforms.Add(AssetType.Hair, mask.Find("fo_hair_front"));
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
        m_transforms.Add(AssetType.Ghutra, gameObject.transform.Find("fo_ghutra_front"));
        m_transforms.Add(AssetType.BackgroundTexture, gameObject.transform.parent.Find("bg_texture"));
    }

    /// <summary>
    /// Update function, called every frame. 
    /// </summary>
    private void Update()
    {
        // Manage visibility of empty sprites. (Empty sprites displays as white planes)
        foreach(Transform currentTransform in transform)
        {
            ManageSpriteVisibility(currentTransform);
        }
    }

    /// <summary>
    /// Generates a random avatar.
    /// </summary>
    public void GenerateRandomAvatar()
    {
        // Head shape
        List<CBaseAsset> tempAssets = AvatarCreatorContext.GetLoadedAssetsByType(AssetType.HeadShape);
        SetFaceObjectPart(tempAssets[Random.Range(0, tempAssets.Count)], false);

        // Ears
        tempAssets = AvatarCreatorContext.GetLoadedAssetsByType(AssetType.Ears);
        SetFaceObjectPart(tempAssets[Random.Range(0, tempAssets.Count)], false);

        // Hair
        tempAssets = AvatarCreatorContext.GetLoadedAssetsByType(AssetType.Hair);
        SetFaceObjectPart(tempAssets[Random.Range(0, tempAssets.Count)], false);

        // Eyes
        tempAssets = AvatarCreatorContext.GetLoadedAssetsByType(AssetType.Eyes);
        SetFaceObjectPart(tempAssets[Random.Range(0, tempAssets.Count)], false);

        // Eyebrows
        tempAssets = AvatarCreatorContext.GetLoadedAssetsByType(AssetType.Eyebrows);
        SetFaceObjectPart(tempAssets[Random.Range(0, tempAssets.Count)], false);

        // Nose
        tempAssets = AvatarCreatorContext.GetLoadedAssetsByType(AssetType.Nose);
        SetFaceObjectPart(tempAssets[Random.Range(0, tempAssets.Count)], false);

        // Lips
        tempAssets = AvatarCreatorContext.GetLoadedAssetsByType(AssetType.Mouth);
        SetFaceObjectPart(tempAssets[Random.Range(0, tempAssets.Count)], false);

        // Moustache - Removed due to request
        //tempAssets = AvatarCreatorContext.GetLoadedAssetsByType(AssetType.Moustache);
        //SetFaceObjectPart(tempAssets[Random.Range(0, tempAssets.Count)], false);

        // Beard - Removed due to request
        //tempAssets = AvatarCreatorContext.GetLoadedAssetsByType(AssetType.Beard);
        //SetFaceObjectPart(tempAssets[Random.Range(0, tempAssets.Count)], false);

        // Body
        tempAssets = AvatarCreatorContext.GetLoadedAssetsByType(AssetType.Body);
        SetFaceObjectPart(tempAssets[Random.Range(0, tempAssets.Count)], false);

        //// Facetexture
        //tempAssets = AvatarCreatorContext.GetLoadedAssetsByType(AssetType.FaceTexture);
        //SetFaceObjectPart(tempAssets[Random.Range(0, tempAssets.Count)], false);

        // Skin color
        AvatarCreatorContext.selectedAssetType = AssetType.HeadShape;
        List<Color> colorList = AvatarCreatorContext.GetPaletteColors(ColorPalette.Skin);
        Color skinColor = colorList[Random.Range(0, colorList.Count)];
        ChangeAssetColor(skinColor);

        // FaceTexture
        AvatarCreatorContext.selectedAssetType = AssetType.FaceTexture;
        ChangeAssetColor(skinColor);
        if(m_transforms[AssetType.FaceTexture].GetComponent<Image>().sprite != null)
        {
            tempAssets = AvatarCreatorContext.GetLoadedAssetsByType(AssetType.FaceTexture);
            SetFaceObjectPart(tempAssets[Random.Range(0, tempAssets.Count)], false);
        }

        // Hair color
        AvatarCreatorContext.selectedAssetType = AssetType.Hair;
        colorList = AvatarCreatorContext.GetPaletteColors(ColorPalette.Default);
        Color color = colorList[Random.Range(0, colorList.Count)];
        ChangeAssetColor(color);

        // Beard color
        AvatarCreatorContext.selectedAssetType = AssetType.Beard;
        ChangeAssetColor(color);

        // Moustache color
        AvatarCreatorContext.selectedAssetType = AssetType.Moustache;
        ChangeAssetColor(color);

        // Eyebrows color
        AvatarCreatorContext.selectedAssetType = AssetType.Eyebrows;
        ChangeAssetColor(color);

        // Background
        tempAssets = AvatarCreatorContext.GetLoadedAssetsByType(AssetType.BackgroundTexture);
        AvatarCreatorContext.selectedAssetType = AssetType.BackgroundTexture;
        SetFaceObjectPart(tempAssets[Random.Range(0, tempAssets.Count)], false);
        ChangeAssetColor(colorList[Random.Range(0, colorList.Count)]);

        // Default selected category
        AvatarCreatorContext.selectedAssetType = AssetType.Body;
    }

    /// <summary>
    /// Manages visibility of a transform.
    /// If there is no asset attached to the transform, it means it is deleted. 
    /// Transform without an asset will be hidden until a new asset will be loaded in the runtime by the user.
    /// </summary>
    /// <param name="root">Transform object of the root asset.</param>
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

    /// <summary>
    /// Loads an asset object to the transform for displaying it on the face.
    /// </summary>
    /// <param name="asset">An asset object.</param>
    /// <param name="isUserAction">True: Loaded by user. False: Loaded by codebase.</param>
    public void SetFaceObjectPart(CBaseAsset asset, bool isUserAction=true)
    {
        // If asset parameter is null, we will manupilate the last selected asset.
        if (asset == null)
            asset = m_currentAsset;

        // If asset is invalid, return.
        if (asset.GetAssetType() == AssetType.None)
            return;

        // Get transforms of the specified asset's type from FaceObject.
        Transform currentTransform = m_transforms[asset.GetAssetType()];

        // If current asset type is Hair, that means currently used asset is a Hair asset.
        if (asset.GetAssetType() == AssetType.Hair)
        {
            // Get transforms of Hair from FaceObject.
            Transform hairFront = currentTransform;
            Transform hairBack = currentTransform.parent.Find("fo_hair_back");

            // If current asset has Front part, assign it with the selected realism level.
            if (asset.GetSprites().ContainsKey(SpritePart.Front))
                hairFront.GetComponent<Image>().sprite = asset.GetSprites()[SpritePart.Front][(int)AvatarCreatorContext.currentRealismLevel];

            // If current asset has Back part, assign it with the selected realism level.
            if (asset.GetSprites().ContainsKey(SpritePart.Back))
            {
                hairBack.GetComponent<Image>().sprite = asset.GetSprites()[SpritePart.Back][(int)AvatarCreatorContext.currentRealismLevel];
                ChangeAssetColor(hairFront.GetComponent<Image>().color);
            }
            else
            {
                hairBack.GetComponent<Image>().sprite = null;
            }

            // Remove ghutra. (We need to display the hair, so ghutra is not needed)
            if(asset.GetSprites()[SpritePart.Front][(int)AvatarCreatorContext.currentRealismLevel] == null)
            {
                m_transforms[AssetType.Ghutra].GetComponent<Image>().sprite = null;
                m_transforms[AssetType.Ghutra].parent.Find("fo_ghutra_back").GetComponent<Image>().sprite = null;
            }


        }
        // If current asset type is Eyebrows or Ears
        else if (asset.GetAssetType() == AssetType.Eyebrows
                    || asset.GetAssetType() == AssetType.Ears)
        {
            // Assign selected asset.
            for (int i = 0; i < currentTransform.childCount; ++i)
            {
                currentTransform.GetChild(i).GetComponent<Image>().sprite = asset.GetSprites()[SpritePart.Default][(int)AvatarCreatorContext.currentRealismLevel];
            }
        }
        // If current asset type is Eyes.
        else if (asset.GetAssetType() == AssetType.Eyes)
        {
            // Get transforms of left and right eyes.
            Transform eye_left = currentTransform.Find("fo_eye_left");
            Transform eye_right = currentTransform.Find("fo_eye_right");

            // Assign assets for left and right eye. 
            eye_left.Find("fo_eye_left_L0").GetComponent<Image>().sprite = asset.GetSprites()[SpritePart.Reserved][(int)AvatarCreatorContext.currentRealismLevel];
            eye_left.Find("fo_eye_left_L1").GetComponent<Image>().sprite = asset.GetSprites()[SpritePart.Back][(int)AvatarCreatorContext.currentRealismLevel];
            eye_left.Find("fo_eye_left_L2").GetComponent<Image>().sprite = asset.GetSprites()[SpritePart.Default][(int)AvatarCreatorContext.currentRealismLevel];
            eye_left.Find("fo_eye_left_L3").GetComponent<Image>().sprite = asset.GetSprites()[SpritePart.Front][(int)AvatarCreatorContext.currentRealismLevel];

            eye_right.Find("fo_eye_right_L0").GetComponent<Image>().sprite = asset.GetSprites()[SpritePart.Reserved][(int)AvatarCreatorContext.currentRealismLevel];
            eye_right.Find("fo_eye_right_L1").GetComponent<Image>().sprite = asset.GetSprites()[SpritePart.Back][(int)AvatarCreatorContext.currentRealismLevel];
            eye_right.Find("fo_eye_right_L2").GetComponent<Image>().sprite = asset.GetSprites()[SpritePart.Default][(int)AvatarCreatorContext.currentRealismLevel];
            eye_right.Find("fo_eye_right_L3").GetComponent<Image>().sprite = asset.GetSprites()[SpritePart.Front][(int)AvatarCreatorContext.currentRealismLevel];
        }
        // If current asset type is Mouth
        else if (asset.GetAssetType() == AssetType.Mouth)
        {
            // Assign assets for mouth.
            currentTransform.Find("fo_mouth_L1").GetComponent<Image>().sprite = asset.GetSprites()[SpritePart.Back][(int)AvatarCreatorContext.currentRealismLevel];
            currentTransform.Find("fo_mouth_L2").GetComponent<Image>().sprite = asset.GetSprites()[SpritePart.Default][(int)AvatarCreatorContext.currentRealismLevel];
        }
        // If current asset type is Body
        else if(asset.GetAssetType() == AssetType.Body)
        {
            // Assign assets for body.
            currentTransform.Find("fo_body_L1").GetComponent<Image>().sprite = asset.GetSprites()[SpritePart.Back][(int)AvatarCreatorContext.currentRealismLevel];
            currentTransform.Find("fo_body_L2").GetComponent<Image>().sprite = asset.GetSprites()[SpritePart.Default][(int)AvatarCreatorContext.currentRealismLevel];

            m_transforms[AssetType.Body].gameObject.SetActive(true);
            m_transforms[AssetType.SpecialBody].gameObject.SetActive(false);
            gameObject.transform.Find("fo_mask").gameObject.GetComponent<Mask>().enabled = false;
        }
        else if(asset.GetAssetType() == AssetType.SpecialBody)
        {
            if (asset.GetSprites().ContainsKey(SpritePart.Back))
                currentTransform.Find("fo_specialbody_L1").GetComponent<Image>().sprite = asset.GetSprites()[SpritePart.Back][(int)AvatarCreatorContext.currentRealismLevel];
            else
                currentTransform.Find("fo_specialbody_L1").GetComponent<Image>().sprite = null;

            if (asset.GetSprites().ContainsKey(SpritePart.Default))
                currentTransform.Find("fo_specialbody_L2").GetComponent<Image>().sprite = asset.GetSprites()[SpritePart.Default][(int)AvatarCreatorContext.currentRealismLevel];
            else
                currentTransform.Find("fo_specialbody_L2").GetComponent<Image>().sprite = null;

            if (asset.GetSprites().ContainsKey(SpritePart.Front))
                currentTransform.Find("fo_specialbody_L3").GetComponent<Image>().sprite = asset.GetSprites()[SpritePart.Front][(int)AvatarCreatorContext.currentRealismLevel];
            else
                currentTransform.Find("fo_specialbody_L3").GetComponent<Image>().sprite = null;

            m_transforms[AssetType.Body].gameObject.SetActive(false);
            m_transforms[AssetType.SpecialBody].gameObject.SetActive(true);
            gameObject.transform.Find("fo_mask").gameObject.GetComponent<Mask>().enabled = true;
        }
        else if(asset.GetAssetType() == AssetType.Ghutra)
        {
            currentTransform.GetComponent<Image>().sprite = asset.GetSprites()[SpritePart.Front][0];
            currentTransform.parent.Find("fo_ghutra_back").GetComponent<Image>().sprite = asset.GetSprites()[SpritePart.Back][0];
        }
        else if (asset.GetAssetType() == AssetType.Beard 
                || asset.GetAssetType() == AssetType.Moustache)
        {
            if (asset.GetSprites()[SpritePart.Default][(int)AvatarCreatorContext.currentRealismLevel] == null)
            {
                if (asset.GetAssetType() == AssetType.Beard)
                {
                    currentTransform.GetComponent<Image>().sprite = null;
                    currentTransform.parent.Find("fo_mask").Find("fo_moustache").GetComponent<Image>().sprite = null;
                }
                else
                {
                    currentTransform.GetComponent<Image>().sprite = null;
                    currentTransform.parent.parent.Find("fo_beard").GetComponent<Image>().sprite = null;
                }
            }
            else
            {
                currentTransform.GetComponent<Image>().sprite = asset.GetSprites()[SpritePart.Default][(int)AvatarCreatorContext.currentRealismLevel];
            }
        }
        else
        {
            currentTransform.GetComponent<Image>().sprite = asset.GetSprites()[SpritePart.Default][(int)AvatarCreatorContext.currentRealismLevel];
        }

        m_currentAsset = asset;
    }

    /// <summary>
    /// Compares given transform's current loaded asset with given asset and if it is the same asset, removes it.
    /// </summary>
    /// <param name="currentObject">Transform to compare.</param>
    /// <param name="currentAsset">Asset to compare.</param>
    /// <returns>True if transform's asset removed. False if it is not.</returns>
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

    /// <summary>
    /// Moves selected asset in horizontal and vertical directions.
    /// </summary>
    /// <param name="modifyFlag">Modification flag. Vertical or Horizontal</param>
    /// <param name="isPositiveRate">True: Increment on move value. False: Decrement on move value.</param>
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

        if (AvatarCreatorContext.selectedAssetType == AssetType.Hair)
            currentObject.parent.Find("fo_hair_back").localPosition = tempPos;
        else if (AvatarCreatorContext.selectedAssetType == AssetType.Ghutra)
            currentObject.parent.Find("fo_ghutra_back").localPosition = tempPos;
    }

    /// <summary>
    /// Resizes or streches selected currently selected asset.
    /// </summary>
    /// <param name="modifyFlag">Modification flag. Resize, StrechHorizontal or StrechVertical.</param>
    /// <param name="isPositiveRate">Ture: Increment on resize/strech. False: Decrement on resize/strech.</param>
    public void ResizeAsset(AssetModifyFlag modifyFlag, bool isPositiveRate)
    {
        Transform currentObject = m_transforms[AvatarCreatorContext.selectedAssetType];

        // Predefined values.
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

    /// <summary>
    /// Sets distance between currently selected asset pairs like eyes, ears.
    /// </summary>
    /// <param name="direction">True: Right. False: Left</param>
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

    /// <summary>
    /// Rotates currently selected asset.
    /// </summary>
    /// <param name="direction">True: To right. False: To left</param>
    public void RotateAsset(bool direction)
    {
        Transform currentObject = m_transforms[AvatarCreatorContext.selectedAssetType];

        // Predefined value.
        //float maxAngle = 30f;
        float rotateOffset = 1f;

        Transform left = currentObject.transform.GetChild(0);
        Transform right = currentObject.transform.GetChild(1);

        if (direction) // to right
        {
                right.Rotate(0, 0, -rotateOffset);
                left.rotation = Quaternion.Inverse(right.localRotation);
        }
        else // to left
        {
                left.Rotate(0, 0, -rotateOffset);
                right.rotation = Quaternion.Inverse(left.localRotation);
        }
    }

    /// <summary>
    /// Changes currently selected asset's color.
    /// </summary>
    /// <param name="color">New color.</param>
    /// <param name="currentObject">Transform object. Uses last selected object if it is not supplied.</param>
    public void ChangeAssetColor(Color color, Transform currentObject=null)
    {
        // If transform object is not supplied, use last selected asset.
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

                    m_transforms[AssetType.SpecialBody].transform.Find("fo_specialbody_L2").GetComponent<Image>().color = color;

                    m_transforms[AssetType.Eyes].Find("fo_eye_left").Find("fo_eye_left_L0").GetComponent<Image>().color = color;
                    m_transforms[AssetType.Eyes].Find("fo_eye_right").Find("fo_eye_right_L0").GetComponent<Image>().color = color;
                    break;
                }
            case AssetType.Mouth:
                {
                    currentObject.Find("fo_mouth_L1").GetComponent<Image>().color = color;
                    break;
                }
            case AssetType.Hair:
                {
                    currentObject.GetComponent<Image>().color = color;
                    currentObject.parent.Find("fo_hair_back").GetComponent<Image>().color = color;
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
            case AssetType.FaceTexture:
                {
                    m_transforms[AssetType.FaceTexture].GetComponent<Image>().color = color;
                    break;
                }
            case AssetType.SpecialBody:
                {
                    m_transforms[AssetType.SpecialBody].transform.Find("fo_specialbody_L3").GetComponent<Image>().color = color;
                    break;
                }
            case AssetType.BackgroundTexture:
                {
                    m_transforms[AssetType.BackgroundTexture].GetComponent<Image>().color = color;
                    AvatarCreatorContext.bgColor = color;
                    break;
                }
        }

        AvatarCreatorContext.logManager.LogAction("AssetColorChanged", ColorUtility.ToHtmlStringRGB(color));
    }

    /// <summary>
    /// Serializes FaceObject.
    /// </summary>
    /// <returns></returns>
    public string Serialize()
    {
        Debug.Log("FaceObjectController:Serialize()");

        StringBuilder content = new StringBuilder();
        content.Append("<FaceObject>\n");
        content.Append(PrepareSerializeLine(gameObject.transform));
        content.Append(SerializeSubObjects(gameObject.transform));
        content.Append("</FaceObject>");

        return content.ToString();
    }

    /// <summary>
    /// Serializes sub-objects.
    /// </summary>
    /// <param name="node">Root transform object to serialize.</param>
    /// <returns></returns>
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

    /// <summary>
    /// Prepares serialization entry of given transform object in XML format.
    /// </summary>
    /// <param name="transform">Transform object to serialize.</param>
    /// <returns></returns>
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

    /// <summary>
    /// Deserializes given data and loads it to FaceObject.
    /// </summary>
    /// <param name="data">Serialized XML data.</param>
    public void Unserialize(string data)
    {
        Debug.Log("FaceObjectController:Unserialize()\n" + data);

        // Initialization
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
        }
    }
}
