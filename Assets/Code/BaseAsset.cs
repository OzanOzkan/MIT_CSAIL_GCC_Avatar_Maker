﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

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
#endregion

public abstract class CBaseAsset
{
    #region Member Values
    protected AssetGender m_assetGender;
    protected AssetType m_assetType;
    protected AssetModifyFlag m_modifyFlags;
    protected Dictionary<SpritePart, Sprite> m_sprites;
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
        m_sprites = new Dictionary<SpritePart, Sprite>();

        if (!isOverride)
        {
            m_sprites.Add(SpritePart.Default, Resources.Load<Sprite>(GetResourcePath(assetPath)));
            Debug.Log("CBaseAsset:LoadSprite: " + GetResourcePath(assetPath));
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
    public Dictionary<SpritePart, Sprite> GetSprites() { return m_sprites; }
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
        m_sprites.Add(SpritePart.Default, Resources.Load<Sprite>(assetPath.Replace("a.png", "a")));
        m_sprites.Add(SpritePart.Front, Resources.Load<Sprite>(assetPath.Replace("a.png", "a")));
        m_sprites.Add(SpritePart.Back, Resources.Load<Sprite>(assetPath.Replace("a.png", "b")));
        Debug.Log("CHair:LoadSpriteOverride: " + GetResourcePath(assetPath));
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
        m_sprites.Add(SpritePart.Default, Resources.Load<Sprite>(GetResourcePath(assetPath)));
        m_sprites.Add(SpritePart.Right, Resources.Load<Sprite>(GetResourcePath(assetPath)));
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
        m_sprites.Add(SpritePart.Default, Resources.Load<Sprite>(GetResourcePath(assetPath)));
        m_sprites.Add(SpritePart.Right, Resources.Load<Sprite>(GetResourcePath(assetPath)));
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
        m_sprites.Add(SpritePart.Default, Resources.Load<Sprite>(GetResourcePath(assetPath)));
        m_sprites.Add(SpritePart.Left, Resources.Load<Sprite>(GetResourcePath(assetPath)));
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
            , assetPath)
    { }
}

// TODO: Body, BG Texture, FG Graphic