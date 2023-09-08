using System;
using System.Collections;
using Characters;
using Characters.Gear.Weapons;
using Services;
using Singletons;
using UnityEngine;

namespace Level;

public class DroppedSkulHead : MonoBehaviour
{
	private const float _lootDistance = 1f;

	private const float _sqrLootDistance = 1f;

	[GetComponent]
	[SerializeField]
	private PoolObject _poolObject;

	[GetComponent]
	[SerializeField]
	private Rigidbody2D _rigidbody;

	private Character _player;

	private SkulHeadController _skulHeadController;

	private void OnEnable()
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		_rigidbody.gravityScale = 3f;
		_rigidbody.velocity = new Vector2(Random.Range(-3f, 3f), Random.Range(5f, 15f));
		_rigidbody.AddTorque((float)(Random.Range(0, 15) * (MMMaths.RandomBool() ? 1 : (-1))));
		_player = Singleton<Service>.Instance.levelManager.player;
		((MonoBehaviour)this).StartCoroutine(CUpdate());
		Weapon[] weapons = _player.playerComponents.inventory.weapon.weapons;
		foreach (Weapon weapon in weapons)
		{
			if (!((Object)(object)weapon == (Object)null) && (((Object)weapon).name.Equals("Skul", StringComparison.OrdinalIgnoreCase) || ((Object)weapon).name.Equals("HeroSkul", StringComparison.OrdinalIgnoreCase)))
			{
				_skulHeadController = weapon.equipped.GetComponent<SkulHeadController>();
				break;
			}
		}
	}

	private IEnumerator CUpdate()
	{
		float time = 0f;
		yield return Chronometer.global.WaitForSeconds(0.5f);
		Vector2 val = default(Vector2);
		while (!((Object)(object)_skulHeadController == (Object)null) && _skulHeadController.cooldown.stacks <= 0)
		{
			Bounds bounds = ((Collider2D)Singleton<Service>.Instance.levelManager.player.collider).bounds;
			Vector3 center = ((Bounds)(ref bounds)).center;
			((Vector2)(ref val))._002Ector(((Component)this).transform.position.x - center.x, ((Component)this).transform.position.y - center.y);
			float sqrMagnitude = ((Vector2)(ref val)).sqrMagnitude;
			time += Chronometer.global.deltaTime;
			if (time >= 0.5f && sqrMagnitude < 1f)
			{
				_skulHeadController.cooldown.time.remainTime = 0f;
				_poolObject.Despawn();
			}
			yield return null;
		}
		_poolObject.Despawn();
	}
}
