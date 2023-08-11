using System;
using System.Collections;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Usables;

public sealed class EffectZone : MonoBehaviour
{
	[SerializeField]
	private Transform _root;

	[MinMaxSlider(0f, 100f)]
	[SerializeField]
	private Vector2 _horizontalRange;

	[SerializeField]
	private LayerMask _terrainLayerMask;

	[SerializeField]
	private SpriteRenderer _renderer;

	[SerializeField]
	private float _sizeMultiplier = 1f;

	[SerializeField]
	private string[] _keys;

	private static Vector2 _terrainFindSize;

	private static NonAllocCaster _caster;

	public Vector2 sizeRange
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return _horizontalRange;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			_horizontalRange = value;
		}
	}

	static EffectZone()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		_caster = new NonAllocCaster(1);
		_terrainFindSize = new Vector2(1f, 0.1f);
	}

	public bool MatchKey(string[] targetKeys)
	{
		foreach (string value in targetKeys)
		{
			string[] keys = _keys;
			for (int j = 0; j < keys.Length; j++)
			{
				if (keys[j].Equals(value, StringComparison.OrdinalIgnoreCase))
				{
					return true;
				}
			}
		}
		return false;
	}

	private void OnEnable()
	{
		((MonoBehaviour)this).StartCoroutine(COnEnable());
	}

	private IEnumerator COnEnable()
	{
		((Renderer)_renderer).enabled = false;
		yield return null;
		Create();
		((Renderer)_renderer).enabled = true;
	}

	public void Create()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		RaycastHit2D hit = default(RaycastHit2D);
		if (!TargetFinder.BoxCast(Vector2.op_Implicit(_root.position), _terrainFindSize, 0f, Vector2.down, 100f, _terrainLayerMask, ref hit))
		{
			SetEffectZoneScale(1f);
		}
		else
		{
			SetPosition(((RaycastHit2D)(ref hit)).collider);
		}
	}

	private void SetPosition(Collider2D platform)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		Bounds bounds = platform.bounds;
		float x = _horizontalRange.x;
		float y = _horizontalRange.y;
		if (((Bounds)(ref bounds)).size.x < x)
		{
			SetPositionToCenter(bounds);
			SetEffectZoneScale(x);
		}
		else if (((Bounds)(ref bounds)).size.x > y)
		{
			SetPositionToClosestSide(bounds);
			SetEffectZoneScale(y);
		}
		else
		{
			SetPositionToCenter(bounds);
			SetEffectZoneScale(((Bounds)(ref bounds)).size.x);
		}
	}

	private void SetPositionToCenter(Bounds bounds)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		_root.position = Vector2.op_Implicit(new Vector2(((Bounds)(ref bounds)).center.x, ((Bounds)(ref bounds)).max.y));
	}

	private void SetPositionToClosestSide(Bounds bounds)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		float num = _horizontalRange.y / 2f;
		float num2 = ((Bounds)(ref bounds)).max.x - _root.position.x;
		float num3 = _root.position.x - ((Bounds)(ref bounds)).min.x;
		float num4 = num - num2;
		float num5 = num - num3;
		if (num4 > 0f)
		{
			_root.position = Vector2.op_Implicit(new Vector2(((Bounds)(ref bounds)).max.x - num, ((Bounds)(ref bounds)).max.y));
		}
		else if (num5 > 0f)
		{
			_root.position = Vector2.op_Implicit(new Vector2(((Bounds)(ref bounds)).min.x + num, ((Bounds)(ref bounds)).max.y));
		}
	}

	private void SetEffectZoneScale(float sizeOfAOE)
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		((Component)_renderer).transform.localScale = new Vector3(sizeOfAOE * _sizeMultiplier, 1f, 1f);
	}
}
