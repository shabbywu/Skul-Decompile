using UnityEngine;

namespace Characters.AI.Hero;

public class DualFinish : MonoBehaviour
{
	[SerializeField]
	[Header("Position")]
	private float _noise = 2f;

	[Header("Rotation")]
	[SerializeField]
	[MinMaxSlider(0f, 90f)]
	private Vector2 _angleRange;

	[SerializeField]
	private GameObject _clockWise;

	[SerializeField]
	private GameObject _counterClockWise;

	public void OnEnable()
	{
		SetPosition();
		SetRotation();
	}

	private void SetPosition()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = Random.insideUnitSphere * _noise;
		if (MMMaths.RandomBool())
		{
			_clockWise.transform.Translate(val);
			_counterClockWise.transform.localPosition = Vector2.op_Implicit(Vector2.zero);
		}
		else
		{
			_clockWise.transform.localPosition = Vector2.op_Implicit(Vector2.zero);
			_counterClockWise.transform.Translate(val);
		}
	}

	private void SetRotation()
	{
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		_clockWise.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(_angleRange.x, _angleRange.y));
		_counterClockWise.transform.rotation = Quaternion.Euler(0f, 0f, 180f - Random.Range(_angleRange.x, _angleRange.y));
	}
}
