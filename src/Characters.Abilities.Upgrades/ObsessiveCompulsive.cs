using System;
using System.Collections.Generic;
using Characters.Operations;
using FX;
using Services;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Upgrades;

[Serializable]
public sealed class ObsessiveCompulsive : Ability
{
	public sealed class Instance : AbilityInstance<ObsessiveCompulsive>
	{
		private struct TargetHistory
		{
			public Character character;

			public int hitCount;

			public float elapsed;

			public bool expired;
		}

		[SerializeField]
		private IDictionary<Character, TargetHistory> _previousHistory;

		private IDictionary<Character, TargetHistory> _currentHistory;

		public Instance(Character owner, ObsessiveCompulsive ability)
			: base(owner, ability)
		{
		}

		public override void UpdateTime(float deltaTime)
		{
			//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
			//IL_0181: Unknown result type (might be due to invalid IL or missing references)
			//IL_0186: Unknown result type (might be due to invalid IL or missing references)
			base.UpdateTime(deltaTime);
			foreach (KeyValuePair<Character, TargetHistory> item in _previousHistory)
			{
				Character key = item.Key;
				TargetHistory value = item.Value;
				value.elapsed += deltaTime;
				if (value.hitCount >= ability._missionHitCount && !value.expired)
				{
					value.character.ability.Remove(ability._marking.ability);
					value.expired = true;
					value.elapsed = ability._markingDuration;
				}
				if (value.elapsed >= ability._markingDuration + ability._cooldownTime)
				{
					_currentHistory.Remove(key);
					PersistentSingleton<SoundManager>.Instance.PlaySound(ability._detachAudioClipInfo, ((Component)owner).transform.position);
					continue;
				}
				if (value.elapsed >= ability._markingDuration && !value.expired)
				{
					if (key.health.dead)
					{
						_currentHistory.Remove(key);
						continue;
					}
					value.expired = true;
					if (value.hitCount < ability._missionHitCount)
					{
						owner.health.TakeHealth(ability._loseHealthAmount);
						Singleton<Service>.Instance.floatingTextSpawner.SpawnPlayerTakingDamage(ability._loseHealthAmount, MMMaths.RandomPointWithinBounds(((Collider2D)owner.collider).bounds));
						((MonoBehaviour)owner).StartCoroutine(ability._onLoseHealth.CRun(owner));
					}
				}
				if (_currentHistory.ContainsKey(key))
				{
					_currentHistory[key] = value;
				}
				else
				{
					_currentHistory.Add(key, value);
				}
			}
			_previousHistory.Clear();
			foreach (KeyValuePair<Character, TargetHistory> item2 in _currentHistory)
			{
				_previousHistory.Add(item2.Key, item2.Value);
			}
		}

		private void HandleOnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
		{
			//IL_00e3: Unknown result type (might be due to invalid IL or missing references)
			Character character = target.character;
			if (!((Object)(object)character == (Object)null) && ability._attackTypeFilter[gaveDamage.attackType] && ability._motionTypeFilter[gaveDamage.motionType] && ability._targetTypeFilter[character.type])
			{
				if (_previousHistory.ContainsKey(character))
				{
					TargetHistory value = _previousHistory[character];
					value.hitCount++;
					_previousHistory[character] = value;
				}
				else
				{
					character.ability.Add(ability._marking.ability);
					_previousHistory.Add(character, new TargetHistory
					{
						character = character
					});
					PersistentSingleton<SoundManager>.Instance.PlaySound(ability._attachAudioClipInfo, ((Component)owner).transform.position);
				}
			}
		}

		protected override void OnAttach()
		{
			Character character = owner;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character.onGaveDamage, new GaveDamageDelegate(HandleOnGaveDamage));
			_previousHistory = new Dictionary<Character, TargetHistory>();
			_currentHistory = new Dictionary<Character, TargetHistory>();
		}

		protected override void OnDetach()
		{
			Character character = owner;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(character.onGaveDamage, new GaveDamageDelegate(HandleOnGaveDamage));
		}
	}

	[SerializeField]
	private SoundInfo _attachAudioClipInfo;

	[SerializeField]
	private SoundInfo _detachAudioClipInfo;

	[SerializeField]
	[Header("필터")]
	private AttackTypeBoolArray _attackTypeFilter;

	[SerializeField]
	private MotionTypeBoolArray _motionTypeFilter;

	[SerializeField]
	private CharacterTypeBoolArray _targetTypeFilter;

	[Header("설정")]
	[SerializeField]
	private int _missionHitCount;

	[SerializeField]
	private float _markingDuration;

	[SerializeField]
	private float _cooldownTime;

	[SerializeField]
	private int _loseHealthAmount;

	[SerializeField]
	[Subcomponent(typeof(OperationInfo))]
	private OperationInfo.Subcomponents _onLoseHealth;

	[SerializeField]
	[AbilityComponent.Subcomponent]
	private AbilityComponent _marking;

	public override void Initialize()
	{
		base.Initialize();
		_marking.Initialize();
		_onLoseHealth.Initialize();
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
