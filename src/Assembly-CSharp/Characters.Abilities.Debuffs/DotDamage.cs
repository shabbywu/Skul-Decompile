using System;
using Characters.Operations;
using Services;
using Singletons;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Characters.Abilities.Debuffs;

[Serializable]
public sealed class DotDamage : Ability
{
	public class Instance : AbilityInstance<DotDamage>
	{
		private float _remainTimeToNextTick;

		public Instance(Character owner, DotDamage ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			if ((Object)(object)ability._attacker == (Object)null)
			{
				ability._attacker = Singleton<Service>.Instance.levelManager.player;
			}
		}

		protected override void OnDetach()
		{
		}

		private void GiveDamage()
		{
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0047: Unknown result type (might be due to invalid IL or missing references)
			Target component = ((Component)owner.collider).GetComponent<Target>();
			if (!((Object)(object)component == (Object)null))
			{
				Damage damage = owner.stat.GetDamage(ability._baseDamage.value, MMMaths.RandomPointWithinBounds(((Collider2D)owner.collider).bounds), ability._hitInfo);
				bool num = ability._attacker.TryAttackCharacter(component, ref damage);
				ability._positionInfo.Attach(component, ability._onHitPoint);
				if (num)
				{
					((MonoBehaviour)ability._attacker).StartCoroutine(ability._onCharacterHit.CRun(ability._attacker, owner));
				}
			}
		}

		public override void UpdateTime(float deltaTime)
		{
			base.UpdateTime(deltaTime);
			_remainTimeToNextTick -= deltaTime;
			if (_remainTimeToNextTick < 0f)
			{
				_remainTimeToNextTick += ability._tickInterval;
				GiveDamage();
			}
		}
	}

	[SerializeField]
	private PositionInfo _positionInfo;

	[SerializeField]
	private Transform _onHitPoint;

	[SerializeField]
	[Subcomponent(typeof(TargetedOperationInfo))]
	private TargetedOperationInfo.Subcomponents _onCharacterHit;

	[SerializeField]
	private CustomFloat _baseDamage;

	[SerializeField]
	private Character _attacker;

	[SerializeField]
	private HitInfo _hitInfo;

	[SerializeField]
	private float _tickInterval;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}

	public override void Initialize()
	{
		base.Initialize();
		_onCharacterHit.Initialize();
	}
}
