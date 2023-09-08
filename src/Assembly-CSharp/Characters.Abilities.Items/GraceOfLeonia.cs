using System;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Items;

[Serializable]
public sealed class GraceOfLeonia : Ability
{
	public sealed class Instance : AbilityInstance<GraceOfLeonia>
	{
		private Characters.Shield.Instance _shieldInstance;

		private float _elapsed;

		public Instance(Character owner, GraceOfLeonia ability)
			: base(owner, ability)
		{
		}

		public override void UpdateTime(float deltaTime)
		{
			base.UpdateTime(deltaTime);
			_elapsed -= deltaTime;
			if (!(_elapsed > 0f))
			{
				_elapsed = ability._operationInterval;
				((MonoBehaviour)owner).StartCoroutine(ability._operations.CRun(owner));
			}
		}

		public override void Refresh()
		{
			base.Refresh();
			if (_shieldInstance != null)
			{
				_shieldInstance.amount = ability._amount;
			}
		}

		private void OnShieldBroke()
		{
			ability.onBroke?.Invoke(_shieldInstance);
			owner.ability.Remove(this);
		}

		protected override void OnAttach()
		{
			owner.stat.AttachOrUpdateValues(ability._stats);
			if (ability._healthType == HealthType.Constant)
			{
				_shieldInstance = owner.health.shield.Add(ability, ability._amount, OnShieldBroke);
			}
			else
			{
				_shieldInstance = owner.health.shield.Add(ability, (float)(owner.health.maximumHealth * (double)ability._amount * 0.009999999776482582), OnShieldBroke);
			}
		}

		protected override void OnDetach()
		{
			owner.stat.DetachValues(ability._stats);
			ability.onDetach?.Invoke(_shieldInstance);
			if (owner.health.shield.Remove(ability))
			{
				_shieldInstance = null;
			}
		}
	}

	public enum HealthType
	{
		Constant,
		Percent
	}

	[SerializeField]
	private float _operationInterval;

	[SerializeField]
	[Subcomponent(typeof(OperationInfo))]
	private OperationInfo.Subcomponents _operations;

	[SerializeField]
	private Stat.Values _stats;

	[SerializeField]
	private float _amount;

	[SerializeField]
	private HealthType _healthType;

	public float amount
	{
		get
		{
			return _amount;
		}
		set
		{
			_amount = value;
		}
	}

	public event Action<Characters.Shield.Instance> onBroke;

	public event Action<Characters.Shield.Instance> onDetach;

	public GraceOfLeonia()
	{
	}

	public GraceOfLeonia(float amount)
	{
		_amount = amount;
	}

	public override void Initialize()
	{
		base.Initialize();
		_operations.Initialize();
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
