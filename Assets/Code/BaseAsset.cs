using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

/// <summary>
/// An empty class for maintaining Unity's reflection.
/// In Unity environment, every .cs file needs at least one class inside, which should be the same with file name.
/// </summary>
public class BaseAsset : MonoBehaviour
{
    // No Implementation
}

#region Enumerations
/// <summary>
/// Enumeration for genders.
/// Stored in the Asset object. Indicates which gender it belongs to.
/// An asset can have only one gender.
/// </summary>
public enum AssetGender
{
    Male,
    Female,
    NoGender
}

/// <summary>
/// Enumeration for asset types.
/// Stored in the Asset object. Indicates which type of asset it is.
/// An asset can only have one type.
/// </summary>
public enum AssetType
{
    HeadShape,
    Ears,
    Hair,
    Eyes,
    Eyebrows,
    Glasses,
    FaceTexture,
    Nose,
    Moustache,
    Beard,
    Mouth,
    Body,
    BackgroundTexture,
    FGGraphic,
    SpecialBody,
    Ghutra,
    None
}

/// <summary>
/// Enumeration for asset modifications. 
/// Stored in the asset object. System allows to modify an asset according to it's modification flags.
/// An asset can have one, more than one and no asset modification flag.
/// </summary>
[System.Flags]
public enum AssetModifyFlag : int
{
    MoveHorizontal      = 1,
    MoveVertical        = 2,
    Resize              = 4,
    StretchHorizontal   = 8,
    StretchVertical     = 16,
    ChangeDistance      = 32,
    Rotate              = 64
}

/// <summary>
/// Enumeration for asset location.
/// Stored in the asset object. If one asset consists of multiple parts, this enum indicates which part it is.
/// An asset part can only have one of sprite part.
/// </summary>
public enum SpritePart
{
    Default,
    Front,
    Back,
    Left,
    Right,
    Reserved
}

/// <summary>
/// Enumeration for realism level.
/// Stored in the asset object. Indicates asset's realism level.
/// </summary>
public enum RealismLevel
{
    A,
    B,
    C
}
#endregion

/// <summary>
/// Base class of an asset.
/// Contains shared members and functions among all assets.
/// All assets are objects and every asset object loads it's sprites from the disk at the object creation time. 
/// For finding assets, placing them, manipulating them etc. system uses loaded asset objects and there is no continuous read/write operation on asset files located on disk.
/// </summary>
public class CBaseAsset
{
    #region Member Variables
    protected AssetGender m_assetGender;
    protected AssetType m_assetType;
    protected AssetModifyFlag m_modifyFlags;
   // protected Dictionary<SpritePart, Sprite> m_sprites;  // TODO: protected Dictionary<RealismLevel, Dictionary<SpritePart, Sprite>> m_sprites;
    protected Dictionary<SpritePart, List<Sprite>> m_sprites;
    #endregion

    public CBaseAsset(AssetGender gender, AssetType assetType, AssetModifyFlag modifyFlags, string assetPath, bool loadSpriteOverride=false)
    {
        m_assetGender = gender;
        m_assetType = assetType;
        m_modifyFlags = modifyFlags;

        LoadSprite(assetPath, loadSpriteOverride);

        Debug.Log("CBaseAsset: " + m_assetType + " " + m_assetGender + " " + m_modifyFlags);
    }

