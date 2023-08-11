using Characters;
using Characters.Abilities;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Level.Traps;

public class PowderKeg : MonoBehaviour
{
	[SerializeField]
	private Character _character;

	[SerializeField]
	private GameObject _remain1;

	[SerializeField]
	private GameObject _remain2;

	[SerializeField]
	private ParticleEffectInfo _particleEffectInfo;

	[Subcomponent(typeof(OperationInfo))]
	[SerializeField]
	private OperationInfo.Subcomponents _operationsOnDie;

	[AbilityAttacher.Subcomponent]
	[SerializeField]
	private AbilityAttacher _abilityAttacher;

	private void Awake()
	{
		_operationsOnDie.Initialize();
		_abilityAttacher.Initialize(_character);
		_abilityAttacher.StartAttach();
		_character.health.onDie += Run;
	}

	private void OnDestroy()
	{
		_abilityAttacher.StopAttach();
		_character.health.onDie -= Run;
	}

	private void Run()
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		_character.health.onDie -= Run;
		_particleEffectInfo.Emit(Vector2.op_Implicit(((Component)_character).transform.position), ((Collider2D)_character.collider).bounds, Vector2.up * 3f);
		if (MMMaths.RandomBool())
		{
			_remain1.gameObject.SetActive(true);
		}
		else
		{
			_remain2.gameObject.SetActive(true);
		}
		((Component)_character.@base).gameObject.SetActive(false);
		((MonoBehaviour)this).StartCoroutine(_operationsOnDie.CRun(_character));
	}
}
