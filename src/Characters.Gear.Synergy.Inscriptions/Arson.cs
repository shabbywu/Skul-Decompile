using System;
using System.Collections;
using Characters.Abilities;
using Characters.Abilities.Upgrades;
using Characters.Operations;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions;

public class Arson : SimpleStatBonusKeyword
{
	[Serializable]
	private class Deathrattle
	{
		[SerializeField]
		[Space]
		private Transform _range;

		[SerializeField]
		private float _explosionDelay;

		[SerializeField]
		private float _cooldownTime;

		private WaitForSeconds _waitForExplosionDelay;

		[Space]
		[SerializeField]
		[CharacterOperation.Subcomponent]
		private CharacterOperation.Subcomponents _operation;

		private float _remainCooldownTime;

		private Arson _arson;

		public void Initialize(Arson volcano)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0018: Expected O, but got Unknown
			_arson = volcano;
			_waitForExplosionDelay = new WaitForSeconds(_explosionDelay);
			_operation.Initialize();
		}

		public void Attach()
		{
			if ((Object)(object)_arson != (Object)null)
			{
				Character character = _arson.character;
				character.onKilled = (Character.OnKilledDelegate)Delegate.Combine(character.onKilled, new Character.OnKilledDelegate(OnKilled));
				_arson.character.status.Register(CharacterStatus.Kind.Burn, CharacterStatus.Timing.Refresh, HandleOnReleaseBurn);
				_arson.character.status.Register(CharacterStatus.Kind.Burn, CharacterStatus.Timing.Release, HandleOnReleaseBurn);
			}
		}

		public void UpdateTime(float deltaTime)
		{
			_remainCooldownTime -= deltaTime;
		}

		public void Detach()
		{
			if ((Object)(object)_arson != (Object)null)
			{
				Character character = _arson.character;
				character.onKilled = (Character.OnKilledDelegate)Delegate.Remove(character.onKilled, new Character.OnKilledDelegate(OnKilled));
				_arson.character.status.Unregister(CharacterStatus.Kind.Burn, CharacterStatus.Timing.Refresh, HandleOnReleaseBurn);
				_arson.character.status.Unregister(CharacterStatus.Kind.Burn, CharacterStatus.Timing.Release, HandleOnReleaseBurn);
			}
		}

		private void HandleOnReleaseBurn(Character attacker, Character target)
		{
			if (!(_remainCooldownTime > 0f) && !((Object)(object)target == (Object)null) && !((Object)(object)target.status == (Object)null) && !target.health.dead)
			{
				_remainCooldownTime = _cooldownTime;
				((MonoBehaviour)_arson).StartCoroutine(CSwampExplode(target));
			}
		}

		private void OnKilled(ITarget target, ref Damage damage)
		{
			if (!(_remainCooldownTime > 0f) && target != null && !((Object)(object)target.character == (Object)null) && !((Object)(object)target.character.status == (Object)null) && target.character.status.burning)
			{
				_remainCooldownTime = _cooldownTime;
				((MonoBehaviour)_arson).StartCoroutine(CSwampExplode(target.character));
			}
		}

		private IEnumerator CSwampExplode(Character target)
		{
			Vector3 position = ((Component)target).transform.position;
			Vector2 offset = ((Collider2D)target.collider).offset;
			position.x += offset.x;
			position.y += offset.y;
			yield return _waitForExplosionDelay;
			((Component)_range).transform.position = position;
			_operation.Run(_arson.character);
		}
	}

	[SerializeField]
	private double[] _statBonusByLevel;

	[SerializeField]
	[Header("1 Step 효과")]
	private OperationByTriggerComponent _step1Ability;

	[SerializeField]
	[Header("4 Step 효과")]
	private Deathrattle _deathrattle;

	protected override double[] statBonusByStep => _statBonusByLevel;

	protected override Stat.Category statCategory => Stat.Category.Percent;

	protected override Stat.Kind statKind => Stat.Kind.EmberDamage;

	protected override void Initialize()
	{
		base.Initialize();
		_deathrattle.Initialize(this);
		_step1Ability.Initialize();
	}

	public override void UpdateBonus(bool wasActive, bool wasOmen)
	{
		UpdateStat();
		if (keyword.step >= 1)
		{
			if (!base.character.ability.Contains(_step1Ability.ability) && base.character.ability.GetInstance<KettleOfSwampWitch>() == null)
			{
				base.character.ability.Add(_step1Ability.ability);
			}
		}
		else
		{
			base.character.ability.Remove(_step1Ability.ability);
		}
		if (keyword.isMaxStep)
		{
			_deathrattle.Attach();
		}
		else
		{
			_deathrattle.Detach();
		}
	}

	private void Update()
	{
		if (keyword.isMaxStep)
		{
			_deathrattle.UpdateTime(((ChronometerBase)base.character.chronometer.master).deltaTime);
		}
	}

	public override void Detach()
	{
		base.character.ability.Remove(_step1Ability.ability);
		_deathrattle.Detach();
	}
}
