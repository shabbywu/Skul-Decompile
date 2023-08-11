using System;
using Characters.Abilities;
using FX;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions;

public sealed class Execution : InscriptionInstance
{
	[Serializable]
	internal sealed class Debuff : Ability
	{
		[Serializable]
		internal class StateInfo
		{
			[Range(0f, 1f)]
			public float healthThreshold;

			[Range(0f, 100f)]
			public float damageMultiplier;

			[Tooltip("처형 조건이 충족될 때 지속적으로 표시될 이펙트")]
			[SerializeField]
			public EffectInfo markEffect = new EffectInfo
			{
				subordinated = true
			};

			[Tooltip("처형 조건이 충족될 때 한 번 표시될 이펙트")]
			[SerializeField]
			public EffectInfo activatingEffect;
		}

		internal abstract class State
		{
			protected Character _owner;

			protected Debuff _ability;

			protected StateInfo _stateInfo;

			public State(Character owner, Debuff ability, StateInfo stateInfo)
			{
				_owner = owner;
				_ability = ability;
				_stateInfo = stateInfo;
			}

			public virtual bool CheckEnter()
			{
				return _owner.health.percent < (double)_stateInfo.healthThreshold;
			}

			public abstract void Enter();

			public abstract void OnTakeDamage(ref Damage damage);

			public abstract void Exit();
		}

		[Serializable]
		private class Step0 : State
		{
			public Step0(Character owner, Debuff ability, StateInfo stateInfo)
				: base(owner, ability, stateInfo)
			{
			}

			public override void Enter()
			{
			}

			public override void Exit()
			{
			}

			public override void OnTakeDamage(ref Damage damage)
			{
			}
		}

		[Serializable]
		private class Step1 : State
		{
			private EffectPoolInstance _markEffectInstance;

			public Step1(Character owner, Debuff ability, StateInfo stateInfo)
				: base(owner, ability, stateInfo)
			{
			}

			public override void Enter()
			{
				//IL_0016: Unknown result type (might be due to invalid IL or missing references)
				//IL_0048: Unknown result type (might be due to invalid IL or missing references)
				_stateInfo.activatingEffect.Spawn(((Component)_owner).transform.position, _owner);
				_markEffectInstance = _stateInfo.markEffect.Spawn(((Component)_owner).transform.position, _owner);
			}

			public override void Exit()
			{
				if ((Object)(object)_markEffectInstance != (Object)null)
				{
					_markEffectInstance.Stop();
					_markEffectInstance = null;
				}
			}

			public override void OnTakeDamage(ref Damage damage)
			{
				damage.percentMultiplier *= 1.0 + (double)_stateInfo.damageMultiplier * 0.01;
			}
		}

		[Serializable]
		private class Step2 : State
		{
			private EffectPoolInstance _markEffectInstance;

			public Step2(Character owner, Debuff ability, StateInfo stateInfo)
				: base(owner, ability, stateInfo)
			{
			}

			public override void Enter()
			{
				//IL_0017: Unknown result type (might be due to invalid IL or missing references)
				_markEffectInstance = _stateInfo.markEffect.Spawn(((Component)_owner).transform.position, _owner);
			}

			public override void Exit()
			{
				if ((Object)(object)_markEffectInstance != (Object)null)
				{
					_markEffectInstance.Stop();
					_markEffectInstance = null;
				}
			}

			public override void OnTakeDamage(ref Damage damage)
			{
				damage.percentMultiplier *= 1.0 + (double)_stateInfo.damageMultiplier * 0.01;
				damage.SetGuaranteedCritical(int.MaxValue, critical: true);
				damage.Evaluate(immuneToCritical: false);
			}
		}

		public class Instance : AbilityInstance<Debuff>
		{
			private State _step0;

			private State _step1;

			private State _step2;

			private State _currentState;

			public Instance(Character owner, Debuff ability, State step0, State step1, State step2)
				: base(owner, ability)
			{
				_step0 = step0;
				_step1 = step1;
				_step2 = step2;
			}

			protected override void OnAttach()
			{
				owner.health.onTakeDamage.Add(int.MinValue, (TakeDamageDelegate)OnTakeDamage);
				owner.health.onChanged += OnHealthChanged;
				_currentState = _step0;
				OnHealthChanged();
			}

			protected override void OnDetach()
			{
				owner.health.onTakeDamage.Remove((TakeDamageDelegate)OnTakeDamage);
				owner.health.onChanged -= OnHealthChanged;
				_currentState.Exit();
			}

			private bool OnTakeDamage(ref Damage damage)
			{
				_currentState.OnTakeDamage(ref damage);
				return false;
			}

			private void OnHealthChanged()
			{
				if (_step2.CheckEnter() && ability.execution.keyword.isMaxStep)
				{
					if (!_currentState.Equals(_step2))
					{
						_currentState.Exit();
						_currentState = _step2;
						_currentState.Enter();
					}
				}
				else if (_step1.CheckEnter() && ability.execution.keyword.step >= 1)
				{
					if (!_currentState.Equals(_step1))
					{
						_currentState.Exit();
						_currentState = _step1;
						_currentState.Enter();
					}
				}
				else if (_step0.CheckEnter() && !_currentState.Equals(_step0))
				{
					_currentState.Exit();
					_currentState = _step0;
					_currentState.Enter();
				}
			}
		}

		[SerializeField]
		private StateInfo _step0;

		[SerializeField]
		private StateInfo _step1;

		[SerializeField]
		private StateInfo _step2;

		public InscriptionInstance execution { get; set; }

		public override IAbilityInstance CreateInstance(Character owner)
		{
			Step0 step = new Step0(owner, this, _step0);
			Step1 step2 = new Step1(owner, this, _step1);
			Step2 step3 = new Step2(owner, this, _step2);
			return new Instance(owner, this, step, step2, step3);
		}
	}

	[SerializeField]
	private Debuff _debuff;

	protected override void Initialize()
	{
		_debuff.Initialize();
		_debuff.execution = this;
	}

	public override void UpdateBonus(bool wasActive, bool wasOmen)
	{
	}

	public override void Attach()
	{
		((PriorityList<GiveDamageDelegate>)base.character.onGiveDamage).Add(int.MinValue, (GiveDamageDelegate)GiveDamageDelegate);
	}

	public override void Detach()
	{
		((PriorityList<GiveDamageDelegate>)base.character.onGiveDamage).Remove((GiveDamageDelegate)GiveDamageDelegate);
	}

	private bool GiveDamageDelegate(ITarget target, ref Damage damage)
	{
		if (keyword.step < 1)
		{
			return false;
		}
		if ((Object)(object)target.character == (Object)null)
		{
			return false;
		}
		if (TargetLayer.IsPlayer(((Component)target.character).gameObject.layer))
		{
			return false;
		}
		if (target.character.ability.Contains(_debuff))
		{
			return false;
		}
		target.character.ability.Add(_debuff);
		return false;
	}
}
