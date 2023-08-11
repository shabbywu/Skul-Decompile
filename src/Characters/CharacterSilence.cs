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
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Expected O, but got Unknown
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Expected O, but got Unknown
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Expected O, but got Unknown
		_skill = new TrueOnlyLogicalSumList(false);
		_dash = new TrueOnlyLogicalSumList(false);
		_basicAttack = new TrueOnlyLogicalSumList(false);
		_jump = new TrueOnlyLogicalSumList(false);
	}

	public void Attach(Action.Type type, object key)
	{
		TrueOnlyLogicalSumList logicalSum = GetLogicalSum(type);
		if (logicalSum != null)
		{
			logicalSum.Attach(key);
		}
	}

	public void Detach(Action.Type type, object key)
	{
		TrueOnlyLogicalSumList logicalSum = GetLogicalSum(type);
		if (logicalSum != null)
		{
			logicalSum.Detach(key);
		}
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
		return (TrueOnlyLogicalSumList)(type switch
		{
			Action.Type.BasicAttack => _basicAttack, 
			Action.Type.Skill => _skill, 
			Action.Type.Jump => _jump, 
			Action.Type.Dash => _dash, 
			_ => null, 
		});
	}
}
