using System;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace Characters.Operations;

internal class OperationInfo : MonoBehaviour
{
	[Serializable]
	internal class Subcomponents : SubcomponentArray<OperationInfo>
	{
		internal void Initialize()
		{
			for (int i = 0; i < _components.Length; i++)
			{
				_components[i].operation.Initialize();
			}
		}

		internal void Sort()
		{
			_components = _components.OrderBy((OperationInfo operation) => operation.timeToTrigger).ToArray();
		}

		internal IEnumerator CRun(Character target)
		{
			return CRun(target, target);
		}

		internal IEnumerator CRun(Character owner, Character target)
		{
			int operationIndex = 0;
			float time = 0f;
			while (operationIndex < _components.Length)
			{
				for (; operationIndex < _components.Length && time >= _components[operationIndex].timeToTrigger; operationIndex++)
				{
					_components[operationIndex].operation.Run(owner, target);
				}
				yield return null;
				time += owner.chronometer.animation.deltaTime;
			}
		}

		internal void StopAll()
		{
			for (int i = 0; i < base.components.Length; i++)
			{
				if (!base.components[i]._stay)
				{
					base.components[i]._operation.Stop();
				}
			}
		}

		internal void ForceStopAll()
		{
			for (int i = 0; i < base.components.Length; i++)
			{
				base.components[i]._operation.Stop();
			}
		}
	}

	[FrameTime]
	[SerializeField]
	private float _timeToTrigger;

	[SerializeField]
	private bool _stay;

	[SerializeField]
	[CharacterOperation.Subcomponent]
	private CharacterOperation _operation;

	public CharacterOperation operation => _operation;

	public float timeToTrigger => _timeToTrigger;

	public bool stay => _stay;

	private void OnDestroy()
	{
		_operation = null;
	}

	public override string ToString()
	{
		string arg = (((Object)(object)_operation == (Object)null) ? "null" : ((object)_operation).GetType().Name);
		return $"{_timeToTrigger:0.##}s({_timeToTrigger * 60f:0.##}f), {arg}";
	}
}
