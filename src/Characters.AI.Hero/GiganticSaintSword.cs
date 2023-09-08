using System.Collections;
using UnityEngine;

namespace Characters.AI.Hero;

public class GiganticSaintSword : MonoBehaviour
{
	public delegate void OnStuckDelegate();

	[SerializeField]
	[Header("Projectiles")]
	private GameObject _projectile;

	[SerializeField]
	private float _dropDuration;

	[SerializeField]
	[Header("Stuck")]
	private GameObject _stuck;

	public bool isStuck => _stuck.gameObject.activeSelf;

	public event OnStuckDelegate OnStuck;

	public void Fire(Vector2 firePosition, float destY)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		_projectile.transform.position = Vector2.op_Implicit(firePosition);
		_projectile.SetActive(true);
		((MonoBehaviour)this).StartCoroutine(CMove(destY));
	}

	private IEnumerator CMove(float destY)
	{
		float elapsed = 0f;
		Vector3 source = _projectile.transform.position;
		Vector2 dest = new Vector2(source.x, destY);
		while (elapsed < _dropDuration)
		{
			elapsed += Chronometer.global.deltaTime;
			_projectile.transform.position = Vector2.op_Implicit(Vector2.Lerp(Vector2.op_Implicit(source), dest, elapsed / _dropDuration));
			yield return null;
		}
		_projectile.transform.position = Vector2.op_Implicit(dest);
		Stuck(dest);
	}

	private void Stuck(Vector2 point)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		this.OnStuck?.Invoke();
		_projectile.SetActive(false);
		_stuck.transform.position = Vector2.op_Implicit(point);
		_stuck.SetActive(true);
	}

	public void Despawn()
	{
		_projectile.SetActive(false);
		_stuck.SetActive(false);
	}
}
