using System.Collections;
using Characters;
using Services;
using Singletons;
using UnityEngine;

namespace FX;

public sealed class ThunderLineEffect : MonoBehaviour
{
	private enum ChronometerType
	{
		Global,
		Character,
		Player
	}

	[SerializeField]
	[Header("Renderer")]
	private LineRenderer _lineRenderer;

	[SerializeField]
	private LineRenderer _outLineRenderer;

	[Header("Point")]
	[SerializeField]
	private Transform _startPoint;

	[SerializeField]
	private Transform _endPoint;

	[SerializeField]
	private bool _fixedStartPosiiton = true;

	[SerializeField]
	private bool _fixedEndPosiiton = true;

	[Header("Amplitude")]
	[SerializeField]
	private Curve _amplitudeCurve;

	[SerializeField]
	private float _amplitude;

	[SerializeField]
	private int _vertexCount;

	[SerializeField]
	[Header("Width")]
	private AnimationCurve _widthCurve;

	[SerializeField]
	private AnimationCurve _outlineWidthCurve;

	[Range(0f, 3f)]
	[SerializeField]
	private float _widthMultiplier;

	[Range(0f, 3f)]
	[SerializeField]
	private float _outlineWidthMultiplier;

	[Header("Color")]
	[SerializeField]
	private Color _startColor;

	[SerializeField]
	private Color _endColor;

	[SerializeField]
	private Color _outlineStartColor;

	[SerializeField]
	private Color _outlineEndColor;

	[SerializeField]
	[Header("Option")]
	private ChronometerType _chronometerType;

	[SerializeField]
	private Character _chonometerOwner;

	[SerializeField]
	[FrameTime]
	private float _updateTime = 1f;

	[SerializeField]
	private bool _outlineEnable;

	[SerializeField]
	private float _noise;

	[SerializeField]
	[Range(0f, 1f)]
	private float _sparkChance;

	[SerializeField]
	private CustomFloat _sparkAmount;

	public void Acitvate()
	{
		((Component)this).gameObject.SetActive(true);
	}

	public void Deactivate()
	{
		((Component)this).gameObject.SetActive(false);
	}

	private void OnEnable()
	{
		((MonoBehaviour)this).StartCoroutine(CDrawLine());
	}

	private IEnumerator CDrawLine()
	{
		Chronometer chronometer = GetChronometer();
		while (true)
		{
			DrawLine();
			if (chronometer == null)
			{
				yield return Chronometer.global.WaitForSeconds(_updateTime);
			}
			else
			{
				yield return ChronometerExtension.WaitForSeconds((ChronometerBase)(object)chronometer, _updateTime);
			}
		}
	}

	private Chronometer GetChronometer()
	{
		switch (_chronometerType)
		{
		case ChronometerType.Character:
			if ((Object)(object)_chonometerOwner == (Object)null)
			{
				return null;
			}
			return _chonometerOwner.chronometer.master;
		case ChronometerType.Player:
			return Singleton<Service>.Instance.levelManager.player.chronometer.master;
		default:
			return null;
		}
	}

