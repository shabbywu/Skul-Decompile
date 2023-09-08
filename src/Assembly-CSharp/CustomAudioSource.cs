using System.Collections;
using Singletons;
using UnityEngine;

public class CustomAudioSource : MonoBehaviour
{
	[SerializeField]
	private AudioSource _audioSource;

	private float _audioOriginVolume;

	private float _fadeFactor = 1f;

	private void Awake()
	{
		if ((Object)(object)_audioSource == (Object)null)
		{
			_audioSource = ((Component)this).GetComponent<AudioSource>();
		}
		if ((Object)(object)_audioSource != (Object)null)
		{
			_audioOriginVolume = _audioSource.volume;
		}
	}

	public void Play()
	{
		if (PersistentSingleton<SoundManager>.Instance.sfxEnabled && !((Object)(object)_audioSource == (Object)null))
		{
			float volume = PersistentSingleton<SoundManager>.Instance.masterVolume * PersistentSingleton<SoundManager>.Instance.sfxVolume * _audioOriginVolume * _fadeFactor;
			_audioSource.volume = volume;
			_audioSource.Play();
		}
	}

	public void Stop()
	{
		_audioSource.Stop();
	}

	public void FadeOut(float time = 1f)
	{
		((MonoBehaviour)this).StartCoroutine(CFadeOut(time));
	}

	public IEnumerator CFadeOut(float time = 1f)
	{
		float t = 0f;
		_ = PersistentSingleton<SoundManager>.Instance.masterVolume * PersistentSingleton<SoundManager>.Instance.sfxVolume * _audioOriginVolume * _fadeFactor;
		yield return null;
		float volume;
		while (t < time)
		{
			_fadeFactor = (time - t) / time;
			yield return null;
			t += Time.unscaledDeltaTime;
			volume = PersistentSingleton<SoundManager>.Instance.masterVolume * PersistentSingleton<SoundManager>.Instance.sfxVolume * _audioOriginVolume * _fadeFactor;
			_audioSource.volume = volume;
		}
		_fadeFactor = 0f;
		volume = PersistentSingleton<SoundManager>.Instance.masterVolume * PersistentSingleton<SoundManager>.Instance.sfxVolume * _audioOriginVolume * _fadeFactor;
		_audioSource.volume = volume;
	}

	public void FadeIn(float time = 1f)
	{
		((MonoBehaviour)this).StartCoroutine(CFadeIn(time));
	}

	public IEnumerator CFadeIn(float time = 1f)
	{
		float t = 0f;
		_ = PersistentSingleton<SoundManager>.Instance.masterVolume * PersistentSingleton<SoundManager>.Instance.sfxVolume * _audioOriginVolume * _fadeFactor;
		yield return null;
		float volume;
		while (t < time)
		{
			_fadeFactor = (0f + t) / time;
			yield return null;
			t += Time.unscaledDeltaTime;
			volume = PersistentSingleton<SoundManager>.Instance.masterVolume * PersistentSingleton<SoundManager>.Instance.sfxVolume * _audioOriginVolume * _fadeFactor;
			_audioSource.volume = volume;
		}
		_fadeFactor = 1f;
		volume = PersistentSingleton<SoundManager>.Instance.masterVolume * PersistentSingleton<SoundManager>.Instance.sfxVolume * _audioOriginVolume * _fadeFactor;
		_audioSource.volume = volume;
	}
}
