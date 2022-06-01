public interface IHealth : IDestroy
{
	public int Health { get; }
	public void GetHealing(int healing);
	public void GetDamage(int damage);
}