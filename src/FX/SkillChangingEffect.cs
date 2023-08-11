using System.Collections;
using UnityEngine;

namespace FX;

public class SkillChangingEffect : MonoBehaviour
{
	[SerializeField]
	private Animator _animator;

	[SerializeField]
	private SpriteRenderer _oldSkill1;

	[SerializeField]
	private SpriteRenderer _oldSkill2;

	[SerializeField]
	private SpriteRenderer _newSkill1;

	[SerializeField]
	private SpriteRenderer _newSkill2;

	private float _animationLength;

	private float _remainTime;

	private void Awake()
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		AnimatorStateInfo currentAnimatorStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
		_animationLength = ((AnimatorStateInfo)(ref currentAnimatorStateInfo)).length;
	}

	public void Play(Sprite[] oldSkills, Sprite[] newSkills)
	{
		if (oldSkills.Length != 0)
		{
			((Renderer)_oldSkill1).enabled = true;
			_oldSkill1.sprite = oldSkills[0];
		}
		else
		{
			((Renderer)_oldSkill1).enabled = false;
		}
		if (oldSkills.Length > 1)
		{
			((Renderer)_oldSkill2).enabled = true;
			_oldSkill2.sprite = oldSkills[1];
		}
		else
		{
			((Renderer)_oldSkill2).enabled = false;
		}
		if (newSkills.Length != 0)
		{
			((Renderer)_newSkill1).enabled = true;
			_newSkill1.sprite = newSkills[0];
		}
		else
		{
			((Renderer)_newSkill1).enabled = false;
		}
		if (newSkills.Length > 1)
		{
			((Renderer)_newSkill2).enabled = true;
			_newSkill2.sprite = newSkills[1];
		}
		else
		{
			((Renderer)_newSkill2).enabled = false;
		}
		PlayAnimation();
	}

	private void PlayAnimation()
	{
		((Component)this).gameObject.SetActive(true);
		((Behaviour)_animator).enabled = true;
		_animator.Play(0, 0, 0f);
		((Behaviour)_animator).enabled = false;
		_remainTime = _animationLength;
		((MonoBehaviour)this).StopAllCoroutines();
		((MonoBehaviour)this).StartCoroutine(CPlay());
	}

	private IEnumerator CPlay()
	{
		_animator.Update(0f);
		while (_remainTime > 0f)
		{
			yield return null;
			float deltaTime = ((ChronometerBase)Chronometer.global).deltaTime;
			_animator.Update(deltaTime);
			_remainTime -= deltaTime;
		}
		((Component)this).gameObject.SetActive(false);
	}
}
