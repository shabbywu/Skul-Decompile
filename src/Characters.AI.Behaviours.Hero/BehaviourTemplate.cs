using System.Collections;
using Characters.Actions;
using UnityEngine;

namespace Characters.AI.Behaviours.Hero;

public class BehaviourTemplate : Behaviour
{
	[SerializeField]
	private float _coolTime;

	[SerializeField]
	private Action[] _actions;

	public bool canUse { get; private set; } = true;


	public override IEnumerator CRun(AIController controller)
	{
		if (_actions.Length < 0)
		{
			Debug.LogError((object)"Action length is 0");
		}
		else
		{
			if (!canUse)
			{
				yield break;
			}
			if (_coolTime > 0f)
			{
				((MonoBehaviour)this).StartCoroutine(CCoolDown(controller.character.chronometer.master));
			}
			Action[] actions = _actions;
			foreach (Action action in actions)
			{
				action.TryStart();
				while (action.running)
				{
					yield return null;
				}
			}
		}
	}

	private IEnumerator CCoolDown(Chronometer chronometer)
	{
		canUse = false;
		yield return ChronometerExtension.WaitForSeconds((ChronometerBase)(object)chronometer, _coolTime);
		canUse = true;
	}
}
