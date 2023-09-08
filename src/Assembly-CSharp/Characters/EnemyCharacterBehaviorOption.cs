using BehaviorDesigner.Runtime;
using Services;
using Singletons;
using UnityEngine;

namespace Characters;

[RequireComponent(typeof(BehaviorDesignerCommunicator))]
public class EnemyCharacterBehaviorOption : MonoBehaviour
{
	public const string Target = "Target";

	public const string IsWander = "IsWander";

	public const string IsChasable = "IsChasable";

	[SerializeField]
	private BehaviorDesignerCommunicator _communicator;

	[Header("타겟찾기 옵션")]
	[SerializeField]
	private bool _setTargetToPlayer;

	[SerializeField]
	private bool _idleUntilFindTarget;

	[Header("공격&추적 옵션")]
	[SerializeField]
	private bool _staticMovement;

	[Header("상태이상 옵션")]
	[SerializeField]
	private bool _isStunAdaptable = true;

	public bool IsStunAdaptable => _isStunAdaptable;

	private void Start()
	{
		SetSharedVariableOptions();
	}

	public void SetBehaviorOption(bool setTargetToPlayer = false, bool idleUntilFindTarget = false, bool staticMovement = false)
	{
		_setTargetToPlayer = setTargetToPlayer;
		_idleUntilFindTarget = idleUntilFindTarget;
		_staticMovement = staticMovement;
		SetSharedVariableOptions();
	}

	private void SetSharedVariableOptions()
	{
		if ((Object)(object)_communicator == (Object)null)
		{
			_communicator = ((Component)this).GetComponent<BehaviorDesignerCommunicator>();
		}
		_communicator.SetVariable<SharedBool>("IsChasable", (object)(!_staticMovement));
		_communicator.SetVariable<SharedBool>("IsWander", (object)(!_idleUntilFindTarget));
		if (_setTargetToPlayer)
		{
			SetTargetToPlayer();
		}
	}

	public void SetTargetToPlayer()
	{
		Character player = Singleton<Service>.Instance.levelManager.player;
		_communicator.SetVariable<SharedCharacter>("Target", (object)player);
	}
}
