using UnityEngine;

public abstract class Personality : MonoBehaviour
{
    public abstract IMonsterState OnPlayerDetected();
}