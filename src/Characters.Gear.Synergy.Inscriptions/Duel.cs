using System;
using System.Collections;
using Characters.Abilities;
using FX;
using Singletons;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions;

public sealed class Duel : InscriptionInstance
{
	private class StatBonus : IAbility, IAbilityInstance
	{
		[NonSerialized]
		public Stat.Values statPerStack = new Stat.Values(new Stat.Value(Stat.Category.Percent, Stat.Kind.TakingDamage, 0.0));

		[NonSerialized]
		public int maxStack;

		private Stat.Values _stat = new Stat.Values(new Stat.Value(Stat.Category.Percent, Stat.Kind.TakingDamage, 1.0));

		private Character _owner;

		private int _stacks;

		private readonly EffectInfo _effect;

		private EffectPoolInstance _effectInstance;

		public Character owner => _owner;

		public IAbility ability => this;

		public float remainTime { get; set; }

		public bool attached { get; private set; }

		public Sprite icon { get; set; }

		public float iconFillAmount => 0f;

		public bool iconFillInversed => false;

		public bool iconFillFlipped => false;

		public int iconStacks => 0;

		public bool expired => false;

		public float duration { get; set; }

		public int iconPriority => 0;

		public bool removeOnSwapWeapon => false;

		public StatBonus(EffectInfo effect)
		{
			_effect = effect;
		}

		public IAbilityInstance CreateInstance(Character owner)
		{
			_owner = owner;
			return this;
		}

		public void Initialize()
		{
		}

		public void UpdateTime(float deltaTime)
		{
		}

		public void Refresh()
		{
			AddStack();
		}

		void IAbilityInstance.Attach()
		{
			//IL_0021: Unknown result type (might be due to invalid IL or missing references)
			attached = true;
			_effectInstance = ((_effect == null) ? null : _effect.Spawn(((Component)owner).transform.position, owner));
			_stacks = 1;
			_owner.stat.AttachValues(_stat);
			UpdateStack();
			((MonoBehaviour)_owner).StartCoroutine(CUpdateAnimation());
		}

		void IAbilityInstance.Detach()
		{
			attached = false;
			if ((Object)(object)_effectInstance != (Object)null)
			{
				_effectInstance.Stop();
				_effectInstance = null;
			}
			_stacks = 0;
			_owner.stat.DetachValues(_stat);
			((MonoBehaviour)_owner).StopCoroutine(CUpdateAnimation());
		}

		public void AddStack()
		{
			if (_stacks != maxStack)
			{
				_stacks++;
				_effectInstance.animator.SetInteger("Stacks", _stacks);
				UpdateStack();
			}
		}

		private void UpdateStack()
		{
			for (int i = 0; i < ((ReorderableArray<Stat.Value>)_stat).values.Length; i++)
			{
				((ReorderableArray<Stat.Value>)_stat).values[i].value = ((ReorderableArray<Stat.Value>)statPerStack).values[i].GetStackedValue(_stacks);
			}
			_owner.stat.SetNeedUpdate();
		}

		private IEnumerator CUpdateAnimation()
		{
			bool update = false;
			while (true)
			{
				yield return null;
				if (update)
				{
					_effectInstance.animator.SetInteger("Stacks", _stacks);
					update = false;
				}
				if (!_owner.attach.gameObject.activeSelf)
				{
					update = true;
				}
			}
		}
	}

	[SerializeField]
	[Header("2세트 효과")]
	private TargetLayer _targetLayer;

	[SerializeField]
	private CharacterTypeBoolArray _characterTypes;

	[Tooltip("혹시 단계별 이펙트를 바꾸고 싶을 경우 Duel 애니메이터의 Transition과 Parameter를 수정하면 됨")]
	[SerializeField]
	private EffectInfo _effect = new EffectInfo
	{
		subordinated = true
	};

	[SerializeField]
	private SoundInfo _attachSound;

	[Range(0f, 100f)]
	[SerializeField]
	private float _statValuePerStack;

	[SerializeField]
	private int[] _maxStackByStep;

	private StatBonus _currentInstance;

	protected override void Initialize()
	{
		_currentInstance = new StatBonus(_effect);
		((ReorderableArray<Stat.Value>)_currentInstance.statPerStack).values[0].value = 1.0 + (double)_statValuePerStack * 0.01;
	}

	public override void UpdateBonus(bool wasActive, bool wasOmen)
	{
		_currentInstance.maxStack = _maxStackByStep[keyword.step];
	}

	public override void Attach()
	{
		Character obj = base.character;
		obj.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(obj.onGaveDamage, new GaveDamageDelegate(OnGaveDamage));
	}

	public override void Detach()
	{
		Character obj = base.character;
		obj.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(obj.onGaveDamage, new GaveDamageDelegate(OnGaveDamage));
	}

	private void OnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f5: Unknown result type (might be due to invalid IL or missing references)
		if (keyword.step < 1 || (Object)(object)target.character == (Object)null || gaveDamage.attackType == Damage.AttackType.None || gaveDamage.attackType == Damage.AttackType.Additional || !((EnumArray<Character.Type, bool>)_characterTypes)[target.character.type] || !ExtensionMethods.Contains(_targetLayer.Evaluate(((Component)base.character).gameObject), ((Component)target.character).gameObject.layer))
		{
			return;
		}
		if (_currentInstance.attached && (Object)(object)_currentInstance.owner != (Object)null && _currentInstance.owner.liveAndActive)
		{
			if ((Object)(object)_currentInstance.owner == (Object)(object)target.character)
			{
				_currentInstance.AddStack();
			}
		}
		else
		{
			target.character.ability.Add(_currentInstance);
			PersistentSingleton<SoundManager>.Instance.PlaySound(_attachSound, target.transform.position);
		}
	}
}
