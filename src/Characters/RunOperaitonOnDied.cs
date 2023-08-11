using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters;

public sealed class RunOperaitonOnDied : MonoBehaviour
{
	[SerializeField]
	private Character _character;

	[SerializeField]
	[Subcomponent(typeof(OperationInfos))]
	private OperationInfos _infos;

	private void Awake()
	{
		_character.health.onDied += OnDied;
	}

	private void OnDied()
	{
		if (!((Object)(object)_character == (Object)null))
		{
			_character.health.onDied -= OnDied;
			if (!((Object)(object)_infos == (Object)null))
			{
				((Component)_infos).gameObject.SetActive(true);
				_infos.Run(_character);
			}
		}
	}
}
