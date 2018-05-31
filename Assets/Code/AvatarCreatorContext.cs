using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.UI;

public class AvatarCreatorContext : MonoBehaviour {

    public static AssetType selectedAssetType { get; set; }
    public static Dictionary<AssetType, List<CBaseAsset>> m_assets = new Dictionary<AssetType, List<CBaseAsset>>();
    public static FaceObjectController faceObject;
    public static LogManager logManager;
    public static bool takeScreenShot = false;
    public static GUID sessionguid;

    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(this.gameObject);

        sessionguid = GUID.Generate();
        GameObject.Find("txt_guid").GetComponent<Text>().text = sessionguid.ToString();

        logManager = gameObject.GetComponent<LogManager>();

        faceObject = GameObject.Find("FaceObject").GetComponent<FaceObjectController>();
        selectedAssetType = AssetType.None;
        
        // No genders are TODO. Depends to the provided content.
        InitAssets("FaceObject/fo_faceshape/", AssetType.HeadShape, AssetGender.NoGender);
        InitAssets("FaceObject/fo_hair/male/", AssetType.Hair, AssetGender.Male);
        InitAssets("FaceObject/fo_hair/female/", AssetType.Hair, AssetGender.Female);
        InitAssets("FaceObject/fo_ears/", AssetType.Ears, AssetGender.NoGender);
        InitAssets("FaceObject/fo_eyes/", AssetType.Eyes, AssetGender.NoGender);
        InitAssets("FaceObject/fo_eyebrows/", AssetType.Eyebrows, AssetGender.NoGender);
        InitAssets("FaceObject/fo_glasses/", AssetType.Glasses, AssetGender.NoGender);
        InitAssets("FaceObject/fo_facedetail/", AssetType.FaceTexture, AssetGender.NoGender);
        InitAssets("FaceObject/fo_nose/", AssetType.Nose, AssetGender.NoGender);
        InitAssets("FaceObject/fo_moustache/", AssetType.Moustache, AssetGender.Male);
        InitAssets("FaceObject/fo_beard/", AssetType.Beard, AssetGender.Male);
        //InitAssets("Assets/Resources/FaceObject/fo_mouth/male/", AssetType.Mouth, AssetGender.Male);          TODO
        //InitAssets("Assets/Resources/FaceObject/fo_mouth/female/", AssetType.Mouth, AssetGender.Female);      TODO
    }

    private void InitAssets(string directoryPath, AssetType assetType, AssetGender assetGender)
    {
        Debug.Log("AvatarCreatorContext:InitAssets: " + directoryPath + " " + assetType + " " + assetGender);

        CBaseAssetFactory assetFactory = new CBaseAssetFactory();
        List<CBaseAsset> assets = new List<CBaseAsset>();

        Object[] sprites = Resources.LoadAll(directoryPath, typeof(Sprite));
        foreach(Sprite sprite in sprites)
        {
            string spritename = sprite.name + ".png";
            if (spritename.Contains("b.png")) // TODO: This check is not good... Figure out later.
                continue;

            assets.Add(assetFactory.CreateAsset(assetType, assetGender, directoryPath + spritename));
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
