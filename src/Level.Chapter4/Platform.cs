using System.Collections;
using Characters;
using Characters.Monsters;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Level.Chapter4;

public class Platform : MonoBehaviour, IPurification, IDivineCrossHelp
{
	[Header("Purification")]
	[SerializeField]
	[Subcomponent(typeof(OperationInfos))]
	private OperationInfos _readyOperations;

	[Subcomponent(typeof(OperationInfos))]
	[SerializeField]
	private OperationInfos _operations;

	[Space]
	[SerializeField]
	private float _duration;

	[SerializeField]
	private Monster _tentaclePrefab;

	[SerializeField]
	private Transform _spawnPoint;

	[Header("Divine Cross")]
	[SerializeField]
	private Transform _divineCrossFirePosition;

	[SerializeField]
	private Collider2D _collider;

	public Collider2D collider => _collider;

	public bool tentacleAlives { get; set; }

	public Transform firePosition => _divineCrossFirePosition;

	private void Awake()
	{
		_readyOperations.Initialize();
		_operations.Initialize();
	}

	public void ShowSign(Character owner)
	{
		((Component)_readyOperations).gameObject.SetActive(true);
		_readyOperations.Run(owner);
	}

	public void Purifiy(Character owner)
	{
		((MonoBehaviour)this).StartCoroutine(CRunPurifiy(owner));
	}

	private IEnumerator CRunPurifiy(Character owner)
	{
		Monster summoned = _tentaclePrefab.Summon(_spawnPoint.position);
		Map.Instance.waveContainer.summonWave.Attach(summoned.character);
		summoned.character.health.onDied += OnDied;
		tentacleAlives = true;
		((Component)_operations).gameObject.SetActive(true);
		_operations.Run(owner);
		yield return ChronometerExtension.WaitForSeconds((ChronometerBase)(object)owner.chronometer.master, _duration);
		void OnDied()
		{
			tentacleAlives = false;
			summoned.character.health.onDied -= OnDied;
		}
	}

	public Vector3 GetFirePosition()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		return _divineCrossFirePosition.position;
	}
}
