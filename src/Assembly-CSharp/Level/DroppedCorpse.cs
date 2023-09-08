using System.Collections;
using Characters;
using Characters.Movements;
using UnityEngine;

namespace Level;

public class DroppedCorpse : DestructibleObject
{
	private static int orderInLayerCount;

	[SerializeField]
	private Character _owner;

	[SerializeField]
	private bool _randomize;

	[SerializeField]
	private bool _collideWithTerrain = true;

	[Information("0이면 영구지속", InformationAttribute.InformationType.Info, false)]
	[SerializeField]
	private float _duration;

	[SerializeField]
	private float _fadeOutDuration;

	[SerializeField]
	private AnimationCurve _fadeOut;

	[SerializeField]
	private SpriteRenderer _spriteRenderer;

	[SerializeField]
	private Rigidbody2D _rigidbody;

	[SerializeField]
	private BoxCollider2D _boxCollider;

	[SerializeField]
	private Vector2 _additionalForce = new Vector2(0f, 0.05f);

	[SerializeField]
	private AdventurerGearReward _reward;

	public bool randomize => _randomize;

	public bool collideWithTerrain => _collideWithTerrain;

	public SpriteRenderer spriteRenderer => _spriteRenderer;

	public override Collider2D collider => (Collider2D)(object)_boxCollider;

	private void Awake()
	{
		if ((Object)(object)_rigidbody == (Object)null)
		{
			_rigidbody = ((Component)this).GetComponent<Rigidbody2D>();
		}
		((Component)this).transform.SetParent(((Component)Map.Instance).transform);
	}

	public void Emit()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00db: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0096: Unknown result type (might be due to invalid IL or missing references)
		Push push = _owner.movement.push;
		((Component)this).transform.position = ((Component)_owner).transform.position;
		if (_owner.lookingDirection == Character.LookingDirection.Right)
		{
			((Component)this).transform.localScale = new Vector3(1f, 1f, 1f);
			((Component)_spriteRenderer).transform.localScale = new Vector3(1f, 1f, 1f);
			((Component)_reward).transform.localScale = new Vector3(1f, 1f, 1f);
		}
		else
		{
			((Component)this).transform.localScale = new Vector3(-1f, 1f, 1f);
			((Component)_spriteRenderer).transform.localScale = new Vector3(-1f, 1f, 1f);
			((Component)_reward).transform.localScale = new Vector3(-1f, 1f, 1f);
		}
		Initialize(push);
	}

	private void Initialize(Push push, float multiplier = 1f)
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
		Initialize(val, multiplier);
	}

	private void Initialize(Vector2 force, float multiplier = 1f)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		force *= multiplier;
		_rigidbody.AddForce(force, (ForceMode2D)1);
		((Renderer)_spriteRenderer).sortingOrder = orderInLayerCount++;
		if (_duration > 0f)
		{
			((MonoBehaviour)this).StartCoroutine(CFadeOut());
		}
	}

	private IEnumerator CFadeOut()
	{
		yield return Chronometer.global.WaitForSeconds(_duration);
		((Component)this).gameObject.SetActive(false);
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
