using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A controller class for Gender Button.
/// </summary>
public class UIGenderButtonController : MonoBehaviour {

    public AssetGender m_assetGender;
    private List<CBaseAsset> m_assetList;
    private AssetType m_lastSelectedAssetType = AssetType.None;

    /// <summary>
    /// Called only once when application started.
    /// </summary>
	void Start ()
    {
        // Not interactable by default state.
        gameObject.GetComponent<Button>().interactable = false;

        // Register to button click events.
        gameObject.GetComponent<Button>().onClick.AddListener(() => OnButtonClick());
    }

    /// <summary>
    /// Called once in every frame.
    /// </summary>
    void Update () {

        // Check selected asset type in every frame and enable/disable button if selected gender has assets.
        if (m_lastSelectedAssetType != AvatarCreatorContext.selectedAssetType)
        {
            if (m_assetGender == AssetGender.NoGender)
                m_assetList = AvatarCreatorContext.GetLoadedAssetsByType(AvatarCreatorContext.selectedAssetType);
            else
                m_assetList = AvatarCreatorContext.GetLoadedAssetsByTypeAndGender(AvatarCreatorContext.selectedAssetType, m_assetGender);

            if(m_assetList.Count > 0)
                gameObject.GetComponent<Button>().interactable = true;
            else
                gameObject.GetComponent<Button>().interactable = false;

            m_lastSelectedAssetType = AvatarCreatorContext.selectedAssetType;
        }
	}

    /// <summary>
    /// Called in every button press.
    /// </summary>
    public void OnButtonClick()
    {
        // Update asset list with selected gender's assets.
        // Todo: Maybe move into the AvatarCreatorContext?
        Transform scrollView = GameObject.Find("Canvas").transform.Find("scrollview").transform.Find("Viewport").transform.Find("Content");
        scrollView.DestroyChildren();

        foreach (CBaseAsset asset in m_assetList)
        {
            GameObject btn = Instantiate(Resources.Load<GameObject>("Button"));
            btn.AddComponent<UIScrollViewButtonController>().faceAsset = asset;
            btn.transform.SetParent(scrollView, false);
        }
        
        // Log user action.
        AvatarCreatorContext.logManager.LogAction("UIButtonClick", m_assetGender.ToString());
    }
}
