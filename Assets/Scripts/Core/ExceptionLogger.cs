using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class ExceptionLogger : CreateSingletonGameObject<ExceptionLogger>
{

    private StreamWriter m_SW;

    private const string m_LogFileName = "ErrorLog.txt";

    // Use this for initialization
    void Start()
    {
        m_SW = new StreamWriter(Application.persistentDataPath + "/" + m_LogFileName, true);
    }

    private void OnEnable()
    {
        Application.logMessageReceived += HandleLog;
    }

    private void OnDisable()
    {
        Application.logMessageReceived -= HandleLog;
    }

    private void HandleLog(string logString, string stackTrace, LogType type)
    {
        if(type == LogType.Exception || type == LogType.Error)
        {
            m_SW.WriteLine("Logged At: " + System.DateTime.Now.ToString() + " *** Log Description: " + logString +
                " *** Stack Trace: " + stackTrace + " *** Log Type: " + type + " *** \n");
        }
    }

    public void Create() { }

    private void OnDestroy()
    {
        m_SW.Close();
    }
}
