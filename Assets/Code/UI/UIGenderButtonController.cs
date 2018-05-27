using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIGenderButtonController : MonoBehaviour {

    public AssetGender m_assetGender;
    private List<CBaseAsset> m_assetList;
    private AssetType m_lastSelectedAssetType = AssetType.None;

	// Use this for initialization
	void Start ()
    {
        gameObject.GetComponent<Button>().interactable = false;
        gameObject.GetComponent<Button>().onClick.AddListener(() => OnButtonClick());
    }
	
	// Update is called once per frame
	void Update () {
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

    public void OnButtonClick()
    {
        // Todo: Maybe into the AvatarCreatorContext?
        Transform scrollView = GameObject.Find("Canvas").transform.Find("scrollview").transform.Find("Viewport").transform.Find("Content");
        scrollView.DestroyChildren();

        foreach (CBaseAsset asset in m_assetList)
        {
            GameObject btn = Instantiate(Resources.Load<GameObject>("Button"));
            btn.AddComponent<UIScrollViewButtonController>().faceAsset = asset;
            btn.transform.SetParent(scrollView, false);
        }
    }
}
