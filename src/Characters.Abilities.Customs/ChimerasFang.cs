using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Operations;
using UnityEngine;

namespace Characters.Abilities.Customs;

[Serializable]
public class ChimerasFang : Ability
{
	public class Instance : AbilityInstance<ChimerasFang>
	{
		private HashSet<Character> _targets;

		public Instance(Character owner, ChimerasFang ability)
			: base(owner, ability)
		{
			_targets = new HashSet<Character>();
		}

		protected override void OnAttach()
		{
			Character character = owner;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character.onGaveDamage, new GaveDamageDelegate(OnOwnerGaveDamage));
		}

		protected override void OnDetach()
		{
			Character character = owner;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(character.onGaveDamage, new GaveDamageDelegate(OnOwnerGaveDamage));
		}

		private void OnOwnerGaveDamage(ITarget target, in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			//IL_010e: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)target.character == (Object)null || target.character.health.dead || !((Component)target.transform).gameObject.activeSelf || (Object)(object)target.character.status == (Object)null || !target.character.status.poisoned || _targets.Contains(target.character))
			{
				return;
			}
			if (ability._normalTypes[target.character.type])
			{
				if (target.character.health.percent > (double)ability._healthPercent * 0.01)
				{
					return;
				}
			}
			else if (!ability._bossTypes[target.character.type] || target.character.health.percent > (double)ability._healthPercentForBoss * 0.01)
			{
				return;
			}
			ability._operationPosition.position = target.transform.position;
			ability._operations.Run(target.character);
			target.character.chronometer.animation.AttachTimeScale(this, ability._timeScaleDuringKilling);
			((MonoBehaviour)target.character).StartCoroutine(CDelayedKill(target.character));
			_targets.Add(target.character);
		}

		private IEnumerator CDelayedKill(Character target)
		{
			yield return Chronometer.global.WaitForSeconds(ability._killingDelay);
			target.chronometer.animation.DetachTimeScale(this);
			if (target.health.dead)
			{
				_targets.Remove(target);
				yield break;
			}
			Damage damage = owner.stat.GetDamage(target.health.currentHealth, MMMaths.RandomPointWithinBounds(((Collider2D)target.collider).bounds), ability._hitInfo);
			owner.Attack(target, ref damage);
			if (!target.health.dead && target.key != Key.Yggdrasil)
			{
				target.health.Kill();
			}
			_targets.Remove(target);
		}
	}

	[SerializeField]
	private HitInfo _hitInfo;

	[SerializeField]
	private CharacterTypeBoolArray _normalTypes;

	[SerializeField]
	private int _healthPercent = 10;

	[SerializeField]
	private CharacterTypeBoolArray _bossTypes;

	[SerializeField]
	private int _healthPercentForBoss = 5;

	[SerializeField]
	private float _timeScaleDuringKilling = 0.3f;

	[SerializeField]
	private float _killingDelay = 1f;

	[Space]
	[SerializeField]
	private Transform _operationPosition;

	[SerializeField]
	[CharacterOperation.Subcomponent]
	private CharacterOperation.Subcomponents _operations;

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
