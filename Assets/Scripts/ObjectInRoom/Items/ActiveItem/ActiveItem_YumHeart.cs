public class ActiveItem_YumHeart : ActiveItem
{
    protected override void SpecificSkill()
    {
        gamePlayer.GetHealing(HealthData.RedOne);
    }
}
