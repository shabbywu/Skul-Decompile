using System.Collections;
using UnityEngine;

namespace Level;

public class ReactiveProp : MonoBehaviour
{
	[SerializeField]
	[GetComponent]
	protected Animator _animator;

	[SerializeField]
	private float _angle;

	[SerializeField]
	private float _distance;

	[SerializeField]
	private Curve _curve;

	private Vector2 _direction;

	protected bool _flying;

	private Vector3 _originPosition;

	private Vector3 _destination;

	private void Awake()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		_originPosition = ((Component)this).transform.localPosition;
	}

	public void ResetPosition()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		((Component)this).transform.localPosition = _originPosition;
	}

	protected void Activate()
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0091: Unknown result type (might be due to invalid IL or missing references)
		//IL_009c: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		_flying = true;
		_direction = Vector2.op_Implicit(Quaternion.AngleAxis(_angle, Vector3.forward) * Vector3.right);
		Vector3 localScale;
		if (_direction.x < 0f)
		{
			localScale = ((Component)this).transform.localScale;
			((Vector3)(ref localScale)).Set(-1f, 1f, 1f);
		}
		else
		{
			localScale = ((Component)this).transform.localScale;
			((Vector3)(ref localScale)).Set(1f, 1f, 1f);
		}
		_destination = _originPosition + Vector2.op_Implicit(_direction) * _distance;
		((MonoBehaviour)this).StartCoroutine(CFlyAway());
	}

	private IEnumerator CReadyToFly()
	{
		_animator.Play("Ready");
		yield return Chronometer.global.WaitForSeconds(0.4f);
	}

	private IEnumerator CFlyAway()
	{
		float t = 0f;
		_animator.Play("Fly");
		for (; t < _curve.duration; t += ((ChronometerBase)Chronometer.global).deltaTime)
		{
			float num = _curve.Evaluate(t);
			((Component)this).transform.localPosition = Vector3.Lerp(_originPosition, _destination, num);
			yield return null;
		}
		((Component)this).gameObject.SetActive(false);
	}
}
