using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Upgrades;

public sealed class Level : MonoBehaviour
{
	[Serializable]
	private class Gem
	{
		[SerializeField]
		private Image _frame;

		[SerializeField]
		private Image _image;

		[SerializeField]
		private Animator _animator;

		[SerializeField]
		private Animator _effect;

		private const float _length = 0.32f;

		private static int empty = Animator.StringToHash("Empty");

		private static int flicker = Animator.StringToHash("Flicker");

		public void Set(int index, int level, Sprite frame)
		{
			((Component)_frame).gameObject.SetActive(true);
			_frame.sprite = frame;
			((Component)_image).gameObject.SetActive(index < level);
			_animator.Play(empty);
		}

		public void Hide()
		{
			((Component)_frame).gameObject.SetActive(false);
			((Component)_image).gameObject.SetActive(false);
		}

		public void HideEffect()
		{
			((Component)_effect).gameObject.SetActive(false);
		}

		public void Flick()
		{
			((Component)_image).gameObject.SetActive(true);
			_animator.Play(flicker);
		}

		public IEnumerator CLevelUp()
		{
			_animator.Play(empty);
			((Behaviour)_effect).enabled = true;
			_effect.Play(0, 0, 0f);
			((Behaviour)_effect).enabled = false;
			((Component)_effect).gameObject.SetActive(true);
			float remainTime = 0.32f;
			while (remainTime > 0f)
			{
				yield return null;
				float deltaTime = Chronometer.global.deltaTime;
				_effect.Update(deltaTime);
				remainTime -= deltaTime;
			}
			((Component)_effect).gameObject.SetActive(false);
		}
	}

	[SerializeField]
	private Gem[] _gems;

	[SerializeField]
	private Sprite _normal;

	[SerializeField]
	private Sprite _risky;

	[SerializeField]
	private bool _flick;

	[SerializeField]
	private float _flickTime = 0.35f;

	public void Set(int level, int maxLevel, bool risky, bool flick = false)
	{
		DeactivateAll();
		Sprite frame = (risky ? _risky : _normal);
		for (int i = 0; i < maxLevel; i++)
		{
			_gems[i].Set(i, level, frame);
		}
		if (_flick && level != maxLevel && flick)
		{
			_gems[level].Flick();
		}
	}

	public void LevelUp(int level)
	{
		((MonoBehaviour)this).StartCoroutine(_gems[level - 1].CLevelUp());
	}

	private void OnDisable()
	{
		for (int i = 0; i < _gems.Length; i++)
		{
			_gems[i].HideEffect();
		}
	}

	private void DeactivateAll()
	{
		for (int i = 0; i < _gems.Length; i++)
		{
			_gems[i].Hide();
		}
	}
}
