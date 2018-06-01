using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;

public class LogManager : MonoBehaviour
{
    protected StringBuilder m_actionLogs;

    public LogManager()
    {
        //  m_actions = new List<CLogAction>();
        m_actionLogs = new StringBuilder();
        m_actionLogs.Append("<Actions>\n");
    }

    public void LogAction(string action, string value)
    {
        //m_actions.Add(new CLogAction(action, value));
        string rowdata = "<Action date=\"" + System.DateTime.Now.ToString()
            + "\" content=\"" + action + "\" value=\"" + value + "\"/>\n";

        m_actionLogs.Append(rowdata);
    }

    public string DumpLogs()
    {
        Debug.Log("LogManager:DumpLogs(): Dumping logs to server.");

        m_actionLogs.Append("</Actions>\n");

        string data = "<SaveFile>\n" + AvatarCreatorContext.faceObject.Serialize() + "\n" + m_actionLogs.ToString() + "</SaveFile>";

        StartCoroutine(PHPRequest(data));

        return data;
    }

    IEnumerator PHPRequest(string data)
    {
        bool successfull = true;

        WWWForm form = new WWWForm();
        form.AddField("filename", AvatarCreatorContext.sessionguid.ToString());
        form.AddField("data", data);
        WWW www = new WWW("http://localhost:8080/savetelemetry.php", form);

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