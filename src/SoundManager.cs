using System.Collections;
using System.Collections.Generic;
using Data;
using FX;
using Services;
using Singletons;
using UnityEngine;

public sealed class SoundManager : PersistentSingleton<SoundManager>
{
	public const float maxMusicVolum = 1f;

	public const float maxSfxVolum = 1f;

	[SerializeField]
	private AudioSource _backgroundMusic;

	[SerializeField]
	private ReusableAudioSource _audioSourcePrefab;

	[SerializeField]
	private NarrationAudioSource _narrationSource;

	private CoroutineReference _coroutine;

	private Dictionary<AudioClip, float> _playHistories = new Dictionary<AudioClip, float>();

	private AudioClip _targetBackgroundClip;

	private CoroutineReference _fadeBackgroundMusicReference;

	private float _internalMusicVolume = 1f;

	public float masterVolume
	{
		get
		{
			return GameData.Settings.masterVolume;
		}
		set
		{
			GameData.Settings.masterVolume = value;
		}
	}

	public bool musicEnabled
	{
		get
		{
			return GameData.Settings.musicEnabled;
		}
		set
		{
			GameData.Settings.musicEnabled = value;
		}
	}

	public bool sfxEnabled
	{
		get
		{
			return GameData.Settings.sfxEnabled;
		}
		set
		{
			GameData.Settings.sfxEnabled = value;
		}
	}

	public float musicVolume
	{
		get
		{
			return GameData.Settings.musicVolume;
		}
		set
		{
			GameData.Settings.musicVolume = value;
		}
	}

	public float sfxVolume
	{
		get
		{
			return GameData.Settings.sfxVolume;
		}
		set
		{
			GameData.Settings.sfxVolume = value;
		}
	}

	public AudioClip backgroundClip => _backgroundMusic.clip;

	protected override void Awake()
	{
		base.Awake();
		if (Object.op_Implicit((Object)(object)_backgroundMusic))
		{
			_targetBackgroundClip = _backgroundMusic.clip;
		}
	}

	public void UpdateMusicVolume()
	{
		if (!Service.quitting)
		{
			_backgroundMusic.volume = musicVolume * masterVolume * _internalMusicVolume;
		}
	}

	public float LoadBackgroundMusicTime(AudioClip clip)
	{
		if (_playHistories.ContainsKey(clip))
		{
			return _playHistories[clip];
		}
		return 0f;
	}

	public void SaveBackgroundMusicTime(AudioSource source)
	{
		if (_playHistories.ContainsKey(source.clip))
		{
			_playHistories[source.clip] = source.time;
		}
		else
		{
			_playHistories.Add(source.clip, source.time);
		}
	}

	public void PlayBackgroundMusic(MusicInfo musicInfo)
	{
		PlayBackgroundMusic(musicInfo.audioClip, musicInfo.volume, musicInfo.fade, musicInfo.loop, musicInfo.usePlayHistory);
	}

	public void PlayBackgroundMusic(AudioClip music, float volume = 1f, bool fade = true, bool loop = true, bool usePlayHistory = false)
	{
		if ((Object)(object)_backgroundMusic == (Object)null || (_backgroundMusic.isPlaying && (Object)(object)_targetBackgroundClip == (Object)(object)music))
		{
			return;
		}
		ResetInternalMusicVolume();
		_targetBackgroundClip = music;
		_backgroundMusic.loop = loop;
		if (!musicEnabled)
		{
			_backgroundMusic.clip = music;
			return;
		}
		_fadeBackgroundMusicReference.Stop();
		_backgroundMusic.volume = musicVolume * masterVolume * volume;
		if (fade && Object.op_Implicit((Object)(object)_backgroundMusic.clip))
		{
			_fadeBackgroundMusicReference = ((MonoBehaviour)(object)this).StartCoroutineWithReference(CFadeBackgroundMusic(music, 1f, usePlayHistory));
			return;
		}
		_backgroundMusic.clip = music;
		_backgroundMusic.Stop();
		if (usePlayHistory && _playHistories.ContainsKey(_backgroundMusic.clip))
		{
			_backgroundMusic.time = _playHistories[_backgroundMusic.clip];
		}
		else
		{
			_backgroundMusic.time = 0f;
		}
		_backgroundMusic.Play();
	}

	public void StopBackGroundMusic()
	{
		if (!((Object)(object)_backgroundMusic.clip == (Object)null))
		{
			if (!_playHistories.ContainsKey(_backgroundMusic.clip))
			{
				_playHistories.Add(_backgroundMusic.clip, _backgroundMusic.time);
			}
			else
			{
				_playHistories[_backgroundMusic.clip] = _backgroundMusic.time;
			}
			_backgroundMusic.Stop();
		}
	}

	public void RemovePlayHistory(AudioClip music)
	{
		_playHistories.Remove(music);
	}

	public void FadeOutBackgroundMusic(float fadeTime = 1f)
	{
		((MonoBehaviour)this).StartCoroutine(CFadeOutBackgroundMusic(fadeTime));
	}

	private IEnumerator CFadeOutBackgroundMusic(float fadeTime = 1f)
	{
		return CFadeBackgroundMusic(null, fadeTime);
	}

