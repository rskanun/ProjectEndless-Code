using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class InsertIcon : TimelineIcon
{
    public Image iconImage;
    public Image highlight;

    [SerializeField]
    private float blinkDelay = 1.0f;

    private void Start()
    {
        // 하이라이트 깜빡임 무한반복
        DOTween.Sequence()
            .Append(highlight.DOFade(0f, blinkDelay))
            .Append(highlight.DOFade(1f, blinkDelay))
            .SetLoops(-1);
    }

    public void SetImage(GameObject actor)
    {
        SpriteRenderer actorImg = actor.GetComponent<SpriteRenderer>();

        // 삽입 아이콘 이미지 지정
        InitImage(actorImg);
    }

    private void InitImage(SpriteRenderer sprite)
    {
        // 임시 색으로 지정
        iconImage.color = sprite.color;
    }
}