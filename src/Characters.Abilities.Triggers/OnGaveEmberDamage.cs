using System;
using UnityEngine;

namespace Characters.Abilities.Triggers;

[Serializable]
public class OnGaveEmberDamage : Trigger
{
	[SerializeField]
	private Transform _moveToHitPosition;

	[SerializeField]
	private CharacterTypeBoolArray _characterTypes = new CharacterTypeBoolArray(true, true, true, true, true, false, false, false);

	[SerializeField]
	private CharacterStatusKindBoolArray _characterStatusKinds;

	[SerializeField]
	private int _killCount = 1;

	private int _remainKillCount;

	private Character _character;

	public OnGaveEmberDamage()
	{
		_remainKillCount = _killCount;
	}

	public override void Attach(Character character)
	{
		_character = character;
		_character.status.onGaveEmberDamage += HandleOnGaveEmberDamage;
	}

	private void HandleOnGaveEmberDamage(Character attacker, Character target)
	{
		//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
		if (!_characterTypes[target.type] || (_characterTypes[Character.Type.Boss] && (target.key == Key.FirstHero1 || target.key == Key.FirstHero2 || target.key == Key.Unspecified)))
		{
			return;
		}
		bool flag = false;
		if ((_characterStatusKinds[CharacterStatus.Kind.Wound] && target.status.wounded) || (_characterStatusKinds[CharacterStatus.Kind.Burn] && target.status.burning) || (_characterStatusKinds[CharacterStatus.Kind.Freeze] && target.status.freezed) || (_characterStatusKinds[CharacterStatus.Kind.Poison] && target.status.poisoned) || (_characterStatusKinds[CharacterStatus.Kind.Stun] && target.status.stuned))
		{
			flag = true;
		}
		if (flag)
		{
			if ((Object)(object)_moveToHitPosition != (Object)null)
			{
				_moveToHitPosition.position = ((Component)target).transform.position;
			}
			_remainKillCount--;
			if (_remainKillCount <= 0)
			{
				_remainKillCount = _killCount;
				Invoke();
			}
		}
	}

	public override void Detach()
	{
		_character.status.onGaveEmberDamage -= HandleOnGaveEmberDamage;
	}
}
