using Characters;
using Characters.Operations;
using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace Level.Traps;

public class RockFall : ControlableTrap
{
	[SerializeField]
	private Character _character;

	[Subcomponent(typeof(OperationInfo))]
	[SerializeField]
	[FormerlySerializedAs("_operations")]
	private OperationInfo.Subcomponents _onGroundedOperations;

	[SerializeField]
	[Subcomponent(typeof(OperationInfo))]
	private OperationInfo.Subcomponents _fallOperations;

	private void Awake()
	{
		_onGroundedOperations.Initialize();
		_fallOperations.Initialize();
		_character.movement.onGrounded += OnGrounded;
	}

	private void OnDestroy()
	{
		_character.movement.onGrounded -= OnGrounded;
	}

	private void OnGrounded()
	{
		_character.movement.onGrounded -= OnGrounded;
		((MonoBehaviour)this).StartCoroutine(_onGroundedOperations.CRun(_character));
	}

	public override void Activate()
	{
		((MonoBehaviour)this).StartCoroutine(_fallOperations.CRun(_character));
	}

	public override void Deactivate()
	{
	}
}
