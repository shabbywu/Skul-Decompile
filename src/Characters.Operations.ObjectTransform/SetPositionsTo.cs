using System.Collections.Generic;
using UnityEngine;

namespace Characters.Operations.ObjectTransform;

public class SetPositionsTo : CharacterOperation
{
	[SerializeField]
	private Transform[] _objects;

	[SerializeField]
	private Transform[] _targets;

	public override void Run(Character owner)
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		ExtensionMethods.Shuffle<Transform>((IList<Transform>)_targets);
		for (int i = 0; i < _objects.Length; i++)
		{
			_objects[i].position = _targets[i].position;
		}
	}
}
