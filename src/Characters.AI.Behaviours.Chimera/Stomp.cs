using System.Collections;
using Characters.Operations;
using UnityEngine;

namespace Characters.AI.Behaviours.Chimera;

public class Stomp : Behaviour
{
	[SerializeField]
	private float _coolTime;

	[SerializeField]
	private Collider2D _trigger;

	[Header("Ready")]
	[SerializeField]
	private OperationInfos _readyOperations;

	[Header("Attack")]
	[SerializeField]
	private OperationInfos _attackOperations;

	[SerializeField]
	[Header("Hit")]
	private OperationInfos _terrainHitOperations;

	[SerializeField]
	[Header("End")]
	private OperationInfos _endOperations;

	private bool _coolDown = true;

	private void Awake()
	{
		_readyOperations.Initialize();
		_attackOperations.Initialize();
		_endOperations.Initialize();
		_terrainHitOperations.Initialize();
	}

	public void Ready(Character character)
	{
		((Component)_readyOperations).gameObject.SetActive(true);
		_readyOperations.Run(character);
	}

	public void Hit(Character character)
	{
		((Component)_terrainHitOperations).gameObject.SetActive(true);
		_terrainHitOperations.Run(character);
	}

	public void End(Character character)
	{
		((Component)_endOperations).gameObject.SetActive(true);
		_endOperations.Run(character);
	}

	public override IEnumerator CRun(AIController controller)
	{
		base.result = Result.Doing;
		((MonoBehaviour)this).StartCoroutine(CoolDown(controller.character.chronometer.master));
		((Component)_attackOperations).gameObject.SetActive(true);
		_attackOperations.Run(controller.character);
		base.result = Result.Done;
		yield break;
	}

	private IEnumerator CoolDown(Chronometer chronometer)
	{
		_coolDown = false;
		yield return ChronometerExtension.WaitForSeconds((ChronometerBase)(object)chronometer, _coolTime);
		_coolDown = true;
	}

	public bool CanUse(AIController controller)
	{
		if (!_coolDown)
		{
			return false;
		}
		return (Object)(object)controller.FindClosestPlayerBody(_trigger) != (Object)null;
	}
}
