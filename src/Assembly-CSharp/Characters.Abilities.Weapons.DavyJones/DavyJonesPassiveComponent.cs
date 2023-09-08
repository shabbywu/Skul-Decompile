namespace Characters.Abilities.Weapons.DavyJones;

public sealed class DavyJonesPassiveComponent : AbilityComponent<DavyJonesPassive>
{
	public float stack
	{
		get
		{
			return base.baseAbility.MakeSaveData();
		}
		set
		{
			base.baseAbility.Load(value);
		}
	}

	public bool IsTop(CannonBallType type)
	{
		CannonBallType? cannonBallType = base.baseAbility.Top();
		if (!cannonBallType.HasValue)
		{
			return false;
		}
		return type == cannonBallType.Value;
	}

	public bool IsEmpty()
	{
		return base.baseAbility.isEmpty;
	}

	public void Push(CannonBallType type)
	{
		base.baseAbility.Push(type, 1);
	}

	public void Push(CannonBallType type, int count)
	{
		base.baseAbility.Push(type, count);
	}

	public void Pop()
	{
		base.baseAbility.Pop();
	}
}
