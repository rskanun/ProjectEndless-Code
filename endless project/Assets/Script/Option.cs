using System.IO;

public class Option
{
    // 저장 파일 위치
    private string folder   = @"Assets\Resources";
    private string file     = @"Assets\Resources\option.txt";

    // 컨트롤키
    private static string left = "left";
    private static string right = "right";
    private static string up = "up";
    private static string down = "down";

    // 액션키
    private static string key_attack = "mouse 0";
    private static string key_dash     = "mouse 1";
    private static string key_interact = "e";

    DirectoryInfo di;

    public void init()
    {
        di = new DirectoryInfo(folder);

        loadOption();
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

            case Key.attack:
                return key_attack;

            case Key.dash:
                return key_dash;

            case Key.interact:
                return key_interact;
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
                key_attack = str[1];
                break;

            case "dash":
                key_dash = str[1];
                break;

            case "interact":
                key_interact = str[1];
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
            "attack: " + key_attack
            + "\ndash: " + key_dash
            + "\ninteract: " + key_interact);

        textWriter.Dispose();
    }
}
