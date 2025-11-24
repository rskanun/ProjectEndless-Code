using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEditor;
using UnityEngine;

public abstract class EntityData : ScriptableObject
{
#if UNITY_EDITOR
    // 테스트 플레이 시 변경되는 값을 되돌리기 위한
    // 백업 데이터의 Json 형태
    private string backupJson;
#endif

    [SerializeField, PropertyOrder(0)]
    private string _name;
    public string Name => _name;

    [SerializeField, PropertyOrder(0)]
    [PreviewField]
    private Sprite _icon;
    public Sprite Icon => _icon;

    [SerializeField, PropertyOrder(1)]
    private BattlePosition _position;
    public BattlePosition Position => _position;

    [SerializeField, PropertyOrder(1)]
    private AttackType _attackType;
    public AttackType AttackType => _attackType;

    [SerializeField, PropertyOrder(1)]
    private PersonalityType _personality;
    public PersonalityType Personality => _personality;

    [Title("스킬 정보")]
    [SerializeField, PropertyOrder(10)]
    private List<Skill> _skills;
    public List<Skill> Skills => _skills;

    [Title("능력치 정보")]
    [SerializeField, PropertyOrder(20)]
    private EntityStats _stats;
    public EntityStats Stats => _stats;

    public void UpdateStats(EntityStats newStats)
    {
        _stats.CopyTo(newStats);
    }

#if UNITY_EDITOR
    private void OnEnable()
    {
        EditorApplication.playModeStateChanged += OnPlayModeChanged;
    }

    private void OnDisable()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeChanged;
    }

    private void OnPlayModeChanged(PlayModeStateChange state)
    {
        // 테스트 플레이 직전일 경우
        if (state == PlayModeStateChange.ExitingEditMode)
        {
            // 현재 데이터 백업
            BackupData();
        }
        // 에디터로 돌아온 경우
        else if (state == PlayModeStateChange.EnteredEditMode)
        {
            // 백업한 데이터로 되돌리기
            RestoreData();
        }
    }
    private void BackupData()
    {
        // 현재 데이터를 Json 형태로 저장
        backupJson = JsonUtility.ToJson(this);
    }

    private void RestoreData()
    {
        // 백업 데이터가 없는 경우 무시
        if (string.IsNullOrEmpty(backupJson))
        {
            return;
        }

        // Json 형태로 저장한 데이터를 현재 객체에 되돌리기
        JsonUtility.FromJsonOverwrite(backupJson, this);
    }
#endif
}