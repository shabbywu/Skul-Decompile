using System;
using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using Services;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Characters.AI;

public sealed class WeepingAngelsAI : AIController
{
	[Serializable]
	private class Setting
	{
		[SerializeField]
		private Sprite _firstFrame;

		[SerializeField]
		private AnimationClip _idle;

		[SerializeField]
		private RunAction _activate;

		internal RunAction activate => _activate;

		public void Apply(CharacterAnimation animation, SpriteRenderer renderer)
		{
			renderer.sprite = _firstFrame;
			animation.SetIdle(_idle);
		}

		public void Dispose()
		{
			_firstFrame = null;
			_idle = null;
			_activate = null;
		}
	}

	[SerializeField]
	[Subcomponent(typeof(CheckWithinSight))]
	[Header("Behaviours")]
	private CheckWithinSight _checkWithinSight;

	[SerializeField]
	[Subcomponent(typeof(HideAndSeek))]
	private HideAndSeek _hideAndSeek;

	[SerializeField]
	[Attack.Subcomponent(true)]
	private Attack _attack;

	[SerializeField]
	private Setting _stage1;

	[SerializeField]
	private Setting _stage2;

	[Header("Tools")]
	[Space]
	[SerializeField]
	private Collider2D _attackTrigger;

	[SerializeField]
	private SpriteRenderer _renderer;

	[SerializeField]
	private CharacterAnimation _characterAnimation;

	private RunAction _activate;

	private void Awake()
	{
		base.behaviours = new List<Behaviour> { _checkWithinSight, _hideAndSeek, _attack };
		if (Singleton<Service>.Instance.levelManager.currentChapter.stageIndex == 0)
		{
			_stage1.Apply(_characterAnimation, _renderer);
			_activate = _stage1.activate;
		}
		else
		{
			_stage2.Apply(_characterAnimation, _renderer);
			_activate = _stage2.activate;
		}
	}

	private void OnDestroy()
	{
		_stage1.Dispose();
		_stage2.Dispose();
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		((MonoBehaviour)this).StartCoroutine(_checkWithinSight.CRun(this));
		((MonoBehaviour)this).StartCoroutine(CProcess());
	}

	protected override IEnumerator CProcess()
	{
		yield return CPlayStartOption();
		yield return CCombat();
	}

	private IEnumerator CCombat()
	{
		while ((Object)(object)base.target == (Object)null)
		{
			yield return null;
		}
		yield return _activate.CRun(this);
		while (!base.dead)
		{
			if ((Object)(object)base.target == (Object)null)
			{
				yield return null;
				continue;
			}
			if (base.stuned)
			{
				yield return null;
				continue;
			}
			yield return _hideAndSeek.CRun(this);
			if (_hideAndSeek.result == Behaviour.Result.Success)
			{
				yield return _attack.CRun(this);
			}
		}
	}
}
