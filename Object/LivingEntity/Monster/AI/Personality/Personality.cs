using UnityEngine;

public abstract class Personality : MonoBehaviour
{
    private Monster _monster;
    protected Monster monster
    {
        get { return _monster; }
    }

    public Personality(Monster monster)
    {
        _monster = monster;
    }

    public abstract IMonsterState OnDetectedPlayer();
}