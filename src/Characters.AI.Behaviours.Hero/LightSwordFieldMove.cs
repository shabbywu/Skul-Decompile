using System.Collections;
using Characters.AI.Hero.LightSwords;
using UnityEngine;

namespace Characters.AI.Behaviours.Hero;

public sealed class LightSwordFieldMove : LightMove
{
	private enum Where
	{
		PlayerBehind,
		PlayerClosest
	}

	[SerializeField]
	private LightSwordFieldHelper _helper;

	[SerializeField]
	private Where _where;

	public override IEnumerator CRun(AIController controller)
	{
		base.result = Result.Doing;
		if (_helper.swords == null)
		{
			yield return _helper.CFire();
		}
		yield return base.CRun(controller);
	}

	protected override LightSword GetDestination()
	{
		return _where switch
		{
			Where.PlayerBehind => _helper.GetBehindPlayer(), 
			Where.PlayerClosest => _helper.GetClosestFromPlayer(), 
			_ => _helper.GetClosestFromPlayer(), 
		};
	}
}
