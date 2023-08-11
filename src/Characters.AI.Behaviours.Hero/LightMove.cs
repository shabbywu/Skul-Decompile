using System.Collections;
using Characters.AI.Hero.LightSwords;
using Characters.Actions;
using UnityEngine;

namespace Characters.AI.Behaviours.Hero;

public abstract class LightMove : Behaviour
{
	[SerializeField]
	private Transform _destination;

	[SerializeField]
	private Action _move;

	public override IEnumerator CRun(AIController controller)
	{
		base.result = Result.Doing;
		LightSword sword = GetDestination();
		if ((Object)(object)sword == (Object)null)
		{
			Debug.LogError((object)"Sword is Null in LightMove");
			yield break;
		}
		_destination.position = sword.GetStuckPosition();
		sword.Sign();
		_move.TryStart();
		while (_move.running)
		{
			yield return null;
		}
		sword.Despawn();
		base.result = Result.Success;
	}

	protected abstract LightSword GetDestination();
}
