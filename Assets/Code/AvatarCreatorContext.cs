using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.UI;
using System.Xml;
using System.Text;

public enum ColorPalette
{
    Default = 0,
    Skin
}

public class AvatarCreatorContext : MonoBehaviour {

    public static AssetType selectedAssetType { get; set; }
    public static RealismLevel currentRealismLevel { get; set; }
    public static Dictionary<AssetType, List<CBaseAsset>> m_assets = new Dictionary<AssetType, List<CBaseAsset>>();
    public static FaceObjectController faceObject;
    public static LogManager logManager;
    public static FileTransferManager fileTransferManager;
    public static bool takeScreenShot = false;
    public static System.Guid sessionguid;

    public static Transform defaultColorPalette;
    public static Transform skinColorPalette;

    // Use this for initialization
    void Start () {
        DontDestroyOnLoad(this.gameObject);

        sessionguid = System.Guid.NewGuid();
        GameObject.Find("txt_guid").GetComponent<Text>().text = sessionguid.ToString();

        logManager = gameObject.GetComponent<LogManager>();
        fileTransferManager = gameObject.GetComponent<FileTransferManager>();

        faceObject = GameObject.Find("FaceObject").GetComponent<FaceObjectController>();
        selectedAssetType = AssetType.None;

        // Assets
        InitAssets("FaceObject/fo_faceshape/", AssetType.HeadShape, AssetGender.NoGender);
        InitAssets("FaceObject/fo_hair/", AssetType.Hair, AssetGender.NoGender);
        InitAssets("FaceObject/fo_ears/", AssetType.Ears, AssetGender.NoGender);
        InitAssets("FaceObject/fo_eyes/", AssetType.Eyes, AssetGender.NoGender);
        InitAssets("FaceObject/fo_eyebrows/", AssetType.Eyebrows, AssetGender.NoGender);
        InitAssets("FaceObject/fo_glasses/", AssetType.Glasses, AssetGender.NoGender);
        InitAssets("FaceObject/fo_facedetail/", AssetType.FaceTexture, AssetGender.NoGender);
        InitAssets("FaceObject/fo_nose/", AssetType.Nose, AssetGender.NoGender);
        InitAssets("FaceObject/fo_moustache/", AssetType.Moustache, AssetGender.NoGender);
        InitAssets("FaceObject/fo_beard/", AssetType.Beard, AssetGender.NoGender);
        InitAssets("FaceObject/fo_mouth/", AssetType.Mouth, AssetGender.NoGender);
        InitAssets("FaceObject/fo_body/", AssetType.Body, AssetGender.NoGender);
        InitAssets("FaceObject/fo_specialbody/", AssetType.SpecialBody, AssetGender.Female);
        InitAssets("FaceObject/fo_ghutra/", AssetType.Ghutra, AssetGender.Male);

        // Set the default selected category as Body.
        GameObject.Find("btn_body").GetComponent<UIAssetCategoryButtonController>().OnButtonClick();

        // Color palette management.
        defaultColorPalette = GameObject.Find("colorpalette_default").transform;
        defaultColorPalette.gameObject.SetActive(false);
        skinColorPalette = GameObject.Find("colorpalette_skin").transform;
        skinColorPalette.gameObject.SetActive(false);
        FillColorPalette();

        // Random avatar generation on the first run.
        faceObject.GenerateRandomAvatar();
    }

    private void Update()
    {
        // Manage color palette.
        switch(selectedAssetType)
        {
            case AssetType.HeadShape:
            case AssetType.Ears:
            case AssetType.Nose:
            case AssetType.FaceTexture:
                {
                    skinColorPalette.gameObject.SetActive(true);
                    defaultColorPalette.gameObject.SetActive(false);
                    break;
                }
            default:
                {
                    defaultColorPalette.gameObject.SetActive(true);
                    skinColorPalette.gameObject.SetActive(false);
                    break;
                }
        }
    }

