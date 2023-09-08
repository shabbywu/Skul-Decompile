using System;
using System.Collections.Generic;
using Characters.Operations;
using FX;
using Level;
using Level.Objects;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Darks;

[Serializable]
public sealed class Recharge : Ability
{
	public sealed class Instance : AbilityInstance<Recharge>
	{
		private class Summons
		{
			public Character character;

			public float remainTime;
		}

		private float _remainCooldownTime;

		private List<Summons> _summons;

		public Instance(Character owner, Recharge ability)
			: base(owner, ability)
		{
			_summons = new List<Summons>();
		}

		public override void UpdateTime(float deltaTime)
		{
			base.UpdateTime(deltaTime);
			for (int num = _summons.Count - 1; num >= 0; num--)
			{
				Summons summons = _summons[num];
				summons.remainTime -= deltaTime;
				if (summons.remainTime <= 0f)
				{
					KillSummons(summons.character);
					_summons.RemoveAt(num);
				}
			}
			_remainCooldownTime -= deltaTime;
			if (_remainCooldownTime <= 0f)
			{
				TryToSummon();
			}
		}

		private void KillSummons(Character summon)
		{
			if ((Object)(object)summon != (Object)null && !summon.health.dead && ((Component)summon).gameObject.activeInHierarchy)
			{
				owner.health.PercentHeal(ability._healPercent);
				((MonoBehaviour)owner).StartCoroutine(ability._onHealed.CRun(owner));
				summon.health.Kill();
			}
		}

		private void TryToSummon()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
			Character character = Object.Instantiate<Character>(ability._toSummonPrefab, Vector2.op_Implicit(GetSummonPoint()), Quaternion.identity);
			DivineShieldEffect componentInChildren = ((Component)character).GetComponentInChildren<DivineShieldEffect>();
			if ((Object)(object)componentInChildren != (Object)null)
			{
				componentInChildren.Activate(owner);
			}
			_summons.Add(new Summons
			{
				character = character,
				remainTime = ability._summonLifeTime
			});
			Map.Instance.waveContainer.Attach(character);
			_remainCooldownTime = ability._summonCooldowntime;
			PersistentSingleton<SoundManager>.Instance.PlaySound(ability._summonSound, ((Component)owner).transform.position);
		}

		private Vector2 GetSummonPoint()
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0035: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_008b: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			if (!owner.movement.TryGetClosestBelowCollider(out var collider, Layers.footholdMask))
			{
				return Vector2.op_Implicit(((Component)owner).transform.position);
			}
			Bounds bounds = collider.bounds;
			float num = Mathf.Max(((Bounds)(ref bounds)).min.x, ((Component)owner).transform.position.x - ability._summonTargetMaxDistance);
			bounds = collider.bounds;
			float num2 = Mathf.Min(((Bounds)(ref bounds)).max.x, ((Component)owner).transform.position.x + ability._summonTargetMaxDistance);
			float num3 = Random.Range(num, num2);
			bounds = collider.bounds;
			return new Vector2(num3, ((Bounds)(ref bounds)).max.y);
		}

		protected override void OnAttach()
		{
			_summons.Clear();
		}

		protected override void OnDetach()
		{
			for (int num = _summons.Count - 1; num >= 0; num--)
			{
				Summons summons = _summons[num];
				KillSummons(summons.character);
			}
			_summons.Clear();
		}
	}

	[SerializeField]
	[Subcomponent(typeof(OperationInfo))]
	private OperationInfo.Subcomponents _onHealed;

	[SerializeField]
	private Character _toSummonPrefab;

	[SerializeField]
	private float _summonCooldowntime;

	[SerializeField]
	private float _summonLifeTime;

	[SerializeField]
	private float _summonTargetMaxDistance;

	[SerializeField]
	private SoundInfo _summonSound;

	[SerializeField]
	[Range(0f, 1f)]
	private float _healPercent;

	public override void Initialize()
	{
		base.Initialize();
		_onHealed.Initialize();
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
