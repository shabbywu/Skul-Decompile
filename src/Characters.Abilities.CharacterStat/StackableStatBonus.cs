using System;
using Unity.Mathematics;
using UnityEngine;

namespace Characters.Abilities.CharacterStat;

[Serializable]
public class StackableStatBonus : Ability
{
	public class Instance : AbilityInstance<StackableStatBonus>
	{
		private Stat.Values _stat;

		public override int iconStacks => (int)((float)ability.stack * ability._iconStacksPerStack);

		public Instance(Character owner, StackableStatBonus ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			_stat = ability._statPerStack.Clone();
			owner.stat.AttachValues(_stat);
			if (ability._resetOnAttach)
			{
				ability.stack = 1;
			}
		}

		protected override void OnDetach()
		{
			owner.stat.DetachValues(_stat);
		}

		public override void Refresh()
		{
			if (ability._refreshRemainTime)
			{
				base.Refresh();
			}
			ability.stack++;
		}

		public void UpdateStack()
		{
			for (int i = 0; i < ((ReorderableArray<Stat.Value>)_stat).values.Length; i++)
			{
				((ReorderableArray<Stat.Value>)_stat).values[i].value = ((ReorderableArray<Stat.Value>)ability._statPerStack).values[i].GetStackedValue(ability.stack);
			}
			owner.stat.SetNeedUpdate();
		}
	}

	[SerializeField]
	[Tooltip("다시 획득할 경우 스택을 초기화할지")]
	private bool _resetOnAttach = true;

	[SerializeField]
	private int _maxStack;

	[SerializeField]
	[Tooltip("스택이 쌓일 때마다 남은 시간을 초기화할지")]
	private bool _refreshRemainTime = true;

	[Tooltip("실제 스택 1개당 아이콘 상에 표시할 스택")]
	[SerializeField]
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

	public int maxStack => _maxStack;

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
		return _instance = new Instance(owner, this);
	}
}
