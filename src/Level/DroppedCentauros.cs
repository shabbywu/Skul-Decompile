using System.Collections;
using Characters;
using Characters.Gear.Quintessences;
using Characters.Operations;
using Characters.Player;
using Services;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Level;

public class DroppedCentauros : MonoBehaviour
{
	[GetComponent]
	[SerializeField]
	private PoolObject _poolObject;

	[SerializeField]
	[Subcomponent(typeof(OperationInfo))]
	private OperationInfo.Subcomponents _onLoot;

	[SerializeField]
	private float _reduceAmount = 20f;

	private Character _player;

	private Quintessence _quintessence;

	private bool _collisionStay;

	public PoolObject Spawn(Vector2 position)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return _poolObject.Spawn(Vector2.op_Implicit(position), true);
	}

	private void Awake()
	{
		_onLoot.Initialize();
	}

	private void OnEnable()
	{
		_player = Singleton<Service>.Instance.levelManager.player;
		((MonoBehaviour)this).StartCoroutine(CUpdate());
		QuintessenceInventory quintessence = _player.playerComponents.inventory.quintessence;
		_quintessence = quintessence.items[0];
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		_collisionStay = true;
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		_collisionStay = false;
	}

	private IEnumerator CUpdate()
	{
		float time = 0f;
		yield return Chronometer.global.WaitForSeconds(0.5f);
		while (!((Object)(object)_quintessence == (Object)null) && !_quintessence.cooldown.time.canUse)
		{
			time += ((ChronometerBase)Chronometer.global).deltaTime;
			if (time >= 0.5f && _collisionStay)
			{
				_quintessence.cooldown.time.remainTime -= _reduceAmount;
				Character player = Singleton<Service>.Instance.levelManager.player;
				_onLoot.StopAll();
				((MonoBehaviour)CoroutineProxy.instance).StartCoroutine(_onLoot.CRun(player));
				_poolObject.Despawn();
			}
			yield return null;
		}
		_poolObject.Despawn();
	}
}
