using UnityEngine;

namespace FX;

[RequireComponent(typeof(LineRenderer))]
public class BezierCurve : MonoBehaviour
{
	[SerializeField]
	private LineRenderer _lineRenderer;

	[SerializeField]
	private Vector2[] _points = (Vector2[])(object)new Vector2[4];

	public int count => _points.Length;

	public void SetVector(int index, Vector2 vector)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		_points[index] = vector;
	}

	public void SetStart(Vector2 vector)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		SetVector(0, vector);
	}

	public void SetEnd(Vector2 vector)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		SetVector(count - 1, vector);
	}

	public void UpdateCurve()
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < _lineRenderer.positionCount; i++)
		{
			float time = (float)i / (float)(_lineRenderer.positionCount - 1);
			_lineRenderer.SetPosition(i, Vector2.op_Implicit(MMMaths.BezierCurve(_points, time)));
		}
	}
}
