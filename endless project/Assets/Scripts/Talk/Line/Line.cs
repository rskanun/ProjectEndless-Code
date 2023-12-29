public enum LineType
{
    Text,   // 대사출력                  -> 대사번호,코드(Text),이름,대사
    Select, // 선택지                    -> 대사번호,코드(Select),선택1,선택2,...,선택n
    Case,   // 선택지 선택에 따른 진행    -> 대사번호,코드(Case),선택지
    End,    // 선택지 종료 선언           -> 대사번호,코드(End)
    Event   // 이벤트(수치 조작 등) 발생  -> 대사번호,코드(Event),명령어
}

public class Line
{
    private LineType code;
    public LineType Code { get { return code; } }

    public Line(LineType code)
    {
        this.code = code;
    }
}