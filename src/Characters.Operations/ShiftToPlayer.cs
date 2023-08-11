using PhysicsUtils;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Operations;

public class ShiftToPlayer : CharacterOperation
{
	[SerializeField]
	private Transform _object;

	[SerializeField]
	private float _offsetY;

	[SerializeField]
	private float _offsetX;

	[SerializeField]
	private bool _fromPlatform;

	[SerializeField]
	private bool _lastStandingPlatform;

	private static NonAllocCaster caster;

	static ShiftToPlayer()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		caster = new NonAllocCaster(1);
	}

	public override void Run(Character owner)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		Character player = Singleton<Service>.Instance.levelManager.player;
		Collider2D platform = GetPlatform();
		float num = ((Component)player).transform.position.x + _offsetX;
		Bounds bounds = platform.bounds;
		float num2 = ((Bounds)(ref bounds)).max.y + _offsetY;
		_object.position = Vector2.op_Implicit(new Vector2(num, num2));
	}

	private Collider2D GetPlatform()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		if (_lastStandingPlatform)
		{
			return null;
		}
		((ContactFilter2D)(ref caster.contactFilter)).SetLayerMask(Layers.groundMask);
		NonAllocCaster obj = caster;
		Vector2 val = Vector2.op_Implicit(((Component)Singleton<Service>.Instance.levelManager.player).transform.position);
		Bounds bounds = ((Collider2D)Singleton<Service>.Instance.levelManager.player.collider).bounds;
		NonAllocCaster val2 = obj.BoxCast(val, Vector2.op_Implicit(((Bounds)(ref bounds)).size), 0f, Vector2.down, 100f);
		if (val2.results.Count == 0)
		{
			return null;
		}
		RaycastHit2D val3 = val2.results[0];
		return ((RaycastHit2D)(ref val3)).collider;
	}
}
