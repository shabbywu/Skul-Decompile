using System;
using UnityEngine;

namespace Characters.Abilities.Weapons;

[Serializable]
public class PrisonerLancePassive : Ability
{
	public class Instance : AbilityInstance<PrisonerLancePassive>
	{
		private int _currentKillCount;

		public override Sprite icon
		{
			get
			{
				if (_currentKillCount < ability._killCount)
				{
					return null;
				}
				return base.icon;
			}
		}

		public Instance(Character owner, PrisonerLancePassive ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
		}

		protected override void OnDetach()
		{
		}

		private void OnKilled(ITarget target, ref Damage damage)
		{
			if (!((Object)(object)target.character == (Object)null) && ability._characterTypeFilter[target.character.type] && damage.key.Equals(ability._attackKey, StringComparison.OrdinalIgnoreCase))
			{
				_currentKillCount++;
			}
		}

		private bool OnGiveDamage(ITarget target, ref Damage damage)
		{
			if (!damage.key.Equals(ability._attackKey, StringComparison.OrdinalIgnoreCase))
			{
				return false;
			}
			damage.criticalChance = 1.0;
			return false;
		}

		public void StartDetect()
		{
			if (_currentKillCount >= ability._killCount)
			{
				owner.onGiveDamage.Add(0, OnGiveDamage);
			}
			_currentKillCount = 0;
			Character character = owner;
			character.onKilled = (Character.OnKilledDelegate)Delegate.Combine(character.onKilled, new Character.OnKilledDelegate(OnKilled));
		}

		public void StopDetect()
		{
			Character character = owner;
			character.onKilled = (Character.OnKilledDelegate)Delegate.Remove(character.onKilled, new Character.OnKilledDelegate(OnKilled));
			owner.onGiveDamage.Remove(OnGiveDamage);
		}
	}

	[SerializeField]
	private string _attackKey;

	[SerializeField]
	private int _killCount;

	[SerializeField]
	private CharacterTypeBoolArray _characterTypeFilter;

	private Instance _instance;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		if (_instance == null)
		{
			_instance = new Instance(owner, this);
		}
		return _instance;
	}

	public void StartDetect()
	{
		_instance.StartDetect();
	}

	public void StopDetect()
	{
		_instance.StopDetect();
	}
}
