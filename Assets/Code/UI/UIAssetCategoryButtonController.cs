﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAssetCategoryButtonController : MonoBehaviour
{
    public AssetType m_assetType;

    // Todo: Maybe into the AvatarCreatorContext?
    Transform scrollView;

    // Use this for initialization
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(() => OnButtonClick());
        scrollView = GameObject.Find("Canvas").transform.Find("scrollview").transform.Find("Viewport").transform.Find("Content");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnButtonClick()
    {
        AvatarCreatorContext.selectedAssetType = m_assetType;
        Debug.Log("Asset type changed to: " + m_assetType.ToString());

        AvatarCreatorContext.logManager.LogAction("UIButtonClick", m_assetType.ToString());
   
        scrollView.DestroyChildren();

        if(m_assetType == AssetType.Beard || m_assetType == AssetType.Moustache)
        {
            FillScrollList(AssetType.Beard);
            FillScrollList(AssetType.Moustache);
        }
        else
        {
            FillScrollList(m_assetType);
        }
    }

    private void FillScrollList(AssetType assetType)
    {
        foreach (CBaseAsset asset in AvatarCreatorContext.GetLoadedAssetsByType(assetType))
        {
            GameObject btn = Instantiate(Resources.Load<GameObject>("asset_selection_button"));
            btn.GetComponent<UIScrollViewButtonController>().faceAsset = asset;
            btn.transform.SetParent(scrollView, false);
        }
    }
}