    #region Shared Methods
    /// <summary>
    /// Loads sprite from given asset path to asset object.
    /// </summary>
    /// <param name="assetPath">Path of the sprite file.</param>
    /// <param name="isOverride">
    /// True: Child asset object will use its own LoadSpriteOverride method. 
    /// False: This method will be used for loading sprites.
    /// </param>
    protected void LoadSprite(string assetPath, bool isOverride)
    {
        m_sprites = new Dictionary<SpritePart, List<Sprite>>();

        if (!isOverride)
        {
            List<Sprite> spriteListToLoad = new List<Sprite>(); // Dummy initialization.

            // Realism?
            if (Resources.Load<Sprite>(assetPath + "_A"))
            {
                spriteListToLoad = new List<Sprite>()
                {
                    Resources.Load<Sprite>(assetPath + "_A"),
                    Resources.Load<Sprite>(assetPath + "_B"),
                    Resources.Load<Sprite>(assetPath + "_C"),
                };
            }
            else
            {
                spriteListToLoad = new List<Sprite>()
                {
                    Resources.Load<Sprite>(assetPath),
                };
            }

            m_sprites.Add(SpritePart.Default, spriteListToLoad);
        }
        else
        {
            this.LoadSpriteOverride(assetPath);
        }
    }

    /// <summary>
    /// Loads sprite from given asset path. Implemented by child asset class.
    /// </summary>
    /// <param name="assetPath">Path of the sprite file.</param>
    protected virtual void LoadSpriteOverride(string assetPath) { }

    /// <summary>
    /// Returns asset type.
    /// </summary>
    /// <returns>
    /// Asset type.
    /// </returns>
    public AssetType GetAssetType() { return m_assetType; }

    /// <summary>
    /// Returns asset gender.
    /// </summary>
    /// <returns>
    /// Gender of this asset.
    /// </returns>
    public AssetGender GetGender() { return m_assetGender; }

    /// <summary>
    /// Returns modify flags of an asset.
    /// </summary>
    /// <returns>
    /// Returns modify flags of this asset.
    /// </returns>
    public AssetModifyFlag GetModifyFlags() { return m_modifyFlags; }

    /// <summary>
    /// Returns parts of this asset.
    /// </summary>
    /// <returns>
    /// Dictionary which categorized by sprite parts and list of sprites.
    /// </returns>
    public Dictionary<SpritePart, List<Sprite>> GetSprites() { return m_sprites; }

    /// <summary>
    /// Returns original sprite path of this asset.
    /// </summary>
    /// <param name="rawAssetPath"></param>
    /// <returns>Original sprite path of this asset as a string.</returns>
    protected string GetResourcePath(string rawAssetPath) { return rawAssetPath.Replace(".png", ""); }
    #endregion
}

/// <summary>
/// Asset factory for decoupling creation process of child asset objects and their types from entire codebase.
/// Responsible for creating respective asset objects.
/// </summary>
public class CBaseAssetFactory
{
    /// <summary>
    /// Creates an asset object with given parameters.
    /// </summary>
    /// <param name="type">Type of the asset.</param>
    /// <param name="gender">Gender of the asset.</param>
    /// <param name="assetPath">Path of asset sprites.</param>
    /// <returns></returns>
    public CBaseAsset CreateAsset(AssetType type, AssetGender gender, string assetPath)
    {
        switch (type)
        {
            case AssetType.HeadShape:
                return new CHeadShape(gender, assetPath);
            case AssetType.Hair:
                return new CHair(gender, assetPath);
            case AssetType.Ears:
                return new CEars(gender, assetPath);
            case AssetType.Eyes:
                return new CEyes(gender, assetPath);
            case AssetType.Eyebrows:
                return new CEyebrows(gender, assetPath);
            case AssetType.Glasses:
                return new CGlasses(gender, assetPath);
            case AssetType.FaceTexture:
                return new CFaceTexture(gender, assetPath);
            case AssetType.Nose:
                return new CNose(gender, assetPath);
            case AssetType.Moustache:
                return new CMoustache(gender, assetPath);
            case AssetType.Beard:
                return new CBeard(gender, assetPath);
            case AssetType.Mouth:
                return new CMouth(gender, assetPath);
            case AssetType.Body:
                return new CBody(gender, assetPath);
            case AssetType.SpecialBody:
                return new CSpecialBody(gender, assetPath);
            case AssetType.Ghutra:
                return new CGhutra(gender, assetPath);
            case AssetType.BackgroundTexture:
                return new CBGTexture(gender, assetPath);
        }

        return null;
    }
}

