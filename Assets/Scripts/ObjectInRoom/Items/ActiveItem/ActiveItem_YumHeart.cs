public class ActiveItem_YumHeart : ActiveItem
{
    protected override void SpecificActiveSkill()
    {
        gamePlayer.GetHealing(HealthData.RedOne);
    }
}
