using System;
using UnityEngine;

namespace Characters.Abilities.Triggers;

[Serializable]
public sealed class OnGaveDamageStatusTarget : Trigger
{
	[SerializeField]
	private Transform _moveToHitPosition;

	[SerializeField]
	private Transform _moveToTargetPosition;

	[SerializeField]
	private bool _onCritical;

	[SerializeField]
	[Tooltip("비어있지 않을 경우, 해당 키를 가진 공격에만 발동됨")]
	private string _attackKey;

	[SerializeField]
	private AttackTypeBoolArray _attackType = new AttackTypeBoolArray(false, true, true, true, false);

	[SerializeField]
	private CharacterTypeBoolArray _characterTypes = new CharacterTypeBoolArray(true, true, true, true, true, false, false, false);

	[SerializeField]
	private CharacterStatusKindBoolArray _characterStatusKinds;

	private Character _character;

	private bool _passPrecondition;

	public override void Attach(Character character)
	{
		_character = character;
		((PriorityList<GiveDamageDelegate>)_character.onGiveDamage).Add(int.MaxValue, (GiveDamageDelegate)HandleOnGiveDamage);
		Character character2 = _character;
		character2.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character2.onGaveDamage, new GaveDamageDelegate(HandleOnGaveDamage));
	}

	private bool HandleOnGiveDamage(ITarget target, ref Damage damage)
	{
		_passPrecondition = false;
		if ((Object)(object)target.character.status == (Object)null || !target.character.status.IsApplying(_characterStatusKinds))
		{
			return false;
		}
		_passPrecondition = true;
		return false;
	}

	public override void Detach()
	{
		((PriorityList<GiveDamageDelegate>)_character.onGiveDamage).Remove((GiveDamageDelegate)HandleOnGiveDamage);
		Character character = _character;
		character.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(character.onGaveDamage, new GaveDamageDelegate(HandleOnGaveDamage));
	}

	private void HandleOnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
	{
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		if (_passPrecondition && ((EnumArray<Character.Type, bool>)_characterTypes)[target.character.type] && ((EnumArray<Damage.AttackType, bool>)_attackType)[gaveDamage.attackType] && (!_onCritical || gaveDamage.critical) && (string.IsNullOrWhiteSpace(_attackKey) || gaveDamage.key.Equals(_attackKey, StringComparison.OrdinalIgnoreCase)))
		{
			if ((Object)(object)_moveToHitPosition != (Object)null)
			{
				_moveToHitPosition.position = Vector2.op_Implicit(gaveDamage.hitPoint);
			}
			if ((Object)(object)_moveToTargetPosition != (Object)null)
			{
				_moveToTargetPosition.position = ((Component)target.collider).transform.position;
			}
			Invoke();
		}
	}
}
