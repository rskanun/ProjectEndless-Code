using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NavigationGroup : MonoBehaviour
{
    [SerializeField] private bool vertical;
    [SerializeField] private bool horizontal;

    private void OnValidate()
    {
        SetupChildsNavigation();
    }

    public void SetupChildsNavigation()
    {
        if (vertical) SetupVerticalNavigation();
        if (horizontal) SetupHorizontalNavigation();
    }

    private void SetupVerticalNavigation()
    {
        // 자식 오브젝트 중 버튼 컴포넌트 수집
        List<Button> buttons = new List<Button>(GetComponentsInChildren<Button>());

        // y값에 따른 내림차순 정렬
        buttons.Sort((a, b) =>
        {
            float ay = a.transform.position.y;
            float by = b.transform.position.y;
            return by.CompareTo(ay);
        });

        // 네비게이션 연결
        for (int i = 0; i < buttons.Count; i++)
        {
            Navigation navi = new Navigation();
            navi.mode = Navigation.Mode.Explicit; // 수동 저장 모드
            navi.selectOnUp = (i > 0) ? buttons[i - 1] : null;
            navi.selectOnDown = (i < buttons.Count - 1) ? buttons[i + 1] : null;

            // 새 네비게이션 등록
            buttons[i].navigation = navi;
        }
    }

    private void SetupHorizontalNavigation()
    {
        // 자식 오브젝트 중 버튼 컴포넌트 수집
        List<Button> buttons = new List<Button>(GetComponentsInChildren<Button>());

        // x값에 따른 내림차순 정렬
        buttons.Sort((a, b) =>
        {
            float ax = a.transform.position.x;
            float bx = b.transform.position.x;
            return bx.CompareTo(ax);
        });

        // 네비게이션 연결
        for (int i = 0; i < buttons.Count; i++)
        {
            Navigation navi = new Navigation();
            navi.mode = Navigation.Mode.Explicit; // 수동 저장 모드
            navi.selectOnLeft = (i > 0) ? buttons[i - 1] : null;
            navi.selectOnRight = (i < buttons.Count - 1) ? buttons[i + 1] : null;

            // 새 네비게이션 등록
            buttons[i].navigation = navi;
        }
    }
}