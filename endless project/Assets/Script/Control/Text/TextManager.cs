using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    // 텍스트 데이터 저장
    private Dictionary<int, string[]> textData;
    private TextUI ui;

    private void Start()
    {
        // 텍스트 데이터 삽입
        textData = new Dictionary<int, string[]>();
        init();
    }

    public void setDialogView(bool isView)
    {
        ui.setDialogView(isView);
    }

    public void setText(string text)
    {
        ui.setText(text);
    }

    private void init()
    {
        // UI init
        ui = GetComponent<TextUI>();

        // text input
        initText();
    }

    private void initText()
    {
        List<string> lines = CSVReader.getLines();

        // 텍스트를 정리할 dummy list
        List<string> strs = new List<string>();
        // 텍스트 코드를 기억할 dummy int
        int beforeNum = 0;

        // 파일 끝까지 한 줄씩 읽기
        foreach (string str in lines)
        {
            // 해당 줄의 스크립트와 코드 분리
            string num = str.Split(',')[0];
            string line = str.Split(',')[1];

            // 새로운 넘버가 출현했을 경우
            if (!string.IsNullOrEmpty(num))
            {
                // 이전 넘버가 있다면
                if (beforeNum != 0)
                {
                    // 이전 넘버 데이터에 텍스트가 정리된 list 추가
                    textData.Add(beforeNum, strs.ToArray());
                    strs.Clear();
                }

                beforeNum = int.Parse(num);
            }

            // 줄바꿈 인식
            line = line.Replace("\\r\\n", "\r\n");
            // 앞에 넘버가 붙는 것에 관계없이 대사를 배열에 추가
            strs.Add(line);
        }

        // 아직 남은 대사가 배열안에 있을 경우 전부 대사 데이터에 저장
        if(!(strs is null))
        {
            textData.Add(beforeNum, strs.ToArray());
            strs.Clear();
        }
    }

    public string[] getText(int id)
    {
        // 해당되는 대화 목록 가져오기
        return textData[id];
    }
}
