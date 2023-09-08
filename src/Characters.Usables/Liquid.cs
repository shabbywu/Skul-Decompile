using System.Collections;
using System.Collections.Generic;
using FX;
using PhysicsUtils;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Usables;

[RequireComponent(typeof(PoolObject))]
public class Liquid : MonoBehaviour
{
	[SerializeField]
	[GetComponent]
	private PoolObject _poolObject;

	[SerializeField]
	private float _spriteUnitSize;

	[SerializeField]
	private LayerMask _terrainMask;

	[SerializeField]
	private SpriteRenderer _body;

	[SerializeField]
	private SpriteRenderer _leftSide;

	[SerializeField]
	private SpriteRenderer _rightSide;

	[SerializeField]
	private BoxCollider2D _collider;

	[SerializeField]
	private float _lifeTime;

	[SerializeField]
	private float _fadeOutDuration;

	[SerializeField]
	private AnimationCurve _fadeOut;

	private static readonly NonAllocOverlapper _sharedOverlapper = new NonAllocOverlapper(99);

	private LiquidMaster _master;

	private RaycastHit2D _terrainHitInfo;

	private bool _hit;

	private int _stack = 1;

	private SpriteRenderer[] _renderers;

	public float size => _collider.size.x;

	public int stack => _stack;

	public void Awake()
	{
		_renderers = (SpriteRenderer[])(object)new SpriteRenderer[3] { _body, _leftSide, _rightSide };
	}

	public bool IsSameTerrain(Collider2D terrain)
	{
		return (Object)(object)((RaycastHit2D)(ref _terrainHitInfo)).collider == (Object)(object)terrain;
	}

