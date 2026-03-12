using System.Collections.Generic;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.UI;

public class ImageRenderer : MonoBehaviour
{
    private Dictionary<string, GameObject> activeImages = new();

    public void AllDestoryImages()
    {
        // 현재 활성화된 모든 이미지 파괴
        activeImages.Values.ForEach(obj => Destroy(obj));

        // 관리 목록 초기화
        activeImages.Clear();
    }

    public void DisplayImage(string guid, Sprite sprite, int x, int y)
    {
        var imgObj = CreateImageObject(sprite, x, y);

        // Sprite 이미지를 토대로 생성된 이미지 오브젝트 Dictionary에 추가
        if (imgObj != null) activeImages.Add(guid, imgObj);
    }

    public void DestroyImage(string guid)
    {
        // 활성화된 이미지 오브젝트 목록에서 탐색
        if (!activeImages.TryGetValue(guid, out var destroyObj))
        {
            // 오브젝트가 존재하지 않는다면 오류를 출력하고서 돌아가기
            Debug.LogWarning($"활성화된 이미지 중에 {guid}은(는) 존재하지 않습니다.");
            return;
        }

        // 목록에서 제거
        activeImages.Remove(guid);

        // 탐색된 오브젝트 제거
        Destroy(destroyObj);
    }

    public void TransformImage(string guid, Vector2 transPos, Color transColor)
    {
        // 활성화된 이미지 오브젝트 목록에서 탐색
        if (!activeImages.TryGetValue(guid, out var transObj))
        {
            // 오브젝트가 존재하지 않는다면 오류를 출력하고서 돌아가기
            Debug.LogWarning($"활성화된 이미지 중에 {guid}은(는) 존재하지 않습니다.");
            return;
        }

        // 위치값 수정
        transObj.transform.localPosition = transPos;

        // 색상 수정
        var component = transObj.GetComponent<Image>();
        component.color = transColor;

        // 수정된 이미지가 가장 앞에 오도록 순서 변경
        transObj.transform.SetAsFirstSibling();
    }

    /// <summary>
    /// Sprite 사이즈에 맞게 이미지 오브젝트 생성 및 위치
    /// </summary>
    private GameObject CreateImageObject(Sprite sprite, int x, int y)
    {
        if (sprite == null)
        {
            Debug.LogWarning("화면에 출력할 sprite 값이 존재하지 않습니다.");
            return null;
        }

        // 이미지를 띄울 오브젝트 생성
        GameObject imgObj = new GameObject("Image Object");

        // 해당 컴포넌트가 장착된 오브젝트를 부모로 설정
        imgObj.transform.SetParent(gameObject.transform, false);

        // 이미지 컴포넌트 추가 및 스프라이트 설정
        Image img = imgObj.AddComponent<Image>();
        img.sprite = sprite;

        // 이미지의 사이즈에 맞게 조정
        float w = sprite.rect.width;
        float h = sprite.rect.height;
        imgObj.GetComponent<RectTransform>().sizeDelta = new Vector2(w, h);

        // 이미지 위치 설정
        imgObj.transform.localPosition = new Vector3(x, y);

        // 만들어진 이미지 오브젝트 반환
        return imgObj;
    }
}