/// <summary>
/// Object of the asset.
/// </summary>
public class CHeadShape : CBaseAsset
{
    /// <summary>
    /// Initializes this subclass (asset) with given parameters.
    /// </summary>
    /// <param name="gender">Gender of this asset.</param>
    /// <param name="assetPath">Path of sprites.</param>
    public CHeadShape(AssetGender gender, string assetPath)
        : base(gender, AssetType.HeadShape 
            , AssetModifyFlag.Resize | AssetModifyFlag.StretchHorizontal | AssetModifyFlag.StretchVertical
            , assetPath)
    { }
}

public class CHair : CBaseAsset
{
    /// <summary>
    /// Initializes this subclass (asset) with given parameters.
    /// </summary>
    /// <param name="gender">Gender of this asset.</param>
    /// <param name="assetPath">Path of sprites.</param>
    public CHair(AssetGender gender, string assetPath)
        : base(gender, AssetType.Hair
            , AssetModifyFlag.MoveVertical | AssetModifyFlag.Resize | AssetModifyFlag.StretchHorizontal | AssetModifyFlag.StretchVertical
            , assetPath, true)
    { }

    /// <summary>
    /// Loads sprite from given asset path. Implemented by child asset class.
    /// </summary>
    /// <param name="assetPath">Path of the sprite file.</param>
    protected override void LoadSpriteOverride(string assetPath)
    {
        Sprite spriteToLoad;

        // Some sprites are layered for rendering order, so we need to handle this.
        if(spriteToLoad = Resources.Load<Sprite>(assetPath + "_A_L1"))
        {
            List<Sprite> backLayers = new List<Sprite>()
            {
                Resources.Load<Sprite>(assetPath + "_A_L1"),
                Resources.Load<Sprite>(assetPath + "_B_L1"),
                Resources.Load<Sprite>(assetPath + "_C_L1")
            };

            m_sprites.Add(SpritePart.Back, backLayers);

            List<Sprite> frontLayers = new List<Sprite>()
            {
                Resources.Load<Sprite>(assetPath + "_A_L2"),
                Resources.Load<Sprite>(assetPath + "_B_L2"),
                Resources.Load<Sprite>(assetPath + "_C_L2")
            };

            m_sprites.Add(SpritePart.Front, frontLayers);
        }
        else
        {
            List<Sprite> spriteListToLoad = new List<Sprite>()
            {
                Resources.Load<Sprite>(assetPath + "_A"),
                Resources.Load<Sprite>(assetPath + "_B"),
                Resources.Load<Sprite>(assetPath + "_C"),
            };

            m_sprites.Add(SpritePart.Front, spriteListToLoad);
        }

        // Old Implementation
        //m_sprites.Add(SpritePart.Default, Resources.Load<Sprite>(assetPath.Replace("a.png", "a")));
        //m_sprites.Add(SpritePart.Front, Resources.Load<Sprite>(assetPath.Replace("a.png", "a")));
        //m_sprites.Add(SpritePart.Back, Resources.Load<Sprite>(assetPath.Replace("a.png", "b")));
        //Debug.Log("CHair:LoadSpriteOverride: " + GetResourcePath(assetPath));
    }
}

/// <summary>
/// Object of the asset.
/// </summary>
public class CEars : CBaseAsset
{
    /// <summary>
    /// Initializes this subclass (asset) with given parameters.
    /// </summary>
    /// <param name="gender">Gender of this asset.</param>
    /// <param name="assetPath">Path of sprites.</param>
    public CEars(AssetGender gender, string assetPath)
        : base(gender, AssetType.Ears
            , AssetModifyFlag.MoveVertical | AssetModifyFlag.Resize | AssetModifyFlag.StretchHorizontal | AssetModifyFlag.StretchVertical
            , assetPath, true)
    { }

