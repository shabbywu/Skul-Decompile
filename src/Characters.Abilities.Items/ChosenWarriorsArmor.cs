using System;
using System.Collections.Generic;
using FX;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Items;

[Serializable]
public sealed class ChosenWarriorsArmor : Ability
{
	public class Instance : AbilityInstance<ChosenWarriorsArmor>
	{
		internal Characters.Shield.Instance _shieldInstance;

		private EffectPoolInstance _loopEffectInstance;

		public Instance(Character owner, ChosenWarriorsArmor ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			Singleton<Service>.Instance.levelManager.onMapChangedAndFadedIn += HandleOnMapChangedAndFadedIn;
		}

		private void OnBreak()
		{
			if (_shieldInstance != null)
			{
				owner.health.shield.Remove(ability);
				_shieldInstance = null;
			}
			if ((Object)(object)_loopEffectInstance != (Object)null)
			{
				_loopEffectInstance.Stop();
				_loopEffectInstance = null;
			}
		}

		private void HandleOnMapChangedAndFadedIn(Map old, Map @new)
		{
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0031: Unknown result type (might be due to invalid IL or missing references)
			if (IsBossMap())
			{
				if (_shieldInstance == null)
				{
					_loopEffectInstance = ability._loopEffectInfo?.Spawn(((Component)owner).transform.position, owner);
					_shieldInstance = owner.health.shield.Add(ability, ability._bossMapAmount, OnBreak);
				}
				else
				{
					_shieldInstance.amount = Mathf.Min(ability._maxAmount, (float)_shieldInstance.amount + ability._bossMapAmount);
				}
			}
			else if (_shieldInstance == null)
			{
				_loopEffectInstance = ability._loopEffectInfo?.Spawn(((Component)owner).transform.position, owner);
				_shieldInstance = owner.health.shield.Add(ability, ability._normalMapAmount, OnBreak);
			}
			else
			{
				_shieldInstance.amount = Mathf.Min(ability._maxAmount, (float)_shieldInstance.amount + ability._normalMapAmount);
			}
		}

		private bool IsBossMap()
		{
			Map instance = Map.Instance;
			if (instance.type == Map.Type.Manual)
			{
				List<Character> allEnemies = instance.waveContainer.GetAllEnemies();
				for (int i = 0; i < allEnemies.Count; i++)
				{
					if (allEnemies[i].type == Character.Type.Boss)
					{
						return true;
					}
				}
			}
			return false;
		}

		protected override void OnDetach()
		{
			Singleton<Service>.Instance.levelManager.onMapChangedAndFadedIn -= HandleOnMapChangedAndFadedIn;
			owner.health.shield.Remove(ability);
			if (_shieldInstance != null)
			{
				owner.health.shield.Remove(ability);
				_shieldInstance = null;
			}
			if ((Object)(object)_loopEffectInstance != (Object)null)
			{
				_loopEffectInstance.Stop();
				_loopEffectInstance = null;
			}
		}

		internal void LoadShield(float value)
		{
			if (_shieldInstance == null)
			{
				_shieldInstance = owner.health.shield.Add(ability, value, OnBreak);
			}
			else
			{
				_shieldInstance.amount = Mathf.Min(ability._maxAmount, value);
			}
		}
	}

	[SerializeField]
	private float _maxAmount;

	[SerializeField]
	private float _normalMapAmount;

	[SerializeField]
	private float _bossMapAmount;

	[SerializeField]
	private EffectInfo _loopEffectInfo = new EffectInfo
	{
		subordinated = true
	};

	private Instance _instance;

	public float amount
	{
		get
		{
			if (_instance == null)
			{
				return 0f;
			}
			if (_instance._shieldInstance == null)
			{
				return 0f;
			}
			return (float)_instance._shieldInstance.amount;
		}
		set
		{
			if (_instance != null)
			{
				_instance.LoadShield(value);
			}
		}
	}

	public void Load(Character owner, int stack)
	{
		owner.ability.Add(this);
		amount = stack;
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		_instance = new Instance(owner, this);
		return _instance;
	}
}
