using System.Collections;
using UnityEngine;

public class TestMoving : MonoBehaviour
{
	[SerializeField]
	private CircleCollider2D _boundary;

	[SerializeField]
	private RopeBridge _leftUp;

	[SerializeField]
	private RopeBridge _rightUp;

	[SerializeField]
	private RopeBridge _leftDown;

	[SerializeField]
	private RopeBridge _rightDown;

	[SerializeField]
	private float _waitTime;

	[SerializeField]
	private float _moveTime;

	[SerializeField]
	private float _speed;

	private Vector2 _direction;

	private bool _moving;

	private bool _directionToggle;

	private void Start()
	{
		((MonoBehaviour)this).StartCoroutine(CProcess());
	}

	private void Update()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		if (_moving)
		{
			((Component)this).transform.Translate(Vector2.op_Implicit(_direction * _speed * Time.deltaTime));
		}
	}

	private IEnumerator CProcess()
	{
		while (true)
		{
			_moving = false;
			yield return Chronometer.global.WaitForSeconds(_waitTime);
			SetDirection();
			_directionToggle = !_directionToggle;
			_moving = true;
			yield return Chronometer.global.WaitForSeconds(_waitTime);
		}
	}

	private void SetDirection()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		float num = Random.Range(((Component)_boundary).transform.position.x - _boundary.radius, ((Component)_boundary).transform.position.x + _boundary.radius);
		float num2 = Random.Range(((Component)_boundary).transform.position.y - _boundary.radius, ((Component)_boundary).transform.position.y + _boundary.radius);
		Vector2 val = new Vector2(num, num2) - Vector2.op_Implicit(((Component)this).transform.position);
		_direction = ((Vector2)(ref val)).normalized;
	}
}