	private void DrawLine()
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_0104: Unknown result type (might be due to invalid IL or missing references)
		//IL_0109: Unknown result type (might be due to invalid IL or missing references)
		//IL_0129: Unknown result type (might be due to invalid IL or missing references)
		//IL_012e: Unknown result type (might be due to invalid IL or missing references)
		//IL_012f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0134: Unknown result type (might be due to invalid IL or missing references)
		//IL_0139: Unknown result type (might be due to invalid IL or missing references)
		//IL_013e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0145: Unknown result type (might be due to invalid IL or missing references)
		//IL_014a: Unknown result type (might be due to invalid IL or missing references)
		//IL_014f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0186: Unknown result type (might be due to invalid IL or missing references)
		//IL_0191: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_0199: Unknown result type (might be due to invalid IL or missing references)
		//IL_019b: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_022f: Unknown result type (might be due to invalid IL or missing references)
		//IL_023a: Unknown result type (might be due to invalid IL or missing references)
		//IL_023f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0242: Unknown result type (might be due to invalid IL or missing references)
		//IL_0244: Unknown result type (might be due to invalid IL or missing references)
		//IL_0245: Unknown result type (might be due to invalid IL or missing references)
		//IL_024d: Unknown result type (might be due to invalid IL or missing references)
		//IL_024f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0254: Unknown result type (might be due to invalid IL or missing references)
		//IL_025e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0260: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02db: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_027a: Unknown result type (might be due to invalid IL or missing references)
		//IL_027c: Unknown result type (might be due to invalid IL or missing references)
		//IL_031d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0322: Unknown result type (might be due to invalid IL or missing references)
		//IL_0313: Unknown result type (might be due to invalid IL or missing references)
		//IL_0327: Unknown result type (might be due to invalid IL or missing references)
		//IL_0359: Unknown result type (might be due to invalid IL or missing references)
		//IL_035e: Unknown result type (might be due to invalid IL or missing references)
		//IL_034f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0363: Unknown result type (might be due to invalid IL or missing references)
		_lineRenderer.widthCurve = _widthCurve;
		_lineRenderer.widthMultiplier = _widthMultiplier;
		_lineRenderer.startColor = _startColor;
		_lineRenderer.endColor = _endColor;
		_lineRenderer.positionCount = _vertexCount;
		if (_outlineEnable)
		{
			((Renderer)_outLineRenderer).enabled = true;
			_outLineRenderer.startColor = _outlineStartColor;
			_outLineRenderer.endColor = _outlineEndColor;
			_outLineRenderer.positionCount = _vertexCount;
			_outLineRenderer.widthMultiplier = _widthMultiplier + _outlineWidthMultiplier;
			_outLineRenderer.widthCurve = _outlineWidthCurve;
		}
		else if ((Object)(object)_outLineRenderer != (Object)null)
		{
			((Renderer)_outLineRenderer).enabled = false;
		}
		Vector2 val = Vector2.op_Implicit(_endPoint.position - _startPoint.position);
		Vector2 normalized = ((Vector2)(ref val)).normalized;
		float chunck = ((Vector2)(ref val)).magnitude / (float)_vertexCount;
		Vector2 normalVector = Vector2.op_Implicit(Quaternion.Euler(0f, 0f, 90f) * Vector2.op_Implicit(normalized));
		Vector2 val2 = Vector2.op_Implicit(_startPoint.position);
		float amplitudeMultiplier = Random.Range(0f - _amplitude, _amplitude);
		float spike = (MMMaths.Chance(_sparkChance) ? _sparkAmount.value : 1f);
		Vector2 noise = Random.insideUnitCircle * _noise;
		Vector2 thunderVertex = GetThunderVertex(normalized, chunck, normalVector, val2, 0, amplitudeMultiplier, spike, noise);
		_lineRenderer.SetPosition(0, Vector2.op_Implicit(_fixedStartPosiiton ? val2 : thunderVertex));
		if (_outlineEnable)
		{
			_outLineRenderer.SetPosition(0, Vector2.op_Implicit(_fixedStartPosiiton ? val2 : thunderVertex));
		}
		for (int i = 1; i < _vertexCount - 1; i++)
		{
			amplitudeMultiplier = Random.Range(0f - _amplitude, _amplitude);
			spike = (MMMaths.Chance(_sparkChance) ? _sparkAmount.value : 1f);
			noise = Random.insideUnitCircle * _noise;
			thunderVertex = GetThunderVertex(normalized, chunck, normalVector, val2, i, amplitudeMultiplier, spike, noise);
			_lineRenderer.SetPosition(i, Vector2.op_Implicit(thunderVertex));
			if (_outlineEnable)
			{
				_outLineRenderer.SetPosition(i, Vector2.op_Implicit(thunderVertex));
			}
		}
		amplitudeMultiplier = Random.Range(0f - _amplitude, _amplitude);
		spike = (MMMaths.Chance(_sparkChance) ? _sparkAmount.value : 1f);
		noise = Random.insideUnitCircle * _noise;
		thunderVertex = GetThunderVertex(normalized, chunck, normalVector, val2, _vertexCount - 1, amplitudeMultiplier, spike, noise);
		_lineRenderer.SetPosition(_vertexCount - 1, Vector2.op_Implicit(_fixedEndPosiiton ? Vector2.op_Implicit(_endPoint.position) : thunderVertex));
		if (_outlineEnable)
		{
			_outLineRenderer.SetPosition(_vertexCount - 1, Vector2.op_Implicit(_fixedEndPosiiton ? Vector2.op_Implicit(_endPoint.position) : thunderVertex));
		}
	}

	private Vector2 GetThunderVertex(Vector2 direcitonVector, float chunck, Vector2 normalVector, Vector2 startPosition, int index, float amplitudeMultiplier, float spike, Vector2 noise)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		float num = _amplitudeCurve.Evaluate((float)index / (float)_lineRenderer.positionCount) * amplitudeMultiplier;
		Vector2 val = normalVector * (num * spike);
		return startPosition + chunck * (float)index * direcitonVector + val + noise;
	}
}
