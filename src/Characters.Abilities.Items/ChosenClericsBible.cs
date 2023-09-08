using System;
using Characters.Operations;
using Level;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Items;

[Serializable]
public sealed class ChosenClericsBible : Ability
{
	public sealed class Instance : AbilityInstance<ChosenClericsBible>
	{
		private static readonly int _activateHash = Animator.StringToHash("Start");

		private int _remainKillCount;

		private float _remainBuffTime;

		private bool _buffAttached;

		private float _remainAttackTime;

		private CoroutineReference _cOnAttachBuffOperations;

		public override Sprite icon
		{
			get
			{
				if (!_buffAttached)
				{
					return base.icon;
				}
				return null;
			}
		}

		public override int iconStacks => ability._killCount - _remainKillCount;

		public Instance(Character owner, ChosenClericsBible ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			((Component)ability._animator).transform.SetParent(owner.attachWithFlip.transform);
			_remainKillCount = ability._killCount;
			Character character = owner;
			character.onKilled = (Character.OnKilledDelegate)Delegate.Combine(character.onKilled, new Character.OnKilledDelegate(HandleOnKilled));
			owner.health.onHealed += HandleOnHealed;
		}

		public override void UpdateTime(float deltaTime)
		{
			base.UpdateTime(deltaTime);
			_remainBuffTime -= deltaTime;
			if (_remainBuffTime <= 0f && _buffAttached)
			{
				DetachBuff();
			}
			UpdateAttachedTime(deltaTime);
		}

		private void UpdateAttachedTime(float deltatime)
		{
			if (_buffAttached)
			{
				_remainAttackTime -= deltatime;
				if (!(_remainAttackTime > 0f))
				{
					_remainAttackTime = ability._onAttachBuffAttackInterval;
					_cOnAttachBuffOperations.Stop();
					_cOnAttachBuffOperations = ((MonoBehaviour)(object)owner).StartCoroutineWithReference(ability._onAttachBuff.CRun(owner));
				}
			}
		}

		private void HandleOnKilled(ITarget target, ref Damage damage)
		{
			Character character = target.character;
			if (!((Object)(object)character == (Object)null) && ability._targetFilter[character.type])
			{
				_remainKillCount--;
				if (_remainKillCount <= 0)
				{
					_remainKillCount = ability._killCount;
					((MonoBehaviour)owner).StartCoroutine(ability._onHealed.CRun(owner));
					owner.health.Heal(ability._healAmount);
				}
			}
		}

		private void AttachBuff()
		{
			if (!_buffAttached)
			{
				((Component)ability._animator).gameObject.SetActive(true);
				ability._animator.Play(_activateHash);
				owner.stat.AttachOrUpdateValues(ability._buff);
				_remainAttackTime = ability._onAttachBuffAttackInterval;
			}
			_remainBuffTime = ability._buffDuration;
			_buffAttached = true;
		}

		private void DetachBuff()
		{
			((Component)ability._animator).gameObject.SetActive(false);
			owner.stat.DetachValues(ability._buff);
			((MonoBehaviour)owner).StartCoroutine(ability._onDetachBuff.CRun(owner));
			_buffAttached = false;
		}

		private void HandleOnHealed(double healed, double overHealed)
		{
			AttachBuff();
		}

		protected override void OnDetach()
		{
			Character character = owner;
			character.onKilled = (Character.OnKilledDelegate)Delegate.Remove(character.onKilled, new Character.OnKilledDelegate(HandleOnKilled));
			owner.health.onHealed -= HandleOnHealed;
			if ((Object)(object)ability._wingParent == (Object)null)
			{
				((Component)ability._animator).transform.SetParent(((Component)Map.Instance).transform);
			}
			else
			{
				((Component)ability._animator).transform.SetParent(owner.attachWithFlip.transform);
			}
			DetachBuff();
		}
	}

	[SerializeField]
	private Transform _wingParent;

	[SerializeField]
	private Animator _animator;

	[Header("회복 필요 조건")]
	[SerializeField]
	private CharacterTypeBoolArray _targetFilter;

	[SerializeField]
	private int _killCount;

	[SerializeField]
	private float _healAmount;

	[SerializeField]
	[Header("회복 시")]
	[Subcomponent(typeof(OperationInfo))]
	private OperationInfo.Subcomponents _onHealed;

	[Subcomponent(typeof(OperationInfo))]
	[SerializeField]
	private OperationInfo.Subcomponents _onAttachBuff;

	[Subcomponent(typeof(OperationInfo))]
	[SerializeField]
	private OperationInfo.Subcomponents _onDetachBuff;

	[SerializeField]
	private Stat.Values _buff;

	[SerializeField]
	private float _buffDuration;

	[SerializeField]
	private float _onAttachBuffAttackInterval = 1f;

	public override void Initialize()
	{
		base.Initialize();
		_onHealed.Initialize();
		_onAttachBuff.Initialize();
		_onDetachBuff.Initialize();
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
