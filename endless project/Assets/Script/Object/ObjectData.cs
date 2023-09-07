using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEditor.Experimental;
using UnityEngine;

public abstract class ObjectData : ScriptableObject, INotifyPropertyChanged
{
    [Header("스테이터스")]
    [SerializeField]
    private int _healthPoint;
    /***************************************************************
     * [ 체력 (Health Point) ]
     * 
     * 오브젝트의 생명력 수치로 0이하로 떨어지면 죽는다.
     ***************************************************************/
    public virtual int HP
    {
        get { return _healthPoint; }
        set
        {
            if(_healthPoint != value)
            {
                // 입력값이 음수일 경우
                if (value < 0)
                    _healthPoint = 0;
                // 입력값이 최대치를 초과한 경우
                else if (value > _maxHealthPoint)
                    _healthPoint = _maxHealthPoint;
                else
                    _healthPoint = value;
            }
        }
    }

    [SerializeField]
    private int _maxHealthPoint;
    public int MaxHP
    {
        get { return _maxHealthPoint; }
        set
        {
            // 입력값이 음수일 경우
            if (value < 0)
                _maxHealthPoint = 0;
            else
                _maxHealthPoint = value;
        }
    }

    [SerializeField]
    private int _strength;
    /***************************************************************
    * [ 근력 (Strength) ]
    * 
    * 오브젝트의 근력 수치로 물리 공격력에 영향을 끼친다.
    * 근력 1당 1의 데미지를 준다.
    ****************************************************************/
    public int STR
    {
        get { return _strength; }
        set
        {
            // 입력값이 음수일 경우
            if (value < 0)
                _strength = 0;
            else
                _strength = value;

            OnPropertyChanged("STR");
        }
    }

    [SerializeField]
    private int _agility;
    /***************************************************************
    * [ 민첩 (Agility) ]
    * 
    * 오브젝트의 민첩 수치로 이동속도에 영향을 끼친다.
    ****************************************************************/
    public virtual int Speed
    {
        get { return _agility; }
        set
        {
            // 입력값이 음수일 경우
            if (value < 0)
                _agility = 0;
            else
                _agility = value;
        }
    }

    [SerializeField]
    private int _defensive;
    /***************************************************************
    * [ 방어력 (Defensive) ]
    * 
    * 오브젝트의 방어력 수치로 받는 데미지에 영향을 끼친다.
    * 방어력 1당 1의 데미지를 줄인다.
    ****************************************************************/
    public int DEF
    {
        get { return _defensive; }
        set
        {
            // 입력값이 음수일 경우
            if (value < 0)
                _defensive = 0;
            else
                _defensive = value;

            OnPropertyChanged("DEF");
        }
    }

    [SerializeField]
    private int _manaPoint;
    /***************************************************************
     * [ 마력 (Mana Point) ]
     * 
     * 오브젝트의 마력 수치로 마력과 관련된 데미지와 방어력에 영향을 끼친다.
     * 마력 수치가 높아질수록 마력을 사용한 공격의 데미지가 올라간다.
     * 마력 수치가 높아질수록 마력 공격의 데미지를 줄여 받는다.
     * 플레이어의 경우 마력의 수치에 따라 각성치 초기값이 달라진다.
     ****************************************************************/
    public int MP
    {
        get { return _manaPoint; }
        set
        {
            // 입력값이 음수일 경우
            if (value < 0)
                _manaPoint = 0;
            else
                _manaPoint = value;
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    protected void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
