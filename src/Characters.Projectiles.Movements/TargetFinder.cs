using System;
using System.Collections.Generic;
using PhysicsUtils;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Projectiles.Movements;

[Serializable]
public class TargetFinder
{
	public enum Method
	{
		Closest,
		First,
		Random,
		Player
	}

	private delegate Target FindDelegate(IReadOnlyList<Collider2D> result);

	private static readonly NonAllocOverlapper _overlapper = new NonAllocOverlapper(15);

	[SerializeField]
	private TargetLayer _layer = new TargetLayer(LayerMask.op_Implicit(2048), allyBody: false, foeBody: true, allyProjectile: false, foeProjectile: false);

	[SerializeField]
	private Method _method;

	[SerializeField]
	private Collider2D _range;

	private IProjectile _projectile;

	private FindDelegate _finder;

	public Collider2D range => _range;

	internal void Initialize(IProjectile projectile)
	{
		_projectile = projectile;
		switch (_method)
		{
		case Method.Closest:
			_finder = FindClosest;
			break;
		case Method.First:
			_finder = FindFirst;
			break;
		case Method.Random:
			_finder = FindRandom;
			break;
		case Method.Player:
			_finder = FindPlayer;
			break;
		}
	}

	public Target Find()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		((ContactFilter2D)(ref _overlapper.contactFilter)).SetLayerMask(_layer.Evaluate(_projectile.gameObject));
		((Behaviour)_range).enabled = true;
		_overlapper.OverlapCollider(_range);
		((Behaviour)_range).enabled = false;
		return _finder((IReadOnlyList<Collider2D>)_overlapper.results);
	}

	private Target FindClosest(IReadOnlyList<Collider2D> result)
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		Target result2 = null;
		float num = float.MaxValue;
		for (int i = 0; i < result.Count; i++)
		{
			Target component = ((Component)result[i]).GetComponent<Target>();
			if (!((Object)(object)component == (Object)null))
			{
				Vector2 val = Vector2.op_Implicit(_projectile.transform.position);
				Vector2 val2 = Vector2.op_Implicit(((Component)component).transform.position) - val;
				float sqrMagnitude = ((Vector2)(ref val2)).sqrMagnitude;
				if (sqrMagnitude < num)
				{
					num = sqrMagnitude;
					result2 = component;
				}
			}
		}
		return result2;
	}

	private Target FindFirst(IReadOnlyList<Collider2D> result)
	{
		return GetComponentExtension.GetComponent<Collider2D, Target>((IEnumerable<Collider2D>)result);
	}

	private Target FindRandom(IReadOnlyList<Collider2D> result)
	{
		List<Target> components = GetComponentExtension.GetComponents<Collider2D, Target>((IEnumerable<Collider2D>)result, true);
		if (components.Count == 0)
		{
			return null;
		}
		return ExtensionMethods.Random<Target>((IEnumerable<Target>)components);
	}

	private Target FindPlayer(IReadOnlyList<Collider2D> result)
	{
		return ((Component)Singleton<Service>.Instance.levelManager.player).GetComponent<Target>();
	}
}
