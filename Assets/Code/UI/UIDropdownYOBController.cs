using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIDropdownYOBController : MonoBehaviour {

    public int MaxYear;
    public int MinYear;

	// Use this for initialization
	void Start () {
        for (int i = MaxYear; i > MinYear; i--)
        {
            Dropdown.OptionData data = new Dropdown.OptionData(i.ToString());
            gameObject.GetComponent<Dropdown>().options.Add(data);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