	private IEnumerator CFadeBackgroundMusic(AudioClip newClip, float fadeTime = 1f, bool usePlayHistory = false)
	{
		float t2 = 0f;
		do
		{
			yield return null;
			t2 += Time.deltaTime * 1f / fadeTime;
			_backgroundMusic.volume = (1f - t2) * musicVolume * masterVolume * _internalMusicVolume;
		}
		while (t2 < 1f);
		_backgroundMusic.volume = 0f;
		StopBackGroundMusic();
		if (!((Object)(object)newClip == (Object)null))
		{
			_backgroundMusic.clip = newClip;
			if (usePlayHistory && _playHistories.ContainsKey(_backgroundMusic.clip))
			{
				_backgroundMusic.time = _playHistories[_backgroundMusic.clip];
			}
			else
			{
				_backgroundMusic.time = 0f;
			}
			_backgroundMusic.Play();
			t2 = 0f;
			do
			{
				yield return null;
				t2 += Time.deltaTime * 1f / fadeTime;
				_backgroundMusic.volume = t2 * musicVolume * masterVolume;
			}
			while (t2 < 1f);
			_backgroundMusic.volume = musicVolume * masterVolume;
		}
	}

	public ReusableAudioSource PlaySound(SoundInfo clipInfo, Vector3 position)
	{
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c0: Unknown result type (might be due to invalid IL or missing references)
		if (!sfxEnabled || (Object)(object)clipInfo.audioClip == (Object)null)
		{
			return null;
		}
		if ((Object)(object)Camera.main == (Object)null)
		{
			return null;
		}
		if (!_playHistories.ContainsKey(clipInfo.audioClip))
		{
			_playHistories.Add(clipInfo.audioClip, Time.unscaledTime);
		}
		else
		{
			if (Time.unscaledTime - _playHistories[clipInfo.audioClip] < clipInfo.uniqueTime)
			{
				return null;
			}
			_playHistories[clipInfo.audioClip] = Time.unscaledTime;
		}
		ReusableAudioSource component = ((Component)_audioSourcePrefab.reusable.Spawn(true)).GetComponent<ReusableAudioSource>();
		position.z = ((Component)Camera.main).transform.position.z;
		((Component)component).transform.position = position;
		AudioSource audioSource = component.audioSource;
		audioSource.volume = sfxVolume * clipInfo.volume * masterVolume;
		audioSource.priority = clipInfo.priority;
		audioSource.panStereo = clipInfo.stereoPan;
		audioSource.bypassEffects = clipInfo.bypassEffects;
		audioSource.bypassListenerEffects = clipInfo.bypassListenerEffects;
		audioSource.bypassReverbZones = clipInfo.bypassReverbZones;
		audioSource.loop = clipInfo.loop;
		audioSource.spatialBlend = clipInfo.spatialBlend;
		component.Play(clipInfo.audioClip, clipInfo.length);
		return component;
	}

	public ReusableAudioSource PlaySound(AudioClip clip, Vector3 position, float uniqueTime = 0.05f)
	{
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		if (!_playHistories.ContainsKey(clip))
		{
			_playHistories.Add(clip, Time.unscaledTime);
		}
		else
		{
			if (Time.unscaledTime - _playHistories[clip] < uniqueTime)
			{
				return null;
			}
			_playHistories[clip] = Time.unscaledTime;
		}
		return PlaySound(clip, position);
	}

	private ReusableAudioSource PlaySound(AudioClip sfx, Vector3 location)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		if (!sfxEnabled)
		{
			return null;
		}
		ReusableAudioSource component = ((Component)_audioSourcePrefab.reusable.Spawn(location, true)).GetComponent<ReusableAudioSource>();
		component.audioSource.volume = sfxVolume * masterVolume;
		component.Play(sfx, 0f);
		return component;
	}

	private ReusableAudioSource ForcePlaySound(AudioClip sfx, Vector3 location)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		ReusableAudioSource component = ((Component)_audioSourcePrefab.reusable.Spawn(location, true)).GetComponent<ReusableAudioSource>();
		component.audioSource.volume = sfxVolume * masterVolume;
		component.Play(sfx, 0f);
		return component;
	}

	public void PlayNarrationSound(SoundInfo info)
	{
		if ((Object)(object)_narrationSource == (Object)null)
		{
			Debug.Log((object)"NarrationSource null");
		}
		if (info == null)
		{
			Debug.Log((object)"info null");
		}
		_narrationSource.Play(info, masterVolume);
	}

	public void PlayNarrationSound(AudioClip clip)
	{
		_narrationSource.Play(clip, masterVolume);
	}

	public void StopNarrationSound()
	{
		_narrationSource.Stop();
	}

	public void SetInternalMusicVolume(float volume)
	{
		_coroutine.Stop();
		_internalMusicVolume = volume;
		UpdateMusicVolume();
	}

	public void SetInternalMusicVolume(float volume, float easeTime, AnimationCurve easeCurve)
	{
		_coroutine.Stop();
		_coroutine = ((MonoBehaviour)(object)this).StartCoroutineWithReference(CFadeMusicVolume(volume, easeTime, easeCurve));
	}

	private IEnumerator CFadeMusicVolume(float volume, float easeTime, AnimationCurve easeCurve)
	{
		float time = 0f;
		float startVolume = _internalMusicVolume;
		while (time < easeTime)
		{
			yield return null;
			time += Time.unscaledDeltaTime;
			_internalMusicVolume = Mathf.LerpUnclamped(startVolume, volume, easeCurve.Evaluate(time / easeTime));
			UpdateMusicVolume();
		}
		_internalMusicVolume = volume;
		UpdateMusicVolume();
	}

	public void ResetInternalMusicVolume()
	{
		SetInternalMusicVolume(1f);
	}
}
