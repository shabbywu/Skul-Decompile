using System.Collections;
using Characters;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Level.Traps;

public class ArcherTrap : MonoBehaviour
{
	[SerializeField]
	private Character _character;

	[Subcomponent(typeof(OperationInfos))]
	[SerializeField]
	private OperationInfos _readyOperations;

	[SerializeField]
	[Subcomponent(typeof(OperationInfos))]
	private OperationInfos _activeOperations;

	[SerializeField]
	private float _activeDelay;

	[SerializeField]
	private float _lifeTime;

	private void Awake()
	{
		_readyOperations.Initialize();
		_activeOperations.Initialize();
	}

	private void OnEnable()
	{
		Ready();
		((MonoBehaviour)this).StartCoroutine(CActivate());
	}

	private void Ready()
	{
		((Component)_readyOperations).gameObject.SetActive(true);
		_readyOperations.Run(_character);
	}

	private void Hide()
	{
		((Component)_character).gameObject.SetActive(false);
	}

	private IEnumerator CActivate()
	{
		yield return Chronometer.global.WaitForSeconds(_activeDelay);
		((Component)_activeOperations).gameObject.SetActive(true);
		_activeOperations.Run(_character);
		((MonoBehaviour)this).StartCoroutine(CSleep());
	}

	private IEnumerator CSleep()
	{
		yield return Chronometer.global.WaitForSeconds(_lifeTime);
		Hide();
	}
}
