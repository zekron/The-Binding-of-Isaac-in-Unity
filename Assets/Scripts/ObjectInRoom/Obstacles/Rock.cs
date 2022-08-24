
public class Rock : RoomObject, IDestroy
{
    public void DestroySelf()
    {
        collisionController.SelfCollider.IsTrigger = true;
    }

    public override void ResetObject()
    {
        
    }
}
