using Characters;
using UnityEngine;

namespace FX.BoundsAttackVisualEffect;

public class RandomWithinIntersect : BoundsAttackVisualEffect
{
	[SerializeField]
	private EffectInfo _normal;

	[SerializeField]
	private EffectInfo _critical;

	private void Awake()
	{
		if ((Object)(object)_critical.effect == (Object)null)
		{
			_critical = _normal;
		}
	}

	public override void Spawn(Character owner, Bounds bounds, in Damage damage, ITarget target)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		EffectInfo obj = (damage.critical ? _critical : _normal);
		Bounds bounds2 = target.collider.bounds;
		Vector3 position = ((!((Bounds)(ref bounds)).Intersects(bounds2)) ? Vector2.op_Implicit(MMMaths.RandomPointWithinBounds(bounds2)) : MMMaths.RandomVector3(Vector2.op_Implicit(MMMaths.Max(Vector2.op_Implicit(((Bounds)(ref bounds)).min), Vector2.op_Implicit(((Bounds)(ref bounds2)).min))), Vector2.op_Implicit(MMMaths.Min(Vector2.op_Implicit(((Bounds)(ref bounds)).max), Vector2.op_Implicit(((Bounds)(ref bounds2)).max)))));
		obj.Spawn(position, owner);
	}
}
