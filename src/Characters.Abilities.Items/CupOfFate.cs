using System;
using Characters.Gear;
using Characters.Gear.Items;
using Characters.Gear.Synergy.Inscriptions;
using Services;
using Singletons;
using Unity.Mathematics;
using UnityEngine;

namespace Characters.Abilities.Items;

[Serializable]
public sealed class CupOfFate : Ability
{
	public class Instance : AbilityInstance<CupOfFate>
	{
		private Stat.Values _stat;

		public override int iconStacks => (int)((float)ability.stack * ability._iconStacksPerStack);

		public Instance(Character owner, CupOfFate ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			Singleton<Service>.Instance.gearManager.onItemInstanceChanged += HandleItemInstanceChanged;
			HandleItemInstanceChanged();
			_stat = ability._statPerStack.Clone();
			owner.stat.AttachValues(_stat);
			if (ability._resetOnAttach)
			{
				ability.stack = 1;
			}
			else
			{
				UpdateStack();
			}
		}

		protected override void OnDetach()
		{
			Singleton<Service>.Instance.gearManager.onItemInstanceChanged -= HandleItemInstanceChanged;
			Inscription.Key[] keys = ability._keys;
			foreach (Inscription.Key key in keys)
			{
				foreach (Item item in Singleton<Service>.Instance.gearManager.GetItemInstanceByKeyword(key))
				{
					item.onDiscard -= TryStackUp;
				}
			}
			owner.stat.DetachValues(_stat);
		}

		private void HandleItemInstanceChanged()
		{
			Inscription.Key[] keys = ability._keys;
			foreach (Inscription.Key key in keys)
			{
				foreach (Item item in Singleton<Service>.Instance.gearManager.GetItemInstanceByKeyword(key))
				{
					item.onDiscard -= TryStackUp;
					item.onDiscard += TryStackUp;
				}
			}
		}

		public void TryStackUp(Characters.Gear.Gear gear)
		{
			if (gear.destructible)
			{
				ability.stack++;
				UpdateStack();
			}
		}

		public void UpdateStack()
		{
			for (int i = 0; i < _stat.values.Length; i++)
			{
				_stat.values[i].value = ability._statPerStack.values[i].GetStackedValue(ability.stack);
			}
			owner.stat.SetNeedUpdate();
		}
	}

	[SerializeField]
	private Inscription.Key[] _keys;

	[Tooltip("다시 획득할 경우 스택을 초기화할지")]
	[SerializeField]
	private bool _resetOnAttach = true;

	[SerializeField]
	private int _maxStack;

	[SerializeField]
	[Tooltip("실제 스택 1개당 아이콘 상에 표시할 스택")]
	private float _iconStacksPerStack = 1f;

	[SerializeField]
	private Stat.Values _statPerStack;

	private Instance _instance;

	private int _stack;

	public int stack
	{
		get
		{
			return _stack;
		}
		set
		{
			_stack = math.min(value, _maxStack);
			if (_instance != null)
			{
				_instance.UpdateStack();
			}
		}
	}

	public void Load(Character owner, int stack)
	{
		owner.ability.Add(this);
		this.stack = stack;
	}

	public override void Initialize()
	{
		base.Initialize();
		if (_maxStack == 0)
		{
			_maxStack = int.MaxValue;
		}
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		_instance = new Instance(owner, this);
		return _instance;
	}
}
