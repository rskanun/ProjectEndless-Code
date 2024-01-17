using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class NoteManager : MonoBehaviour
{
    // 파일 함수
    private Dictionary<int, SaveData> saveFiles;
    private int latestFileNum;

    [Header("참조 스크립트")]
    [SerializeField] private SaveManager saveManager;
    [SerializeField] private LoadManager loadManager;
    [SerializeField] private MenuController menuController;
    [SerializeField] private NoteUI ui;

    public void InitSaveFile()
    {
        saveFiles = GetSaveFile();

        // 오브젝트 배치
        ui.InitSaveFileObj(saveFiles);
    }

    private Dictionary<int, SaveData> GetSaveFile()
    {
        Dictionary<int, SaveData> saveFiles = new Dictionary<int, SaveData>();

        DirectoryInfo di = new DirectoryInfo(SaveFileInfo.Instance.FilePath);
        foreach (FileInfo file in di.GetFiles())
        {
            string fileName = file.Name;
            int fileNum = SaveFileInfo.Instance.GetFileNum(fileName);

            // 올바른 파일 번호를 얻었을 경우에만 데이터를 가져옴
            if (fileNum >= 0)
            {
                try
                {
                    // 최신 파일 넘버 수정
                    if (latestFileNum < fileNum)
                    {
                        latestFileNum = fileNum;
                    }

                    saveFiles[fileNum] = loadManager.ReadSaveFile(file.FullName);
                }
                catch (Exception e)
                {
                    Debug.LogWarning($"Error reading file '{fileName}': {e.Message}");
                }
            }
        }

        return saveFiles;
    }

    /************************************************************
    * [게임 데이터 저장]
    * 
    * 현재 진행 상황을 저장
    ************************************************************/

    public void SaveHandler(int id)
    {
        if (id == 0)
        {
            // 파일 번호는 1번부터 시작
            // 0번 -> 최신 파일 번호의 다음 번호
            id = ++latestFileNum;
        }

        // 해당 번호를 가진 파일 검색
        if (saveFiles.ContainsKey(id))
        {
            // 해당 번호를 가진 파일이 있으면 덮어씌우기 여부 확인
            Confirm.CreateMsg("이미 저장된 내용이 있는 파일입니다. 그래도 덮어 씌우시겠습니까?", "계속", "취소")
            .SetYesHandler(() =>
            {
                SaveGame(id);
            }).Show();
        }
        else
        {
            // 해당 번호를 가진 파일이 없으면 계속 진행
            SaveGame(id);
        }
    }

    private void SaveGame(int id)
    {
        SaveFileInfo fileInfo = SaveFileInfo.Instance;

        // 파일 경로
        string name = fileInfo.GetFileName(id);
        string filePath = Path.Combine(fileInfo.FilePath, name);

        // 현재상황 세이브
        SaveData data = saveManager.SaveGameData(filePath);

        // 세이브 파일 업데이트
        UpdateSaveFile(id, data);

        Alert.CreateMsg("데이터 기록이 완료되었습니다!").Show();
    }

    private void UpdateSaveFile(int id, SaveData data)
    {
        if (saveFiles.ContainsKey(id))
        {
            // 이미 존재하는 파일에 저장하는 경우
            RewriteSaveFile(id, data);
        }
        else
        {
            // 새 파일에 저장하는 경우
            AddNewSaveFile(id, data);
        }

        saveFiles[id] = data;
    }

    private void AddNewSaveFile(int id, SaveData data)
    {
        // 새 오브젝트 생성
        ui.AddSaveFile(id, data);

        // 세이브 파일 추가 버튼 업데이트
        ui.UpdateAddSaveButton();
    }

    private void RewriteSaveFile(int id, SaveData data)
    {
        // 오브젝트 정보 다시 작성
        ui.ReloadSaveFileObjInfo(id, data);
    }

    /************************************************************
    * [게임 데이터 불러오기]
    * 
    * 현재 진행 상황에 세이브 데이터를 불러오기
    ************************************************************/

    public void LoadHandler(int id)
    {
        SaveData data = saveFiles[id];

        if (data != null)
        {
            menuController.CloseAllApps();
            menuController.CloseMenu();

            loadManager.LoadGameData(data);
        }
    }    

    private void LoadGame(SaveData data)
    {
        if (data.storyData.date < )
    }
}