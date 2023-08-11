using UnityEngine;

namespace Characters.AI;

public class MagicianBallAssigner : MonoBehaviour
{
	[SerializeField]
	private float _radius;

	private void Awake()
	{
		Assign();
	}

	private void Assign()
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
		if (((Component)this).transform.childCount <= 0)
		{
			Debug.LogError((object)(((Object)this).name + " has no child"));
			return;
		}
		int num = 360 / ((Component)this).transform.childCount;
		Vector3 val = Vector2.op_Implicit(Vector2.up * _radius);
		for (int i = 0; i < ((Component)this).transform.childCount; i++)
		{
			((Component)this).transform.GetChild(i).position = ((Component)this).transform.position + val;
			((Component)this).transform.GetChild(i).rotation = Quaternion.Euler(0f, 0f, (float)(num * i + 90));
			val = Quaternion.Euler(0f, 0f, (float)num) * val;
		}
	}
}
