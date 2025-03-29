public abstract class Module
{
    //모듈 사용
    public abstract void EquipModule(Skill skill);

    //모듈 사용해제
    public abstract void UnEquipModule(Skill skill);

    //복제
    public abstract Module Clone();
}
