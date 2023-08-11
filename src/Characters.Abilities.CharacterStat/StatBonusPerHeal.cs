using System;
using FX;
using Singletons;
using Unity.Mathematics;
using UnityEngine;

namespace Characters.Abilities.CharacterStat;

[Serializable]
public class StatBonusPerHeal : Ability
{
	public class Instance : AbilityInstance<StatBonusPerHeal>
	{
		private EffectPoolInstance _buffLoopEffectInstance;

		private Stat.Values _stat;

		private float _remainBuffDuration;

		private bool _attachedBuff;

		public override Sprite icon
		{
			get
			{
				if (!_attachedBuff)
				{
					return null;
				}
				return base.icon;
			}
		}

		public override float iconFillAmount => _remainBuffDuration / ability._buffDuration;

		public override int iconStacks => ability.stack;

		public Instance(Character owner, StatBonusPerHeal ability)
			: base(owner, ability)
		{
			base.iconFillInversed = true;
		}

		protected override void OnAttach()
		{
			_stat = ability._statPerStack.Clone();
			owner.health.onHealed += HandleOnHealed;
		}

		private void HandleOnHealed(double healed, double overHealed)
		{
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			_remainBuffDuration = ability._buffDuration;
			double num = 0.0;
			if (ability._healed)
			{
				num += healed;
			}
			if (ability._overhealed)
			{
				num += overHealed;
			}
			PersistentSingleton<SoundManager>.Instance.PlaySound(ability._buffAttachAudioClipInfo, ((Component)owner).transform.position);
			int num2 = Mathf.CeilToInt((float)num / ability._healAmountPerStack);
			if (_attachedBuff)
			{
				if (num2 >= ability.stack)
				{
					ability.stack = num2;
				}
			}
			else
			{
				AttachBuff();
				ability.stack = num2;
			}
		}

		protected override void OnDetach()
		{
			owner.stat.DetachValues(_stat);
			owner.health.onHealed -= HandleOnHealed;
		}

		public override void UpdateTime(float deltaTime)
		{
			base.UpdateTime(deltaTime);
			_remainBuffDuration -= deltaTime;
			if (_remainBuffDuration < 0f && _attachedBuff)
			{
				DetachBuff();
			}
		}

		private void AttachBuff()
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			_attachedBuff = true;
			_buffLoopEffectInstance = ((ability._buffLoopEffect != null) ? ability._buffLoopEffect.Spawn(((Component)owner).transform.position, owner) : null);
			owner.stat.AttachValues(_stat);
		}

		private void DetachBuff()
		{
			_attachedBuff = false;
			if ((Object)(object)_buffLoopEffectInstance != (Object)null)
			{
				_buffLoopEffectInstance.Stop();
			}
			owner.stat.DetachValues(_stat);
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
	private EffectInfo _buffLoopEffect = new EffectInfo
	{
		subordinated = true
	};

	[SerializeField]
	private SoundInfo _buffAttachAudioClipInfo;

	[SerializeField]
	private float _buffDuration;

	[SerializeField]
	private int _maxStack;

	[SerializeField]
	private float _healAmountPerStack = 1f;

	[SerializeField]
	private Stat.Values _statPerStack;

	[SerializeField]
	private bool _healed = true;

	[SerializeField]
	private bool _overhealed;

	private Instance _instance;

	private int _stack;

	private int stack
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
