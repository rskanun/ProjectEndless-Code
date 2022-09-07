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
        set { healthPoint = value; }
    }

    [SerializeField]
    private int maxHealthPoint;
    public int maxHp
    {
        get { return maxHealthPoint; }
        set { maxHealthPoint = value; }
    }

    [SerializeField]
    private int strength;
    /***************************************************************
    * [ 근력 (Strength) ]
    * 
    * 오브젝트의 근력 수치로 공격력에 영향을 끼친다.
    * 근력 1당 1의 데미지를 준다.
    ****************************************************************/
    public int str
    {
        get { return strength; }
        set { strength = value; }
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
        set { agility = value; }
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
        set { defensive = value; }
    }
}
