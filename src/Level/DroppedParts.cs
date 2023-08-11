using System.Collections;
using Characters;
using Characters.Movements;
using FX;
using UnityEngine;

namespace Level;

public class DroppedParts : DestructibleObject, IPoolObjectCopiable<DroppedParts>
{
	public enum Priority
	{
		High,
		Middle,
		Low
	}

	private static int orderInLayerCount;

	[SerializeField]
	private Priority _priority;

	[MinMaxSlider(0f, 30f)]
	[SerializeField]
	private Vector2Int _count = new Vector2Int(1, 1);

	[SerializeField]
	private bool _randomize;

	[SerializeField]
	private bool _collideWithTerrain = true;

	[SerializeField]
	[Information(/*Could not decode attribute arguments.*/)]
	private float _duration;

	[SerializeField]
	private float _fadeOutDuration;

	[SerializeField]
	private AnimationCurve _fadeOut;

	[GetComponent]
	[SerializeField]
	private PoolObject _poolObject;

	[GetComponent]
	[SerializeField]
	private SpriteRenderer _spriteRenderer;

	[GetComponent]
	[SerializeField]
	private Rigidbody2D _rigidbody;

	[SerializeField]
	[GetComponent]
	private BoxCollider2D _boxCollider;

	[SerializeField]
	[GetComponent]
	private CircleCollider2D _circleCollider;

	[SerializeField]
	private Vector2 _additionalForce = new Vector2(0f, 10f);

	public Priority priority => _priority;

	public Vector2Int count => _count;

	public bool randomize => _randomize;

	public bool collideWithTerrain => _collideWithTerrain;

	public PoolObject poolObject => _poolObject;

	public SpriteRenderer spriteRenderer => _spriteRenderer;

	public override Collider2D collider => (Collider2D)(object)_circleCollider;

	private void OnDestroy()
	{
		_spriteRenderer.sprite = null;
		_spriteRenderer = null;
	}

	public void Copy(DroppedParts to)
	{
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_011b: Unknown result type (might be due to invalid IL or missing references)
		//IL_015d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ee: Unknown result type (might be due to invalid IL or missing references)
		//IL_0204: Unknown result type (might be due to invalid IL or missing references)
		//IL_0275: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0253: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)to._spriteRenderer.sprite != (Object)(object)_spriteRenderer.sprite)
		{
			to._spriteRenderer.sprite = _spriteRenderer.sprite;
		}
		to._spriteRenderer.color = _spriteRenderer.color;
		to._spriteRenderer.flipX = _spriteRenderer.flipX;
		to._spriteRenderer.flipY = _spriteRenderer.flipY;
		((Renderer)to._spriteRenderer).sharedMaterial = ((Renderer)_spriteRenderer).sharedMaterial;
		to._spriteRenderer.drawMode = _spriteRenderer.drawMode;
		((Renderer)to._spriteRenderer).sortingLayerID = ((Renderer)_spriteRenderer).sortingLayerID;
		((Renderer)to._spriteRenderer).sortingOrder = ((Renderer)_spriteRenderer).sortingOrder;
		to._spriteRenderer.maskInteraction = _spriteRenderer.maskInteraction;
		to._spriteRenderer.spriteSortPoint = _spriteRenderer.spriteSortPoint;
		((Renderer)to._spriteRenderer).renderingLayerMask = ((Renderer)_spriteRenderer).renderingLayerMask;
		to._rigidbody.bodyType = _rigidbody.bodyType;
		to._rigidbody.sharedMaterial = _rigidbody.sharedMaterial;
		to._rigidbody.useAutoMass = _rigidbody.useAutoMass;
		to._rigidbody.constraints = _rigidbody.constraints;
		to._rigidbody.mass = _rigidbody.mass;
		to._rigidbody.drag = _rigidbody.drag;
		to._rigidbody.angularDrag = _rigidbody.angularDrag;
		to._rigidbody.gravityScale = _rigidbody.gravityScale;
		((Behaviour)to._boxCollider).enabled = ((Behaviour)_boxCollider).enabled;
		if (((Behaviour)to._boxCollider).enabled)
		{
			((Collider2D)to._boxCollider).offset = ((Collider2D)_boxCollider).offset;
			to._boxCollider.size = _boxCollider.size;
			to._boxCollider.edgeRadius = _boxCollider.edgeRadius;
		}
		((Behaviour)to._circleCollider).enabled = ((Behaviour)_circleCollider).enabled;
		if (((Behaviour)to._circleCollider).enabled)
		{
			((Collider2D)to._circleCollider).offset = ((Collider2D)_circleCollider).offset;
			to._circleCollider.radius = _circleCollider.radius;
		}
		to._additionalForce = _additionalForce;
		((Component)to).gameObject.layer = ((Component)this).gameObject.layer;
		to._priority = _priority;
		to._count = _count;
		to._randomize = _randomize;
		to._collideWithTerrain = _collideWithTerrain;
		to._duration = _duration;
		to._fadeOut = _fadeOut;
	}

