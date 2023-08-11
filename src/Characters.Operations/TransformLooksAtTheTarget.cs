using Characters.AI;
using UnityEngine;

namespace Characters.Operations;

public class TransformLooksAtTheTarget : CharacterOperation
{
	[SerializeField]
	private Transform _transform;

	[SerializeField]
	private AIController _controller;

	public override void Run(Character owner)
	{
		_transform.LookAt(((Component)_controller.target).transform);
	}
}
