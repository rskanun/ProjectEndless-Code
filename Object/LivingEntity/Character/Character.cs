public class Character : Entity
{
    public void OnJoinBattle()
    {
        // 본인의 데이터를 파티데이터에서 가져옴
        CharacterData data = PartyData.Instance.GetCharacter(Name);

        // 데이터 덮어씌우기
        SkillList = data.Skills;
        Stat = data.Stat;

        // 오브젝트 활성화
        gameObject.SetActive(true);
    }
}