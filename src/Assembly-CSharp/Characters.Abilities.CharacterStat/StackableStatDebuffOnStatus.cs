using System;
using Unity.Mathematics;
using UnityEngine;

namespace Characters.Abilities.CharacterStat;

[Serializable]
public class StackableStatDebuffOnStatus : Ability
{
	public class Instance : AbilityInstance<StackableStatDebuffOnStatus>
	{
		private Stat.Values _stat;

		public override int iconStacks => (int)((float)ability.stack * ability._iconStacksPerStack);

		public Instance(Character owner, StackableStatDebuffOnStatus ability)
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
			AttachTrigger();
		}

		protected override void OnDetach()
		{
			DetachTrigger();
			owner.stat.DetachValues(_stat);
			owner.stat.DetachValues(ability._maxStackBounsStat);
		}

		public void UpdateStack()
		{
			for (int i = 0; i < _stat.values.Length; i++)
			{
				_stat.values[i].value = ability._statPerStack.values[i].GetStackedValue(ability.stack);
			}
			if (ability.stack == ability._maxStack)
			{
				owner.stat.AttachValues(ability._maxStackBounsStat);
			}
			else
			{
				owner.stat.DetachValues(ability._maxStackBounsStat);
			}
			owner.stat.SetNeedUpdate();
		}

		private void AttachTrigger()
		{
			switch (ability._kind)
			{
			case CharacterStatus.Kind.Wound:
				if (!ability._onRelease)
				{
					owner.status.onApplyWound += IncreaseStack;
				}
				else
				{
					owner.status.onApplyBleed += IncreaseStack;
				}
				break;
			case CharacterStatus.Kind.Burn:
				if (!ability._onRelease)
				{
					owner.status.onApplyBurn += IncreaseStack;
				}
				else
				{
					owner.status.onReleaseBurn += IncreaseStack;
				}
				break;
			case CharacterStatus.Kind.Freeze:
				if (!ability._onRelease)
				{
					owner.status.onApplyFreeze += IncreaseStack;
				}
				else
				{
					owner.status.onReleaseFreeze += IncreaseStack;
				}
				break;
			case CharacterStatus.Kind.Poison:
				if (!ability._onRelease)
				{
					owner.status.onApplyPoison += IncreaseStack;
				}
				else
				{
					owner.status.onReleasePoison += IncreaseStack;
				}
				break;
			case CharacterStatus.Kind.Stun:
				if (!ability._onRelease)
				{
					owner.status.onApplyStun += IncreaseStack;
				}
				else
				{
					owner.status.onReleaseStun += IncreaseStack;
				}
				break;
			}
		}

		private void DetachTrigger()
		{
			switch (ability._kind)
			{
			case CharacterStatus.Kind.Wound:
				if (!ability._onRelease)
				{
					owner.status.onApplyWound -= IncreaseStack;
				}
				else
				{
					owner.status.onApplyBleed -= IncreaseStack;
				}
				break;
			case CharacterStatus.Kind.Burn:
				if (!ability._onRelease)
				{
					owner.status.onApplyBurn -= IncreaseStack;
				}
				else
				{
					owner.status.onReleaseBurn -= IncreaseStack;
				}
				break;
			case CharacterStatus.Kind.Freeze:
				if (!ability._onRelease)
				{
					owner.status.onApplyFreeze -= IncreaseStack;
				}
				else
				{
					owner.status.onReleaseFreeze -= IncreaseStack;
				}
				break;
			case CharacterStatus.Kind.Poison:
				if (!ability._onRelease)
				{
					owner.status.onApplyPoison -= IncreaseStack;
				}
				else
				{
					owner.status.onReleasePoison -= IncreaseStack;
				}
				break;
			case CharacterStatus.Kind.Stun:
				if (!ability._onRelease)
				{
					owner.status.onApplyStun -= IncreaseStack;
				}
				else
				{
					owner.status.onReleasePoison -= IncreaseStack;
				}
				break;
			}
		}

		private void IncreaseStack(Character owner, Character target)
		{
			ability.stack++;
		}
	}

	[SerializeField]
	[Tooltip("다시 획득할 경우 스택을 초기화할지")]
	private bool _resetOnAttach = true;

	[SerializeField]
	private int _maxStack;

	[Tooltip("스택이 쌓일 때마다 남은 시간을 초기화할지")]
	[SerializeField]
	private bool _refreshRemainTime = true;

	[SerializeField]
	[Tooltip("실제 스택 1개당 아이콘 상에 표시할 스택")]
	private float _iconStacksPerStack = 1f;

	[SerializeField]
	private bool _onRelease;

	[SerializeField]
	internal CharacterStatus.Kind _kind;

	[SerializeField]
	private Stat.Values _statPerStack;

	[SerializeField]
	private Stat.Values _maxStackBounsStat;

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
