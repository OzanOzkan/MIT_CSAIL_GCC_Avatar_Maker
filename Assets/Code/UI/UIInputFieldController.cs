using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInputFieldController : MonoBehaviour {

    private InputField m_inputField;

	// Use this for initialization
	void Start () {
        m_inputField = gameObject.GetComponent<InputField>();
	}
	
	// Update is called once per frame
	void Update () {
        if(m_inputField.isFocused)
        {
            gameObject.transform.Find("Placeholder").GetComponent<Text>().text = "";
        }
	}
}
