using UnityEngine;

namespace CutScenes.Shots.Events;

public class SetTransformPosition : Event
{
	[SerializeField]
	private Transform _target;

	[SerializeField]
	private Transform _destinationTransform;

	[SerializeField]
	private Vector3 _offset;

	public override void Run()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		_target.position = _destinationTransform.position + _offset;
	}
}
