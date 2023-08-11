using Characters;
using Characters.Abilities;
using GameResources;
using Services;
using Singletons;
using UnityEngine;

namespace Level.Altars;

public class SteelAltar : MonoBehaviour
{
	[GetComponent]
	[SerializeField]
	private Altar _altar;

	[SerializeField]
	private ShieldComponent _shieldComponent;

	private string _floatingText;

	private void Awake()
	{
		_altar.onDestroyed += OnAltarDestroyed;
		_floatingText = Localization.GetLocalizedString("floating/altar/steel");
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
		player.ability.Add(_shieldComponent.ability);
		Bounds bounds = ((Collider2D)player.collider).bounds;
		Singleton<Service>.Instance.floatingTextSpawner.SpawnBuff(_floatingText, Vector2.op_Implicit(new Vector2(((Bounds)(ref bounds)).center.x, ((Bounds)(ref bounds)).max.y + 1f)));
	}
}
