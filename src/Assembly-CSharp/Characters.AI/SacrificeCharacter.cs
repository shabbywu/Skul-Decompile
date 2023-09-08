using Characters.AI.Behaviours;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.AI;

public sealed class SacrificeCharacter : MonoBehaviour
{
	[SerializeField]
	private AIController _aiController;

	[SerializeField]
	private Sacrifice _sacrifice;

	[SerializeField]
	[Range(0f, 1f)]
	private double _triggerHealthPercent;

	[SerializeField]
	[Subcomponent(typeof(OperationInfo))]
	private OperationInfo.Subcomponents _onForceSacrifice;

	private Character _character;

	private void Awake()
	{
		_character = _aiController.character;
		_onForceSacrifice.Initialize();
	}

	public void Run(bool force = false)
	{
		if (!AlreadyCheck() && (Object)(object)_aiController != (Object)null && ((Component)_aiController).gameObject.activeSelf)
		{
			if (force)
			{
				_aiController.character.CancelAction();
				((MonoBehaviour)this).StartCoroutine(_onForceSacrifice.CRun(_character));
			}
			((MonoBehaviour)this).StartCoroutine(_sacrifice.CRun(_aiController));
		}
	}

	public bool CanSacrifice()
	{
		return _character.health.percent < _triggerHealthPercent;
	}

	private bool AlreadyCheck()
	{
		return _sacrifice.result == Behaviour.Result.Doing;
	}

	private void Update()
	{
		if (!_character.health.dead && !AlreadyCheck() && CanSacrifice())
		{
			Run();
		}
	}
}
