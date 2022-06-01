
public class Rock : RoomObject, IDestroy
{
    public void DestroySelf()
    {
        platform.SelfCollider.IsTrigger = true;
    }

    public override void ResetObject()
    {
        
    }
}
