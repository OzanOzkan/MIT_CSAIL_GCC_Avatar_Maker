using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

/// <summary>
/// A class for logging user's actions during runtime. Those actions are used for analyzing user's behaviors.
/// </summary>
public class LogManager : MonoBehaviour
{
    protected StringBuilder m_actionLogs;

    /// <summary>
    /// Constructor.
    /// </summary>
    public LogManager()
    {
        //  m_actions = new List<CLogAction>();
        m_actionLogs = new StringBuilder();
        m_actionLogs.Append("<Actions>\n");
    }

    /// <summary>
    /// Logs an action.
    /// </summary>
    /// <param name="action">Action name.</param>
    /// <param name="value">Action value.</param>
    public void LogAction(string action, string value)
    {
        //m_actions.Add(new CLogAction(action, value));
        string rowdata = "<Action date=\"" + System.DateTime.Now.ToString()
            + "\" content=\"" + action + "\" value=\"" + value + "\"/>\n";

        m_actionLogs.Append(rowdata);
    }

    /// <summary>
    /// Saves user's all of recorded logs to server.
    /// </summary>
    /// <returns></returns>
    public string DumpLogs()
    {
        Debug.Log("LogManager:DumpLogs(): Dumping logs to server.");

        m_actionLogs.Append("</Actions>\n");

        string data = "<SaveFile>\n" 
            + AvatarCreatorContext.SerializeUIFields()
            + AvatarCreatorContext.faceObject.Serialize() + "\n" 
            + m_actionLogs.ToString() 
            + "</SaveFile>";

        StartCoroutine(PHPRequest(data));

        return data;
    }

    /// <summary>
    /// Creates a PHP request to server for saving serialized data into a file, which will be stored on server side.
    /// This function called by DumpLogs()
    /// </summary>
    /// <param name="data">Serialized data.</param>
    /// <returns></returns>
    IEnumerator PHPRequest(string data)
    {
        bool successfull = true;

        WWWForm form = new WWWForm();
        form.AddField("filename", AvatarCreatorContext.sessionguid.ToString());
        form.AddField("data", data);
        WWW www = new WWW("http://127.0.0.1/projects/avatarmaker/savetelemetry.php", form);

        yield return www;
        if (www.error != null)
        {
            successfull = false;
        }
        else
        {
            Debug.Log(www.text);
            successfull = true;
        }
    }
}