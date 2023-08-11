using System;
using UnityEngine;

namespace Characters.Abilities.Triggers;

[Serializable]
public class OnStatusTargetKilled : Trigger
{
	[SerializeField]
	private Transform _moveToHitPosition;

	[SerializeField]
	private bool _onCritical;

	[Tooltip("비어있지 않을 경우, 해당 키를 가진 공격에만 발동됨")]
	[SerializeField]
	private string _attackKey;

	[SerializeField]
	private CharacterTypeBoolArray _characterTypes = new CharacterTypeBoolArray(true, true, true, true, true, false, false, false);

	[SerializeField]
	private CharacterStatusKindBoolArray _characterStatusKinds;

	[SerializeField]
	private int _killCount = 1;

	private int _remainKillCount;

	private Character _character;

	public OnStatusTargetKilled()
	{
		_remainKillCount = _killCount;
	}

	public override void Attach(Character character)
	{
		_character = character;
		Character character2 = _character;
		character2.onKilled = (Character.OnKilledDelegate)Delegate.Combine(character2.onKilled, new Character.OnKilledDelegate(OnCharacterKilled));
	}

	public override void Detach()
	{
		Character character = _character;
		character.onKilled = (Character.OnKilledDelegate)Delegate.Remove(character.onKilled, new Character.OnKilledDelegate(OnCharacterKilled));
	}

	private void OnCharacterKilled(ITarget target, ref Damage damage)
	{
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		if (!((EnumArray<Character.Type, bool>)_characterTypes)[target.character.type] || (((EnumArray<Character.Type, bool>)_characterTypes)[Character.Type.Boss] && (target.character.key == Key.FirstHero1 || target.character.key == Key.FirstHero2 || target.character.key == Key.Unspecified)) || (_onCritical && !damage.critical) || (!string.IsNullOrWhiteSpace(_attackKey) && !damage.key.Equals(_attackKey, StringComparison.OrdinalIgnoreCase)))
		{
			return;
		}
		bool flag = false;
		if ((((EnumArray<CharacterStatus.Kind, bool>)_characterStatusKinds)[CharacterStatus.Kind.Wound] && target.character.status.wounded) || (((EnumArray<CharacterStatus.Kind, bool>)_characterStatusKinds)[CharacterStatus.Kind.Burn] && target.character.status.burning) || (((EnumArray<CharacterStatus.Kind, bool>)_characterStatusKinds)[CharacterStatus.Kind.Freeze] && target.character.status.freezed) || (((EnumArray<CharacterStatus.Kind, bool>)_characterStatusKinds)[CharacterStatus.Kind.Poison] && target.character.status.poisoned) || (((EnumArray<CharacterStatus.Kind, bool>)_characterStatusKinds)[CharacterStatus.Kind.Stun] && target.character.status.stuned))
		{
			flag = true;
		}
		if (flag)
		{
			if ((Object)(object)_moveToHitPosition != (Object)null)
			{
				_moveToHitPosition.position = Vector2.op_Implicit(damage.hitPoint);
			}
			_remainKillCount--;
			if (_remainKillCount <= 0)
			{
				_remainKillCount = _killCount;
				Invoke();
			}
		}
	}
}
