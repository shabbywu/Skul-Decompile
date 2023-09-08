using System;
using Characters.Movements;
using UnityEngine;

namespace Characters.Abilities.Triggers;

[Serializable]
public class OnGaveDamage : Trigger
{
	[SerializeField]
	private double _minDamage = 1.0;

	[Range(0f, 1f)]
	[SerializeField]
	private double _minDamagePercent;

	[SerializeField]
	private Transform _moveToHitPosition;

	[Header("Filter")]
	[SerializeField]
	private bool _needCritical;

	[SerializeField]
	private bool _backOnly;

	[SerializeField]
	[Tooltip("비어있지 않을 경우, 해당 키를 가진 공격에만 발동됨")]
	private string _attackKey;

	[SerializeField]
	private CharacterTypeBoolArray _targetType = new CharacterTypeBoolArray(true, true, true, true, true, true, true, true, true);

	[SerializeField]
	private MotionTypeBoolArray _attackTypes;

	[SerializeField]
	private AttackTypeBoolArray _damageTypes;

	private Character _character;

	public override void Attach(Character character)
	{
		_character = character;
		Character character2 = _character;
		character2.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character2.onGaveDamage, new GaveDamageDelegate(OnCharacterGaveDamage));
	}

	public override void Detach()
	{
		Character character = _character;
		character.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(character.onGaveDamage, new GaveDamageDelegate(OnCharacterGaveDamage));
	}

	private void OnCharacterGaveDamage(ITarget target, in Damage originalDamage, in Damage tookDamage, double damageDealt)
	{
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0112: Unknown result type (might be due to invalid IL or missing references)
		//IL_0128: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)target.character == (Object)null || tookDamage.amount < _minDamage || tookDamage.amount < target.character.health.maximumHealth * _minDamagePercent || (_needCritical && !tookDamage.critical) || !_attackTypes[tookDamage.motionType] || !_damageTypes[tookDamage.attackType] || !_targetType[target.character.type] || (!string.IsNullOrWhiteSpace(_attackKey) && !tookDamage.key.Equals(_attackKey, StringComparison.OrdinalIgnoreCase)))
		{
			return;
		}
		if (_backOnly)
		{
			if (_character.movement.config.type == Movement.Config.Type.Static)
			{
				return;
			}
			Vector3 position = target.transform.position;
			Vector3 position2 = ((Component)_character).transform.position;
			if ((target.character.lookingDirection == Character.LookingDirection.Right && position.x < position2.x) || (target.character.lookingDirection == Character.LookingDirection.Left && position.x > position2.x))
			{
				return;
			}
		}
		if ((Object)(object)_moveToHitPosition != (Object)null)
		{
			_moveToHitPosition.position = Vector2.op_Implicit(tookDamage.hitPoint);
		}
		Invoke();
	}
}
