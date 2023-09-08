using System.Collections;
using Characters.Operations.Fx;
using UnityEditor;
using UnityEngine;

namespace Characters.Actions;

[RequireComponent(typeof(Action))]
public class LoopSoundOnAction : MonoBehaviour
{
	[GetComponent]
	[SerializeField]
	private Action _action;

	[Subcomponent(typeof(PlaySound))]
	[SerializeField]
	private PlaySound _sound;

	[Tooltip("사운드를 반복 재생하는 인터벌 값입니다.\n실제 값 보다 0.05~0.1초 가량 더 적게 설정하면 자연스럽게 Loop 됩니다.")]
	[SerializeField]
	private float _repeatInterval = 1f;

	private bool _running;

	private Coroutine _playLoopSound;

	private void Awake()
	{
		_sound.Initialize();
		_action.onStart += StartLoop;
	}

	private void OnDisable()
	{
		if (_playLoopSound != null)
		{
			((MonoBehaviour)this).StopCoroutine(_playLoopSound);
			StopLoop();
		}
	}

	private void StartLoop()
	{
		if (!_running)
		{
			if (_playLoopSound != null)
			{
				((MonoBehaviour)this).StopCoroutine(_playLoopSound);
			}
			_running = true;
			_playLoopSound = ((MonoBehaviour)this).StartCoroutine(CPlayLoopSound());
		}
	}

	private void StopLoop()
	{
		_sound.Stop();
		_running = false;
	}

	private IEnumerator CPlayLoopSound()
	{
		float delay = 0f;
		float time = Time.time;
		while (_action.running)
		{
			if (delay <= 0f)
			{
				_sound.Run(_action.owner);
				delay += _repeatInterval;
			}
			delay -= Time.time - time;
			time = Time.time;
			yield return null;
		}
		StopLoop();
	}
}
