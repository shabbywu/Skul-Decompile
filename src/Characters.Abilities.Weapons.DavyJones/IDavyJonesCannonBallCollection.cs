namespace Characters.Abilities.Weapons.DavyJones;

public interface IDavyJonesCannonBallCollection
{
	void Push(CannonBallType cannon, int count);

	void Pop();
}
