using System.Collections;
using Characters.Actions;
using Characters.Operations;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.AI.Mercenarys;

public class Soulmate : MonoBehaviour
{
	[SerializeField]
	private Character _character;

	[Header("Movement")]
	[SerializeField]
	private float _timeToChase = 2f;

	[SerializeField]
	private float _minimumDistance = 1.5f;

	[SerializeField]
	private Action _teleport;

	[SerializeField]
	private Transform _teleportDestination;

	[SerializeField]
	[Header("Buff")]
	private float _buffInterval = 30f;

	[SerializeField]
	private AttachAbility[] _buffs;

	private Character _owner;

	private float _timeAwayfromOwner;

	private bool _hidden;

	private float _buffElapsed;

	private void Start()
	{
		if (WitchBonus.instance.soul.fatalMind.level != 0)
		{
			Object.DontDestroyOnLoad((Object)(object)_teleportDestination);
			_teleportDestination.SetParent((Transform)null);
		}
	}

	private void OnEnable()
	{
		((MonoBehaviour)this).StartCoroutine(CProcess());
	}

	private IEnumerator CBuff()
	{
		while (true)
		{
			if (_hidden)
			{
				yield return null;
				continue;
			}
			_buffElapsed += _owner.chronometer.master.deltaTime;
			if (_buffElapsed >= _buffInterval)
			{
				_buffs.Random().Run(_owner);
				_buffElapsed = 0f;
			}
			yield return null;
		}
	}

	private IEnumerator CSetOwner()
	{
		while ((Object)(object)_owner == (Object)null)
		{
			yield return null;
			_owner = Singleton<Service>.Instance.levelManager.player;
		}
	}

	private IEnumerator CProcess()
	{
		yield return CSetOwner();
		((MonoBehaviour)this).StartCoroutine(CMove());
		((MonoBehaviour)this).StartCoroutine(CBuff());
	}

	private IEnumerator CMove()
	{
		while (true)
		{
			if (_hidden)
			{
				yield return null;
				continue;
			}
			Collider2D lastStandingCollider = _owner.movement.controller.collisionState.lastStandingCollider;
			if ((Object)(object)lastStandingCollider == (Object)null)
			{
				yield return null;
				continue;
			}
			Collider2D lastStandingCollider2 = _character.movement.controller.collisionState.lastStandingCollider;
			if ((Object)(object)lastStandingCollider2 == (Object)null)
			{
				yield return null;
				continue;
			}
			if ((Object)(object)lastStandingCollider != (Object)(object)lastStandingCollider2)
			{
				_timeAwayfromOwner += _owner.chronometer.master.deltaTime;
				if (_timeAwayfromOwner > _timeToChase)
				{
					yield return CTeleport();
					_timeAwayfromOwner = 0f;
				}
			}
			else
			{
				_timeAwayfromOwner = 0f;
				float num = ((Component)_owner).transform.position.x - ((Component)_character).transform.position.x;
				if (Mathf.Abs(num) > _minimumDistance)
				{
					_character.movement.MoveHorizontal(new Vector2(Mathf.Sign(num), 0f));
				}
			}
			yield return null;
		}
	}

	private IEnumerator CTeleport()
	{
		_teleportDestination.position = ((Component)_owner).transform.position;
		_teleport.TryStart();
		while (_teleport.running)
		{
			yield return null;
		}
	}

	public void Hide()
	{
		if (WitchBonus.instance.soul.fatalMind.level != 0)
		{
			_hidden = true;
			((Component)_character).gameObject.SetActive(false);
			_buffElapsed = 0f;
			_timeAwayfromOwner = 0f;
		}
	}

	public IEnumerator CAppearance()
	{
		yield return CSetOwner();
		Collider2D lastStandingCollider;
		while (true)
		{
			lastStandingCollider = _owner.movement.controller.collisionState.lastStandingCollider;
			if (!((Object)(object)lastStandingCollider == (Object)null))
			{
				break;
			}
			yield return null;
		}
		Transform teleportDestination = _teleportDestination;
		float num = ((Component)_owner).transform.position.x - 1f;
		Bounds bounds = lastStandingCollider.bounds;
		teleportDestination.position = Vector2.op_Implicit(new Vector2(num, ((Bounds)(ref bounds)).max.y));
		((Component)_character).gameObject.SetActive(true);
		_teleport.TryStart();
		while (_teleport.running)
		{
			yield return null;
		}
		_hidden = false;
	}
}
