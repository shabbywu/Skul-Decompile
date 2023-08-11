using System.Collections;
using Characters;
using Characters.Actions;
using Characters.Operations;
using PhysicsUtils;
using UnityEditor;
using UnityEngine;

namespace Level.Traps;

public class JumpPlatform : MonoBehaviour
{
	[SerializeField]
	private Character _character;

	[SerializeField]
	private Action _jumpAction;

	[SerializeField]
	private Collider2D _trigger;

	[SerializeField]
	private LayerMask _targetLayer;

	[SerializeField]
	private Collider2D _jumpPlatform;

	[SerializeField]
	[Subcomponent(typeof(TargetedOperationInfo))]
	private TargetedOperationInfo.Subcomponents _operationToTarget;

	private static readonly NonAllocOverlapper _lapper;

	static JumpPlatform()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		_lapper = new NonAllocOverlapper(15);
	}

	private void Start()
	{
		_operationToTarget.Initialize();
		((MonoBehaviour)this).StartCoroutine(CRun());
	}

	private IEnumerator CRun()
	{
		while (true)
		{
			yield return null;
			((ContactFilter2D)(ref _lapper.contactFilter)).SetLayerMask(_targetLayer);
			ReadonlyBoundedList<Collider2D> results = _lapper.OverlapCollider(_trigger).results;
			if (results.Count <= 0)
			{
				continue;
			}
			foreach (Collider2D item in results)
			{
				if (((Component)(object)item).TryFindCharacterComponent(out var character) && !((Object)(object)character == (Object)null) && SteppedOn(character))
				{
					Jump(character);
				}
			}
		}
	}

	private bool SteppedOn(Character character)
	{
		if ((Object)(object)character.movement.controller.collisionState.lastStandingCollider != (Object)(object)_jumpPlatform)
		{
			return false;
		}
		if (!character.movement.isGrounded)
		{
			return false;
		}
		return true;
	}

	private void Jump(Character target)
	{
		_jumpAction.TryStart();
		((MonoBehaviour)this).StartCoroutine(_operationToTarget.CRun(_character, target));
	}
}
