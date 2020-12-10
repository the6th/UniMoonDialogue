using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugLogToText : MonoBehaviour
{
    [SerializeField]
    Text logText = null;
    [SerializeField]
    TextMesh logTextMesh = null;

    private const int LOG_MAX = 20;
    private Queue<string> logStack = new Queue<string>(LOG_MAX);

    // Use this for initialization
    void Awake()
    {
        Application.logMessageReceived += LogCallback;  // ログが書き出された時のコールバック設定
        Debug.LogWarning("DebugLogToText:start");   // テストでワーニングログをコール
    }

    // Update is called once per frame
    void Update()
    {
        if (this.logStack == null || this.logStack.Count == 0)
            return;
        string s = null;
        foreach (string log in logStack)
        {
            s += log;
        }
        if (logText)
            logText.text = s;
        if (logTextMesh)
            logTextMesh.text = s;
    }

    private void OnDestroy()
    {
        Application.logMessageReceived -= LogCallback;  // 
    }

    /// <summary>
    /// ログを取得するコールバック
    /// </summary>
    /// <param name="condition">メッセージ</param>
    /// <param name="stackTrace">コールスタック</param>
    /// <param name="type">ログの種類</param>
    public void LogCallback(string condition, string stackTrace, LogType type)
    {
        // 通常ログまで表示すると邪魔なので無視
        //一旦warning消す
        //if (type == LogType.Warning) return;

        string trace = null;
        string color = null;

        switch (type)
        {
            case LogType.Log:
                // UnityEngine.Debug.XXXの冗長な情報をとる
                trace = stackTrace.Remove(0, (stackTrace.IndexOf("\n") + 1));
                color = "white";
                break;
            case LogType.Warning:
                trace = stackTrace.Remove(0, (stackTrace.IndexOf("\n") + 1));
                color = "aqua";
                break;
            case LogType.Error:
            case LogType.Assert:
                trace = stackTrace.Remove(0, (stackTrace.IndexOf("\n") + 1));
                color = "red";
                break;
            case LogType.Exception:
                trace = stackTrace;
                color = "red";
                break;
        }

        // ログの行制限
        if (this.logStack.Count == LOG_MAX) this.logStack.Dequeue();

        string message = string.Format("<color={0}>{1}</color>\r\n", color, condition, ""/*trace*/);
        this.logStack.Enqueue(message);
    }
}