    /// <summary>
    /// Loads sprite from given asset path. Implemented by child asset class.
    /// </summary>
    /// <param name="assetPath">Path of the sprite file.</param>
    protected override void LoadSpriteOverride(string assetPath)
    {
        List<Sprite> spriteListToLoad = new List<Sprite>()
            {
                Resources.Load<Sprite>(assetPath + "_A"),
                Resources.Load<Sprite>(assetPath + "_B"),
                Resources.Load<Sprite>(assetPath + "_C"),
            };

        m_sprites.Add(SpritePart.Default, spriteListToLoad);
        m_sprites.Add(SpritePart.Left, spriteListToLoad);

        // Old implementation
        //m_sprites.Add(SpritePart.Default, Resources.Load<Sprite>(GetResourcePath(assetPath)));
        //m_sprites.Add(SpritePart.Right, Resources.Load<Sprite>(GetResourcePath(assetPath)));
        Debug.Log("CEars:LoadSpriteOverride: " + GetResourcePath(assetPath));
    }
}

/// <summary>
/// Object of the asset.
/// </summary>
public class CEyes : CBaseAsset
{
    /// <summary>
    /// Initializes this subclass (asset) with given parameters.
    /// </summary>
    /// <param name="gender">Gender of this asset.</param>
    /// <param name="assetPath">Path of sprites.</param>
    public CEyes(AssetGender gender, string assetPath)
        : base(gender, AssetType.Eyes
           , AssetModifyFlag.MoveVertical | AssetModifyFlag.Resize | AssetModifyFlag.ChangeDistance | AssetModifyFlag.Rotate
           , assetPath, true)
    { }

    /// <summary>
    /// Loads sprite from given asset path. Implemented by child asset class.
    /// </summary>
    /// <param name="assetPath">Path of the sprite file.</param>
    protected override void LoadSpriteOverride(string assetPath)
    {
        List<Sprite> skinColorizedLayers = new List<Sprite>()
            {
                Resources.Load<Sprite>(assetPath + "_A_L0"),
                Resources.Load<Sprite>(assetPath + "_B_L0"),
                Resources.Load<Sprite>(assetPath + "_C_L0")
            };

        m_sprites.Add(SpritePart.Reserved, skinColorizedLayers);

        List<Sprite> nonColorizedLayers = new List<Sprite>()
            {
                Resources.Load<Sprite>(assetPath + "_A_L1"),
                Resources.Load<Sprite>(assetPath + "_B_L1"),
                Resources.Load<Sprite>(assetPath + "_C_L1")
            };

        m_sprites.Add(SpritePart.Back, nonColorizedLayers);

        List<Sprite> colorizedLayers = new List<Sprite>()
            {
                Resources.Load<Sprite>(assetPath + "_A_L2"),
                Resources.Load<Sprite>(assetPath + "_B_L2"),
                Resources.Load<Sprite>(assetPath + "_C_L2")
            };

        m_sprites.Add(SpritePart.Default, colorizedLayers);

        List<Sprite> topReflectionLayers = new List<Sprite>()
            {
                Resources.Load<Sprite>(assetPath + "_A_L3"),
                Resources.Load<Sprite>(assetPath + "_B_L3"),
                Resources.Load<Sprite>(assetPath + "_C_L3")
            };

        m_sprites.Add(SpritePart.Front, topReflectionLayers);

        // Old implementation
        //m_sprites.Add(SpritePart.Default, Resources.Load<Sprite>(GetResourcePath(assetPath)));
        //m_sprites.Add(SpritePart.Right, Resources.Load<Sprite>(GetResourcePath(assetPath)));
        Debug.Log("CEyes:LoadSpriteOverride: " + GetResourcePath(assetPath));
    }
}

/// <summary>
/// Object of the asset.
/// </summary>
public class CEyebrows : CBaseAsset
{
    /// <summary>
    /// Initializes this subclass (asset) with given parameters.
    /// </summary>
    /// <param name="gender">Gender of this asset.</param>
    /// <param name="assetPath">Path of sprites.</param>
    public CEyebrows(AssetGender gender, string assetPath)
        : base(gender, AssetType.Eyebrows
            , AssetModifyFlag.MoveVertical | AssetModifyFlag.Resize | AssetModifyFlag.StretchHorizontal | AssetModifyFlag.StretchVertical
            | AssetModifyFlag.ChangeDistance | AssetModifyFlag.Rotate
            , assetPath, true)
    { }

