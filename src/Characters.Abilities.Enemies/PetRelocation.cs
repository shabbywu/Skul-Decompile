using System;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Enemies;

[Serializable]
public sealed class PetRelocation : Ability
{
	public class Instance : AbilityInstance<PetRelocation>
	{
		private float _elapsed;

		public Instance(Character owner, PetRelocation ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			_elapsed = 0f;
		}

		protected override void OnDetach()
		{
		}

		public override void Refresh()
		{
			base.Refresh();
			_elapsed = 0f;
		}

		public override void UpdateTime(float deltaTime)
		{
			base.UpdateTime(deltaTime);
			Character target = ability._target;
			if (!((Object)(object)target == (Object)null) && !target.health.dead)
			{
				_elapsed += deltaTime;
				if (_elapsed > ability._checkInterval)
				{
					TeleportToTarget();
					_elapsed -= ability._checkInterval;
				}
			}
		}

		private void TeleportToTarget()
		{
			Collider2D lastStandingCollider = ability._target.movement.controller.collisionState.lastStandingCollider;
			if (!((Object)(object)lastStandingCollider == (Object)null))
			{
				Collider2D lastStandingCollider2 = owner.movement.controller.collisionState.lastStandingCollider;
				if (!((Object)(object)lastStandingCollider2 == (Object)null) && !((Object)(object)lastStandingCollider == (Object)(object)lastStandingCollider2))
				{
					((MonoBehaviour)owner).StartCoroutine(ability._operations.CRun(owner));
				}
			}
		}
	}

	[SerializeField]
	private float _checkInterval = 1f;

	[SerializeField]
	private Character _target;

	[SerializeField]
	[Subcomponent(typeof(OperationInfo))]
	private OperationInfo.Subcomponents _operations;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
