using System.Collections;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.AI.Hero;

public class SaintField : MonoBehaviour
{
	[SerializeField]
	private Character _owner;

	[SerializeField]
	private float _duration = 10f;

	[SerializeField]
	private GameObject _leftPillarOfLight;

	[SerializeField]
	private GameObject _rightPillarOfLight;

	[SerializeField]
	private Transform _fireTransform;

	[SerializeField]
	private GiganticSaintSword _sword;

	[SerializeField]
	private float _height;

	private Character _player;

	private CoroutineReference _checkPlayerOutReference;

	public bool isStuck => _sword.isStuck;

	private void Start()
	{
		_player = Singleton<Service>.Instance.levelManager.player;
		_sword.OnStuck += ActivePillarOfLight;
		_sword.OnStuck += delegate
		{
			((MonoBehaviour)this).StartCoroutine(CExpire());
		};
		_owner.health.onDiedTryCatch += DeactivePillarOfLight;
	}

	public void DropGiganticSaintSword()
	{
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		Bounds bounds = _player.movement.controller.collisionState.lastStandingCollider.bounds;
		float y = ((Bounds)(ref bounds)).max.y;
		_sword.Fire(Vector2.op_Implicit(_fireTransform.position), y);
	}

	private void ActivePillarOfLight()
	{
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)Singleton<Service>.Instance.levelManager.player == (Object)null))
		{
			_checkPlayerOutReference = CoroutineReferenceExtension.StartCoroutineWithReference((MonoBehaviour)(object)this, CCheckPlayerOut());
			_leftPillarOfLight.SetActive(true);
			_rightPillarOfLight.SetActive(true);
		}
	}

	public void DeactivePillarOfLight()
	{
		_leftPillarOfLight.SetActive(false);
		_rightPillarOfLight.SetActive(false);
		_sword.Despawn();
		((CoroutineReference)(ref _checkPlayerOutReference)).Stop();
	}

	private IEnumerator CCheckPlayerOut()
	{
		Character player = Singleton<Service>.Instance.levelManager.player;
		while (!_owner.health.dead)
		{
			if (!Contains())
			{
				((Component)player).transform.position = ((Component)_owner).transform.position;
				yield return Chronometer.global.WaitForSeconds(1f);
			}
			yield return null;
		}
		bool Contains()
		{
			if ((Object)(object)player.movement.controller.collisionState.lastStandingCollider != (Object)(object)_owner.movement.controller.collisionState.lastStandingCollider)
			{
				return false;
			}
			return true;
		}
	}

	private IEnumerator CExpire()
	{
		float elapsed = 0f;
		while (elapsed < _duration)
		{
			yield return null;
			elapsed += ((ChronometerBase)Chronometer.global).deltaTime;
			if (!ContainsPlayer())
			{
				break;
			}
		}
		DeactivePillarOfLight();
	}

	private bool ContainsPlayer()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		Character player = Singleton<Service>.Instance.levelManager.player;
		if (_leftPillarOfLight.transform.position.x > ((Component)player).transform.position.x)
		{
			return false;
		}
		if (_rightPillarOfLight.transform.position.x < ((Component)player).transform.position.x)
		{
			return false;
		}
		return true;
	}
}