    /// <summary>
    /// Loads sprite from given asset path. Implemented by child asset class.
    /// </summary>
    /// <param name="assetPath">Path of the sprite file.</param>
    protected override void LoadSpriteOverride(string assetPath)
    {
        List<Sprite> spriteListToLoad = new List<Sprite>()
            {
                Resources.Load<Sprite>(assetPath + "_A"),
                Resources.Load<Sprite>(assetPath + "_B"),
                Resources.Load<Sprite>(assetPath + "_C"),
            };

        m_sprites.Add(SpritePart.Default, spriteListToLoad);
        m_sprites.Add(SpritePart.Left, spriteListToLoad);

        // Old implementation
        //m_sprites.Add(SpritePart.Default, Resources.Load<Sprite>(GetResourcePath(assetPath)));
        //m_sprites.Add(SpritePart.Left, Resources.Load<Sprite>(GetResourcePath(assetPath)));
        Debug.Log("CEyebrows:LoadSpriteOverride: " + GetResourcePath(assetPath));
    }
}

/// <summary>
/// Object of the asset.
/// </summary>
public class CGlasses : CBaseAsset
{
    /// <summary>
    /// Initializes this subclass (asset) with given parameters.
    /// </summary>
    /// <param name="gender">Gender of this asset.</param>
    /// <param name="assetPath">Path of sprites.</param>
    public CGlasses(AssetGender gender, string assetPath)
        : base(gender, AssetType.Glasses, AssetModifyFlag.MoveVertical, assetPath)
    { }
}

/// <summary>
/// Object of the asset.
/// </summary>
public class CFaceTexture : CBaseAsset
{
    /// <summary>
    /// Initializes this subclass (asset) with given parameters.
    /// </summary>
    /// <param name="gender">Gender of this asset.</param>
    /// <param name="assetPath">Path of sprites.</param>
    public CFaceTexture(AssetGender gender, string assetPath)
        : base(gender, AssetType.FaceTexture, AssetModifyFlag.Resize | AssetModifyFlag.StretchHorizontal | AssetModifyFlag.StretchVertical, assetPath)
    { }
}

/// <summary>
/// Object of the asset.
/// </summary>
public class CNose : CBaseAsset
{
    /// <summary>
    /// Initializes this subclass (asset) with given parameters.
    /// </summary>
    /// <param name="gender">Gender of this asset.</param>
    /// <param name="assetPath">Path of sprites.</param>
    public CNose(AssetGender gender, string assetPath)
        : base(gender, AssetType.Nose,
            AssetModifyFlag.MoveVertical | AssetModifyFlag.Resize | AssetModifyFlag.StretchHorizontal | AssetModifyFlag.StretchVertical, assetPath)
    { }
}

/// <summary>
/// Object of the asset.
/// </summary>
public class CMoustache : CBaseAsset
{
    /// <summary>
    /// Initializes this subclass (asset) with given parameters.
    /// </summary>
    /// <param name="gender">Gender of this asset.</param>
    /// <param name="assetPath">Path of sprites.</param>
    public CMoustache(AssetGender gender, string assetPath)
        : base(gender, AssetType.Moustache
            , AssetModifyFlag.MoveVertical | AssetModifyFlag.Resize | AssetModifyFlag.StretchHorizontal | AssetModifyFlag.StretchVertical
            , assetPath)
    { }
}

