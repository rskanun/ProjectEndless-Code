using System.Collections;
using TMPro;
using UnityEngine;

public class Dialogue : MonoBehaviour
{
    private bool isPrinting;
    private string currentText;
    public bool IsPrinting => isPrinting;

    // 글자 타이핑 코루틴
    private Coroutine typingCoroutine;

    [Header("대화 관련 오브젝트")]
    [SerializeField] private TextMeshProUGUI nameField;
    [SerializeField] private TextMeshProUGUI textField;
    [SerializeField] private GameObject endMark;

    public void SetDialogView(bool isView)
    {
        gameObject.SetActive(isView);
    }

    public void PrintText(string text)
    {
        isPrinting = true;
        currentText = text;

        // 다이어로그 활성화
        SetDialogView(true);

        // 타이핑 중인 텍스트가 있다면 멈춤
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);

        typingCoroutine = StartCoroutine(TextDelayPrint(text));
    }

    private IEnumerator TextDelayPrint(string line)
    {
        // 출력할 문장이 null인 경우 빈 값으로 넣기
        if (line == null) line = "";

        int textCnt = 0;
        textField.text = "";

        float typingSpeed = OptionData.Instance.TypingSpeed;
        WaitForSeconds typing = new WaitForSeconds(typingSpeed);

        // 텍스트 출력 종료 표시 제거
        endMark.SetActive(false);

        // 대화 진행 도중일 경우
        while (textCnt < line.Length && isPrinting)
        {
            yield return typing;

            // 한 글자씩 대화를 출력
            textField.text += line[textCnt++];
        }

        // 텍스트 출력 종료 시 표시 띄우기
        endMark.SetActive(true);

        typingCoroutine = null;
        isPrinting = false;
    }

    public void TextSkip()
    {
        isPrinting = false;

        // 타이핑 출력 종료
        StopCoroutine(typingCoroutine);

        // 모든 텍스트 띄우기
        textField.text = currentText;

        // 텍스트 출력 종료 표시 띄우기
        endMark.SetActive(true);
    }

    public void SetName(string name)
    {
        nameField.text = name;
    }
}