	private void Awake()
	{
		_rigidbody = ((Component)this).GetComponent<Rigidbody2D>();
	}

	public void Initialize(Push push, float multiplier = 1f, bool interpolate = true)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = Vector2.zero;
		if (push != null && !push.expired)
		{
			val = push.direction * push.totalForce;
		}
		val *= multiplier;
		Initialize(val, multiplier, interpolate);
	}

	public void Initialize(Vector2 force, float multiplier = 1f, bool interpolate = true)
	{
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00af: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		if (interpolate)
		{
			if (Mathf.Abs(force.x) < 0.66f && Mathf.Abs(force.y) < 0.66f)
			{
				force = Random.insideUnitCircle;
			}
			force.y = Mathf.Abs(force.y);
			force *= multiplier;
			force = Vector2.op_Implicit(Quaternion.AngleAxis(Random.Range(-15f, 15f), Vector3.forward) * Vector2.op_Implicit(force) * Random.Range(0.8f, 1.2f));
			force += _additionalForce;
			_rigidbody.AddForce(force * Random.Range(0.5f, 1f), (ForceMode2D)1);
			_rigidbody.AddTorque(Random.Range(-0.5f, 0.5f), (ForceMode2D)1);
		}
		else
		{
			_rigidbody.AddForce(force, (ForceMode2D)1);
		}
		((Renderer)_spriteRenderer).sortingOrder = orderInLayerCount++;
		if (_duration > 0f)
		{
			((MonoBehaviour)this).StartCoroutine(CFadeOut());
		}
	}

	private IEnumerator CFadeOut()
	{
		yield return Chronometer.global.WaitForSeconds(_duration);
		if (_fadeOut.length > 0)
		{
			yield return poolObject.CFadeOut(_spriteRenderer, (ChronometerBase)(object)Chronometer.global, _fadeOut, _duration);
		}
		poolObject.Despawn();
	}

	public override void Hit(Character from, ref Damage damage, Vector2 force)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_007d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		//IL_0098: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		if (Mathf.Abs(force.x) < 0.66f && Mathf.Abs(force.y) < 0.66f)
		{
			force = Random.insideUnitCircle;
		}
		force.y = Mathf.Abs(force.y);
		force *= 3f;
		force = Vector2.op_Implicit(Quaternion.AngleAxis(Random.Range(-15f, 15f), Vector3.forward) * Vector2.op_Implicit(force) * Random.Range(0.8f, 1.2f));
		if (_rigidbody.IsTouchingLayers())
		{
			force += _additionalForce;
		}
		_rigidbody.AddForce(force * Random.Range(0.5f, 1f), (ForceMode2D)1);
		_rigidbody.AddTorque(Random.Range(-0.5f, 0.5f), (ForceMode2D)1);
	}
}
