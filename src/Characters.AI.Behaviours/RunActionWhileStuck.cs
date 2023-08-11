using System.Collections;
using Characters.Actions;
using UnityEngine;

namespace Characters.AI.Behaviours;

public sealed class RunActionWhileStuck : Behaviour
{
	[SerializeField]
	private Action _action;

	[SerializeField]
	private float _maxTime = 10f;

	private bool _stayCollision;

	public override IEnumerator CRun(AIController controller)
	{
		base.result = Result.Doing;
		_stayCollision = false;
		_ = controller.character;
		yield return CUpdate(controller);
		base.result = Result.Success;
	}

	private void OnEnterCollision(RaycastHit2D obj)
	{
		_stayCollision = true;
	}

	private IEnumerator CUpdate(AIController controller)
	{
		Character character = controller.character;
		float elapsed = 0f;
		if (!_action.TryStart())
		{
			base.result = Result.Fail;
			yield break;
		}
		while (_action.running && base.result == Result.Doing && elapsed <= _maxTime && !CheckCollision(character))
		{
			Move(controller.character);
			yield return null;
			elapsed += ((ChronometerBase)controller.character.chronometer.master).deltaTime;
		}
	}

	private bool CheckCollision(Character character)
	{
		if (character.lookingDirection == Character.LookingDirection.Right && character.movement.controller.collisionState.right)
		{
			return true;
		}
		if (character.lookingDirection == Character.LookingDirection.Left && character.movement.controller.collisionState.left)
		{
			return true;
		}
		return false;
	}

	private void Move(Character character)
	{
		int num = ((character.lookingDirection != 0) ? 180 : 0);
		character.movement.Move((float)num);
	}
}