    private void InitAssets(string directoryPath, AssetType assetType, AssetGender assetGender)
    {
        Debug.Log("AvatarCreatorContext:InitAssets: " + directoryPath + " " + assetType + " " + assetGender);

        CBaseAssetFactory assetFactory = new CBaseAssetFactory();
        List<CBaseAsset> assets = new List<CBaseAsset>();

        Object[] sprites = Resources.LoadAll(directoryPath, typeof(Sprite));
        string lastLoadedSpriteName = "";

        // Add empty asset in order to delete the already selected one.
        if(assetType == AssetType.Hair || assetType == AssetType.Eyebrows || assetType == AssetType.Glasses
            || assetType == AssetType.FaceTexture || assetType == AssetType.Moustache || assetType == AssetType.Beard)
            assets.Add(assetFactory.CreateAsset(assetType, assetGender, ""));

        foreach(Sprite sprite in sprites)
        {
            string currentSpriteName = sprite.name.Split('_')[0];
            if (lastLoadedSpriteName == currentSpriteName)
                continue;

            assets.Add(assetFactory.CreateAsset(assetType, assetGender, directoryPath + currentSpriteName));
            lastLoadedSpriteName = currentSpriteName;
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

        foreach (CBaseAsset asset in m_assets[type])
        {
            if (asset.GetGender() == gender)
                returnList.Add(asset);
        }

        return returnList;
    }

    public static CBaseAsset FindAssetByName(string name)
    {
        if (name == "")
            return null;

        foreach (List<CBaseAsset> currentAssetlist in m_assets.Values)
        {
            foreach (CBaseAsset currentAsset in currentAssetlist)
            {
                foreach (KeyValuePair<SpritePart, List<Sprite>> sprites in currentAsset.GetSprites())
                {
                    if (sprites.Value[0] != null && sprites.Value[0].name == name)
                        return currentAsset;
                }
            }
        }

        return null;
    }

    public static CBaseAsset FindAssetByName(AssetType type, string name)
    {
        if (name == "")
            return null;

        foreach (CBaseAsset currentAsset in m_assets[type])
        {
            foreach (KeyValuePair<SpritePart, List<Sprite>> sprites in currentAsset.GetSprites())
            {
                if (sprites.Value[0] != null && sprites.Value[0].name == name)
                    return currentAsset;
            }
        }

        return null;
    }

    public static void SaveAvatarToFile()
    {
        string savedata = logManager.DumpLogs();
        fileTransferManager.DownloadSaveFile(savedata);
    }

    public static string SerializeUIFields()
    {
        StringBuilder returnString = new StringBuilder();
        returnString.Append("<AvatarInfo>\n");
        returnString.Append("<Info name=\"" + GameObject.Find("inputfield_avatarname").GetComponent<InputField>().text + "\"/>\n");
        returnString.Append("<Info dob=\"" + GameObject.Find("dropdown_dob").GetComponent<Dropdown>().value.ToString() + "\"/>\n");
        returnString.Append("<Info gender=\"" + GameObject.Find("dropdown_gender").GetComponent<Dropdown>().value.ToString() + "\"/>\n");
        returnString.Append("<Info bio=\"" + GameObject.Find("inputfield_info").GetComponent<InputField>().text + "\"/>\n");
        returnString.Append("</AvatarInfo>\n");

        return returnString.ToString();
    }

    public static void UnserializeUIFields(string data)
    {
        Debug.Log("AvatarCreatorContext.UnserializeUIFields()");

        XmlDocument doc = new XmlDocument();
        doc.LoadXml(data);

        XmlNode root = doc.DocumentElement.SelectSingleNode("/SaveFile/AvatarInfo");
        foreach (XmlNode node in root.ChildNodes)
        {
            foreach (XmlAttribute attr in node.Attributes)
            {
                switch (attr.Name)
                {
                    case "name":
                        {
                            GameObject.Find("inputfield_avatarname").GetComponent<InputField>().text = attr.Value;
                            break;
                        }
                    case "dob":
                        {
                            GameObject.Find("dropdown_dob").GetComponent<Dropdown>().value = System.Convert.ToInt32(attr.Value);
                            break;
                        }
                    case "gender":
                        {
                            GameObject.Find("dropdown_gender").GetComponent<Dropdown>().value = System.Convert.ToInt32(attr.Value);
                            break;
                        }
                    case "bio":
                        {
                            GameObject.Find("inputfield_info").GetComponent<InputField>().text = attr.Value;
                            break;
                        }
                }
            }
        }
    }

    // FileTransferManager calls this method.
    public static void LoadAvatarFromFile(string data)
    {
        faceObject.Unserialize(data);
        UnserializeUIFields(data);
    }

    public static void FillColorPalette()
    {
        // 19 x 9
        List<string> defaultPaletteColors = new List<string>()
        {
            "#ffcdd2", "#f8bbd0", "#e1bee7", "#d1c4e9", "#c5cae9", "#bbdefb", "#b3e5fc", "#b2ebf2", "#c8e6c9", "#dcedc8", "#f0f4c3", "#fff9c4", "#ffecb3", "#ffe0b2", "#ffccbc", "#d7ccc8", "#f5f5f5", "#cfd8dc",
            "#ef9a9a", "#f48fb1", "#ce93d8", "#b39ddb", "#9fa8da", "#90caf9", "#81d4fa", "#80deea", "#a5d6a7", "#c5e1a5", "#e6ee9c", "#fff59d", "#ffe082", "#ffcc80", "#ffab91", "#bcaaa4", "#eeeeee", "#b0bec5",
            "#e57373", "#f06292", "#ba68c8", "#9575cd", "#7986cb", "#64b5f6", "#4fc3f7", "#4dd0e1", "#81c784", "#aed581", "#dce775", "#fff176", "#ffd54f", "#ffb74d", "#ff8a65", "#9a8881", "#e0e0e0", "#90a4ae",
            "#ef5350", "#ec407a", "#ab47bc", "#7e57c2", "#5c6bc0", "#42a5f5", "#29b6f6", "#26c6da", "#66bb6a", "#9ccc65", "#d4e157", "#ffee58", "#ffca28", "#ffa726", "#ff7043", "#8d6e63", "#bdbdbd", "#78909c",
            "#f44336", "#e91e63", "#9c27b0", "#673ab7", "#3f51b5", "#2196f3", "#03a9f4", "#00bcd4", "#4caf50", "#8bc34a", "#cddc39", "#ffeb3b", "#ffc107", "#ff9800", "#ff5722", "#795548", "#9e9e9e", "#607d8b",
            "#e53935", "#d81b60", "#8e24aa", "#5e35b1", "#3949ab", "#1e88e5", "#039be5", "#00acc1", "#43a047", "#7cb342", "#c0ca33", "#fdd835", "#ffb300", "#fb8c00", "#f4511e", "#6d4c41", "#757575", "#546e7a",
            "#d32f2f", "#c2185b", "#7b1fa2", "#512da8", "#303f9f", "#1976d2", "#0288d1", "#0097a7", "#388e3c", "#689f38", "#afb42b", "#fbc02d", "#ffa000", "#f57c00", "#e64a19", "#5d4037", "#616161", "#455a64",
            "#c62828", "#ad1457", "#6a1b9a", "#4527a0", "#283593", "#1565c0", "#0277bd", "#00838f", "#2e7d32", "#558b2f", "#9e9d24", "#f9a825", "#ff8f00", "#ef6c00", "#d84315", "#4e342e", "#424242", "#37474f",
            "#b71c1c", "#880e4f", "#4a148c", "#311b92", "#1a237e", "#0d47a1", "#01579b", "#006064", "#1b5e20", "#33691e", "#827717", "#f57f17", "#ff6f00", "#e65100", "#bf360c", "#3e2723", "#212121", "#263238"
        };

        // 12 x 3
        List<string> skinPaletteColors = new List<string>()
        {
            "#fff5ec", "#fee7d5", "#ffe9c9", "#fff4d8", "#fbedc6", "#fed597", "#fcdeba", "#f7d4c1", "#efc394", "#f2b596", "#f7b295", "#e6c8a6",
            "#f5cead", "#ebb996", "#f0a96b", "#fec484", "#f4b46c", "#d09752", "#c79769", "#c3885c", "#b57b55", "#9b613c", "#c47a55", "#ac8b6a",
            "#b38466", "#8a5f4c", "#b46032", "#e2804f", "#a0572a", "#8e5422", "#5e2508", "#604325", "#6c4639", "#2d1c12", "#452a21", "#352e28"
        };

        foreach (string hex in defaultPaletteColors)
        {
            Color tempColor;
            ColorUtility.TryParseHtmlString(hex, out tempColor);
            GameObject btn = Instantiate(Resources.Load<GameObject>("color_palette_button"));
            btn.GetComponent<Image>().color = tempColor;
            btn.transform.SetParent(defaultColorPalette, false);
        }

        foreach (string hex in skinPaletteColors)
        {
            Color tempColor;
            ColorUtility.TryParseHtmlString(hex, out tempColor);
            GameObject btn = Instantiate(Resources.Load<GameObject>("color_palette_button"));
            btn.GetComponent<Image>().color = tempColor;
            btn.transform.SetParent(skinColorPalette, false);
        }
    }

    public static List<Color> GetPaletteColors(ColorPalette palette)
    {
        List<Color> returnList = new List<Color>();

        if(palette == ColorPalette.Default)
        {
            for(int i =0; i < defaultColorPalette.childCount; ++i)
            {
                returnList.Add(defaultColorPalette.GetChild(i).GetComponent<Image>().color);
            }
        }
        else
        {
            for (int i = 0; i < skinColorPalette.childCount; ++i)
            {
                returnList.Add(skinColorPalette.GetChild(i).GetComponent<Image>().color);
            }
        }

        return returnList;
    }
}
