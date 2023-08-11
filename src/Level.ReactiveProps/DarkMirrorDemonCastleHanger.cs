using Characters;
using Characters.Actions;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Level.ReactiveProps;

public sealed class DarkMirrorDemonCastleHanger : MonoBehaviour
{
	[Subcomponent(typeof(OperationInfo))]
	[SerializeField]
	private OperationInfo.Subcomponents _operations;

	private bool _broken;

	private void Awake()
	{
		_operations.Initialize();
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		if (_broken)
		{
			return;
		}
		Character component = ((Component)collision).GetComponent<Character>();
		if ((Object)(object)component == (Object)null || component.movement.velocity.y > 0f)
		{
			return;
		}
		foreach (Action action in component.actions)
		{
			if (action.running && action.type == Action.Type.JumpAttack && !((Object)(object)((Component)action).GetComponent<PowerbombAction>() == (Object)null))
			{
				Break(component);
			}
		}
	}

	private void Break(Character owner)
	{
		_broken = true;
		((MonoBehaviour)Map.Instance).StartCoroutine(_operations.CRun(owner));
		Object.Destroy((Object)(object)this);
	}
}
