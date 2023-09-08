using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters;

public class CharacterHitOperation : MonoBehaviour
{
	[SerializeField]
	[GetComponent]
	private Character _character;

	[GetComponent]
	[SerializeField]
	private CharacterHealth _health;

	[SerializeField]
	[Subcomponent(typeof(OperationInfo))]
	private OperationInfo.Subcomponents _hitOperations;

	private void Awake()
	{
		_health.onTookDamage += OnTookDamage;
	}

	private void OnTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
	{
		if (!_health.dead)
		{
			((MonoBehaviour)this).StartCoroutine(_hitOperations.CRun(_character));
		}
	}
}
