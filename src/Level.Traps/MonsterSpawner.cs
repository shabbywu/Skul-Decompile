using System.Collections;
using Characters;
using UnityEngine;

namespace Level.Traps;

public class MonsterSpawner : MonoBehaviour
{
	private enum Target
	{
		LooseSubject,
		StrangeSubject
	}

	[SerializeField]
	private Prop _prop;

	[SerializeField]
	private GameObject _destroyedBody;

	[SerializeField]
	private Character _looseSubject;

	[SerializeField]
	private Character _strangeSubject;

	[SerializeField]
	private bool _containInWave = true;

	[SerializeField]
	private Target _target;

	[SerializeField]
	private CustomFloat _cinematicDuration;

	private void Awake()
	{
		_prop.onDestroy += SpawnCharacter;
	}

	private void SpawnCharacter()
	{
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		_destroyedBody.SetActive(true);
		Character character = ((_target == Target.LooseSubject) ? _looseSubject : _strangeSubject);
		((Component)character).gameObject.SetActive(true);
		if (_containInWave)
		{
			Map.Instance.waveContainer.Attach(character);
		}
		((Behaviour)character.collider).enabled = true;
		CoroutineReferenceExtension.StartCoroutineWithReference((MonoBehaviour)(object)character, CAttachCinematic(character));
	}

	private IEnumerator CAttachCinematic(Character character)
	{
		character.cinematic.Attach((object)this);
		yield return (object)new WaitForSeconds(_cinematicDuration.value);
		character.cinematic.Detach((object)this);
	}
}
