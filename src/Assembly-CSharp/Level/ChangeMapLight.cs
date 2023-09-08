using Characters;
using UnityEngine;

namespace Level;

public class ChangeMapLight : MonoBehaviour
{
	[SerializeField]
	[Information("레이어를 Interaction으로 설정하고 트리거 콜라이더를 넣어주세요.", InformationAttribute.InformationType.Info, false)]
	private Color _color = Color.white;

	[SerializeField]
	private float _intensity = 1f;

	[SerializeField]
	private float _changingTime = 1f;

	private bool CheckPlayer(GameObject target)
	{
		Character component = target.GetComponent<Character>();
		if ((Object)(object)component == (Object)null)
		{
			return false;
		}
		if (component.type != Character.Type.Player)
		{
			return false;
		}
		return true;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		if (CheckPlayer(((Component)collision).gameObject))
		{
			Map.Instance.ChangeLight(_color, _intensity, _changingTime);
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (CheckPlayer(((Component)collision).gameObject))
		{
			Map.Instance.RestoreLight(_changingTime);
		}
	}
}
