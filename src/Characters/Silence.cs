using Characters.Actions;

namespace Characters;

public sealed class Silence
{
	private TrueOnlyLogicalSumList _trueOnlyLogicalSumList = new TrueOnlyLogicalSumList();

	private Character _owner;

	public bool value => _trueOnlyLogicalSumList.value;

	public Silence(Character owner)
	{
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
