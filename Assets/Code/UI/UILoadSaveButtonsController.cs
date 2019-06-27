using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using UnityEngine.EventSystems;
using System.Text;

/// <summary>
/// A controller class for load/save/screenshot buttons.
/// </summary>
public class UILoadSaveButtonsController : MonoBehaviour {

    /// <summary>
    /// Button type.
    /// </summary>
    public enum ButtonType
    {
        LoadButton,
        SaveButton,
        ScreenShotButton
    }

    public ButtonType m_buttonType;

    /// <summary>
    /// Called only once when application started.
    /// </summary>
    void Start()
    {
        // Register to click event.
        EventTrigger trigger = gameObject.AddComponent<EventTrigger>();
        var pointerDown = new EventTrigger.Entry();
        pointerDown.eventID = EventTriggerType.PointerDown;
        pointerDown.callback.AddListener((e) => OnButtonPointerDown());
        trigger.triggers.Add(pointerDown);
    }

    /// <summary>
    /// Called in every button press.
    /// </summary>
    public void OnButtonPointerDown()
    {
        // Delegate the operation according to button type.
        if (m_buttonType == ButtonType.ScreenShotButton)
            AvatarCreatorContext.takeScreenShot = true;
        else if (m_buttonType == ButtonType.LoadButton)
            AvatarCreatorContext.fileTransferManager.UploadFile();
        else if (m_buttonType == ButtonType.SaveButton)
            AvatarCreatorContext.SaveAvatarToFile();

        // Log user action.
        AvatarCreatorContext.logManager.LogAction("UIButtonClick", m_buttonType.ToString());
    }
}