	public Liquid Spawn(Vector2 position, LiquidMaster master)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		Liquid liquid = Spawn(position);
		liquid._master = master;
		liquid._master.Add(liquid);
		return liquid;
	}

	public Liquid Spawn(Vector2 position)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		Liquid component = ((Component)_poolObject.Spawn(true)).GetComponent<Liquid>();
		((Component)component).transform.position = Vector2.op_Implicit(position);
		component._stack = 1;
		((MonoBehaviour)component).StartCoroutine("CActivate");
		Singleton<Service>.Instance.levelManager.onMapLoaded -= component.Despawn;
		Singleton<Service>.Instance.levelManager.onMapLoaded += component.Despawn;
		return component;
	}

	private IEnumerator CActivate()
	{
		((Renderer)_body).enabled = false;
		yield return null;
		((Renderer)_body).enabled = true;
		Color color = _body.color;
		_body.color = new Color(color.r, color.g, color.b, 1f);
		Activate();
	}

	private IEnumerator CFadeOut()
	{
		yield return Chronometer.global.WaitForSeconds(_lifeTime);
		if ((Object)(object)_master != (Object)null)
		{
			_master.Remove(this);
		}
		if (_fadeOut.length > 0)
		{
			yield return _poolObject.CFadeOut(_renderers, Chronometer.global, _fadeOut, _fadeOutDuration);
		}
	}

	private void Activate()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0131: Unknown result type (might be due to invalid IL or missing references)
		//IL_0136: Unknown result type (might be due to invalid IL or missing references)
		//IL_0151: Unknown result type (might be due to invalid IL or missing references)
		//IL_0166: Unknown result type (might be due to invalid IL or missing references)
		//IL_016b: Unknown result type (might be due to invalid IL or missing references)
		_collider.size = new Vector2(_spriteUnitSize, _collider.size.y);
		_body.size = new Vector2(_spriteUnitSize, _body.size.y);
		_terrainHitInfo = default(RaycastHit2D);
		_hit = TargetFinder.BoxCast(Vector2.op_Implicit(((Component)this).transform.position), new Vector2(_collider.size.x, 0.1f), 0f, Vector2.down, 100f, _terrainMask, ref _terrainHitInfo);
		NonAllocOverlapper val = new NonAllocOverlapper(99);
		((ContactFilter2D)(ref val.contactFilter)).SetLayerMask(LayerMask.op_Implicit(4096));
		if (val.OverlapCollider((Collider2D)(object)_collider).GetComponents<Liquid>(true).Count == 0)
		{
			MakeDefaultShape();
		}
		else
		{
			MakeLiquidShape();
		}
		_leftSide.color = Color.white;
		_rightSide.color = Color.white;
		((Component)_leftSide).transform.localPosition = Vector2.op_Implicit(new Vector2((0f - _body.size.x) / 2f, 0f));
		((Component)_rightSide).transform.localPosition = Vector2.op_Implicit(new Vector2(_body.size.x / 2f, 0f));
		if (_lifeTime > 0f)
		{
			((MonoBehaviour)this).StartCoroutine(CFadeOut());
		}
	}

	private void MakeDefaultShape()
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		float width = _spriteUnitSize;
		Vector2 position = Vector2.op_Implicit(((Component)this).transform.position);
		Clamp(Vector2.op_Implicit(((Component)this).transform.position), ref width, ref position);
		((Component)this).transform.position = Vector2.op_Implicit(position);
	}

	private void MakeLiquidShape()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		((ContactFilter2D)(ref _sharedOverlapper.contactFilter)).SetLayerMask(LayerMask.op_Implicit(4096));
		List<Liquid> components = _sharedOverlapper.OverlapCollider((Collider2D)(object)_collider).GetComponents<Liquid>(true);
		components = components.FindAll((Liquid target) => target.IsSameTerrain(((RaycastHit2D)(ref _terrainHitInfo)).collider));
		if (components.Count == 1)
		{
			if ((Object)(object)components[0] == (Object)(object)this)
			{
				return;
			}
			Expand(components[0]);
			MakeLiquidShape();
		}
		if (components.Count >= 2)
		{
			Combine(components[0], components[1]);
			MakeLiquidShape();
		}
	}

	private void Expand(Liquid liquid)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_0126: Unknown result type (might be due to invalid IL or missing references)
		//IL_0137: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_014c: Unknown result type (might be due to invalid IL or missing references)
		//IL_015f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_0103: Unknown result type (might be due to invalid IL or missing references)
		//IL_0116: Unknown result type (might be due to invalid IL or missing references)
		float num = _body.size.x / 2f;
		float num2 = ((Component)liquid).transform.position.x + num;
		float num3 = ((Component)liquid).transform.position.x - num;
		float width = liquid.size + _body.size.x;
		float y = _body.size.y;
		Vector2 position = Vector2.op_Implicit(((Component)liquid).transform.position);
		if (((Component)this).transform.position.x >= num2)
		{
			float num4 = _body.size.x / 2f;
			((Vector2)(ref position))._002Ector(((Component)liquid).transform.position.x + num4, ((Component)this).transform.position.y);
		}
		else if (((Component)this).transform.position.x <= num3)
		{
			float num5 = _body.size.x / 2f;
			((Vector2)(ref position))._002Ector(((Component)liquid).transform.position.x - num5, ((Component)this).transform.position.y);
		}
		Clamp(position, ref width, ref position);
		((Component)this).transform.position = Vector2.op_Implicit(position);
		_collider.size = new Vector2(width, y);
		_body.size = new Vector2(width, y);
		_stack += liquid.stack;
		liquid.Despawn();
	}

	private void Combine(Liquid liquid1, Liquid liquid2)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		float width = liquid1.size + liquid2.size + _spriteUnitSize;
		float y = _body.size.y;
		Vector2 position = Vector2.op_Implicit(((Component)this).transform.position);
		Clamp(Vector2.op_Implicit(((Component)this).transform.position), ref width, ref position);
		_collider.size = new Vector2(width, y);
		_body.size = new Vector2(width, y);
		((Component)this).transform.position = Vector2.op_Implicit(position);
		_stack += liquid1.stack + liquid2.stack;
		liquid1.Despawn();
		liquid2.Despawn();
	}

	private void Clamp(Vector2 originPosition, ref float width, ref Vector2 position)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00da: Unknown result type (might be due to invalid IL or missing references)
		//IL_00df: Unknown result type (might be due to invalid IL or missing references)
		if (!_hit)
		{
			return;
		}
		Bounds bounds = ((RaycastHit2D)(ref _terrainHitInfo)).collider.bounds;
		if (width >= ((Bounds)(ref bounds)).size.x)
		{
			width = ((Bounds)(ref bounds)).size.x;
			position = new Vector2(((Bounds)(ref bounds)).center.x, originPosition.y);
			return;
		}
		float num = width / 2f;
		float num2 = position.x - num;
		float num3 = position.x + num;
		if (num3 > ((Bounds)(ref bounds)).max.x)
		{
			float num4 = num3 - ((Bounds)(ref bounds)).max.x;
			position = new Vector2(originPosition.x - num4, originPosition.y);
		}
		else if (num2 < ((Bounds)(ref bounds)).min.x)
		{
			float num5 = ((Bounds)(ref bounds)).min.x - num2;
			position = new Vector2(originPosition.x + num5, originPosition.y);
		}
	}

	private void Despawn()
	{
		Singleton<Service>.Instance.levelManager.onMapLoaded -= Despawn;
		if ((Object)(object)_master != (Object)null)
		{
			_master.Remove(this);
		}
		_poolObject.Despawn();
	}
}
