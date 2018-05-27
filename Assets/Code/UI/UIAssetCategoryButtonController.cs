using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAssetCategoryButtonController : MonoBehaviour
{
    public AssetType m_assetType;

    // Use this for initialization
    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(() => OnButtonClick());
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnButtonClick()
    {
        AvatarCreatorContext.selectedAssetType = m_assetType;
        Debug.Log("Asset type changed to: " + m_assetType.ToString());

        // Todo: Maybe into the AvatarCreatorContext?
        Transform scrollView = GameObject.Find("Canvas").transform.Find("scrollview").transform.Find("Viewport").transform.Find("Content");
        scrollView.DestroyChildren();

        foreach (CBaseAsset asset in AvatarCreatorContext.GetLoadedAssetsByType(m_assetType))
        {
            GameObject btn = Instantiate(Resources.Load<GameObject>("Button"));
            btn.AddComponent<UIScrollViewButtonController>().faceAsset = asset;
            btn.transform.SetParent(scrollView, false);
        }
    }
}
