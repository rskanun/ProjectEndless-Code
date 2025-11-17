using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

public class TalkManager : MonoBehaviour
{
    [Header("참조 스크립트")]
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private SelectManager selectManager;
    [SerializeField] private ImageRenderer imageRenderer;
    [SerializeField] private EventManager eventManager;

    [Space]
    // 현재 라인 진행 상황
    [SerializeField] private bool isPrinting;
    [SerializeField] private bool isTalking;
    [SerializeField] private int selectIndex;
    private Coroutine readLineCoroutine;
    private Line readLine;
    private Npc talkedNPC;

    // Line 처리 핸들러
    private Dictionary<LineType, Action<Line>> lineHandler;

    private void Awake()
    {
        lineHandler = new()
        {
            {LineType.Text,         line => PrintTextLine((TextLine)line)},
            {LineType.Select,       line => ActiveSelection((SelectLine)line)},
            {LineType.Image,        line => RenderImage((ImageLine)line)},
            {LineType.Destroy,      line => DestroyImage((DestroyLine)line)},
            {LineType.Transform,    line => TransformImage((TransformLine)line)},
            {LineType.BGM,          line => { } },
            {LineType.SE,           line => { } },
            {LineType.Event,        line => { } },
        };
    }

    private void OnEnable()
    {
        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
    }

    private void OnDisable()
    {
        LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
    }

    private void OnLocaleChanged(Locale newLocale)
    {
        // 언어 변경이 일어났다면 테이블 다시 불러오기
        ScenarioManager.Instance.ReloadLocalizationTables().Forget();

        // 현재 진행 중인 라인 중단
        StopCoroutine(readLineCoroutine);

        // 마지막 진행 중이던 라인 재시작
        readLineCoroutine = StartCoroutine(ReadLines(talkedNPC, readLine));
    }

    public void TalkHandler()
    {
        if (isTalking)
        {
            // 대화 중이면 해당 대화를 스킵
            SkipToTalk();
        }
    }

    private void SkipToTalk()
    {
        // 선택창이 띄워진 경우 패스
        if (selectManager.IsSelectOpen) return;

        if (dialogueManager.IsPrinting)
        {
            // 텍스트가 출력 중인 경우 한 번에 출력
            dialogueManager.TextSkip();
        }
        else
        {
            // 텍스트 출력이 끝난 경우 대화창 종료
            dialogueManager.CloseDialogue();

            // 다음 대화 출력
            NextTalk();
        }
    }

    private void NextTalk()
    {
        isPrinting = false;
    }

    public void StartTalk(Npc npc)
    {
        talkedNPC = npc;

        // 플레이어 조작 컨트롤러 비활성화
        ControlContext.Instance.DisableController(typeof(PlayerController));

        // 대화 조작 컨트롤러 활성화
        ControlContext.Instance.EnableController(typeof(TalkController));

        // 대화 처음 시작 시 해당되는 대화의 첫 부분 가져오기
        Line intro = GetIntroLine(npc);

        // 현재 완료 및 수주가 가능한 퀘스트 처리
        CheckToQuest(npc);

        // 대사 읽기 시작
        readLineCoroutine = StartCoroutine(ReadLines(npc, intro));
    }

    private void CheckToQuest(Npc npc)
    {
        // 완료할 퀘스트가 있는 경우 완료
        if (!TryCompleteQuest(npc, out _))
        {
            // 수주할 퀘스트가 있는 경우 수주
            TryAcceptQuest(npc, out _);
        }
    }

    private bool TryCompleteQuest(Npc npc, out QuestData completeQuest)
    {
        completeQuest = npc.GetCompletableQuest();
        if (completeQuest != null)
        {
            // 퀘스트 완료
            QuestManager.Instance.CompleteCurrentQuest();

            return true;
        }

        return false;
    }

    private bool TryAcceptQuest(Npc npc, out QuestData acceptQuest)
    {
        acceptQuest = npc.GetAcceptableQuest();
        if (acceptQuest != null)
        {
            // 퀘스트 수주
            QuestManager.Instance.AcceptQuest(acceptQuest);

            return true;
        }

        return false;
    }

    private IEnumerator ReadLines(Npc npc, Line intro)
    {
        // 스킵 버튼 오류 방지용
        yield return null;

        isTalking = true;

        readLine = intro;
        while (readLine != null)
        {
            // 대사 하나하나 출력
            while (readLine != null)
            {
                lineHandler[readLine.code]?.Invoke(readLine);

                // 대사를 출력하는 동안 대기
                yield return new WaitWhile(() => isPrinting);

                // 다음 대사 준비
                readLine = GetNextLine(readLine);
            }

            // 다음 이어질 대본이 있는 지 확인
            readLine = GetNextScriptEntry(npc);
        }

        // 대사를 모두 읽었다면 대사 출력 멈추기
        EndTalk();
    }

