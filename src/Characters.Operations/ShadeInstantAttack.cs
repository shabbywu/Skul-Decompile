using System.Collections.Generic;
using FX.BoundsAttackVisualEffect;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Operations;

public class ShadeInstantAttack : CharacterOperation
{
	private const int _limit = 3;

	[SerializeField]
	private HitInfo _hitInfo = new HitInfo(Damage.AttackType.Melee);

	[SerializeField]
	private Collider2D _attackRange;

	[SerializeField]
	private int _damage1;

	[SerializeField]
	private int _damage2;

	[SerializeField]
	private int _damage3;

	[BoundsAttackVisualEffect.Subcomponent]
	[SerializeField]
	private BoundsAttackVisualEffect.Subcomponents _effect1;

	[BoundsAttackVisualEffect.Subcomponent]
	[SerializeField]
	private BoundsAttackVisualEffect.Subcomponents _effect2;

	[BoundsAttackVisualEffect.Subcomponent]
	[SerializeField]
	private BoundsAttackVisualEffect.Subcomponents _effect3;

	private int[] _damages;

	private BoundsAttackVisualEffect.Subcomponents[] _effects;

	private TargetLayer _layer = new TargetLayer(LayerMask.op_Implicit(0), allyBody: false, foeBody: true, allyProjectile: false, foeProjectile: false);

	private NonAllocOverlapper _overlapper;

	private float _remainTimeToNextAttack;

	private void Awake()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		_overlapper = new NonAllocOverlapper(3);
		_damages = new int[3] { _damage1, _damage2, _damage3 };
		_effects = new BoundsAttackVisualEffect.Subcomponents[3] { _effect1, _effect2, _effect3 };
		((Behaviour)_attackRange).enabled = false;
	}

	public override void Run(Character owner)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_0105: Unknown result type (might be due to invalid IL or missing references)
		//IL_010c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0111: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0135: Unknown result type (might be due to invalid IL or missing references)
		//IL_013a: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0144: Unknown result type (might be due to invalid IL or missing references)
		//IL_014e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0150: Unknown result type (might be due to invalid IL or missing references)
		//IL_0155: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0170: Unknown result type (might be due to invalid IL or missing references)
		((ContactFilter2D)(ref _overlapper.contactFilter)).SetLayerMask(_layer.Evaluate(((Component)owner).gameObject));
		((Behaviour)_attackRange).enabled = true;
		Bounds bounds = _attackRange.bounds;
		_overlapper.OverlapCollider(_attackRange);
		((Behaviour)_attackRange).enabled = false;
		List<Target> components = GetComponentExtension.GetComponents<Collider2D, Target>((IEnumerable<Collider2D>)_overlapper.results, true);
		if (components.Count == 0)
		{
			return;
		}
		int num = _damages[components.Count - 1];
		BoundsAttackVisualEffect.Subcomponents subcomponents = _effects[components.Count - 1];
		for (int i = 0; i < components.Count; i++)
		{
			Target target = components[i];
			if (!((Object)(object)target == (Object)null) && !((Object)(object)target.character == (Object)null) && !((Object)(object)target.character == (Object)(object)owner) && target.character.liveAndActive)
			{
				Bounds bounds2 = target.collider.bounds;
				Bounds val = default(Bounds);
				((Bounds)(ref val)).min = Vector2.op_Implicit(MMMaths.Max(Vector2.op_Implicit(((Bounds)(ref bounds)).min), Vector2.op_Implicit(((Bounds)(ref bounds2)).min)));
				((Bounds)(ref val)).max = Vector2.op_Implicit(MMMaths.Min(Vector2.op_Implicit(((Bounds)(ref bounds)).max), Vector2.op_Implicit(((Bounds)(ref bounds2)).max)));
				Vector2 hitPoint = MMMaths.RandomPointWithinBounds(val);
				Damage damage = owner.stat.GetDamage(num, hitPoint, _hitInfo);
				subcomponents.Spawn(owner, val, in damage, target);
				if (!target.character.cinematic.value)
				{
					owner.TryAttackCharacter(target, ref damage);
				}
			}
		}
	}
}
