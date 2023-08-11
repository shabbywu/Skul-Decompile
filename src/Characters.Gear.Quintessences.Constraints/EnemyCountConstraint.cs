using Level;
using UnityEngine;

namespace Characters.Gear.Quintessences.Constraints;

public sealed class EnemyCountConstraint : Constraint
{
	private enum Compare
	{
		Greater,
		Lower,
		Equal
	}

	[SerializeField]
	private Compare _compare;

	[SerializeField]
	private int _count;

	public override bool Pass()
	{
		int num = 0;
		foreach (Character item in Map.Instance.waveContainer)
		{
			if (((Component)item).gameObject.activeInHierarchy)
			{
				num++;
			}
		}
		return _compare switch
		{
			Compare.Equal => num == _count, 
			Compare.Greater => num > _count, 
			Compare.Lower => num < _count, 
			_ => true, 
		};
	}
}
