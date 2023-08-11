using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.Actions;
using Characters.Movements;
using UnityEditor;
using UnityEngine;

namespace Characters.AI;

public sealed class ServantAI : AIController
{
	[SerializeField]
	[Subcomponent(typeof(Confusing))]
	private Confusing _confusing;

	[SerializeField]
	private Action _clash;

	[SerializeField]
	private BoxCollider2D _clashCollider;

	[SerializeField]
	private CharacterStatus.ApplyInfo _status;

	[Range(1f, 4f)]
	[SerializeField]
	private int _grade = 1;

	[SerializeField]
	private PushInfo _rightPushInfo;

	[SerializeField]
	private PushInfo _leftPushInfo;

	[Subcomponent(typeof(RepeatPlaySound))]
	[SerializeField]
	private RepeatPlaySound _screaming;

	private bool _confusingState;

	private bool _crashable = true;

	private void Awake()
	{
		base.behaviours = new List<Behaviour> { _confusing };
	}

	protected override void OnEnable()
	{
		((MonoBehaviour)this).StartCoroutine(CProcess());
	}

	protected override IEnumerator CProcess()
	{
		_confusingState = true;
		yield return CPlayStartOption();
		((MonoBehaviour)this).StartCoroutine(CConfusing());
		((MonoBehaviour)this).StartCoroutine(CCombat());
		((MonoBehaviour)this).StartCoroutine(CObserveHit());
	}

	private IEnumerator CConfusing()
	{
		while (!base.dead)
		{
			yield return null;
			if (_confusingState && character.movement.isGrounded)
			{
				yield return _confusing.CRun(this);
			}
		}
	}

	private IEnumerator CCombat()
	{
		_screaming.Play();
		while (!base.dead)
		{
			yield return null;
			if (!_crashable || base.character.stunedOrFreezed)
			{
				continue;
			}
			Character character = FindClosestPlayerBody((Collider2D)(object)_clashCollider);
			if (!((Object)(object)character == (Object)null) && !character.cinematic.value)
			{
				if (((Component)base.character).transform.position.x > ((Component)character).transform.position.x)
				{
					character.movement.push.ApplyKnockback(base.character, _rightPushInfo);
					base.character.movement.push.ApplyKnockback(character, _leftPushInfo);
					character.lookingDirection = Character.LookingDirection.Left;
				}
				else
				{
					character.movement.push.ApplyKnockback(base.character, _leftPushInfo);
					base.character.movement.push.ApplyKnockback(character, _rightPushInfo);
					character.lookingDirection = Character.LookingDirection.Right;
				}
				character.status?.Apply(base.character, _status);
				_confusing.StopPropagation();
				_screaming.Stop();
				_confusingState = false;
				_clash.TryStart();
				while (_clash.running)
				{
					yield return null;
				}
				_confusingState = true;
				_screaming.Play();
			}
		}
	}

	private IEnumerator CObserveHit()
	{
		while (!base.dead)
		{
			yield return null;
			if (character.hit.action.running)
			{
				_crashable = false;
			}
			else
			{
				_crashable = true;
			}
		}
	}
}
