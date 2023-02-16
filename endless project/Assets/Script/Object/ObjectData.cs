using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObjectData : ScriptableObject
{
    [SerializeField]
    private int healthPoint;
    /***************************************************************
     * [ 체력 (Health Point) ]
     * 
     * 오브젝트의 생명력 수치로 0이하로 떨어지면 죽는다.
     ***************************************************************/
    public int hp
    {
        get { return healthPoint; }
        set
        {
            // 입력값이 음수일 경우
            if (value < 0)
                healthPoint = 0;
            // 입력값이 최대치를 초과한 경우
            else if (value > maxHealthPoint)
                healthPoint = maxHealthPoint;
            else
                healthPoint = value;
        }
    }

    [SerializeField]
    private int maxHealthPoint;
    public int maxHP
    {
        get { return maxHealthPoint; }
        set
        {
            // 입력값이 음수일 경우
            if (value < 0)
                maxHealthPoint = 0;
            else
                maxHealthPoint = value;
        }
    }

    [SerializeField]
    private int strength;
    /***************************************************************
    * [ 근력 (Strength) ]
    * 
    * 오브젝트의 근력 수치로 물리 공격력에 영향을 끼친다.
    * 근력 1당 1의 데미지를 준다.
    ****************************************************************/
    public int str
    {
        get { return strength; }
        set
        {
            // 입력값이 음수일 경우
            if (value < 0)
                strength = 0;
            else
                strength = value;
        }
    }

    [SerializeField]
    private int agility;
    /***************************************************************
    * [ 민첩 (Agility) ]
    * 
    * 오브젝트의 민첩 수치로 이동속도에 영향을 끼친다.
    ****************************************************************/
    public int speed
    {
        get { return agility; }
        set
        {
            // 입력값이 음수일 경우
            if (value < 0)
                agility = 0;
            else
                agility = value;
        }
    }

    [SerializeField]
    private int defensive;
    /***************************************************************
    * [ 방어력 (Defensive) ]
    * 
    * 오브젝트의 방어력 수치로 받는 데미지에 영향을 끼친다.
    * 방어력 1당 1의 데미지를 줄인다.
    ****************************************************************/
    public int def
    {
        get { return defensive; }
        set
        {
            // 입력값이 음수일 경우
            if (value < 0)
                defensive = 0;
            else
                defensive = value;
        }
    }
}