/// <summary>
/// Object of the asset.
/// </summary>
public class CBeard : CBaseAsset
{
    /// <summary>
    /// Initializes this subclass (asset) with given parameters.
    /// </summary>
    /// <param name="gender">Gender of this asset.</param>
    /// <param name="assetPath">Path of sprites.</param>
    public CBeard(AssetGender gender, string assetPath)
        : base(gender, AssetType.Beard
            , AssetModifyFlag.MoveVertical | AssetModifyFlag.Resize | AssetModifyFlag.StretchHorizontal | AssetModifyFlag.StretchVertical
            , assetPath)
    { }
}

/// <summary>
/// Object of the asset.
/// </summary>
public class CMouth : CBaseAsset
{
    /// <summary>
    /// Initializes this subclass (asset) with given parameters.
    /// </summary>
    /// <param name="gender">Gender of this asset.</param>
    /// <param name="assetPath">Path of sprites.</param>
    public CMouth(AssetGender gender, string assetPath)
        : base(gender ,AssetType.Mouth
            , AssetModifyFlag.MoveHorizontal | AssetModifyFlag.MoveVertical | AssetModifyFlag.Resize | AssetModifyFlag.StretchHorizontal | AssetModifyFlag.StretchVertical
            , assetPath, true)
    { }

    /// <summary>
    /// Loads sprite from given asset path. Implemented by child asset class.
    /// </summary>
    /// <param name="assetPath">Path of the sprite file.</param>
    protected override void LoadSpriteOverride(string assetPath)
    {
        List<Sprite> colorizedLayers = new List<Sprite>()
            {
                Resources.Load<Sprite>(assetPath + "_A_L1"),
                Resources.Load<Sprite>(assetPath + "_B_L1"),
                Resources.Load<Sprite>(assetPath + "_C_L1")
            };

        m_sprites.Add(SpritePart.Back, colorizedLayers);

        List<Sprite> nonColorizedLayers = new List<Sprite>()
            {
                Resources.Load<Sprite>(assetPath + "_A_L2"),
                Resources.Load<Sprite>(assetPath + "_B_L2"),
                Resources.Load<Sprite>(assetPath + "_C")
            };

        m_sprites.Add(SpritePart.Default, nonColorizedLayers);

        // Old implementation
        //m_sprites.Add(SpritePart.Default, Resources.Load<Sprite>(GetResourcePath(assetPath)));
        //m_sprites.Add(SpritePart.Right, Resources.Load<Sprite>(GetResourcePath(assetPath)));
        Debug.Log("CMouth:LoadSpriteOverride: " + GetResourcePath(assetPath));
    }
}

/// <summary>
/// Object of the asset.
/// </summary>
public class CBody : CBaseAsset
{
    /// <summary>
    /// Initializes this subclass (asset) with given parameters.
    /// </summary>
    /// <param name="gender">Gender of this asset.</param>
    /// <param name="assetPath">Path of sprites.</param>
    public CBody(AssetGender gender, string assetPath)
        : base(gender, AssetType.Body
            , AssetModifyFlag.Resize | AssetModifyFlag.StretchHorizontal | AssetModifyFlag.StretchVertical
            , assetPath, true)
    { }

    /// <summary>
    /// Loads sprite from given asset path. Implemented by child asset class.
    /// </summary>
    /// <param name="assetPath">Path of the sprite file.</param>
    protected override void LoadSpriteOverride(string assetPath)
    {
        List<Sprite> nonColorizedLayers = new List<Sprite>()
            {
                Resources.Load<Sprite>(assetPath + "_A_L1")
            };

        m_sprites.Add(SpritePart.Back, nonColorizedLayers);

        List<Sprite> colorizedLayers = new List<Sprite>()
            {
                Resources.Load<Sprite>(assetPath + "_A_L2")
            };

        m_sprites.Add(SpritePart.Default, colorizedLayers);

        Debug.Log("CBody:LoadSpriteOverride: " + GetResourcePath(assetPath));
    }
}

