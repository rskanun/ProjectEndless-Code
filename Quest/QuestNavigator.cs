using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class QuestNavigator : MonoBehaviour
{
    // 퀘스트 네비게이션 오브젝트
    [SerializeField] private NavigatorObject naviObj;

    private Queue<string> questPath = new Queue<string>();
    private string targetMap;
    private int targetObj;

    private void OnEnable()
    {
        // 씬 로드 이벤트 등록
        SceneManager.sceneLoaded += OnSceneLoaded;

        // 퀘스트 이벤트 등록
        QuestManager.Instance.AddListener(OnStart);
    }

    private void OnDisable()
    {
        // 씬 로드 이벤트 해제
        SceneManager.sceneLoaded -= OnSceneLoaded;

        // 퀘스트 이벤트 해제
        QuestManager.Instance.RemoveListener(OnStart);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬 이동이 일어난 경우 목표가 있는지 우선 확인
        if (targetMap == null || targetObj == 0)
        {
            // 목표가 없을 경우 현재 퀘스트가 있는 지 확인
            if (GameData.Instance.CurrentQuest == null)
            {
                // 현재 퀘스트가 없으면 네비게이션 종료
                naviObj.OnComplete();
                return;
            }

            // 현재 퀘스트가 있으면 네비 시작
            OnStart();
            return;
        }

        // 목표가 있다면 해당 씬에 맵이 있는 지 확인
        MapData moveMap = MapManager.FindMapDataCurrentScene();
        if (moveMap != null)
        {
            // 맵이 맞다면 맵 이동 함수를 실행
            OnMoveMap(moveMap);
            return;
        }

        // 맵이 아닌 경우 네비게이션 종료
        OnStop();
    }

    private void OnMoveMap(MapData moveMap)
    {
        // 플레이어가 맵을 이동한 경우 경로를 이탈했는지 확인
        if (questPath.Count < 1 || questPath.Peek() != moveMap.ID)
        {
            // 경로를 이탈했다면 새로운 경로 탐색
            OnResearch();
            return;
        }

        // 경로를 이탈하지 않았다면 계속해서 다음 목표 가르키기
        UpdateNavigation(questPath.Dequeue());
    }

    public void OnStart()
    {
        QuestData curQuest = GameData.Instance.CurrentQuest;

        // 현재 받은 퀘스트가 없을 경우
        if (curQuest == null)
        {
            // 네비게이션 종료
            OnStop();
            return;
        }

        // 목표 맵과 NPC 설정
        targetMap = curQuest.MapID;
        targetObj = curQuest.ObjectID;

        // 경로 업데이트
        if (UpdatePath(targetMap) == false)
        {
            // 업데이트에 실패했을 경우 네비게이션 종료
            OnStop();
            return;
        }

        // 네비게이션 설정 및 작동
        UpdateNavigation(questPath.Dequeue());
    }

    private void OnResearch()
    {
        // 지정된 경로에서 벗어난 경우
        // 경로 업데이트
        if (UpdatePath(targetMap) == false)
        {
            // 업데이트에 실패했을 경우 네비게이션 종료
            OnStop();
            return;
        }

        // 네비게이션 설정 및 작동
        UpdateNavigation(questPath.Dequeue());
    }

    private void OnStop()
    {
        // 목표 도달 시 목표 맵과 NPC 초기화
        targetMap = null;
        targetObj = 0;

        // 경로 초기화
        questPath.Clear();

        // 네비게이션 종료
        naviObj.OnComplete();
    }

    private bool UpdatePath(string mapID)
    {
        // 경로 탐색
        MapData currentMapData = GameData.Instance.CurrentMap;
        List<string> path = SearchShortestPath(currentMapData.ID, mapID);

        // 경로 탐색에 성공했을 경우
        if (path != null)
        {
            // 경로 설정 후 성공 리턴
            questPath = new Queue<string>(path);
            return true;
        }

        return false;
    }

    private void UpdateNavigation(string currentMap)
    {
        // 목표 오브젝트 설정
        GameObject targetObj = GetNextTarget(currentMap);

        // 네비게이션 작동
        naviObj.SetTarget(targetObj);
    }

    /************************************************************
    * [경로 탐색]
    * 
    * BFS를 사용한 최단 경로 탐색 및 다음 이동 목표 설정
    ************************************************************/

    private List<string> SearchShortestPath(string startMapID, string targetMapID)
    {
        Queue<MapNode> mapQueue = new Queue<MapNode>();
        HashSet<string> visitMaps = new HashSet<string>();

        // 노드 초기화
        MapNode firstNode = new MapNode(startMapID, new List<string> { startMapID });
        mapQueue.Enqueue(firstNode);

        // BFS 최단경로 탐색
        while (mapQueue.Count > 0)
        {
            MapNode node = mapQueue.Dequeue();
            string mapID = node.MapID;

            // 목표 도달 시
            if (mapID == targetMapID)
            {
                // 현재 경로 리턴
                return node.Path;
            }

            // 방문 안 한 맵만 진행
            if (visitMaps.Contains(mapID) == false)
            {
                visitMaps.Add(mapID);

                // 탐색 중인 맵과 연결된 맵 가져오기
                List<string> connectedMaps = MapDatabase.Instance.GetConnectedMap(mapID);

                foreach (string nextMapID in connectedMaps)
                {
                    if (visitMaps.Contains(nextMapID) == false)
                    {
                        List<string> newPath = new List<string>(node.Path) { nextMapID };
                        mapQueue.Enqueue(new MapNode(nextMapID, newPath));
                    }
                }
            }
        }

        return null;
    }

    private GameObject GetNextTarget(string currentMap)
    {
        if (currentMap == targetMap)
        {
            // 현재 맵이 목표한 맵일 경우 오브젝트의 위치 서칭
            return SearchInteractiveObj(targetObj);
        }

        // 다음 맵 이동을 위한 포탈 위치 리턴
        return SearchPortal(questPath.Peek());
    }

    private GameObject SearchInteractiveObj(int id)
    {
        // 임시로 NPC 태그 설정
        // 추후 상호작용이 가능한 오브젝트 태그로 변경할 예정
        return SearchObject<Npc>("NPC", npc => npc.GetID() == id);
    }

    private GameObject SearchPortal(string linkedMap)
    {
        return SearchObject<Portal>("Portal", portal
            => portal.LinkedScene == MapDatabase.Instance.GetLinkedSceneName(linkedMap));
    }

    private GameObject SearchObject<T>(string tag, Func<T, bool> condition) where T : Component
    {
        // 현재 맵(씬)에 있는 오브젝트 탐색
        foreach (GameObject obj in GameObject.FindGameObjectsWithTag(tag))
        {
            T objComponent = obj.GetComponent<T>();
            if (objComponent != null && condition(objComponent))
            {
                // 해당 오브젝트가 조건에 만족하는 경우 리턴
                return obj;
            }
        }

        return null;
    }

    private class MapNode
    {
        public string MapID { get; }
        public List<string> Path { get; }

        public MapNode(string mapID, List<string> path)
        {
            MapID = mapID;
            Path = path;
        }
    }
}