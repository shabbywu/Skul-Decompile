using Characters.Gear.Weapons;
using Characters.Player;
using UnityEngine;

namespace Characters;

public sealed class SilenceMark : MonoBehaviour
{
	[SerializeField]
	private Character _character;

	[SerializeField]
	private SpriteRenderer _image;

	private Weapon _lastWeapon;

	private void Update()
	{
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		if (!_character.silence.value)
		{
			if (((Renderer)_image).enabled)
			{
				((Renderer)_image).enabled = false;
			}
			return;
		}
		WeaponInventory weapon = _character.playerComponents.inventory.weapon;
		if (!((Renderer)_image).enabled || (Object)(object)_lastWeapon == (Object)null || (Object)(object)_lastWeapon != (Object)(object)weapon.current)
		{
			((Renderer)_image).enabled = true;
			Bounds bounds = ((Collider2D)_character.collider).bounds;
			_lastWeapon = weapon.current;
			((Component)_image).transform.position = Vector2.op_Implicit(new Vector2(((Bounds)(ref bounds)).min.x - 0.3f, ((Bounds)(ref bounds)).max.y + 0.35f));
		}
	}
}
