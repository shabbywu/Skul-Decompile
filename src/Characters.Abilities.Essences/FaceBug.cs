using System;
using FX;
using UnityEngine;

namespace Characters.Abilities.Essences;

[Serializable]
public sealed class FaceBug : Ability
{
	public class Instance : AbilityInstance<FaceBug>
	{
		private Bounds _before;

		private EffectPoolInstance _spawned;

		public Instance(Character owner, FaceBug ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			owner.stat.AttachValues(ability._stat);
			EffectInfo attachEffectInfo = ability._attachEffectInfo;
			Bounds bounds = ((Collider2D)owner.collider).bounds;
			_spawned = attachEffectInfo.Spawn(((Bounds)(ref bounds)).center, owner);
		}

		protected override void OnDetach()
		{
			owner.stat.DetachValues(ability._stat);
			if ((Object)(object)_spawned != (Object)null)
			{
				_spawned.Stop();
				_spawned = null;
			}
		}

		public override void Refresh()
		{
			//IL_003d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0042: Unknown result type (might be due to invalid IL or missing references)
			//IL_0045: Unknown result type (might be due to invalid IL or missing references)
			base.Refresh();
			if ((Object)(object)_spawned != (Object)null)
			{
				_spawned.Stop();
				_spawned = null;
			}
			EffectInfo attachEffectInfo = ability._attachEffectInfo;
			Bounds bounds = ((Collider2D)owner.collider).bounds;
			_spawned = attachEffectInfo.Spawn(((Bounds)(ref bounds)).center, owner);
		}

		public override void UpdateTime(float deltaTime)
		{
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			//IL_003a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0046: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			base.UpdateTime(deltaTime);
			BoxCollider2D collider = owner.collider;
			if (_before != ((Collider2D)collider).bounds)
			{
				Transform transform = ((Component)_spawned).transform;
				Bounds bounds = ((Collider2D)collider).bounds;
				transform.position = ((Bounds)(ref bounds)).center;
				_before = ((Collider2D)collider).bounds;
			}
		}
	}

	[SerializeField]
	private EffectInfo _attachEffectInfo;

	[SerializeField]
	private Stat.Values _stat;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
