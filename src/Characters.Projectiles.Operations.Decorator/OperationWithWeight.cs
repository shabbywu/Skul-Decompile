using System;
using System.Linq;
using UnityEngine;

namespace Characters.Projectiles.Operations.Decorator;

public class OperationWithWeight : MonoBehaviour
{
	[Serializable]
	public class Subcomponents : SubcomponentArray<OperationWithWeight>
	{
		public void RunWeightedRandom(IProjectile projectile)
		{
			Operation operation = null;
			float num = Random.Range(0f, base._components.Sum((OperationWithWeight c) => c._weight));
			for (int i = 0; i < base._components.Length; i++)
			{
				num -= base._components[i]._weight;
				if (num <= 0f)
				{
					operation = base._components[i]._operation;
					break;
				}
			}
			if (!((Object)(object)operation == (Object)null))
			{
				operation.Run(projectile);
			}
		}
	}

	[SerializeField]
	private float _weight = 1f;

	[SerializeField]
	[Operation.Subcomponent]
	private Operation _operation;

	public override string ToString()
	{
		string arg = (((Object)(object)_operation == (Object)null) ? "Do Nothing" : ((object)_operation).GetType().Name);
		return $"{arg}({_weight})";
	}
}
