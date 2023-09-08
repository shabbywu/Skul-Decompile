using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours;

public class KeepDistance : Behaviour
{
	private enum Type
	{
		Move,
		MoveToDistanceWithTarget,
		BackStepFromTarget,
		BackStepToWide
	}

	[SerializeField]
	private Type _type;

	[SerializeField]
	private float _moveCooldownTime;

	[SerializeField]
	[MinMaxSlider(0f, 10f)]
	private Vector2 _distance;

	[SerializeField]
	[UnityEditor.Subcomponent(typeof(MoveToDestination))]
	private MoveToDestination _moveToDestination;

	[UnityEditor.Subcomponent(typeof(BackStep))]
	[SerializeField]
	private BackStep _backStep;

	private bool _moveCanUse = true;

	private void Start()
	{
		_childs = new List<Behaviour> { _moveToDestination, _backStep };
	}

	public override IEnumerator CRun(AIController controller)
	{
		base.result = Result.Doing;
		switch (_type)
		{
		case Type.Move:
			yield return MoveToDestination(controller);
			break;
		case Type.BackStepFromTarget:
			yield return BackStepFromTarget(controller);
			break;
		case Type.BackStepToWide:
			yield return BackStepToWide(controller);
			break;
		}
		base.result = Result.Done;
	}

	private IEnumerator MoveToDestination(AIController controller)
	{
		Character character = controller.character;
		Character target = controller.target;
		if (!((Object)(object)character.movement.controller.collisionState.lastStandingCollider == (Object)null))
		{
			Bounds bounds = character.movement.controller.collisionState.lastStandingCollider.bounds;
			Vector2 val = ((((Component)target).transform.position.x - ((Component)character).transform.position.x > 0f) ? Vector2.left : Vector2.right);
			float num = Random.Range(_distance.x, _distance.y);
			float num2 = ((val == Vector2.left) ? Mathf.Max(((Bounds)(ref bounds)).min.x, ((Component)character).transform.position.x - num) : Mathf.Min(((Bounds)(ref bounds)).max.x, ((Component)character).transform.position.x + num));
			Vector3 position = ((Component)character).transform.position;
			if (val == Vector2.right && ((Bounds)(ref bounds)).max.x < position.x + num)
			{
				num2 = Mathf.Max(((Bounds)(ref bounds)).min.x, position.x - num);
			}
			else if (val == Vector2.left && ((Bounds)(ref bounds)).min.x > position.x - num)
			{
				num2 = Mathf.Min(((Bounds)(ref bounds)).max.x, position.x + num);
			}
			controller.destination = new Vector2(num2, 0f);
			_moveCanUse = false;
			((MonoBehaviour)this).StartCoroutine(CCheckMoveCoolDown(controller.character.chronometer.master));
			yield return _moveToDestination.CRun(controller);
		}
	}

	private IEnumerator BackStepFromTarget(AIController controller)
	{
		Character character = controller.character;
		Vector2 val = ((((Component)controller.target).transform.position.x - ((Component)character).transform.position.x > 0f) ? Vector2.right : Vector2.left);
		Bounds bounds = character.movement.controller.collisionState.lastStandingCollider.bounds;
		float num = Random.Range(_distance.x, _distance.y);
		if (val == Vector2.right && ((Bounds)(ref bounds)).min.x > ((Component)character).transform.position.x - num)
		{
			character.ForceToLookAt(Character.LookingDirection.Left);
		}
		else if (val == Vector2.left && ((Bounds)(ref bounds)).max.x < ((Component)character).transform.position.x + num)
		{
			character.ForceToLookAt(Character.LookingDirection.Right);
		}
		yield return _backStep.CRun(controller);
	}

	private IEnumerator BackStepToWide(AIController controller)
	{
		Character character = controller.character;
		Character target = controller.target;
		Bounds targetPlatformBounds = target.movement.controller.collisionState.lastStandingCollider.bounds;
		Vector2 move = ((((Bounds)(ref targetPlatformBounds)).center.x > ((Component)character).transform.position.x) ? Vector2.right : Vector2.left);
		character.movement.move = move;
		yield return _backStep.CRun(controller);
		if (((Bounds)(ref targetPlatformBounds)).center.x > ((Component)character).transform.position.x)
		{
			character.lookingDirection = Character.LookingDirection.Left;
		}
		else
		{
			character.lookingDirection = Character.LookingDirection.Right;
		}
	}

	private IEnumerator CCheckMoveCoolDown(Chronometer chronometer)
	{
		yield return chronometer.WaitForSeconds(_moveCooldownTime);
		_moveCanUse = true;
	}

	public bool CanUseBackStep()
	{
		return _backStep.CanUse();
	}

	public bool CanUseBackMove()
	{
		return _moveCanUse;
	}
}
