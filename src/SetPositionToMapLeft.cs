using Level;
using UnityEngine;

public class SetPositionToMapLeft : MonoBehaviour
{
	private void Start()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		Transform transform = ((Component)this).transform;
		Bounds bounds = Map.Instance.bounds;
		float x = ((Bounds)(ref bounds)).min.x;
		bounds = Map.Instance.bounds;
		transform.position = Vector2.op_Implicit(new Vector2(x, ((Bounds)(ref bounds)).max.y));
	}
}
