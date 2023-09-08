using Characters.AI.Hero.LightSwords;
using UnityEngine;

namespace Characters.AI.Conditions.Customs;

public sealed class LightSwordCount : Condition
{
	private enum Comparer
	{
		GreaterThan,
		LessThan
	}

	[SerializeField]
	private LightSwordFieldHelper _helper;

	[SerializeField]
	private int _count;

	[SerializeField]
	private Comparer _comparer;

	protected override bool Check(AIController controller)
	{
		int activatedSwordCount = _helper.GetActivatedSwordCount();
		return _comparer switch
		{
			Comparer.GreaterThan => activatedSwordCount >= _count, 
			Comparer.LessThan => activatedSwordCount <= _count, 
			_ => false, 
		};
	}
}
