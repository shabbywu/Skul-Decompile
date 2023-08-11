using System.Collections;
using FX;
using Singletons;
using UnityEngine;

namespace Characters;

public class RepeatPlaySound : MonoBehaviour
{
	[SerializeField]
	private float _startDelay;

	[SerializeField]
	private Vector2 _interval;

	[SerializeField]
	private bool _playOnAwake = true;

	[SerializeField]
	private SoundInfo _audioClipInfo;

	[SerializeField]
	private Transform _position;

	private ReusableAudioSource _reusableAudioSource;

	private Coroutine _coroutine;

	private float _elapsed;

	private float interval => Random.Range(_interval.x, _interval.y);

	public void Play()
	{
		_coroutine = ((MonoBehaviour)this).StartCoroutine(CLoop());
	}

	public void Stop()
	{
		ReusableAudioSource reusableAudioSource = _reusableAudioSource;
		if (reusableAudioSource != null)
		{
			reusableAudioSource.Stop();
		}
		if (_coroutine != null)
		{
			((MonoBehaviour)this).StopCoroutine(_coroutine);
		}
	}

	private void Start()
	{
		if (_playOnAwake)
		{
			Play();
		}
	}

	private IEnumerator CLoop()
	{
		yield return Chronometer.global.WaitForSeconds(_startDelay);
		while (true)
		{
			_reusableAudioSource = PersistentSingleton<SoundManager>.Instance.PlaySound(_audioClipInfo, ((Object)(object)_position == (Object)null) ? ((Component)this).transform.position : _position.position);
			yield return Chronometer.global.WaitForSeconds(interval);
		}
	}

	private void OnDisable()
	{
		Stop();
	}

	private void OnDestroy()
	{
		Stop();
	}
}
