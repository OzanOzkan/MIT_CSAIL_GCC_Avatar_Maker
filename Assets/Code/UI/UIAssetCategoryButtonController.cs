using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Controller class for Asset Category Buttons.
/// Buttons located at the left of scroll list.
/// </summary>
public class UIAssetCategoryButtonController : MonoBehaviour
{
    public AssetType m_assetType;
    public Sprite m_selectedBorder;
    public Sprite m_notSelectedBorder;

    // Todo: Maybe it should be moved into the AvatarCreatorContext?
    Transform scrollView;

    /// <summary>
    /// Called only once when application started.
    /// </summary>
    void Start()
    {
        // Register to button click events.
        gameObject.GetComponent<Button>().onClick.AddListener(() => OnButtonClick());

        // Find the scroll list game object.
        scrollView = GameObject.Find("Canvas").transform.Find("scrollview").transform.Find("Viewport").transform.Find("Content");
    }

    /// <summary>
    /// Called once in every frame.
    /// </summary>
    void Update()
    {
        // Add border to selected type, remove the border from previously selected type.
        if (AvatarCreatorContext.selectedAssetType != m_assetType)
            gameObject.transform.GetChild(0).GetComponent<Image>().sprite = m_notSelectedBorder;
        else
            gameObject.transform.GetChild(0).GetComponent<Image>().sprite = m_selectedBorder;
    }

    /// <summary>
    /// Called in every button press.
    /// </summary>
    public void OnButtonClick()
    {
        // Set selected asset type and active button.
        AvatarCreatorContext.selectedAssetType = m_assetType;
        AvatarCreatorContext.activeAssetCategoryButton = this;

        Debug.Log("Asset type changed to: " + m_assetType.ToString());

        // Log user action.
        AvatarCreatorContext.logManager.LogAction("AssetSelected", m_assetType.ToString());

        // Update asset list.
        UpdateScrollList();
    }

    /// <summary>
    /// Updates asset list according to currently selected asset type.
    /// </summary>
    public void UpdateScrollList()
    {
        // Reset everything.
        scrollView.DestroyChildren();

        // Invoke loading assets.
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

    /// <summary>
    /// Populates scroll list with assets in selected asset type.
    /// </summary>
    /// <param name="assetType">Asset type</param>
    private void FillScrollList(AssetType assetType)
    {
        // Fetch all assets in given asset type and instantiate buttons for them.
        foreach (CBaseAsset asset in AvatarCreatorContext.GetLoadedAssetsByType(assetType))
        {
            GameObject btn = Instantiate(Resources.Load<GameObject>("asset_selection_button"));
            btn.GetComponent<UIScrollViewButtonController>().faceAsset = asset;
            btn.transform.SetParent(scrollView, false);
        }
    }
}
