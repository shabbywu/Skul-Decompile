using System.Collections.Generic;
using Characters.Movements;
using UnityEngine;

namespace Level;

public class DynamicPlatform : MonoBehaviour
{
	private readonly List<CharacterController2D> _controllers = new List<CharacterController2D>();

	private Vector3 _positionBefore;

	private void Awake()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		_positionBefore = ((Component)this).transform.position;
	}

	private void Update()
	{
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		if (_controllers.Count > 0)
		{
			Vector3 val = ((Component)this).transform.position - _positionBefore;
			Physics2D.SyncTransforms();
			for (int i = 0; i < _controllers.Count; i++)
			{
				_controllers[i].Move(Vector2.op_Implicit(val));
			}
		}
		_positionBefore = ((Component)this).transform.position;
	}

	public void Attach(CharacterController2D controller)
	{
		_controllers.Add(controller);
	}

	public bool Detach(CharacterController2D controller)
	{
		return _controllers.Remove(controller);
	}
}
