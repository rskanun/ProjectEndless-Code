using System.IO;
using UnityEngine;

public class Option : MonoBehaviour
{
    // 저장 파일 위치
    private string folder   = @"Assets\Resources";
    private string file     = @"Assets\Resources\option.txt";

    // 컨트롤키
    public static KeyCode left  = KeyCode.LeftArrow;
    public static KeyCode right = KeyCode.RightArrow;
    public static KeyCode up    = KeyCode.UpArrow;
    public static KeyCode down  = KeyCode.DownArrow;

    // 액션키
    public static KeyCode action    = KeyCode.Mouse0;
    public static KeyCode dash      = KeyCode.Mouse1;
    public static KeyCode interact  = KeyCode.E;

    // 선택키
    public static KeyCode select    = KeyCode.Return;

    // 취소 및 돌아가기 키
    public static KeyCode cancel    = KeyCode.Escape;

    // 옵션(ESC)키
    public static KeyCode menu = KeyCode.Escape;

    // 스크립트 속도
    public static float typingSpeed = 0.025f;

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

    private void loadOption()
    {
        if (!di.Exists)
            di.Create();

        if (!File.Exists(file))
            saveOption();

        foreach (string s in File.ReadLines(file))
        {
            
        }
    }

    private void saveOption()
    {
        if (!di.Exists)
            di.Create();

        StreamWriter textWriter = File.CreateText(file);
        textWriter.WriteLine();

        textWriter.Dispose();
    }
}
