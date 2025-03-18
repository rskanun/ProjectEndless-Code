using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NoteUI : MonoBehaviour
{
    [Header("프리팹")]
    [SerializeField] private GameObject saveFilePrifab;
    [SerializeField] private Transform prifabParentTransform;
    [Header("오브젝트")]
    [SerializeField] private GameObject viewer;
    [SerializeField] private GameObject notice;
    [SerializeField] private GameObject saveAddObj;

    private Dictionary<int, GameObject> saveFileObjs = new Dictionary<int, GameObject>();
    private const int MAX_FILE = 10;

    private void OnDisable()
    {
        DestroySaveFiles();

        ActiveAddSaveButton(false);
        ActiveNotice(false);
    }

    private void DestroySaveFiles()
    {
        foreach (GameObject obj in saveFileObjs.Values)
        {
            Destroy(obj);
        }

        saveFileObjs.Clear();
    }

    /************************************************************
    * [세이브 파일 오브젝트]
    * 
    * 세이브 파일 오브젝트 관리
    ************************************************************/

    public void InitSaveFileObj(Dictionary<int, SaveData> saveFiles)
    {
        // id 값에 따른 오름차순 정렬
        List<int> keys = new List<int>(saveFiles.Keys);
        keys.Sort();

        foreach (int id in keys)
        {
            SaveData data = saveFiles[id];

            AddSaveFile(id, data);
        }
    }

    public void AddSaveFile(int id, SaveData data)
    {
        GameObject saveFileObj = Instantiate(saveFilePrifab, prifabParentTransform);
        SaveDataManager manager = saveFileObj.GetComponent<SaveDataManager>();

        manager.SetData(data);
        manager.SetCallBack(() => NoteContext.Instance.OnClickNote(id));

        saveFileObjs[id] = saveFileObj;

        // 뷰어 높이 설정
        UpdateViewerSize();
    }

    private void UpdateViewerSize()
    {
        VerticalLayoutGroup component = viewer.GetComponent<VerticalLayoutGroup>();
        float top = component.padding.top;
        float bottom = component.padding.bottom;
        float spacing = component.spacing;

        // 오브젝트 높이
        float objHeight = saveFilePrifab.GetComponent<RectTransform>().rect.height;

        // 총 길이
        // (세이브 추가 오브젝트 높이 = 세이브 파일 오브젝트 높이 가정)
        int count = (saveAddObj.activeSelf) ? saveFileObjs.Count + 1 : saveFileObjs.Count;
        float height = top + bottom + spacing * count + objHeight * count;

        RectTransform rect = viewer.GetComponent<RectTransform>();
        rect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
    }

    public void ReloadSaveFileObjInfo(int id, SaveData data)
    {
        GameObject reloadObj = saveFileObjs[id];
        SaveDataManager manager = reloadObj.GetComponent<SaveDataManager>();

        manager.SetData(data);
    }

    /************************************************************
    * [부가 오브젝트]
    * 
    * 세이브 및 로드 환경에서만 나타나는 부가적인 오브젝트 관리
    ************************************************************/

    private void ActiveAddSaveButton(bool isActive)
    {
        saveAddObj.SetActive(isActive);
    }

    public void UpdateAddSaveButton()
    {
        // 최대 개수 달성 시 세이브 파일 추가 버튼 삭제
        if (saveFileObjs.Count >= MAX_FILE)
        {
            ActiveAddSaveButton(false);
        }

        // 최대 개수 이하로 내려오면 다시 세이브 파일 추가 버튼 생성
        else if (saveAddObj.activeSelf == false)
        {
            ActiveAddSaveButton(true);
        }
    }

    private void ActiveNotice(bool isActive)
    {
        notice.SetActive(isActive);
    }

    public void InitNotice()
    {
        if (saveFileObjs.Count <= 0)
        {
            // 세이브 파일이 없을 경우 알림 문구 띄움
            ActiveNotice(true);
        }
        else ActiveNotice(false);
    }
}