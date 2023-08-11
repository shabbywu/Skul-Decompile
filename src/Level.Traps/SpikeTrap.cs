using System;
using System.Collections;
using System.Linq;
using Characters;
using Characters.Actions;
using Characters.Gear.Weapons;
using Characters.Player;
using PhysicsUtils;
using UnityEngine;

namespace Level.Traps;

public class SpikeTrap : MonoBehaviour
{
	[SerializeField]
	private Character _character;

	[SerializeField]
	private float _interval = 2f;

	[SerializeField]
	private Characters.Actions.Action _attackAction;

	[SerializeField]
	private Collider2D _range;

	[SerializeField]
	private readonly NonAllocOverlapper _overlapper = new NonAllocOverlapper(1);

	[SerializeField]
	[Space]
	private Weapon[] _weaponsToExclude;

	private string[] _weaponNamesToExclude;

	private void Awake()
	{
		_attackAction.Initialize(_character);
		_weaponNamesToExclude = _weaponsToExclude.Select((Weapon weapon) => ((Object)weapon).name).ToArray();
		((MonoBehaviour)this).StartCoroutine(CAttack());
	}

	private IEnumerator CAttack()
	{
		while (true)
		{
			yield return Chronometer.global.WaitForSeconds(0.1f);
			if (FindPlayer())
			{
				_attackAction.TryStart();
				yield return _attackAction.CWaitForEndOfRunning();
				yield return Chronometer.global.WaitForSeconds(_interval);
			}
		}
	}

	private bool FindPlayer()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		((Behaviour)_range).enabled = true;
		((ContactFilter2D)(ref _overlapper.contactFilter)).SetLayerMask(LayerMask.op_Implicit(512));
		_overlapper.OverlapCollider(_range);
		((Behaviour)_range).enabled = false;
		Target component = _overlapper.GetComponent<Target>();
		if ((Object)(object)component == (Object)null || (Object)(object)component.character == (Object)null || !component.character.movement.isGrounded)
		{
			return false;
		}
		PlayerComponents playerComponents = component.character.playerComponents;
		if (playerComponents != null && _weaponNamesToExclude.Any((string name) => name.Equals(((Object)playerComponents.inventory.weapon.polymorphOrCurrent).name, StringComparison.OrdinalIgnoreCase)))
		{
			return false;
		}
		return true;
	}
}
