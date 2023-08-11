using System.Collections;
using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

[TaskDescription("Chronometer로 체크하는 쿨다운")]
public sealed class ChronometerCoolDown : Conditional
{
	public SharedCharacter chronomaterOwner;

	public SharedFloat duration = 2f;

	public SharedBool preCoolDown;

	private bool _canUse;

	private Character _owner;

	private CoroutineReference _cooldownReference;

	public override void OnAwake()
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		((Task)this).OnAwake();
		if (((SharedVariable<bool>)preCoolDown).Value)
		{
			_owner = ((SharedVariable<Character>)chronomaterOwner).Value;
			_cooldownReference = CoroutineReferenceExtension.StartCoroutineWithReference((MonoBehaviour)(object)_owner, CCooldown(_owner.chronometer.master));
		}
		else
		{
			_canUse = true;
		}
	}

	public override TaskStatus OnUpdate()
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		if (((SharedVariable<float>)duration).Value != 0f)
		{
			if (_canUse)
			{
				_owner = ((SharedVariable<Character>)chronomaterOwner).Value;
				_cooldownReference = CoroutineReferenceExtension.StartCoroutineWithReference((MonoBehaviour)(object)_owner, CCooldown(_owner.chronometer.master));
				return (TaskStatus)2;
			}
			return (TaskStatus)1;
		}
		return (TaskStatus)2;
	}

	private IEnumerator CCooldown(Chronometer chronometer)
	{
		_canUse = false;
		float elapsed = 0f;
		float durationValue = ((SharedVariable<float>)duration).Value;
		while (elapsed < durationValue)
		{
			elapsed += ((ChronometerBase)chronometer).deltaTime;
			yield return null;
		}
		_canUse = true;
	}
}
