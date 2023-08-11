using Characters;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Level.Traps;

public class Orb : MonoBehaviour
{
	[SerializeField]
	private Character _owner;

	[SerializeField]
	[Subcomponent(typeof(OperationInfo))]
	private OperationInfo.Subcomponents _onEnable;

	private float _radian;

	private void Awake()
	{
		_onEnable.Initialize();
	}

	public void Initialize(float startAngle)
	{
		_radian = startAngle;
	}

	private void OnEnable()
	{
		((MonoBehaviour)this).StartCoroutine(_onEnable.CRun(_owner));
	}

	public void MoveCenteredOn(Vector3 pivot, float radious, float amount)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = pivot - ((Component)_owner).transform.position;
		_radian += amount;
		_owner.movement.MoveHorizontal(Vector2.op_Implicit(val) + new Vector2(Mathf.Cos(_radian), Mathf.Sin(_radian)) * radious);
	}
}
