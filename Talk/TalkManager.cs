using System;
using System.Collections;
using System.Collections.Generic;
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
    private ScenarioScene currentScript;

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
            {LineType.Event,        line => ExcuteEvent((EventLine)line) },
        };
    }

    private void OnEnable()
    {
        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
        TalkContext.Instance.RegisterManager(this);
    }

    private void OnDisable()
    {
        LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
        TalkContext.Instance.RemoveManager();
    }

    private async void OnLocaleChanged(Locale newLocale)
    {
        // 언어 변경이 일어났다면 테이블 다시 불러오기
        await ScenarioManager.Instance.ReloadLocalizationTables();

        // 현재 진행 중인 라인 중단
        //StopCoroutine(readLineCoroutine);

        // 마지막 진행 중이던 라인 재시작
        //readLineCoroutine = StartCoroutine(ReadLines(talkedNPC, readLine));
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
        // 플레이어 조작 컨트롤러 비활성화
        ControlContext.Instance.DisableController(typeof(PlayerController));

        // 대화 조작 컨트롤러 활성화
        ControlContext.Instance.EnableController(typeof(TalkController));

        // 대화 처음 시작 시 대본 가져오기
        currentScript = GetScenarioScene(npc);

        // 대사 읽기 시작
        StartCoroutine(ReadLines(npc, currentScript));
    }

    private ScenarioScene GetScenarioScene(Npc npc)
    {
        // 수주 가능한 퀘스트가 있는 경우
        var quest = npc.GetAcceptableQuest();
        if (quest != null) return GetQuestScenarioScene(quest, QuestState.Inactive);

        // 완료 가능한 퀘스트가 있는 경우
        quest = npc.GetCompletableQuest();
        if (quest != null) return GetQuestScenarioScene(quest, QuestState.Completed);

        // 진행 중인 퀘스트가 있는 경우
        quest = npc.GetAcceptedQuest();
        if (quest != null) return GetQuestScenarioScene(quest, QuestState.OnGoing);

        // 모든 조건에 충족하지 않으면 일반 대사
        return npc.GetDialogueScene();
    }

    private ScenarioScene GetQuestScenarioScene(QuestData quest, QuestState state)
    {
        if (quest == null)
            return null;

        return ScenarioManager.Instance.GetQuestScenarioScene(quest.ID, state);
    }

    private IEnumerator ReadLines(Npc npc, ScenarioScene scene)
    {
        // 스킵 버튼 오류 방지용
        yield return null;

        isTalking = true;

        while (scene != null)
        {
            // 퀘스트 상태 갱신
            UpdateQuestState(npc);

            // 대사 하나하나 출력
            foreach (var readLine in scene)
            {
                lineHandler[readLine.code]?.Invoke(readLine);

                // 대사를 출력하는 동안 대기
                yield return new WaitWhile(() => isPrinting);
            }

            // 다음 이어질 대본이 있는 지 확인
            scene = GetNextScene(npc);
        }

        // 대사를 모두 읽었다면 대사 출력 멈추기
        EndTalk();
    }

    private void UpdateQuestState(Npc npc)
    {
        // 완료 가능한 퀘스트가 있는 경우 완료하기
        var quest = npc.GetCompletableQuest();
        if (quest != null)
        {
            QuestManager.Instance.CompleteQuest(quest);
            return;
        }

        // 수주 가능한 퀘스트가 있는 경우 수주하기
        quest = npc.GetAcceptableQuest();
        if (quest != null)
        {
            QuestManager.Instance.AcceptQuest(quest);
            return;
        }
    }

    private ScenarioScene GetNextScene(Npc npc)
    {
        // 수주 가능한 퀘스트 확인
        var quest = npc.GetAcceptableQuest();
        if (quest != null)
        {
            return GetQuestScenarioScene(quest, QuestState.Inactive);
        }

        // 완료 가능한 퀘스트 확인
        quest = npc.GetCompletableQuest();
        if (quest != null)
        {
            return GetQuestScenarioScene(quest, QuestState.Completed);
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
        ControlContext.Instance.EnableController(typeof(PlayerController));

        // 대화 조작 컨트롤러 비활성화
        ControlContext.Instance.DisableController(typeof(TalkController));
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

        selectManager.OpenSelect(line, SelectOption);
    }

    private void SelectOption(int index)
    {
        currentScript.SelectOption(index);

        isPrinting = false;
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

    /// <summary>
    /// EventLine에 따른 이벤트 실행 함수
    /// </summary>
    private void ExcuteEvent(EventLine line)
    {
        line.dialogueEvent?.Execute();
    }
}