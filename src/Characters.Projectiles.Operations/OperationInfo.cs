using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Characters.Projectiles.Operations;

internal class OperationInfo : MonoBehaviour
{
	[Serializable]
	internal class Subcomponents : SubcomponentArray<OperationInfo>
	{
		internal void Sort()
		{
			base._components = base._components.OrderBy((OperationInfo operation) => operation.timeToTrigger).ToArray();
		}

		internal IEnumerator CRun(IProjectile projectile)
		{
			int operationIndex = 0;
			float time = 0f;
			while (operationIndex < base._components.Length)
			{
				for (; operationIndex < base._components.Length && time >= base._components[operationIndex].timeToTrigger; operationIndex++)
				{
					base._components[operationIndex].operation.Run(projectile);
				}
				yield return null;
				time += ((ChronometerBase)projectile.owner.chronometer.projectile).deltaTime;
			}
		}
	}

	[SerializeField]
	[FrameTime]
	private float _timeToTrigger;

	[Operation.Subcomponent]
	[SerializeField]
	private Operation _operation;

	public Operation operation => _operation;

	public float timeToTrigger => _timeToTrigger;

	public override string ToString()
	{
		string arg = (((Object)(object)_operation == (Object)null) ? "null" : ((object)_operation).GetType().Name);
		return $"{_timeToTrigger:0.##}s({_timeToTrigger * 60f:0.##}f), {arg}";
	}
}
