using Characters;
using Characters.Actions;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Level.Traps;

public class IronDoor : ControlableTrap
{
	[SerializeField]
	private Character _character;

	[SerializeField]
	private Collider2D _blockCollider;

	[SerializeField]
	private Action _downAction;

	[SerializeField]
	private Action _upAction;

	[Subcomponent(typeof(OperationInfo))]
	[SerializeField]
	private OperationInfo.Subcomponents _hitOperations;

	private void Awake()
	{
		_hitOperations.Initialize();
		_character.health.onDied += Run;
		((Component)_character).gameObject.SetActive(false);
	}

	private void Run()
	{
		_character.health.onDied -= Run;
		((MonoBehaviour)this).StartCoroutine(_hitOperations.CRun(_character));
	}

	public override void Activate()
	{
		if (!base.activated)
		{
			_character.CancelAction();
			((Component)_character).gameObject.SetActive(true);
			((Behaviour)_blockCollider).enabled = true;
			_downAction.TryStart();
			base.activated = true;
		}
	}

	public override void Deactivate()
	{
		if (base.activated && !_character.health.dead)
		{
			_character.CancelAction();
			_upAction.TryStart();
			((Behaviour)_blockCollider).enabled = false;
			base.activated = false;
		}
	}
}
