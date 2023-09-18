using System.ComponentModel;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(fileName = "Monster", menuName = "scriptable Object/Monster", order = 3)]
public class MonsterData : ObjectData, INotifyPropertyChanged
{
    public override int HP
    {
        get { return base.HP; }
        set
        {
            base.HP = value;
            OnPropertyChanged("HP");
        }
    }

    public override int Mana 
    { 
        get => base.Mana; 
        set
        {
            base.Mana = value;
            OnPropertyChanged("Mana");
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged(string propertyName)
    {
        if (PropertyChanged != null)
        {
            PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    // 해당 몬스터 ai
}