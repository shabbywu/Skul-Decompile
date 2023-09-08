using System;
using Characters.Movements;
using UnityEngine;

namespace Characters.Abilities.Customs;

[Serializable]
public class HorizontalBouncy : Ability
{
	public class Instance : AbilityInstance<HorizontalBouncy>
	{
		public Instance(Character owner, HorizontalBouncy ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			owner.movement.controller.collisionState.rightCollisionDetector.OnEnter += Flip;
			owner.movement.controller.collisionState.leftCollisionDetector.OnEnter += Flip;
		}

		protected override void OnDetach()
		{
			owner.movement.controller.collisionState.rightCollisionDetector.OnEnter -= Flip;
			owner.movement.controller.collisionState.leftCollisionDetector.OnEnter -= Flip;
		}

		private void Flip(RaycastHit2D hit)
		{
			if ((8u & (uint)((Component)((RaycastHit2D)(ref hit)).collider).gameObject.layer) != 0)
			{
				owner.ForceToLookAt((owner.lookingDirection != Character.LookingDirection.Left) ? Character.LookingDirection.Left : Character.LookingDirection.Right);
				owner.movement.push.ApplyKnockback(owner, ability._pushInfo);
			}
		}
	}

	[SerializeField]
	private PushInfo _pushInfo = new PushInfo(ignoreOtherForce: false, expireOnGround: false);

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
