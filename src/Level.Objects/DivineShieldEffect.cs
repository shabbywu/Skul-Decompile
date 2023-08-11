using Characters;
using FX.Connections;
using UnityEngine;

namespace Level.Objects;

public class DivineShieldEffect : MonoBehaviour
{
	[SerializeField]
	private Connection _connection;

	public void Activate(Character target)
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		((Component)this).gameObject.SetActive(true);
		Vector2 endOffset = default(Vector2);
		((Vector2)(ref endOffset))._002Ector(0f, target.collider.size.y * 0.5f);
		_connection.Connect(((Component)this).transform, Vector2.zero, ((Component)target).transform, endOffset);
	}
}
