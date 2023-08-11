using System.Collections;
using UnityEngine;

namespace FX;

public class LineEffect : MonoBehaviour
{
	[SerializeField]
	private Transform _body;

	[SerializeField]
	private float _thickness = 1f;

	[SerializeField]
	private float _duration = 0.1f;

	private Vector2 _startPoint;

	private Vector2 _endPoint;

	public Vector2 startPoint
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return _startPoint;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			_startPoint = value;
		}
	}

	public Vector2 endPoint
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return _endPoint;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			_endPoint = value;
		}
	}

	public void Run()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005c: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		((Component)_body).gameObject.SetActive(false);
		float num = Vector2.Distance(_startPoint, _endPoint);
		_body.localScale = Vector2.op_Implicit(new Vector2(_thickness, num));
		Vector3 val = Vector2.op_Implicit(_endPoint - _startPoint);
		float num2 = Mathf.Atan2(val.y, val.x) * 57.29578f - 90f;
		_body.rotation = Quaternion.AngleAxis(num2, Vector3.forward);
		((Component)this).transform.position = Vector2.op_Implicit(new Vector2(_startPoint.x, _startPoint.y + 1f));
		((Component)_body).gameObject.SetActive(true);
		((MonoBehaviour)this).StartCoroutine(CDeactive());
	}

	public void Hide()
	{
		((Component)_body).gameObject.SetActive(false);
	}

	private IEnumerator CDeactive()
	{
		yield return Chronometer.global.WaitForSeconds(_duration);
		((Component)_body).gameObject.SetActive(false);
	}
}
