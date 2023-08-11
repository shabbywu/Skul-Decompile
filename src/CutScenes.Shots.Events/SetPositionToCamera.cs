using UnityEngine;

namespace CutScenes.Shots.Events;

public class SetPositionToCamera : Event
{
	[SerializeField]
	private Transform _target;

	[SerializeField]
	private Vector3 _offset;

	public override void Run()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		_target.position = ((Component)Camera.main).transform.position + _offset;
	}
}
