using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    private Dictionary<int, string[]> textData;

    // Game Object
    public Text textLine;
    public GameObject textDialogue;

    // Variable
    private int textLineNum;
    private int lineCnt;

    private string[] lines;

    private float typingSpeed;
    private float setTypingSpeed;

    private bool isTalking = false;

    private void Awake()
    {
        // 텍스트와 텍스트창 숨김
        textLine.gameObject.SetActive(false);
        textDialogue.gameObject.SetActive(false);

        // 타이핑 속도 가져오기
        setTypingSpeed = Option.getTypingSpeed();

        // 텍스트 데이터 삽입
        textData = new Dictionary<int, string[]>();
        initText();
    }

    public bool talk(NPC npc)
    {
        // 대화 처음 시작 시 텍스트 창 활성화 및 대화 가져오기
        if (textLineNum == 0)
        {
            initTalk(npc);
        }

        // 대화 진행
        if (textLineNum <= (lines.Length - 1))
        {
            talking();
            return true;
        }

        // 대화 종료 시 초기화 및 텍스트 창 비활성화
        else
        {
            initTalk(npc);
            return false;
        }
            
    }

    private void initTalk(NPC npc)
    {
        // 대화 시작 시
        if (!isTalking)
        {
            // 텍스트 및 텍스트창 활성화
            textLine.gameObject.SetActive(true);
            textDialogue.gameObject.SetActive(true);

            // 해당되는 대화목록 가져오기
            lines = getText(npc.id);

            // 대화 진행상태로 변경
            isTalking = true;
        }

        // 대화 종료 시
        else
        {
            // 변수 초기화
            textLineNum = 0;
            lineCnt = 0;
            textLine.text = "";

            // 텍스트 및 텍스트창 비활성화
            textLine.gameObject.SetActive(false);
            textDialogue.gameObject.SetActive(false);

            // 대화 종료상태로 변경
            isTalking = false;
        }
    }

    private void talking()
    {
        // 대화 목록 중 표시될 대화 가져오기
        string str = lines[textLineNum];

        // 한 글자도 출력이 안 됐을 경우
        if (lineCnt == 0)
        {
            // 지정된 타이핑 속도로 출력
            typingSpeed = setTypingSpeed;
            textLine.text = "";
            StartCoroutine(talkDelay(lines[textLineNum]));

        }

        // 대화 출력 도중이라면
        else if (lineCnt < str.Length)
        {
            // 한 번에 출력
            typingSpeed = 0;
        }
    }
    IEnumerator talkDelay(string str)
    {
        // 대화 진행 도중일 경우
        while (lineCnt < str.Length)
        {
            // 한 글자씩 대화를 출력
            textLine.text = str.Substring(0, lineCnt + 1); // #substring 효율문제 질문
            lineCnt++;

            yield return new WaitForSeconds(typingSpeed);

        }

        // 모두 출력했다면 남은 글자수를 0으로 초기화 후 다음 라인으로 넘어가기
        lineCnt = 0;
        textLineNum++;
    }

    private void initText()
    {
        // 대화 목록
        // 추후 CSV 방식으로 바꿀 예정
        textData.Add(1, new string[]
        {
            "네가 이곳의 주인공이구나?",
            "그거 알아? \r\n사실 이곳은 가상세계야."
        });

        textData.Add(2, new string[]
        {
            "표지판에 박힌 가시에 찔렸다."
        });
    }

    public string[] getText(int id)
    {
        // 해당되는 대화 목록 가져오기
        return textData[id];
    }
}
