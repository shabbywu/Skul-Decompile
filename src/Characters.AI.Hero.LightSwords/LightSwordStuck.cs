using System.Collections;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Hero.LightSwords;

public class LightSwordStuck : MonoBehaviour
{
	private static short _currentSortingOrder = 1;

	private static int[] _points = new int[5] { 45, 75, 90, 105, 135 };

	[SerializeField]
	private SpriteRenderer[] _bodyContainer;

	[SerializeField]
	private Transform[] _trailEffectTransformContainer;

	[SerializeField]
	private Transform _trailEffectTransform;

	private SpriteRenderer _body;

	[SerializeField]
	private Color _startColor;

	[SerializeField]
	private Color _endColor;

	[SerializeField]
	private Curve _hitColorCurve;

	[SerializeField]
	[Subcomponent(typeof(OperationInfos))]
	private OperationInfos _onStuck;

	private int _order;

	private void Awake()
	{
		_order = _currentSortingOrder++;
		_onStuck.Initialize();
	}

	public void OnStuck(Character owner, Vector2 position, float angle)
	{
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		Hide();
		int num = Evaluate(angle);
		_body = _bodyContainer[num];
		_trailEffectTransform.SetParent(_trailEffectTransformContainer[num]);
		((Renderer)_body).sortingOrder = _order;
		((Component)this).transform.position = Vector2.op_Implicit(position);
		((Component)_onStuck).gameObject.SetActive(true);
		_onStuck.Run(owner);
		Show();
	}

	public void Despawn()
	{
		Hide();
	}

	public void Sign()
	{
		((MonoBehaviour)this).StartCoroutine(CEaseColor());
	}

	private int Evaluate(float angle)
	{
		angle -= 180f;
		int result = 0;
		float num = float.MaxValue;
		for (int i = 0; i < _points.Length; i++)
		{
			float num2 = Mathf.Abs(angle - (float)_points[i]);
			if (num2 < num)
			{
				result = i;
				num = num2;
			}
		}
		return result;
	}

	private void Show()
	{
		if ((Object)(object)_body != (Object)null)
		{
			((Component)_body).gameObject.SetActive(true);
		}
	}

	private void Hide()
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)_body != (Object)null)
		{
			_body.color = _startColor;
			((Component)_body).gameObject.SetActive(false);
		}
	}

	private IEnumerator CEaseColor()
	{
		float duration = _hitColorCurve.duration;
		for (float time = 0f; time < duration; time += Chronometer.global.deltaTime)
		{
			_body.color = Color.Lerp(_startColor, _endColor, _hitColorCurve.Evaluate(time));
			yield return null;
		}
		_body.color = _endColor;
	}
}
