using System.IO;
using UnityEngine;

public enum Key
{
    up, down, right, left,
    jump, action, dash, interact
}

public class Option : MonoBehaviour
{
    // 저장 파일 위치
    private string folder   = @"Assets\Resources";
    private string file     = @"Assets\Resources\option.txt";

    // 컨트롤키
    private static string left  = "left";
    private static string right = "right";
    private static string up    = "up";
    private static string down  = "down";

    // 액션키
    private static string action    = "mouse 0";
    private static string dash      = "mouse 1";
    private static string interact  = "e";

    // 스크립트 속도
    private static float typingSpeed = 0.025f;

    DirectoryInfo di;

    private void Awake()
    {
        // 폴더 위치 설정
        di = new DirectoryInfo(folder);

        // 설정 불러오기
        loadOption();
    }

    public static float getTypingSpeed()
    {
        return typingSpeed;
    }

    public static string getKey(Key key)
    {
        switch (key)
        {
            case Key.left:
                return left;

            case Key.right:
                return right;

            case Key.up:
                return up;

            case Key.down:
                return down;

            case Key.action:
                return action;

            case Key.dash:
                return dash;

            case Key.interact:
                return interact;
        }

        return null;
    }

    private void loadOption()
    {
        if (!di.Exists)
            di.Create();

        if (!File.Exists(file))
            saveOption();

        foreach (string s in File.ReadLines(file))
        {
            setKey(s);
        }
    }

    private void setKey(string s)
    {
        string[] str = s.Split(new string[] { ": " }, System.StringSplitOptions.None);

        switch (str[0])
        {
            case "attack":
                action = str[1];
                break;

            case "dash":
                dash = str[1];
                break;

            case "interact":
                interact = str[1];
                break;

            default:
                break;
        }
    }

    private void saveOption()
    {
        if (!di.Exists)
            di.Create();

        StreamWriter textWriter = File.CreateText(file);
        textWriter.WriteLine(
            "action: " + action
            + "\ndash: " + dash
            + "\ninteract: " + interact);

        textWriter.Dispose();
    }
}
