using Characters.Actions.Cooldowns;
using Characters.Gear.Weapons;
using Characters.Operations;
using Characters.Player;
using UnityEditor;
using UnityEngine;

namespace Characters.Actions.Constraints;

public class CooldownConstraint : Constraint
{
	[SerializeField]
	[Cooldown.Subcomponent]
	private Cooldown _cooldown;

	[UnityEditor.Subcomponent(typeof(OperationInfo))]
	[SerializeField]
	private OperationInfo.Subcomponents _operationsWhenReady;

	private Weapon _weapon;

	private WeaponInventory _inventory;

	public bool canUse => _cooldown.canUse;

	public override void Initilaize(Action action)
	{
		base.Initilaize(action);
		_cooldown.Initialize(action.owner);
		_inventory = ((Component)action.owner).GetComponent<WeaponInventory>();
		if ((Object)(object)_inventory == (Object)null)
		{
			_cooldown.onReady += RunOperationsWhenReady;
			return;
		}
		_weapon = ((Component)this).GetComponentInParent<Weapon>();
		_cooldown.onReady += RunOperationsWhenReadyWithCheckWeapon;
	}

	private void RunOperationsWhenReady()
	{
		((MonoBehaviour)this).StartCoroutine(_operationsWhenReady.CRun(_action.owner));
	}

	private void RunOperationsWhenReadyWithCheckWeapon()
	{
		if ((Object)(object)_inventory.polymorphOrCurrent == (Object)(object)_weapon)
		{
			((MonoBehaviour)this).StartCoroutine(_operationsWhenReady.CRun(_action.owner));
		}
	}

	private void OnDisable()
	{
		_cooldown.onReady -= RunOperationsWhenReady;
		_cooldown.onReady -= RunOperationsWhenReadyWithCheckWeapon;
	}

	public override bool Pass()
	{
		return _cooldown.canUse;
	}

	public override void Consume()
	{
		_cooldown.Consume();
	}
}
