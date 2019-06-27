using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A controller class for text imput fields.
/// </summary>
public class UIInputFieldController : MonoBehaviour {

    private InputField m_inputField;

    /// <summary>
    /// Called only once when application started.
    /// </summary>
	void Start () {
        m_inputField = gameObject.GetComponent<InputField>();
	}

    /// <summary>
    /// Called once in every frame.
    /// </summary>
    void Update () {

        // If the imput field has the focus, remove placeholder text in order to present an empty imput field.
        if(m_inputField.isFocused)
        {
            gameObject.transform.Find("Placeholder").GetComponent<Text>().text = "";
        }
	}
}
