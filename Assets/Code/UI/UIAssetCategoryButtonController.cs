using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAssetCategoryButtonController : MonoBehaviour
{
    public AssetType m_assetType;
    public Sprite m_selectedBorder;
    public Sprite m_notSelectedBorder;

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
        if (AvatarCreatorContext.selectedAssetType != m_assetType)
            gameObject.transform.GetChild(0).GetComponent<Image>().sprite = m_notSelectedBorder;
        else
        {
            gameObject.transform.GetChild(0).GetComponent<Image>().sprite = m_selectedBorder;
            //UpdateScrollList();
        }
    }

    public void OnButtonClick()
    {
        AvatarCreatorContext.selectedAssetType = m_assetType;
        Debug.Log("Asset type changed to: " + m_assetType.ToString());

        AvatarCreatorContext.logManager.LogAction("AssetSelected", m_assetType.ToString());

        UpdateScrollList();
    }

    private void UpdateScrollList()
    {
        scrollView.DestroyChildren();

        if (m_assetType == AssetType.Beard || m_assetType == AssetType.Moustache)
        {
            FillScrollList(AssetType.Beard);
            FillScrollList(AssetType.Moustache);
        }
        else if (m_assetType == AssetType.Ears)
        {
            FillScrollList(AssetType.Ears);
            FillScrollList(AssetType.Nose);
        }
        else if (m_assetType == AssetType.Body)
        {
            FillScrollList(AssetType.Body);
            FillScrollList(AssetType.SpecialBody);
        }
        else if (m_assetType == AssetType.Hair)
        {
            FillScrollList(AssetType.Hair);
            FillScrollList(AssetType.Ghutra);
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
