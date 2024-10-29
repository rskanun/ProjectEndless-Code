using UnityEngine;

public class EventManager : MonoBehaviour
{
    [Header("게임 데이터")]
    [SerializeField] private PlayerData playerData;

    public void GetCommandEvent(string str)
    {
        string[] commands = str.Split(' ');
        string command = commands[0];

        switch (command)
        {
            case "addAP":
                // 커맨드의 두 번째 단어가 숫자일 경우 해당 숫자만큼 AP 증가
                if (int.TryParse(commands[1], out int ap))
                {
                    playerData.AP = ap;
                }
                break;

            default:
                Debug.Log(command + " is an incorrect command!");
                break;
        }
    }
}