    private Line GetNextLine(Line currentLine)
    {
        // 선택지(다음 대사가 여러 개 준비되어 있는 라인)가 아니라면 첫 대사 선택
        if (currentLine is not SelectLine)
        {
            return currentLine.nextLines.FirstOrDefault();
        }

        // 선택지에서 고른 대사에 문제가 있는 경우
        if (selectIndex < 0)
        {
            // 경고문을 띄우고서 첫번째 선택지 선택
            Debug.LogError("선택지 선택에 문제가 있어 임시적으로 첫번째 선택지를 선택합니다.");
            selectIndex = 0;
        }

        // 선택지에서 고른 대사로 이동
        return currentLine.nextLines.ElementAtOrDefault(selectIndex);
    }

    private Line GetNextScriptEntry(Npc npc)
    {
        // 수주 가능한 퀘스트 확인
        if (TryAcceptQuest(npc, out QuestData acceptQuest))
        {
            return GetQuestIntro(acceptQuest, QuestState.ACCEPTABLE);
        }

        // 완료 가능한 퀘스트 확인
        if (TryCompleteQuest(npc, out QuestData completeQuest))
        {
            return GetQuestIntro(completeQuest, QuestState.COMPLETABLE);
        }

        // 이어질 퀘스트 대화가 없으면 null을 반환
        return null;
    }

    private void EndTalk()
    {
        isTalking = false;

        // 활성화된 이미지 모두 파괴
        imageRenderer.AllDestoryImages();

        // 플레이어 조작 컨트롤러 활성화
        ControlContext.Instance.ConnectController(typeof(PlayerController));

        // 대화 조작 컨트롤러 비활성화
        ControlContext.Instance.DisconnectController(typeof(TalkController));
    }

    private Line GetIntroLine(Npc npc)
    {
        // 진행 가능한 퀘스트가 있는 경우
        QuestData quest = npc.GetAcceptableQuest();
        if (quest != null) return GetQuestIntro(quest, QuestState.ACCEPTABLE);

        // 완료 가능한 퀘스트가 있는 경우
        quest = npc.GetCompletableQuest();
        if (quest != null) return GetQuestIntro(quest, QuestState.COMPLETABLE);

        // 진행 중인 퀘스트가 있는 경우
        quest = npc.GetAcceptedQuest();
        if (quest != null) return GetQuestIntro(quest, QuestState.ONGOING);

        return npc.GetIntroLine();
    }

    private Line GetQuestIntro(QuestData quest, QuestState state)
    {
        if (quest == null)
            return null;

        return ScenarioManager.Instance.GetQuestIntro(quest.ID, state);
    }

    /************************************************************
    * [라인 출력 관리]
    * 
    * 라인을 읽고서 거기에 따른 인게임 이벤트 제어
    ************************************************************/

    /// <summary>
    /// TextLine에 따른 대사 출력 함수
    /// </summary>
    private void PrintTextLine(TextLine line)
    {
        isPrinting = true;

        dialogueManager.PrintText(line);
    }

    /// <summary>
    /// SelectLine에 따른 선택지를 띄우는 함수
    /// </summary>
    private void ActiveSelection(SelectLine line)
    {
        isPrinting = true;

        selectManager.OpenSelect(line, idx => selectIndex = idx);
    }

    /// <summary>
    /// ImageLine에 따른 이미지 생성 함수
    /// </summary>
    private void RenderImage(ImageLine line)
    {
        // 이미지 생성
        int x = (int)line.pos.x;
        int y = (int)line.pos.y;

        imageRenderer.DisplayImage(line.guid, line.sprite, x, y);
    }

    /// <summary>
    /// DestroyLine에 따른 이미지 파괴 함수
    /// </summary>
    private void DestroyImage(DestroyLine line)
    {
        imageRenderer.DestroyImage(line.target);
    }

    /// <summary>
    /// TransformLine에 따른 이미지 변형 함수
    /// </summary>
    private void TransformImage(TransformLine line)
    {
        imageRenderer.TransformImage(line.target, line.pos, line.color);
    }

    private void SetAudio(AudioLine line)
    {

    }

    private void ActiveEvent(EventLine line)
    {
        string command = line.Command;
        eventManager.GetCommandEvent(command);
    }
}