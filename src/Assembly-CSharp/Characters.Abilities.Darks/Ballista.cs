using System;
using System.Collections.Generic;
using Level;
using UnityEngine;

namespace Characters.Abilities.Darks;

[Serializable]
public sealed class Ballista : Ability
{
	public sealed class Instance : AbilityInstance<Ballista>
	{
		private float _remainCooldownTime;

		private List<Character> _summons;

		public Instance(Character owner, Ballista ability)
			: base(owner, ability)
		{
		}

		public override void UpdateTime(float deltaTime)
		{
			base.UpdateTime(deltaTime);
			if (_summons.Count < ability._maxCount)
			{
				_remainCooldownTime -= deltaTime;
				if (_remainCooldownTime <= 0f)
				{
					TryToSummon();
				}
			}
		}

		private void TryToSummon()
		{
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			Character summoned;
			if (_summons.Count <= ability._maxCount)
			{
				summoned = Object.Instantiate<Character>(ability._toSummonPrefab, Vector2.op_Implicit(GetSummonPoint()), Quaternion.identity);
				Map.Instance.waveContainer.Attach(summoned);
				_summons.Add(summoned);
				summoned.ForceToLookAt(owner.lookingDirection);
				summoned.health.onDiedTryCatch += HandleSummonDied;
				_remainCooldownTime = ability._summonCooldowntime;
			}
			void HandleSummonDied()
			{
				summoned.health.onDiedTryCatch -= HandleSummonDied;
				_summons.Remove(summoned);
			}
		}

		private Vector2 GetSummonPoint()
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0075: Unknown result type (might be due to invalid IL or missing references)
			//IL_007a: Unknown result type (might be due to invalid IL or missing references)
			//IL_007d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0087: Unknown result type (might be due to invalid IL or missing references)
			if (!owner.movement.TryGetClosestBelowCollider(out var collider, LayerMask.op_Implicit(8)))
			{
				return Vector2.op_Implicit(((Component)owner).transform.position);
			}
			float x = ((Component)owner).transform.position.x;
			float num = ((owner.lookingDirection == Character.LookingDirection.Right) ? (x + ability._summonDistance) : (x - ability._summonDistance));
			Bounds bounds = collider.bounds;
			return new Vector2(num, ((Bounds)(ref bounds)).max.y);
		}

		protected override void OnAttach()
		{
			_summons = new List<Character>(ability._maxCount);
		}

		protected override void OnDetach()
		{
			for (int num = _summons.Count - 1; num >= 0; num--)
			{
				_summons[num].health.Kill();
			}
		}
	}

	[SerializeField]
	private Character _toSummonPrefab;

	[SerializeField]
	private float _summonCooldowntime;

	[SerializeField]
	private float _summonDistance;

	[SerializeField]
	private int _maxCount;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
