using UnityEngine;

public class FixedParallax : MonoBehaviour
{
	[Tooltip("카메라 위치로 인해 변경되는 오브젝트의 중심점을 움직입니다.\n카메라의 중심이 정확히 이 오브젝트의 위치 + offset에 위치할 때 이 오브젝트가 에디터상에서 배치한 그 위치에 보여집니다.")]
	[SerializeField]
	private Vector2 _offset;

	[SerializeField]
	[Tooltip("카메라로부터 떨어진 거리에 이 값을 곱한 만큼 움직입니다. 0이면 움직이지 않고, 1이면 항상 카메라에 붙어 다니게 됩니다. 값의 범위에 제한은 없습니다.")]
	private Vector2 _positionRatio;

	private Vector3 _origin;

	private void Awake()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		_origin = ((Component)this).transform.position;
	}

	private void LateUpdate()
	{
		SetPosition();
	}

	private void SetPosition()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = Vector2.op_Implicit(_origin);
		Vector2 val2 = Vector2.op_Implicit(((Component)Camera.main).transform.position - (_origin + Vector2.op_Implicit(_offset))) * _positionRatio;
		val += val2;
		((Component)this).transform.position = Vector2.op_Implicit(val);
	}
}
