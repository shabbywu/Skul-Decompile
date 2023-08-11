using UnityEngine;

namespace Characters.AI.Conditions;

public class HealthCondition : Condition
{
	private enum Comparer
	{
		GreaterThan,
		LessThan
	}

	[SerializeField]
	private Comparer _compare;

	[SerializeField]
	[Range(0f, 1f)]
	private float _percent;

	protected override bool Check(AIController controller)
	{
		return _compare switch
		{
			Comparer.GreaterThan => controller.character.health.percent >= (double)_percent, 
			Comparer.LessThan => controller.character.health.percent <= (double)_percent, 
			_ => false, 
		};
	}
}
