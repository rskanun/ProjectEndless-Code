using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillInformationWindow : MonoBehaviour
{
    [SerializeField] private SkillInformationWindow upSubWindow; // 애니메이션용 서브창
    [SerializeField] private SkillInformationWindow downSubWindow; // 애니메이션용 서브창
    [Space]
    [SerializeField] private Transform content;
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI nameField;
    [SerializeField] private TextMeshProUGUI typeField;
    [SerializeField] private TextMeshProUGUI infoField;
    [SerializeField] private TextMeshProUGUI descriptionField;

    private RectTransform rectTrans;

    // 애니메이션 설정
    private float openDuration = 0.2f;
    private float swapDuration = 0.35f;
    private Ease swapEase = Ease.InCubic;

    private void OnValidate()
    {
        if (content == null) return;

        rectTrans = content.GetComponent<RectTransform>();
    }

    /// <summary>
    /// 현재 메뉴 화면에 스킬 정보 띄우기
    /// </summary>
    /// <param name="skill">처음 화면에 띄워질 스킬</param>
    public void OpenWindow(Skill skill)
    {
        // 정보 설정
        SetupInformation(skill);

        // 화면 활성화
        gameObject.SetActive(true);

        // 초기 위치 설정
        float width = rectTrans.rect.width;
        transform.localPosition += new Vector3(width, 0);

        // 키 잠금
        ControlContext.Instance.KeyLock();

        // 화면 전환 애니메이션 실행
        // 종료 후 키 잠금 해제
        transform.DOLocalMoveX(0, openDuration)
            .OnComplete(() => ControlContext.Instance.KeyUnlock());
    }

    /// <summary>
    /// 현재 메뉴 화면을 닫고서 캐릭터 정보 불러오기
    /// </summary>
    public void CloseWindow()
    {
        // 위치 저장
        Vector3 originPos = transform.localPosition;

        // 키 잠금
        ControlContext.Instance.KeyLock();

        // 화면 전환 애니메이션 실행
        float width = rectTrans.rect.width;
        transform.DOLocalMoveX(width, openDuration)
            .OnComplete(() =>
            {
                // 애니메이션 종료 후, 화면 비활성화
                gameObject.SetActive(false);

                // 본래 위치 돌아가기
                transform.localPosition = originPos;

                // 키 잠금 해제
                ControlContext.Instance.KeyUnlock();
            });
    }

    /// <summary>
    /// 다른 스킬 정보로 넘어가기
    /// </summary>
    /// <param name="skill"></param>
    /// <param name="isReverseMove">애니메이션이 반대로 작동할 것인지 여부</param>
    public void SwapInfo(Skill skill, bool isReverseMove = false)
    {
        // 서브창 정보 설정
        upSubWindow.SetupInformation(skill);
        downSubWindow.SetupInformation(skill);

        // 정보 전환 애니메이션 실행
        Vector3 originPos = content.localPosition;

        float scrollAmount = rectTrans.rect.height / 3;
        float direction = isReverseMove ? -1f : 1f;
        float endValue = content.localPosition.y + scrollAmount * direction;

        content.DOLocalMoveY(endValue, swapDuration)
            .SetEase(swapEase)
            .OnComplete(() =>
            {
                // 해당 창의 정보 갱신
                SetupInformation(skill);

                // 본래 위치 돌아가기
                content.localPosition = originPos;
            });
    }

    private void SetupInformation(Skill skill)
    {
        icon.sprite = skill.IconSprite;
        nameField.text = skill.Name;
        typeField.text = skill.GetTypeName();
        infoField.text = GetSkillInfoStr(skill);
        descriptionField.text = skill.Description;
    }

    private string GetSkillInfoStr(Skill skill)
    {
        var sb = new System.Text.StringBuilder();

        sb.AppendLine($"소모 기력: {skill.CostSP}");
        sb.AppendLine($"사용 턴: {skill.CostTurn}");
        sb.Append($"타격 범위: {skill.TargetType.GetTypeName()}");

        return sb.ToString();
    }
}