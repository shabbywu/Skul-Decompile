using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Gear.Quintessences;

public class RunOperation : UseQuintessence
{
	[Subcomponent(typeof(OperationInfo))]
	[SerializeField]
	private OperationInfo.Subcomponents _operations;

	[SerializeField]
	private Transform _flipObject;

	protected override void Awake()
	{
		base.Awake();
		_quintessence.onEquipped += _operations.Initialize;
	}

	protected override void OnUse()
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)_flipObject != (Object)null)
		{
			if (_quintessence.owner.lookingDirection == Character.LookingDirection.Right)
			{
				_flipObject.localScale = Vector2.op_Implicit(new Vector2(1f, 1f));
			}
			else
			{
				_flipObject.localScale = Vector2.op_Implicit(new Vector2(-1f, 1f));
			}
		}
		((MonoBehaviour)this).StartCoroutine(_operations.CRun(_quintessence.owner));
	}

	private void OnDisable()
	{
		_operations.StopAll();
	}
}
