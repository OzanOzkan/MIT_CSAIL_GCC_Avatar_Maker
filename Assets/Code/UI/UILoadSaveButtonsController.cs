using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.EventSystems;
using System.Text;

public class UILoadSaveButtonsController : MonoBehaviour {

    public enum ButtonType
    {
        LoadButton,
        SaveButton,
        ScreenShotButton
    }

    public ButtonType m_buttonType;

    // Use this for initialization
    void Start()
    {
        EventTrigger trigger = gameObject.AddComponent<EventTrigger>();
        var pointerDown = new EventTrigger.Entry();
        pointerDown.eventID = EventTriggerType.PointerDown;
        pointerDown.callback.AddListener((e) => OnButtonPointerDown());
        trigger.triggers.Add(pointerDown);
    }

    public void OnButtonPointerDown()
    {
        if (m_buttonType == ButtonType.ScreenShotButton)
            AvatarCreatorContext.takeScreenShot = true;
        else if (m_buttonType == ButtonType.LoadButton)
            AvatarCreatorContext.fileTransferManager.UploadFile();
        else if (m_buttonType == ButtonType.SaveButton)
            AvatarCreatorContext.SaveAvatarToFile();

        AvatarCreatorContext.logManager.LogAction("UIButtonClick", m_buttonType.ToString());
    }
}
