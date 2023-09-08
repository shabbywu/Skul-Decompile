using System.Collections;
using Characters.Operations;
using UnityEngine;

namespace Characters.AI.Behaviours.Chimera;

public class WreckDrop : Behaviour
{
	[SerializeField]
	private float _coolTime;

	[SerializeField]
	[Range(0f, 1f)]
	private float _triggerHealthPercent;

	[SerializeField]
	[Header("Out")]
	private OperationInfos _outReadyOperations;

	[SerializeField]
	private OperationInfos _outJumpOperations;

	[SerializeField]
	[Header("In")]
	private OperationInfos _inSignOperations;

	[SerializeField]
	private OperationInfos _inReadyOperations;

	[SerializeField]
	private OperationInfos _inWreckDropFireOperations;

	[SerializeField]
	private OperationInfos _inLandingOperations;

	private bool _coolDown = true;

	private void Awake()
	{
		_outReadyOperations.Initialize();
		_outJumpOperations.Initialize();
		_inReadyOperations.Initialize();
		_inWreckDropFireOperations.Initialize();
		_inLandingOperations.Initialize();
	}

	public void OutReady(Character character)
	{
		((Component)_outReadyOperations).gameObject.SetActive(true);
		_outReadyOperations.Run(character);
		character.status.unstoppable.Attach(character);
	}

	public void OutJump(Character character)
	{
		((Component)_outJumpOperations).gameObject.SetActive(true);
		_outJumpOperations.Run(character);
	}

	public void InSign(Character character)
	{
		((Component)_inSignOperations).gameObject.SetActive(true);
		_inSignOperations.Run(character);
	}

	public void InReady(Character character)
	{
		((Component)_inReadyOperations).gameObject.SetActive(true);
		_inReadyOperations.Run(character);
	}

	public void InWreckDrop(Character character)
	{
		((Component)_inWreckDropFireOperations).gameObject.SetActive(true);
		_inWreckDropFireOperations.Run(character);
	}

	public void InLanding(Character character)
	{
		((Component)_inLandingOperations).gameObject.SetActive(true);
		_inLandingOperations.Run(character);
	}

	public override IEnumerator CRun(AIController controller)
	{
		base.result = Result.Doing;
		((MonoBehaviour)this).StartCoroutine(CoolDown(controller.character.chronometer.animation));
		((Component)_inWreckDropFireOperations).gameObject.SetActive(true);
		_inWreckDropFireOperations.Run(controller.character);
		base.result = Result.Done;
		yield break;
	}

	private IEnumerator CoolDown(Chronometer chronometer)
	{
		_coolDown = false;
		yield return chronometer.WaitForSeconds(_coolTime);
		_coolDown = true;
	}

	public bool CanUse(Character character)
	{
		if (_coolDown)
		{
			return character.health.percent < (double)_triggerHealthPercent;
		}
		return false;
	}
}
