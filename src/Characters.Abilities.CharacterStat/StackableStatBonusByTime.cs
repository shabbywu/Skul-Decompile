using System;
using UnityEngine;

namespace Characters.Abilities.CharacterStat;

[Serializable]
public class StackableStatBonusByTime : Ability
{
	public class Instance : AbilityInstance<StackableStatBonusByTime>
	{
		private float _remainUpdateTime;

		private int _stacks;

		private Stat.Values _stat;

		public override float iconFillAmount
		{
			get
			{
				if (_stacks >= ability._maxStack)
				{
					return base.iconFillAmount;
				}
				if (_remainUpdateTime > 0f)
				{
					if (!ability._visibleIconStack)
					{
						return base.remainTime / ability.duration;
					}
					return _remainUpdateTime / ability._updateInterval;
				}
				return base.iconFillAmount;
			}
		}

		public override int iconStacks
		{
			get
			{
				if (!ability._visibleIconStack)
				{
					return 0;
				}
				return (int)((float)_stacks * ability._iconStacksPerStack);
			}
		}

		public Instance(Character owner, StackableStatBonusByTime ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			_stat = ability._statPerStack.Clone();
			_stacks = ability._startStack;
			_remainUpdateTime = ability._updateInterval;
			owner.stat.AttachValues(_stat);
			UpdateStack();
		}

		protected override void OnDetach()
		{
			owner.stat.DetachValues(_stat);
		}

		public override void Refresh()
		{
			base.Refresh();
			_stacks = ability._startStack;
			UpdateStack();
			_remainUpdateTime = ability._updateInterval;
		}

		private void UpdateStack()
		{
			for (int i = 0; i < ((ReorderableArray<Stat.Value>)_stat).values.Length; i++)
			{
				((ReorderableArray<Stat.Value>)_stat).values[i].value = ((ReorderableArray<Stat.Value>)ability._statPerStack).values[i].GetStackedValue(_stacks);
			}
			owner.stat.SetNeedUpdate();
		}

		public override void UpdateTime(float deltaTime)
		{
			base.UpdateTime(deltaTime);
			if ((!ability._positive || _stacks < ability._maxStack) && (ability._positive || _stacks > 0))
			{
				_remainUpdateTime -= deltaTime;
				if (_remainUpdateTime < 0f)
				{
					_remainUpdateTime += ability._updateInterval;
					_stacks += (ability._positive ? 1 : (-1));
					UpdateStack();
				}
			}
		}
	}

	[SerializeField]
	private bool _visibleIconStack = true;

	[SerializeField]
	private bool _positive = true;

	[SerializeField]
	private float _updateInterval;

	[SerializeField]
	private int _startStack;

	[SerializeField]
	private int _maxStack;

	[SerializeField]
	[Tooltip("실제 스택 1개당 아이콘 상에 표시할 스택")]
	private float _iconStacksPerStack = 1f;

	[SerializeField]
	private Stat.Values _statPerStack;

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
		return new Instance(owner, this);
	}
}
