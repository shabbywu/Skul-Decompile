using System.Collections;
using Characters.Projectiles;
using FX;
using Level;
using Singletons;
using UnityEngine;

namespace Characters.Operations.Customs.GlacialSkull;

public class IceBergProp : DestructibleObject
{
	public delegate void DidHitDelegate(Character owner, in Damage damage, Vector2 force);

	[SerializeField]
	private Key _key = Key.SmallProp;

	private Collider2D _collider;

	[SerializeField]
	private Color _startColor;

	[SerializeField]
	private Color _endColor;

	[SerializeField]
	private Curve _hitColorCurve;

	private CoroutineReference _cEaseColorReference;

	[SerializeField]
	[GetComponent]
	private SpriteRenderer _spriteRenderer;

	[GetComponent]
	[SerializeField]
	private Animator _animator;

	[SerializeField]
	private SoundInfo _hitSound;

	[SerializeField]
	[Header("Projectile")]
	private Projectile _projectile;

	[SerializeField]
	private float _projectileBaseDamage = 10f;

	[SerializeField]
	private CustomFloat _speedMultiplier = new CustomFloat(0.98f, 1.02f);

	[SerializeField]
	private CustomFloat _extraAngle = new CustomFloat(-2f, 2f);

	[SerializeField]
	private float _positionCircleNoise = 0.2f;

	private Target[] _targets;

	public Key key => _key;

	public override Collider2D collider => _collider;

	public event DidHitDelegate onDidHit;

	private void Awake()
	{
		Initialize();
	}

	private void OnEnable()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		_spriteRenderer.color = _endColor;
	}

	public void Initialize()
	{
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		((Component)this).gameObject.SetActive(true);
		base.destroyed = false;
		_targets = ((Component)this).GetComponentsInChildren<Target>(true);
		_collider = _targets[0].collider;
		for (int i = 0; i < _targets.Length; i++)
		{
			Target obj = _targets[i];
			((Behaviour)obj.collider).enabled = true;
			((Behaviour)obj).enabled = true;
		}
		if ((Object)(object)_animator != (Object)null)
		{
			((Behaviour)_animator).enabled = true;
			_animator.Play(0, 0, 0f);
		}
		_spriteRenderer.color = _endColor;
	}

	public override void Hit(Character owner, ref Damage damage, Vector2 force)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		if (damage.motionType == Damage.MotionType.Basic && owner.type == Character.Type.Player)
		{
			damage.Evaluate(immuneToCritical: false);
			this.onDidHit?.Invoke(owner, in damage, force);
			FireProjectile(owner, damage.hitPoint);
			PersistentSingleton<SoundManager>.Instance.PlaySound(_hitSound, ((Component)this).transform.position);
			((CoroutineReference)(ref _cEaseColorReference)).Stop();
			_cEaseColorReference = CoroutineReferenceExtension.StartCoroutineWithReference((MonoBehaviour)(object)this, CEaseColor());
		}
	}

	private void FireProjectile(Character owner, Vector2 firePosition)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		int num = ((owner.lookingDirection == Character.LookingDirection.Left) ? 180 : 0);
		firePosition += Random.insideUnitCircle * _positionCircleNoise;
		((Component)_projectile.reusable.Spawn(Vector2.op_Implicit(firePosition), true)).GetComponent<Projectile>().Fire(owner, _projectileBaseDamage, (float)num + _extraAngle.value, flipX: false, flipY: false, _speedMultiplier.value);
	}

	private IEnumerator CEaseColor()
	{
		float duration = _hitColorCurve.duration;
		for (float time = 0f; time < duration; time += ((ChronometerBase)Chronometer.global).deltaTime)
		{
			_spriteRenderer.color = Color.Lerp(_startColor, _endColor, _hitColorCurve.Evaluate(time));
			yield return null;
		}
		_spriteRenderer.color = _endColor;
	}
}
