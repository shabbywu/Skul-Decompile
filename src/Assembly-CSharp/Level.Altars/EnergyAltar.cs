using System;
using System.Collections;
using Characters;
using Characters.Abilities;
using GameResources;
using Services;
using Singletons;
using UnityEngine;

namespace Level.Altars;

public class EnergyAltar : MonoBehaviour
{
	[SerializeField]
	[GetComponent]
	private Altar _altar;

	[SerializeField]
	private PoolObject _effect;

	[SerializeField]
	private HealComponent _healComponent;

	private string _floatingText;

	private void Awake()
	{
		((MonoBehaviour)this).StartCoroutine(CHeal());
		_altar.onDestroyed += OnAltarDestroyed;
		_floatingText = Localization.GetLocalizedString("floating/altar/energy");
	}

	private void OnAltarDestroyed()
	{
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		Character player = Singleton<Service>.Instance.levelManager.player;
		player.ability.Add(_healComponent.ability);
		Bounds bounds = ((Collider2D)player.collider).bounds;
		Singleton<Service>.Instance.floatingTextSpawner.SpawnBuff(_floatingText, Vector2.op_Implicit(new Vector2(((Bounds)(ref bounds)).center.x, ((Bounds)(ref bounds)).max.y + 1f)));
		((MonoBehaviour)this).StopCoroutine("CHeal");
	}

	private IEnumerator CHeal()
	{
		while (true)
		{
			yield return Chronometer.global.WaitForSeconds(3f);
			foreach (Character character in _altar.characters)
			{
				if (!character.health.dead)
				{
					character.health.Heal(Math.Max(5.0, character.health.maximumHealth * 0.0666));
				}
			}
		}
	}
}
