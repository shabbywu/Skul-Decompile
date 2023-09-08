using UnityEngine;

namespace BT;

public class TranslateTowards : Node
{
	[SerializeField]
	private CustomFloat _speedX;

	[SerializeField]
	private CustomFloat _speedY;

	[Range(0f, 1f)]
	[SerializeField]
	private float _rightChance;

	[Range(0f, 1f)]
	[SerializeField]
	private float _upChance;

	private float _speedXValue;

	private float _speedYValue;

	protected override void OnInitialize()
	{
		_speedXValue = _speedX.value;
		_speedYValue = _speedY.value;
		base.OnInitialize();
	}

	protected override NodeState UpdateDeltatime(Context context)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_008c: Unknown result type (might be due to invalid IL or missing references)
		Transform val = context.Get<Transform>(Key.OwnerTransform);
		if ((Object)(object)val == (Object)null)
		{
			Debug.LogError((object)"OwnerTransform is null");
			return NodeState.Fail;
		}
		float deltaTime = context.deltaTime;
		Vector2 zero = Vector2.zero;
		zero += (MMMaths.Chance(_rightChance) ? Vector2.right : Vector2.left) * _speedXValue;
		zero += (MMMaths.Chance(_upChance) ? Vector2.up : Vector2.down) * _speedYValue;
		val.Translate(Vector2.op_Implicit(zero * deltaTime));
		return NodeState.Success;
	}
}
