using Characters.Monsters;
using UnityEngine;

namespace Characters.AI.Conditions;

public class MonsterCount : Condition
{
	private enum Comparer
	{
		GreaterThan,
		LessThan
	}

	[SerializeField]
	private MonsterContainer _minionContainer;

	[SerializeField]
	private Comparer _compare;

	[SerializeField]
	[Range(0f, 100f)]
	private int _count;

	protected override bool Check(AIController controller)
	{
		int num = _minionContainer.Count();
		return _compare switch
		{
			Comparer.GreaterThan => num >= _count, 
			Comparer.LessThan => num <= _count, 
			_ => false, 
		};
	}
}
