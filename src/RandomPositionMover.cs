using UnityEngine;

public class RandomPositionMover : MonoBehaviour
{
	public float pickerInterval;

	public float radius;

	public GameObject player;

	public Vector2 randomPointInCircle;

	private void Start()
	{
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		if (pickerInterval == 0f)
		{
			pickerInterval = 3f;
		}
		randomPointInCircle = Vector2.zero;
		((MonoBehaviour)this).InvokeRepeating("PickRandomPointInCircle", Random.Range(0f, pickerInterval), pickerInterval);
	}

	private void PickRandomPointInCircle()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		((Component)this).transform.position = player.transform.position;
		randomPointInCircle = Vector2.op_Implicit(((Component)this).transform.localPosition) + Random.insideUnitCircle * radius;
		((Component)this).transform.localPosition = Vector2.op_Implicit(randomPointInCircle);
	}

	private void Update()
	{
	}
}
