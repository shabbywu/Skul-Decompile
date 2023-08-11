using Characters.Actions;

namespace Characters;

public sealed class Silence
{
	private TrueOnlyLogicalSumList _trueOnlyLogicalSumList = new TrueOnlyLogicalSumList(false);

	private Character _owner;

	public bool value => _trueOnlyLogicalSumList.value;

	public Silence(Character owner)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		_owner = owner;
	}

	public void Attach(object key)
	{
		foreach (Action action in _owner.actions)
		{
			if (action.type == Action.Type.Skill && action.running)
			{
				action.ForceEnd();
			}
		}
		_trueOnlyLogicalSumList.Attach(key);
	}

	public bool Detach(object key)
	{
		return _trueOnlyLogicalSumList.Detach(key);
	}
}
