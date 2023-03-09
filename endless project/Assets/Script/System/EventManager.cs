using Assets.Script.System.Stat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [SerializeField]
    private PlayerData player;

    [Space]
    [Header("참조 스크립트")]
    public HUD hud;

    public void getCommandEvent(string str)
    {
        string[] commands = str.Split(' ');
        string command = str.Split(' ')[0];

        switch (command)
        {
            case "getDamage":
                // 커맨드의 두 번째 단어가 숫자일 경우 해당 숫자만큼 데미지
                if (int.TryParse(commands[1], out int damage))
                {
                    getDamage(damage);
                }
                break;

            case "addAP":
                // 커맨드의 두 번째 단어가 숫자일 경우 해당 숫자만큼 AP 증가
                if (int.TryParse(commands[1], out int ap))
                {
                    addAP(ap);
                }
                break;

            default:
                Debug.Log(command + " is an incorrect command!");
                break;
        }
    }

    /************************************************************
    * [커맨드 이벤트]
    * 
    * Text Manager를 통해 읽은 커맨드 이벤트 관리
    ************************************************************/

    void getDamage(int damage) { hud.HP -= damage; }
    void addAP(int ap) { hud.AP += ap; }
}
