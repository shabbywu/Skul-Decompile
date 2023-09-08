using System.Collections;
using Characters.Abilities.Constraints;
using Characters.Actions;
using Characters.Controllers;
using Characters.Movements;
using InControl;
using Unity.Mathematics;
using UnityEngine;

namespace Characters.Gear.Weapons;

public class SkulWait : MonoBehaviour
{
	private const float _waitingTime = 20f;

	[SerializeField]
	private Weapon _weapon;

	[SerializeField]
	private Action _action;

	[SerializeField]
	[Constraint.Subcomponent]
	private Constraint.Subcomponents _constraints;

	private PlayerInput _input;

	private void OnEnable()
	{
		((MonoBehaviour)this).StartCoroutine(CCheckWait());
	}

	private IEnumerator CCheckWait()
	{
		yield return null;
		_input = ((Component)_weapon.owner).GetComponent<PlayerInput>();
		float waitedTime = 0f;
		while (true)
		{
			Movement movement = _weapon.owner.movement;
			waitedTime += Chronometer.global.deltaTime;
			if (math.abs(movement.moved.x) > 0.0001f || math.abs(movement.moved.y) > 0.0001f)
			{
				waitedTime = 0f;
				yield return null;
				continue;
			}
			if (!_constraints.Pass())
			{
				waitedTime = 0f;
				yield return null;
				continue;
			}
			if (PressedAnyKey())
			{
				waitedTime = 0f;
			}
			if (waitedTime > 20f)
			{
				waitedTime = 0f;
				_action.TryStart();
				yield return CWaitForWaitAction();
			}
			yield return null;
		}
	}

	private bool PressedAnyKey()
	{
		for (int i = 0; i < Button.count; i++)
		{
			if (_input[i] != null && ((OneAxisInputControl)_input[i]).WasPressed)
			{
				return true;
			}
		}
		return false;
	}

	private IEnumerator CWaitForWaitAction()
	{
		Movement movement = _weapon.owner.movement;
		while (_action.running && _constraints.Pass())
		{
			yield return null;
			if (PressedAnyKey() || math.abs(movement.moved.x) > 0.0001f || math.abs(movement.moved.y) > 0.0001f)
			{
				break;
			}
		}
		_weapon.owner.CancelAction();
	}
}
