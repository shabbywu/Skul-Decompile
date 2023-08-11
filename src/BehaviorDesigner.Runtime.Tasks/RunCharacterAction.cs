using System.Collections;
using Characters;
using Characters.Actions;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

[TaskDescription("액션을 실행한다")]
[TaskIcon("Assets/Behavior Designer/Icon/RunCharacterAction.png")]
public sealed class RunCharacterAction : Action
{
	[SerializeField]
	private SharedCharacter _character;

	[SerializeField]
	private SharedCharacterAction _action;

	[SerializeField]
	private bool _tryUntilSucceed;

	[SerializeField]
	private bool _checkLastMotionStayTime;

	private bool _running;

	private bool _trying;

	private bool _stayActionRunning;

	private Character _characterValue;

	private Action _actionValue;

	private Motion _lastMotion;

	private CoroutineReference _coroutineReference;

	public override void OnAwake()
	{
		_characterValue = ((SharedVariable<Character>)_character).Value;
		_actionValue = ((SharedVariable<Action>)_action).Value;
		if ((Object)(object)_characterValue == (Object)null || (Object)(object)_actionValue == (Object)null)
		{
			return;
		}
		_actionValue.Initialize(_characterValue);
		if (_checkLastMotionStayTime)
		{
			_lastMotion = _actionValue.motions[_actionValue.motions.Length - 1];
			if (_lastMotion.stay)
			{
				_lastMotion.onStart += CheckLastMotionStayTime;
			}
		}
	}

	public override void OnStart()
	{
		if (!_characterValue.stunedOrFreezed)
		{
			_trying = _actionValue.TryStart();
			_running = true;
		}
	}

	public override TaskStatus OnUpdate()
	{
		if (_action != null)
		{
			if (!_running)
			{
				if (!_characterValue.stunedOrFreezed)
				{
					_running = true;
					_trying = _actionValue.TryStart();
					return (TaskStatus)3;
				}
				return (TaskStatus)3;
			}
			if (_tryUntilSucceed && !_trying)
			{
				_trying = _actionValue.TryStart();
				if (_trying)
				{
					return (TaskStatus)3;
				}
				return (TaskStatus)1;
			}
			if (_checkLastMotionStayTime && _lastMotion.stay && _stayActionRunning)
			{
				return (TaskStatus)2;
			}
			if (!_actionValue.running)
			{
				return (TaskStatus)2;
			}
			return (TaskStatus)3;
		}
		return (TaskStatus)1;
	}

	public override void OnEnd()
	{
		((Task)this).OnEnd();
		_stayActionRunning = false;
		_running = false;
		_trying = false;
	}

	private void CheckLastMotionStayTime()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		((CoroutineReference)(ref _coroutineReference)).Stop();
		_coroutineReference = CoroutineReferenceExtension.StartCoroutineWithReference((MonoBehaviour)(object)_characterValue, CCheckLastMotionStayTime());
	}

	private IEnumerator CCheckLastMotionStayTime()
	{
		_stayActionRunning = false;
		float t = 0f;
		float length = _actionValue.motions[_actionValue.motions.Length - 1].length;
		while (t < length)
		{
			t += ((ChronometerBase)_characterValue.chronometer.animation).deltaTime;
			yield return null;
		}
		_stayActionRunning = true;
	}
}
