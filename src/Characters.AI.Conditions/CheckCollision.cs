using Characters.Operations;
using PhysicsUtils;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Conditions;

public sealed class CheckCollision : Condition
{
	[Subcomponent(typeof(OperationInfos))]
	[SerializeField]
	private OperationInfos _operationInfos;

	[SerializeField]
	private Transform _point;

	[SerializeField]
	private BoxCollider2D _box;

	[SerializeField]
	private LayerMask _checkLayer;

	private static readonly NonAllocOverlapper _nonAllocOverlapper;

	static CheckCollision()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		_nonAllocOverlapper = new NonAllocOverlapper(1);
	}

	private void Awake()
	{
		_operationInfos.Initialize();
	}

	protected override bool Check(AIController controller)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		((Component)_operationInfos).gameObject.SetActive(true);
		_operationInfos.Run(controller.character);
		Vector2 size = _box.size;
		Vector2 val = Vector2.op_Implicit(_point.position) + ((Collider2D)_box).offset;
		((ContactFilter2D)(ref _nonAllocOverlapper.contactFilter)).SetLayerMask(_checkLayer);
		ReadonlyBoundedList<Collider2D> results = _nonAllocOverlapper.OverlapBox(val, size, 0f).results;
		Debug.DrawLine(((Component)controller.character).transform.position, _point.position, Color.red, 10f);
		return results.Count > 0;
	}
}
