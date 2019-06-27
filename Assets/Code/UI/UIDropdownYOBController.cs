using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A controller class for Year of Birth UI field.
/// </summary>
public class UIDropdownYOBController : MonoBehaviour {

    // Minimum and maximum years, configured from Unity editor.
    public int MaxYear;
    public int MinYear;

    /// <summary>
    /// Called only once when application started.
    /// </summary>
	void Start () {

        // Populate dropdown list with given year range.
        for (int i = MaxYear; i > MinYear; i--)
        {
            Dropdown.OptionData data = new Dropdown.OptionData(i.ToString());
            gameObject.GetComponent<Dropdown>().options.Add(data);
        }
	}

    /// <summary>
    /// Called once in every frame.
    /// </summary>
    void Update () {
		
	}
}
