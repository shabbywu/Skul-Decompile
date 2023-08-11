using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Pope;

public sealed class Barrier : MonoBehaviour
{
	[SerializeField]
	private Character _owner;

	[SerializeField]
	[Subcomponent(typeof(OperationInfos))]
	private OperationInfos _onSpawnOperations;

	[SerializeField]
	[Subcomponent(typeof(OperationInfos))]
	private OperationInfos _onCrackOperations;

	[SerializeField]
	[Subcomponent(typeof(OperationInfos))]
	private OperationInfos _onDespawnOperations;

	public void Spawn()
	{
		if (!((Object)(object)_onSpawnOperations == (Object)null))
		{
			((Component)_onSpawnOperations).gameObject.SetActive(true);
			_onSpawnOperations.Run(_owner);
		}
	}

	public void Crack()
	{
		if (!((Object)(object)_onCrackOperations == (Object)null))
		{
			((Component)_onCrackOperations).gameObject.SetActive(true);
			_onCrackOperations.Run(_owner);
		}
	}

	public void Despawn()
	{
		if (!((Object)(object)_onDespawnOperations == (Object)null))
		{
			((Component)_onDespawnOperations).gameObject.SetActive(true);
			_onDespawnOperations.Run(_owner);
		}
	}
}