/// <summary>
/// Object of the asset.
/// </summary>
public class CSpecialBody : CBaseAsset
{
    /// <summary>
    /// Initializes this subclass (asset) with given parameters.
    /// </summary>
    /// <param name="gender">Gender of this asset.</param>
    /// <param name="assetPath">Path of sprites.</param>
    public CSpecialBody(AssetGender gender, string assetPath)
    : base(gender, AssetType.SpecialBody
        , AssetModifyFlag.Resize | AssetModifyFlag.StretchHorizontal | AssetModifyFlag.StretchVertical
        , assetPath, true)
    { }

    /// <summary>
    /// Loads sprite from given asset path. Implemented by child asset class.
    /// </summary>
    /// <param name="assetPath">Path of the sprite file.</param>
    protected override void LoadSpriteOverride(string assetPath)
    {
        // Some sprites are layered for rendering order, so we need to handle this.
        if (Resources.Load<Sprite>(assetPath + "_A_L1"))
        {
            List<Sprite> backLayers = new List<Sprite>()
            {
                Resources.Load<Sprite>(assetPath + "_A_L1"),
                Resources.Load<Sprite>(assetPath + "_B_L1"),
                Resources.Load<Sprite>(assetPath + "_C_L1")
            };

            m_sprites.Add(SpritePart.Back, backLayers);

            List<Sprite> defaultLayers = new List<Sprite>()
            {
                Resources.Load<Sprite>(assetPath + "_A_L2"),
                Resources.Load<Sprite>(assetPath + "_B_L2"),
                Resources.Load<Sprite>(assetPath + "_C_L2")
            };

            m_sprites.Add(SpritePart.Default, defaultLayers);

            List<Sprite> frontLayers = new List<Sprite>()
            {
                Resources.Load<Sprite>(assetPath + "_A_L3"),
                Resources.Load<Sprite>(assetPath + "_B_L3"),
                Resources.Load<Sprite>(assetPath + "_C_L3")
            };

            m_sprites.Add(SpritePart.Front, frontLayers);
        }
        else
        {
            List<Sprite> spriteListToLoad = new List<Sprite>()
            {
                Resources.Load<Sprite>(assetPath + "_A"),
                Resources.Load<Sprite>(assetPath + "_B"),
                Resources.Load<Sprite>(assetPath + "_C"),
            };

            m_sprites.Add(SpritePart.Front, spriteListToLoad);
        }
    }
}

/// <summary>
/// Object of the asset.
/// </summary>
public class CGhutra : CBaseAsset
{
    /// <summary>
    /// Initializes this subclass (asset) with given parameters.
    /// </summary>
    /// <param name="gender">Gender of this asset.</param>
    /// <param name="assetPath">Path of sprites.</param>
    public CGhutra(AssetGender gender, string assetPath)
    : base(gender, AssetType.Ghutra
        , AssetModifyFlag.MoveVertical | AssetModifyFlag.Resize | AssetModifyFlag.StretchHorizontal | AssetModifyFlag.StretchVertical
        , assetPath, true)
    { }

    /// <summary>
    /// Loads sprite from given asset path. Implemented by child asset class.
    /// </summary>
    /// <param name="assetPath">Path of the sprite file.</param>
    protected override void LoadSpriteOverride(string assetPath)
    {
        List<Sprite> backLayers = new List<Sprite>()
            {
                Resources.Load<Sprite>(assetPath + "_L1")
            };

        m_sprites.Add(SpritePart.Back, backLayers);

        List<Sprite> frontLayers = new List<Sprite>()
            {
                Resources.Load<Sprite>(assetPath + "_L2")
            };

        m_sprites.Add(SpritePart.Front, frontLayers);
    }
}

/// <summary>
/// Object of the asset.
/// </summary>
public class CBGTexture : CBaseAsset
{
    /// <summary>
    /// Initializes this subclass (asset) with given parameters.
    /// </summary>
    /// <param name="gender">Gender of this asset.</param>
    /// <param name="assetPath">Path of sprites.</param>
    public CBGTexture(AssetGender gender, string assetPath)
        : base(gender, AssetType.BackgroundTexture, 0, assetPath)
    { }
}