using System.Collections;
using UnityEngine;

namespace Level;

public class DroppedMummyGunSupply : MonoBehaviour
{
	[SerializeField]
	private PoolObject _poolObject;

	[SerializeField]
	private SpriteRenderer _parachuteRenderer;

	[SerializeField]
	private float _fallSpeed;

	private DroppedMummyGun _gun;

	private float _targetY;

	private RigidbodyConstraints2D _rigidbodyConstraints;

	private void OnDisable()
	{
		if (!((Object)(object)_gun == (Object)null))
		{
			_gun.onPickedUp -= OnGunPickedUp;
		}
	}

	public void Spawn(DroppedMummyGun droppedMummyGun, Vector3 position, float targetY)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		DroppedMummyGunSupply component = ((Component)_poolObject.Spawn(position, true)).GetComponent<DroppedMummyGunSupply>();
		component.Clear();
		component.Initialize(droppedMummyGun, targetY);
	}

	private void Clear()
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		foreach (Transform item in ((Component)this).transform)
		{
			DroppedMummyGun component = ((Component)item).GetComponent<DroppedMummyGun>();
			if ((Object)(object)component != (Object)null)
			{
				component.Despawn();
			}
		}
	}

	private void Initialize(DroppedMummyGun droppedMummyGun, float targetY)
	{
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		_gun = droppedMummyGun;
		_gun.onPickedUp -= OnGunPickedUp;
		_gun.onPickedUp += OnGunPickedUp;
		((Component)_gun).transform.SetParent(((Component)this).transform, false);
		((Component)_gun).transform.localPosition = Vector3.zero;
		_rigidbodyConstraints = _gun.rigidbody.constraints;
		_gun.rigidbody.constraints = (RigidbodyConstraints2D)7;
		_targetY = targetY;
		((Renderer)_parachuteRenderer).enabled = true;
		((MonoBehaviour)this).StartCoroutine(CFall());
	}

	private void OnGunPickedUp()
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		_gun.onPickedUp -= OnGunPickedUp;
		_gun.rigidbody.constraints = _rigidbodyConstraints;
		_poolObject.Despawn();
	}

	private IEnumerator CFall()
	{
		do
		{
			yield return null;
			((Component)this).transform.Translate(0f, (0f - _fallSpeed) * Chronometer.global.deltaTime, 0f);
		}
		while (!(((Component)this).transform.position.y < _targetY));
		Vector3 position = ((Component)this).transform.position;
		position.y = _targetY;
		((Component)this).transform.position = position;
		((Renderer)_parachuteRenderer).enabled = false;
	}
}
