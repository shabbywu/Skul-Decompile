using System;
using Characters.Abilities;
using Characters.Operations;
using Characters.Utils;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Characters.Gear.Synergy.Inscriptions;

public sealed class Brawl : InscriptionInstance
{
	[Serializable]
	private class Instance : IAbility, IAbilityInstance
	{
		[Header("공통")]
		[SerializeField]
		private CharacterTypeBoolArray _hitTargetTypeFilter;

		[SerializeField]
		private AttackTypeBoolArray _attackTypeFilter;

		[SerializeField]
		private MotionTypeBoolArray _motionTypeFilter;

		[SerializeField]
		[Header("2세트 효과")]
		private AttackDamage _attackDamage;

		[SerializeField]
		private float _minimumCooldownTime;

		[SerializeField]
		private float _hitIntervalPerUnit;

		[SerializeField]
		private int _maxHitPerUnit;

		[SerializeField]
		private string _attackKey;

		[SerializeField]
		[Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _operations;

		private StackHistoryManager<Character> _historyManager;

		[Header("4세트 효과")]
		[SerializeField]
		private Sprite _icon;

		[SerializeField]
		private int _cycle;

		[SerializeField]
		private PositionInfo _positionInfo;

		[SerializeField]
		private Transform _targetPoint;

		[SerializeField]
		[Subcomponent(typeof(OperationInfo))]
		private OperationInfo.Subcomponents _enhanceOperations;

		private int _attackCount;

		private readonly int _historyCapacity = 128;

		internal InscriptionInstance inscriptionInstance;

		private Character _owner;

		public float duration { get; set; }

		public int iconPriority => 0;

		public bool removeOnSwapWeapon => false;

		public Character owner => _owner;

		public IAbility ability => this;

		public float remainTime { get; set; }

		public bool attached => true;

		public Sprite icon
		{
			get
			{
				if (!inscriptionInstance.keyword.isMaxStep)
				{
					return null;
				}
				return _icon;
			}
		}

		public float iconFillAmount => 0f;

		public bool iconFillInversed => false;

		public bool iconFillFlipped => false;

		public int iconStacks => _attackCount;

		public bool expired => false;

		public void Attach()
		{
			Character character = owner;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character.onGaveDamage, new GaveDamageDelegate(HandleOnGaveDamage));
		}

		public IAbilityInstance CreateInstance(Character owner)
		{
			_owner = owner;
			_operations.Initialize();
			_enhanceOperations.Initialize();
			_historyManager = new StackHistoryManager<Character>(_historyCapacity);
			return this;
		}

		public void Detach()
		{
			if (!((Object)(object)owner == (Object)null))
			{
				Character character = owner;
				character.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(character.onGaveDamage, new GaveDamageDelegate(HandleOnGaveDamage));
			}
		}

		public void Initialize()
		{
		}

		public void Refresh()
		{
		}

		public void UpdateTime(float deltaTime)
		{
		}

		private void HandleOnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
		{
			if (!((EnumArray<Damage.AttackType, bool>)_attackTypeFilter)[gaveDamage.attackType] || !((EnumArray<Damage.MotionType, bool>)_motionTypeFilter)[gaveDamage.motionType] || !((EnumArray<Character.Type, bool>)_hitTargetTypeFilter)[target.character.type] || gaveDamage.key.Equals(_attackKey, StringComparison.OrdinalIgnoreCase) || inscriptionInstance.keyword.step < 1)
			{
				return;
			}
			Character character = target.character;
			_historyManager.ClearIfExpired();
			if (_historyManager.IsElapsedFromLastTime(character, _minimumCooldownTime, defaultResult: true) && _historyManager.TryAddStack(character, 1, _maxHitPerUnit, _hitIntervalPerUnit))
			{
				if (inscriptionInstance.keyword.isMaxStep)
				{
					_attackCount++;
				}
				Attack(target);
			}
		}

		private void Attack(ITarget target)
		{
			_positionInfo.Attach(target, _targetPoint);
			if (_attackCount >= _cycle && inscriptionInstance.keyword.isMaxStep)
			{
				_attackCount = 0;
				((MonoBehaviour)owner).StartCoroutine(_enhanceOperations.CRun(owner));
			}
			else
			{
				((MonoBehaviour)owner).StartCoroutine(_operations.CRun(owner));
			}
		}
	}

	[SerializeField]
	private Instance _ability;

	protected override void Initialize()
	{
	}

	public override void UpdateBonus(bool wasActive, bool wasOmen)
	{
	}

	public override void Attach()
	{
		_ability.inscriptionInstance = this;
		base.character.ability.Add(_ability);
	}

	public override void Detach()
	{
		base.character.ability.Remove(_ability);
	}
}
