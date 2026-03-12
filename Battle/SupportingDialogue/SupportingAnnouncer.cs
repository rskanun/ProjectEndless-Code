using UnityEngine;

[CreateAssetMenu(fileName = "Announcer", menuName = "Supporting Announcer")]
public class SupportingAnnouncer : ScriptableObject
{
    [Header("개전 대사")]
    [SerializeField, TextArea(1, 2)]
    private string _battleStart; // 일반 전투 시작
    public string BattleStart => _battleStart;

    [SerializeField, TextArea(1, 2)]
    private string _playerAmbush; // 플레이어 기습으로 시작
    public string PlayerAmbush => _playerAmbush;

    [SerializeField, TextArea(1, 2)]
    private string _enemyAmbush; // 적 기습으로 시작
    public string EnemyAmbush => _enemyAmbush;

    [SerializeField, TextArea(1, 2)]
    private string _bossEncount; // 보스전 시작
    public string BossEncount => _bossEncount;

    [Header("반격 대사")]
    [SerializeField, TextArea(1, 2)]
    private string _parryingSuccess; // 패링에 성공하여 적을 흐트러놓았을 때
    public string ParryingSuccess => _parryingSuccess;

    [Header("적 처리 대사")]
    [SerializeField, TextArea(1, 2)]
    private string _killEnemy; // 적을 처리한 경우
    public string KillEnemy => _killEnemy;

    [Header("파티 기절 대사")]
    [SerializeField, TextArea(1, 2)]
    private string _knockdownPlayer; // 플레이어의 파티 중 누군가 쓰러진 경우
    public string KnockdownPlayer => _knockdownPlayer;

    [Header("전투 승리 대사")]
    [SerializeField, TextArea(1, 2)]
    private string _battleVictory; // 전투 승리 시 대사
    public string BattleVictory => _battleVictory;
}