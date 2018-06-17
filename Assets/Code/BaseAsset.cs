using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;

public class BaseAsset : MonoBehaviour
{
    // This class is needed for Unity's reflection.
}

#region Enumerations
public enum AssetGender
{
    Male,
    Female,
    NoGender
}

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
    None
}

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

public enum SpritePart
{
    Default,
    Front,
    Back,
    Left,
    Right
}

public enum RealismLevel
{
    A,
    B,
    C
}
#endregion

public class CBaseAsset
{
    #region Member Values
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
    protected void LoadSprite(string assetPath, bool isOverride)
    {
        // m_sprites = new Dictionary<SpritePart, Sprite>();
        m_sprites = new Dictionary<SpritePart, List<Sprite>>();

        if (!isOverride)
        {
            List<Sprite> spriteListToLoad = new List<Sprite>(); // Dummy init.

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

            // Old implementation
            //if (assetPath.Length > 0)
            //{
            //    m_sprites.Add(SpritePart.Default, Resources.Load<Sprite>(GetResourcePath(assetPath)));
            //    Debug.Log("CBaseAsset:LoadSprite: " + GetResourcePath(assetPath));

            //    return;
            //}

            //// For the dummy CBaseAsset creation.
            //m_sprites.Add(SpritePart.Default, null);
            //m_sprites.Add(SpritePart.Front, null);
            //m_sprites.Add(SpritePart.Back, null);
            //m_sprites.Add(SpritePart.Left, null);
            //m_sprites.Add(SpritePart.Right, null);
        }
        else
        {
            this.LoadSpriteOverride(assetPath);
        }
    }

    protected virtual void LoadSpriteOverride(string assetPath) { }

    public AssetType GetAssetType() { return m_assetType; }
    public AssetGender GetGender() { return m_assetGender; }
    public AssetModifyFlag GetModifyFlags() { return m_modifyFlags; }
    public Dictionary<SpritePart, List<Sprite>> GetSprites() { return m_sprites; }
    protected string GetResourcePath(string rawAssetPath) { return rawAssetPath.Replace(".png", ""); }
    #endregion
}

public class CBaseAssetFactory
{
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
        }

        return null;
    }
}

public class CHeadShape : CBaseAsset
{
    public CHeadShape(AssetGender gender, string assetPath)
        : base(gender, AssetType.HeadShape 
            , AssetModifyFlag.Resize | AssetModifyFlag.StretchHorizontal | AssetModifyFlag.StretchVertical
            , assetPath)
    { }
}

public class CHair : CBaseAsset
{
    public CHair(AssetGender gender, string assetPath)
        : base(gender, AssetType.Hair
            , AssetModifyFlag.MoveVertical | AssetModifyFlag.Resize | AssetModifyFlag.StretchHorizontal | AssetModifyFlag.StretchVertical
            , assetPath, true)
    { }

    protected override void LoadSpriteOverride(string assetPath)
    {
        Sprite spriteToLoad;

        // Layered?
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

            m_sprites.Add(SpritePart.Front, backLayers);
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

public class CEars : CBaseAsset
{
    public CEars(AssetGender gender, string assetPath)
        : base(gender, AssetType.Ears
            , AssetModifyFlag.MoveVertical | AssetModifyFlag.Resize | AssetModifyFlag.StretchHorizontal | AssetModifyFlag.StretchVertical
            , assetPath, true)
    { }

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

public class CEyes : CBaseAsset
{
    public CEyes(AssetGender gender, string assetPath)
        : base(gender, AssetType.Eyes
           , AssetModifyFlag.MoveVertical | AssetModifyFlag.Resize | AssetModifyFlag.ChangeDistance | AssetModifyFlag.Rotate
           , assetPath, true)
    { }

    protected override void LoadSpriteOverride(string assetPath)
    {
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

public class CEyebrows : CBaseAsset
{
    public CEyebrows(AssetGender gender, string assetPath)
        : base(gender, AssetType.Eyebrows
            , AssetModifyFlag.MoveVertical | AssetModifyFlag.Resize | AssetModifyFlag.StretchHorizontal | AssetModifyFlag.StretchVertical
            | AssetModifyFlag.ChangeDistance | AssetModifyFlag.Rotate
            , assetPath, true)
    { }

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

public class CGlasses : CBaseAsset
{
    public CGlasses(AssetGender gender, string assetPath)
        : base(gender, AssetType.Glasses, AssetModifyFlag.MoveVertical, assetPath)
    { }
}

public class CFaceTexture : CBaseAsset
{
    public CFaceTexture(AssetGender gender, string assetPath)
        : base(gender, AssetType.FaceTexture, AssetModifyFlag.Resize | AssetModifyFlag.StretchHorizontal | AssetModifyFlag.StretchVertical, assetPath)
    { }
}

public class CNose : CBaseAsset
{
    public CNose(AssetGender gender, string assetPath)
        : base(gender, AssetType.Nose,
            AssetModifyFlag.MoveVertical | AssetModifyFlag.Resize | AssetModifyFlag.StretchHorizontal | AssetModifyFlag.StretchVertical, assetPath)
    { }
}

public class CMoustache : CBaseAsset
{
    public CMoustache(AssetGender gender, string assetPath)
        : base(gender, AssetType.Moustache
            , AssetModifyFlag.MoveVertical | AssetModifyFlag.Resize | AssetModifyFlag.StretchHorizontal | AssetModifyFlag.StretchVertical
            , assetPath)
    { }
}

public class CBeard : CBaseAsset
{
    public CBeard(AssetGender gender, string assetPath)
        : base(gender, AssetType.Beard
            , AssetModifyFlag.MoveVertical | AssetModifyFlag.Resize | AssetModifyFlag.StretchHorizontal | AssetModifyFlag.StretchVertical
            , assetPath)
    { }
}

public class CMouth : CBaseAsset
{
    public CMouth(AssetGender gender, string assetPath)
        : base(gender ,AssetType.Mouth
            , AssetModifyFlag.MoveHorizontal | AssetModifyFlag.Resize | AssetModifyFlag.StretchHorizontal | AssetModifyFlag.StretchVertical
            , assetPath, true)
    { }

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

public class CBody : CBaseAsset
{
    public CBody(AssetGender gender, string assetPath)
        : base(gender, AssetType.Body
            , AssetModifyFlag.Resize | AssetModifyFlag.StretchHorizontal | AssetModifyFlag.StretchVertical
            , assetPath, true)
    { }

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

// TODO: BG Texture, FG Graphic