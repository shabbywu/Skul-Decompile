using System.Collections;
using Characters;
using Characters.Gear.Weapons;
using Services;
using Singletons;
using UnityEngine;

namespace Tutorials;

public class GiveBasicSkul : MonoBehaviour
{
	[SerializeField]
	private Weapon _basicSkul;

	private IEnumerator Start()
	{
		Character player = Singleton<Service>.Instance.levelManager.player;
		player.CancelAction();
		yield return (object)new WaitForEndOfFrame();
		player.playerComponents.inventory.weapon.ForceEquipAt(_basicSkul.Instantiate(), 0);
		player.animationController.ForceUpdate();
		Object.Destroy((Object)(object)this);
	}
}
