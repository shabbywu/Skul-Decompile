using System.Collections;
using Characters.Actions;
using UnityEngine;

namespace Characters.AI.Behaviours;

public sealed class ContinuousTackle : Behaviour
{
	[SerializeField]
	private Action _actionOnce;

	[Range(1f, 10f)]
	[SerializeField]
	private int _count;

	[Range(0f, 20f)]
	[SerializeField]
	private float _coolTime;

	public bool canUse { get; private set; } = true;


	public override IEnumerator CRun(AIController controller)
	{
		base.result = Result.Doing;
		Character character = controller.character;
		for (int i = 0; i < _count; i++)
		{
			if (_actionOnce.TryStart())
			{
				while (_actionOnce.running && base.result == Result.Doing)
				{
					yield return null;
				}
				character.ForceToLookAt((character.lookingDirection == Character.LookingDirection.Right) ? Character.LookingDirection.Left : Character.LookingDirection.Right);
			}
		}
		((MonoBehaviour)this).StartCoroutine(CCooldown(controller.character.chronometer.master));
		base.result = Result.Done;
	}

	private IEnumerator CCooldown(Chronometer chronometer)
	{
		canUse = false;
		yield return ChronometerExtension.WaitForSeconds((ChronometerBase)(object)chronometer, _coolTime);
		canUse = true;
	}
}
