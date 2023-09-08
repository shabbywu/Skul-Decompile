using System.Collections;
using UnityEngine;

namespace Characters.Projectiles;

[RequireComponent(typeof(PoolObject))]
public class DeflectedProjectile : MonoBehaviour
{
	[GetComponent]
	[SerializeField]
	private PoolObject _reusable;

	[SerializeField]
	private SpriteRenderer _renderer;

	[SerializeField]
	private float _duration;

	[SerializeField]
	private float _minimumSpeed = 0.35f;

	[SerializeField]
	private float _speedMultiplier = 1f;

	private float _speed;

	private Vector2 _direction;

	[SerializeField]
	private Vector2 _yAngleRange = new Vector2(-0.1f, 0.1f);

	public PoolObject reusable => _reusable;

	public void Deflect(Vector2 direction, Sprite projectileSprite, Vector2 scale, float speed)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		_direction.x = direction.x * -1f;
		_direction.y = Random.Range(_yAngleRange.x, _yAngleRange.y);
		_speed = ((speed < _minimumSpeed) ? _minimumSpeed : speed);
		_renderer.sprite = projectileSprite;
		((Component)this).transform.localScale = Vector2.op_Implicit(scale);
		float num = Mathf.Atan2(_direction.y, _direction.x) * 57.29578f;
		((Component)this).transform.rotation = Quaternion.Euler(0f, 0f, num);
		((MonoBehaviour)this).StartCoroutine(CDespawn());
	}

	private void Update()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		((Component)this).transform.Translate(Vector2.op_Implicit(_direction * _speed * _speedMultiplier), (Space)0);
	}

	private IEnumerator CDespawn()
	{
		yield return Chronometer.global.WaitForSeconds(_duration);
		_reusable.Despawn();
	}
}
