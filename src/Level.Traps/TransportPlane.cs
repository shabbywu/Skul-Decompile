using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Characters;
using Characters.Abilities.Constraints;
using Characters.Actions;
using Characters.Gear.Weapons;
using Characters.Player;
using PhysicsUtils;
using UnityEngine;

namespace Level.Traps;

[ExecuteAlways]
public class TransportPlane : ControlableTrap
{
	[SerializeField]
	[GetComponent]
	private SpriteRenderer _spriteRenderer;

	[SerializeField]
	private BoxCollider2D _collider;

	[SerializeField]
	private Character _character;

	[SerializeField]
	private Characters.Actions.Action _action;

	[SerializeField]
	private int _size = 2;

	[SerializeField]
	private float _speed;

	[SerializeField]
	private float _castDistance;

	[SerializeField]
	private LayerMask _layer;

	private List<Character> targets = new List<Character>();

	private IEnumerator _coroutine;

	private static readonly NonAllocCaster _caster;

	[Constraint.Subcomponent]
	[SerializeField]
	private Constraint[] _constraints;

	[SerializeField]
	[Space]
	private Weapon[] _weaponsToExclude;

	private string[] _weaponNamesToExclude;

	static TransportPlane()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		_caster = new NonAllocCaster(15);
	}

	private void SetSize()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		Vector2 size = _spriteRenderer.size;
		size.x = _size + 1;
		_spriteRenderer.size = size;
		Vector2 size2 = _collider.size;
		size2.x = (float)(_size + 1) - 1.5f;
		_collider.size = size2;
	}

	private void SetSpeed()
	{
	}

	private void Awake()
	{
		_weaponNamesToExclude = _weaponsToExclude.Select((Weapon weapon) => ((Object)weapon).name).ToArray();
		_coroutine = CRun();
		SetSize();
		SetSpeed();
	}

	private void Update()
	{
	}

	public override void Activate()
	{
		_action.TryStart();
		((MonoBehaviour)this).StartCoroutine(_coroutine);
	}

	public override void Deactivate()
	{
		if (_action.running)
		{
			_character.CancelAction();
		}
		((MonoBehaviour)this).StopCoroutine(_coroutine);
	}

	private IEnumerator CRun()
	{
		while (true)
		{
			yield return null;
			if (!_constraints.Pass())
			{
				continue;
			}
			foreach (Character target in targets)
			{
				if ((Object)(object)target == (Object)null || !target.liveAndActive)
				{
					targets.Remove(target);
				}
				else
				{
					target.movement.force.x += (float)((_character.lookingDirection == Character.LookingDirection.Right) ? 1 : (-1)) * _speed * ((ChronometerBase)_character.chronometer.master).deltaTime;
				}
			}
		}
	}

	private void OnTriggerEnter2D(Collider2D other)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		if (ExtensionMethods.Contains(_layer, ((Component)other).gameObject.layer))
		{
			AddTarget(((Component)other).gameObject);
		}
	}

	private void OnTriggerExit2D(Collider2D other)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		if (ExtensionMethods.Contains(_layer, ((Component)other).gameObject.layer))
		{
			RemoveTarget(((Component)other).gameObject);
		}
	}

	private void AddTarget(GameObject target)
	{
		if (target.TryFindCharacterComponent(out var character))
		{
			PlayerComponents playerComponents = character.playerComponents;
			if (playerComponents == null || !_weaponNamesToExclude.Any((string name) => name.Equals(((Object)playerComponents.inventory.weapon.polymorphOrCurrent).name, StringComparison.OrdinalIgnoreCase)))
			{
				targets.Add(character);
			}
		}
	}

	private void RemoveTarget(GameObject target)
	{
		if (target.TryFindCharacterComponent(out var character) && targets.Contains(character))
		{
			targets.Remove(character);
		}
	}
}
