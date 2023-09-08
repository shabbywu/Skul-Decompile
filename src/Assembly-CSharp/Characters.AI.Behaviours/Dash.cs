using System.Collections;
using System.Collections.Generic;
using Characters.Actions;
using Hardmode;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours;

public class Dash : Behaviour
{
	[SerializeField]
	[MinMaxSlider(0f, 20f)]
	private Vector2 _minMaxDistance;

	[UnityEditor.Subcomponent(typeof(MoveToDestination))]
	[SerializeField]
	private MoveToDestination _moveToDestination;

	[SerializeField]
	private Action _motion;

	private Stat.Values _hardmodeStat;

	protected void Start()
	{
		_childs = new List<Behaviour> { _moveToDestination };
		_hardmodeStat = new Stat.Values(new Stat.Value(Stat.Category.Constant, Stat.Kind.MovementSpeed, 50.0));
	}

	public override IEnumerator CRun(AIController controller)
	{
		Character target = controller.target;
		Character character = controller.character;
		float num = Mathf.Abs(((Component)character).transform.position.x - ((Component)target).transform.position.x);
		float x = _minMaxDistance.x;
		float y = _minMaxDistance.y;
		_motion.TryStart();
		base.result = Result.Doing;
		if (Singleton<HardmodeManager>.Instance.hardmode)
		{
			character.stat.AttachValues(_hardmodeStat);
		}
		if (num >= x && num < y)
		{
			controller.destination = Vector2.op_Implicit(((Component)target).transform.position);
			yield return _moveToDestination.CRun(controller);
		}
		else if (num >= y)
		{
			if (((Component)character).transform.position.x < ((Component)target).transform.position.x)
			{
				controller.destination = new Vector2(((Component)character).transform.position.x + y, ((Component)character).transform.position.y);
			}
			else
			{
				controller.destination = new Vector2(((Component)character).transform.position.x - y, ((Component)character).transform.position.y);
			}
			yield return _moveToDestination.CRun(controller);
		}
		if (Singleton<HardmodeManager>.Instance.hardmode)
		{
			character.stat.DetachValues(_hardmodeStat);
		}
		character.CancelAction();
		base.result = Result.Done;
	}
}
