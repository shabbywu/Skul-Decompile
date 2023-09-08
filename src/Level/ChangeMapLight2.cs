using Characters;
using UnityEngine;

namespace Level;

public class ChangeMapLight2 : MonoBehaviour
{
	[Information("레이어를 Interaction으로 설정하고 트리거 콜라이더를 넣어주세요.", InformationAttribute.InformationType.Info, false)]
	[SerializeField]
	private Color _color = Color.white;

	[SerializeField]
	private float _intensity = 1f;

	[SerializeField]
	private float _changingTime = 1f;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		Character component = ((Component)collision).GetComponent<Character>();
		if (!((Object)(object)component == (Object)null) && component.type == Character.Type.Player)
		{
			Map.Instance.ChangeLight(_color, _intensity, _changingTime);
		}
	}
}
