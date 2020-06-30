using UnityEngine;
using TMPro;

public static class Debugger
{
    public static void CreateLog(string text)
    {
        Debug.Log(text);
        MainMenuController.Instance.lastLog.text = text;
    }
}
