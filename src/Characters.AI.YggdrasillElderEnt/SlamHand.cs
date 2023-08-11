using System.Collections;
using Characters.Operations;
using Hardmode;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.YggdrasillElderEnt;

public class SlamHand : MonoBehaviour
{
	[SerializeField]
	[FrameTime]
	private float _fistSlamAttackLength = 0.16f;

	[SerializeField]
	[FrameTime]
	private float _fistSlamRecoveryLength = 0.66f;

	[SerializeField]
	[FrameTime]
	private float _attackDelayFromtargeting = 0.16f;

	[SerializeField]
	[GetComponent]
	private Collider2D _collider;

	[SerializeField]
	private AIController _ai;

	[SerializeField]
	private GameObject _monsterBody;

	[SerializeField]
	private YggdrasillElderEntCollisionDetector _collisionDetector;

	[SerializeField]
	[Header("For Terrain")]
	private SpriteRenderer _sprite;

	[SerializeField]
	private Collider2D _terrainCollider;

	[Subcomponent(typeof(OperationInfos))]
	[SerializeField]
	private OperationInfos _onSign;

	[Subcomponent(typeof(OperationInfos))]
	[SerializeField]
	private OperationInfos _onSlamStart;

	[SerializeField]
	[Subcomponent(typeof(OperationInfos))]
	private OperationInfos _onSlamEnd;

	[SerializeField]
	[Subcomponent(typeof(OperationInfos))]
	private OperationInfos _onSlamEndInHardmode;

	[SerializeField]
	[Subcomponent(typeof(OperationInfos))]
	private OperationInfos _onRecoverySign;

	[Subcomponent(typeof(OperationInfos))]
	[SerializeField]
	private OperationInfos _onRecovery;

	[SerializeField]
	[Subcomponent(typeof(OperationInfos))]
	private OperationInfos _onEnd;

	private Coroutine _cChangeToTerrainReference;

	private Vector3 _origin;

	private Vector3 _source;

	public Vector3 destination { private get; set; }

	private void Start()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		_origin = ((Component)this).transform.position;
		_onSign.Initialize();
		_onSlamStart.Initialize();
		_onSlamEnd.Initialize();
		_onRecoverySign.Initialize();
		_onRecovery.Initialize();
		_onEnd.Initialize();
	}

	public void ActiavteHand()
	{
		((Component)this).gameObject.SetActive(true);
	}

	public void DeactivateHand()
	{
		((Component)this).gameObject.SetActive(false);
	}

	public void Sign()
	{
		((Component)_onSign).gameObject.SetActive(true);
		_onSign.Run(_ai.character);
	}

	public IEnumerator CSlam()
	{
		_source = _origin;
		yield return Chronometer.global.WaitForSeconds(_attackDelayFromtargeting);
		StartSlam();
		yield return CMoveTarget(_fistSlamAttackLength);
		EndSlam();
	}

	private void StartSlam()
	{
		((Component)_onSlamStart).gameObject.SetActive(true);
		_onSlamStart.Run(_ai.character);
		StartCollisionDetect();
	}

	private void EndSlam()
	{
		if (Singleton<HardmodeManager>.Instance.hardmode)
		{
			((Component)_onSlamEndInHardmode).gameObject.SetActive(true);
			_onSlamEndInHardmode.Run(_ai.character);
		}
		else
		{
			((Component)_onSlamEnd).gameObject.SetActive(true);
			_onSlamEnd.Run(_ai.character);
		}
		ActivateTerrain();
		_collisionDetector.Stop();
	}

	public IEnumerator CVibrate()
	{
		float elapsedTime = 0f;
		float length = 0.45f;
		float shakeAmount = 0.25f;
		CharacterChronometer chronometer = _ai.character.chronometer;
		while (true)
		{
			((Component)_sprite).transform.localPosition = Random.insideUnitSphere * shakeAmount;
			elapsedTime += ((ChronometerBase)chronometer.animation).deltaTime;
			if (elapsedTime > length)
			{
				break;
			}
			yield return null;
		}
		((Component)_sprite).transform.localPosition = Vector3.zero;
	}

	public IEnumerator CRecover()
	{
		_source = ((Component)this).transform.position;
		destination = _origin;
		((MonoBehaviour)this).StopCoroutine(_cChangeToTerrainReference);
		DeactivateTerrain();
		yield return CMoveTarget(_fistSlamRecoveryLength);
	}

	private IEnumerator CMoveTarget(float length)
	{
		float elapsedTime = 0f;
		CharacterChronometer chronometer = _ai.character.chronometer;
		Vector3 dest = destination;
		while (true)
		{
			Vector2 val = Vector2.Lerp(Vector2.op_Implicit(_source), Vector2.op_Implicit(dest), elapsedTime / length);
			((Component)this).transform.position = Vector2.op_Implicit(val);
			elapsedTime += ((ChronometerBase)chronometer.animation).deltaTime;
			if (elapsedTime > length)
			{
				break;
			}
			yield return null;
		}
		((Component)this).transform.position = dest;
	}

	private void ActivateTerrain()
	{
		_cChangeToTerrainReference = ((MonoBehaviour)this).StartCoroutine(CChangeTerrain());
	}

	private IEnumerator CChangeTerrain()
	{
		while (true)
		{
			yield return null;
			Character character = _ai.FindClosestPlayerBody(_collider);
			((Behaviour)_collider).enabled = true;
			if (!((Object)(object)character != (Object)null))
			{
				((Component)_terrainCollider).gameObject.SetActive(true);
			}
		}
	}

	private void DeactivateTerrain()
	{
		((Component)_terrainCollider).gameObject.SetActive(false);
	}

	private void StartCollisionDetect()
	{
		_collisionDetector.Initialize(_monsterBody, _collider);
		((MonoBehaviour)this).StartCoroutine(_collisionDetector.CRun(((Component)this).transform));
	}
}
