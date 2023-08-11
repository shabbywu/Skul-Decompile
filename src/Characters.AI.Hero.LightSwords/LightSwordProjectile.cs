using System.Collections;
using UnityEngine;

namespace Characters.AI.Hero.LightSwords;

public class LightSwordProjectile : MonoBehaviour
{
	[SerializeField]
	private GameObject _body;

	[SerializeField]
	private float _duration = 0.5f;

	public IEnumerator CFire(Vector2 firePosition, Vector2 destination, float angle)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		Initialize(firePosition, angle);
		Show();
		yield return CMove(firePosition, destination);
		Hide();
	}

	private IEnumerator CMove(Vector2 src, Vector2 dest)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		float elapsed = 0f;
		while (elapsed < _duration)
		{
			yield return null;
			elapsed += ((ChronometerBase)Chronometer.global).deltaTime;
			((Component)this).transform.position = Vector2.op_Implicit(Vector2.Lerp(src, dest, elapsed / _duration));
		}
		((Component)this).transform.position = Vector2.op_Implicit(dest);
	}

	private void Initialize(Vector2 position, float angle)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		((Component)this).transform.position = Vector2.op_Implicit(position);
		_body.transform.rotation = Quaternion.Euler(0f, 0f, angle);
	}

	private void Show()
	{
		_body.SetActive(true);
	}

	public void Hide()
	{
		_body.SetActive(false);
	}
}
