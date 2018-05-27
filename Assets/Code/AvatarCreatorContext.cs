using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class AvatarCreatorContext : MonoBehaviour {

    public static AssetType selectedAssetType { get; set; }
    public static Dictionary<AssetType, List<CBaseAsset>> m_assets = new Dictionary<AssetType, List<CBaseAsset>>();
    public static FaceObjectController faceObject;

    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(this.gameObject);

        faceObject = GameObject.Find("FaceObject").GetComponent<FaceObjectController>();
        selectedAssetType = AssetType.None;
        
        // No genders are TODO. Depends to the provided content.
        InitAssets("Assets/Resources/FaceObject/fo_faceshape/", AssetType.HeadShape, AssetGender.NoGender);
        InitAssets("Assets/Resources/FaceObject/fo_hair/male/", AssetType.Hair, AssetGender.Male);
        InitAssets("Assets/Resources/FaceObject/fo_hair/female/", AssetType.Hair, AssetGender.Female);
        InitAssets("Assets/Resources/FaceObject/fo_ears/", AssetType.Ears, AssetGender.NoGender);
        InitAssets("Assets/Resources/FaceObject/fo_eyes/", AssetType.Eyes, AssetGender.NoGender);
        InitAssets("Assets/Resources/FaceObject/fo_eyebrows/", AssetType.Eyebrows, AssetGender.NoGender);
        InitAssets("Assets/Resources/FaceObject/fo_glasses/", AssetType.Glasses, AssetGender.NoGender);
        InitAssets("Assets/Resources/FaceObject/fo_facedetail/", AssetType.FaceTexture, AssetGender.NoGender);
        InitAssets("Assets/Resources/FaceObject/fo_nose/", AssetType.Nose, AssetGender.NoGender);
        InitAssets("Assets/Resources/FaceObject/fo_moustache/", AssetType.Moustache, AssetGender.Male);
        InitAssets("Assets/Resources/FaceObject/fo_beard/", AssetType.Beard, AssetGender.Male);
        //InitAssets("Assets/Resources/FaceObject/fo_mouth/male/", AssetType.Mouth, AssetGender.Male);          TODO
        //InitAssets("Assets/Resources/FaceObject/fo_mouth/female/", AssetType.Mouth, AssetGender.Female);      TODO
    }

    private void InitAssets(string directoryPath, AssetType assetType, AssetGender assetGender)
    {
        Debug.Log("AvatarCreatorContext:InitAssets: " + directoryPath + " " + assetType + " " + assetGender);

        string filepattern = "*.png";
        if (assetType == AssetType.Hair)
            filepattern = "*a.png";

        CBaseAssetFactory assetFactory = new CBaseAssetFactory();
        List<CBaseAsset> assets = new List<CBaseAsset>();

        DirectoryInfo dir = new DirectoryInfo(directoryPath);
        FileInfo[] info = dir.GetFiles(filepattern);

        foreach (FileInfo finfo in info)
        {
            assets.Add(assetFactory.CreateAsset(assetType, assetGender, directoryPath.Replace("Assets/Resources/", "") + finfo.Name));
        }

        if(m_assets.ContainsKey(assetType))
            m_assets[assetType].AddRange(assets);
        else
            m_assets.Add(assetType, assets);
    }

    public static Dictionary<AssetType, List<CBaseAsset>> GetLoadedAssets()
    {
        return m_assets;
    }

    public static List<CBaseAsset> GetLoadedAssetsByType(AssetType type)
    {
        return m_assets[type];
    }

    public static List<CBaseAsset> GetLoadedAssetsByTypeAndGender(AssetType type, AssetGender gender)
    {
        List<CBaseAsset> returnList = new List<CBaseAsset>();
        
        foreach(CBaseAsset asset in m_assets[type])
        {
            if (asset.GetGender() == gender)
                returnList.Add(asset);
        }

        return returnList;
    }
}
