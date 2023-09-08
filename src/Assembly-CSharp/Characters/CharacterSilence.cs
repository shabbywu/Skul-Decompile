using Characters.Actions;

namespace Characters;

public sealed class CharacterSilence
{
	private TrueOnlyLogicalSumList _skill;

	private TrueOnlyLogicalSumList _dash;

	private TrueOnlyLogicalSumList _basicAttack;

	private TrueOnlyLogicalSumList _jump;

	public CharacterSilence()
	{
		_skill = new TrueOnlyLogicalSumList();
		_dash = new TrueOnlyLogicalSumList();
		_basicAttack = new TrueOnlyLogicalSumList();
		_jump = new TrueOnlyLogicalSumList();
	}

	public void Attach(Action.Type type, object key)
	{
		GetLogicalSum(type)?.Attach(key);
	}

	public void Detach(Action.Type type, object key)
	{
		GetLogicalSum(type)?.Detach(key);
	}

	public bool CanUse(Action.Type type)
	{
		TrueOnlyLogicalSumList logicalSum = GetLogicalSum(type);
		if (logicalSum == null)
		{
			return true;
		}
		if (type == Action.Type.Dash)
		{
			Detach(Action.Type.Skill, this);
		}
		return !logicalSum.value;
	}

	private TrueOnlyLogicalSumList GetLogicalSum(Action.Type type)
	{
		return type switch
		{
			Action.Type.BasicAttack => _basicAttack, 
			Action.Type.Skill => _skill, 
			Action.Type.Jump => _jump, 
			Action.Type.Dash => _dash, 
			_ => null, 
		};
	}
}
