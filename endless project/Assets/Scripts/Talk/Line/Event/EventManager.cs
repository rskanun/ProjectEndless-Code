using UnityEngine;

public class EventManager : MonoBehaviour
{
    [Header("참조 스크립트")]
    [SerializeField] private Player player;

    public void getCommandEvent(string str)
    {
        string[] commands = str.Split(' ');
        string command = commands[0];

        switch (command)
        {
            case "getDamage":
                // 커맨드의 두 번째 단어가 숫자일 경우 해당 숫자만큼 데미지
                if (int.TryParse(commands[1], out int damage))
                {
                    player.OnDamage(damage);
                }
                break;

            case "addAP":
                // 커맨드의 두 번째 단어가 숫자일 경우 해당 숫자만큼 AP 증가
                if (int.TryParse(commands[1], out int ap))
                {
                    player.ApproachAwaken(ap);
                }
                break;

            default:
                Debug.Log(command + " is an incorrect command!");
                break;
        }
    }
}
