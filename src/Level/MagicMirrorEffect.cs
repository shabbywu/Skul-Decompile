using Characters;
using Characters.Player;
using Services;
using Singletons;
using UnityEngine;

namespace Level;

public sealed class MagicMirrorEffect : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer _mirrorSkul;

	[SerializeField]
	private float _hideDistance = 5f;

	[SerializeField]
	private float _movementDelta;

	private Character _player;

	private Transform _playerTransform;

	private WeaponInventory _weaponInventory;

	private void Start()
	{
		_player = Singleton<Service>.Instance.levelManager.player;
		_weaponInventory = _player.playerComponents.inventory.weapon;
		_playerTransform = ((Component)_player).transform;
	}

	private void Update()
	{
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ca: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)_weaponInventory == (Object)null))
		{
			_mirrorSkul.sprite = _weaponInventory.polymorphOrCurrent.characterAnimation.spriteRenderer.sprite;
			_mirrorSkul.flipX = _player.lookingDirection == Character.LookingDirection.Left;
			float num = Mathf.Abs(((Component)this).transform.position.x - _playerTransform.position.x);
			Vector3 val = _playerTransform.position - ((Component)this).transform.position;
			Vector3 normalized = ((Vector3)(ref val)).normalized;
			if (num > _hideDistance)
			{
				((Component)_mirrorSkul).gameObject.SetActive(false);
				return;
			}
			((Component)_mirrorSkul).gameObject.SetActive(true);
			Vector3 position = ((Component)this).transform.position + _movementDelta * num * normalized;
			position.y = _playerTransform.position.y + 0.26f;
			((Component)_mirrorSkul).transform.position = position;
		}
	}
}
