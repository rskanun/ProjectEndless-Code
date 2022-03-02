using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cursor : MonoBehaviour
{
    public Texture2D icon;

    void Start()
    {
        StartCoroutine("mouse_position");
    }

    IEnumerator mouse_position()
    {
        // 모든 렌더링이 완료될 때까지 대기
        yield return new WaitForEndOfFrame();

        Vector2 mPos = Vector2.zero;

        // 커서가 이미지의 가운데가 되도록 설정
        mPos.x = icon.width / 2;
        mPos.y = icon.height / 2;

        Cursor.SetCursor(icon, mPos, CursorMode.ForceSoftware);
    }
}
