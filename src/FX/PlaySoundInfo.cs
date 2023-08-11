using System.Collections;
using Singletons;
using UnityEngine;

namespace FX;

public class PlaySoundInfo : MonoBehaviour
{
	[Information(/*Could not decode attribute arguments.*/)]
	[SerializeField]
	private bool _playOnEnable = true;

	[SerializeField]
	private SoundInfo _soundInfo;

	private AudioSource _audioSource;

	public bool playing { get; private set; }

	private void Awake()
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		if (PersistentSingleton<SoundManager>.Instance.sfxEnabled && !((Object)(object)_soundInfo.audioClip == (Object)null))
		{
			_audioSource = ((Component)this).gameObject.AddComponent<AudioSource>();
			Vector3 position = ((Component)_audioSource).transform.position;
			position.z = ((Component)Camera.main).transform.position.z;
			((Component)_audioSource).transform.position = position;
			_audioSource.clip = _soundInfo.audioClip;
			_audioSource.priority = _soundInfo.priority;
			_audioSource.panStereo = _soundInfo.stereoPan;
			_audioSource.bypassEffects = _soundInfo.bypassEffects;
			_audioSource.bypassListenerEffects = _soundInfo.bypassListenerEffects;
			_audioSource.bypassReverbZones = _soundInfo.bypassReverbZones;
			_audioSource.loop = _soundInfo.loop;
			_audioSource.spatialBlend = _soundInfo.spatialBlend;
			_audioSource.playOnAwake = _playOnEnable;
			UpdateVolume();
			if (!_playOnEnable)
			{
				_audioSource.Stop();
			}
		}
	}

	private void OnEnable()
	{
		if (_playOnEnable && (_soundInfo.length != 0f || !_soundInfo.loop))
		{
			((MonoBehaviour)this).StartCoroutine(CPlay());
		}
	}

	private void OnDisable()
	{
		playing = false;
	}

	public IEnumerator CPlay()
	{
		if ((Object)(object)_soundInfo.audioClip == (Object)null)
		{
			Debug.LogWarning((object)("AudioClip of PlaySoundInfo is null. Object name " + ((Object)this).name + "."));
			yield break;
		}
		float length = _soundInfo.length;
		if (_soundInfo.length == 0f)
		{
			length = _soundInfo.audioClip.length;
		}
		UpdateVolume();
		_audioSource.Play();
		playing = true;
		yield return (object)new WaitForSecondsRealtime(length);
		_audioSource.Stop();
		playing = false;
	}

	public void Play()
	{
		if (!playing)
		{
			((MonoBehaviour)this).StartCoroutine(CPlay());
		}
	}

	public void Stop()
	{
		if (playing)
		{
			((MonoBehaviour)this).StopAllCoroutines();
			_audioSource.Stop();
			playing = false;
		}
	}

	private void UpdateVolume()
	{
		_audioSource.volume = PersistentSingleton<SoundManager>.Instance.sfxVolume * _soundInfo.volume * PersistentSingleton<SoundManager>.Instance.masterVolume;
	}
}
