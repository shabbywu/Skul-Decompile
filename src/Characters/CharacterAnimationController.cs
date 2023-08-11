using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Characters;

public class CharacterAnimationController : MonoBehaviour
{
	public class KeyAttribute : PopupAttribute
	{
		public KeyAttribute()
			: base(true, new object[2] { "CharacterBody", "Polymorph" })
		{
		}
	}

	[Serializable]
	public class AnimationInfo : ReorderableArray<AnimationInfo.KeyClip>
	{
		[Serializable]
		public class KeyClip
		{
			[Key]
			[SerializeField]
			private string _key = "CharacterBody";

			[SerializeField]
			private AnimationClip _clip;

			public string key => _key;

			public AnimationClip clip => _clip;

			public KeyClip(string key, AnimationClip clip)
			{
				_key = key;
				_clip = clip;
			}

			public void Dispose()
			{
				_key = null;
				_clip = null;
			}
		}

		private Dictionary<string, AnimationClip> _dictionary;

		public Dictionary<string, AnimationClip> dictionary => _dictionary ?? (_dictionary = base.values.ToDictionary((KeyClip v) => v.key, (KeyClip v) => v.clip));

		public AnimationClip defaultClip
		{
			get
			{
				if (base.values.Length == 0)
				{
					return null;
				}
				return base.values[0].clip;
			}
		}

		public AnimationInfo(params KeyClip[] keyClips)
		{
			base.values = keyClips;
		}

		public void Dispose()
		{
			_dictionary?.Clear();
			KeyClip[] values = base.values;
			for (int i = 0; i < values.Length; i++)
			{
				values[i].Dispose();
			}
		}
	}

	public class Parameter
	{
		public bool walk;

		public bool grounded;

		public float movementSpeed;

		public float ySpeed;

		public bool flipX;

		public void CopyFrom(Parameter parameter)
		{
			walk = parameter.walk;
			grounded = parameter.grounded;
			movementSpeed = parameter.movementSpeed;
			ySpeed = parameter.ySpeed;
			flipX = parameter.flipX;
		}
	}

	public const string characterBodyKey = "CharacterBody";

	public const string polymorphKey = "Polymorph";

	public readonly Parameter parameter = new Parameter();

	[SerializeField]
	[GetComponent]
	private Character _character;

	public List<CharacterAnimation> animations = new List<CharacterAnimation>();

	private Coroutine _coroutine;

	public event Action onExpire;

	private void Update()
	{
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < animations.Count; i++)
		{
			CharacterAnimation characterAnimation = animations[i];
			characterAnimation.speed = ((ChronometerBase)_character.chronometer.animation).timeScale / Time.timeScale;
			((AnimatorVariable<bool>)(object)characterAnimation.parameter.walk).Value = parameter.walk;
			((AnimatorVariable<bool>)(object)characterAnimation.parameter.grounded).Value = parameter.grounded;
			((AnimatorVariable<float>)(object)characterAnimation.parameter.movementSpeed).Value = parameter.movementSpeed;
			((AnimatorVariable<float>)(object)characterAnimation.parameter.ySpeed).Value = parameter.ySpeed;
			((Component)characterAnimation).transform.localScale = (Vector3)(parameter.flipX ? new Vector3(-1f, 1f, 1f) : Vector3.one);
		}
	}

	public void Initialize()
	{
		((Component)this).GetComponentsInChildren<CharacterAnimation>(true, animations);
		animations.ForEach(delegate(CharacterAnimation animation)
		{
			animation.Initialize();
		});
	}

	public void ForceUpdate()
	{
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < animations.Count; i++)
		{
			CharacterAnimation characterAnimation = animations[i];
			characterAnimation.speed = ((ChronometerBase)_character.chronometer.animation).timeScale / Time.timeScale;
			((AnimatorVariable<bool>)(object)characterAnimation.parameter.walk).ForceSet(parameter.walk);
			((AnimatorVariable<bool>)(object)characterAnimation.parameter.grounded).ForceSet(parameter.grounded);
			((AnimatorVariable<float>)(object)characterAnimation.parameter.movementSpeed).ForceSet(parameter.movementSpeed);
			((AnimatorVariable<float>)(object)characterAnimation.parameter.ySpeed).ForceSet(parameter.ySpeed);
			((Component)characterAnimation).transform.localScale = (Vector3)(parameter.flipX ? new Vector3(-1f, 1f, 1f) : Vector3.one);
		}
	}

	public void UpdateScale()
	{
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < animations.Count; i++)
		{
			((Component)animations[i]).transform.localScale = (Vector3)(parameter.flipX ? new Vector3(-1f, 1f, 1f) : Vector3.one);
		}
	}

	public void Play(AnimationInfo animationInfo, float speed)
	{
		if (_coroutine != null)
		{
			((MonoBehaviour)this).StopCoroutine(_coroutine);
			_coroutine = null;
		}
		foreach (CharacterAnimation animation in animations)
		{
			if (((Behaviour)animation).isActiveAndEnabled && animationInfo.dictionary.TryGetValue(animation.key, out var value))
			{
				animation.Play(value, speed);
			}
		}
	}

	public void Play(AnimationInfo animationInfo, float length, float speed)
	{
		Play(animationInfo, speed);
		_coroutine = ((MonoBehaviour)this).StartCoroutine(ExpireInSeconds(length));
	}

	public void Stun()
	{
		if (_coroutine != null)
		{
			((MonoBehaviour)this).StopCoroutine(_coroutine);
			_coroutine = null;
		}
		foreach (CharacterAnimation animation in animations)
		{
			animation.Stun();
		}
	}

	public void Unmove(AnimationInfo info)
	{
		if (_coroutine != null)
		{
			((MonoBehaviour)this).StopCoroutine(_coroutine);
			_coroutine = null;
		}
		Play(info, 1f);
	}

	private IEnumerator ExpireInSeconds(float seconds)
	{
		while (seconds >= 0f)
		{
			yield return null;
			seconds -= ((ChronometerBase)_character.chronometer.animation).deltaTime;
		}
		StopAll();
		this.onExpire?.Invoke();
	}

	public void Loop()
	{
		foreach (CharacterAnimation animation in animations)
		{
			animation.Play();
		}
	}

	public void StopAll()
	{
		foreach (CharacterAnimation animation in animations)
		{
			animation.Stop();
		}
	}
}
