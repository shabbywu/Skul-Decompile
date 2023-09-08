using UnityEngine;

namespace TwoDLaserPack;

public class BloodEffect : MonoBehaviour
{
	public float fadespeed = 2f;

	public float timeBeforeFadeStarts = 1f;

	private float elapsedTimeBeforeFadeStarts;

	private SpriteRenderer sprite;

	private Color spriteColor;

	private void Awake()
	{
		sprite = ((Component)this).gameObject.GetComponent<SpriteRenderer>();
	}

	private void OnEnable()
	{
	}

	private void OnDisable()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		spriteColor = new Color(((Component)sprite).GetComponent<Renderer>().material.color.r, ((Component)sprite).GetComponent<Renderer>().material.color.g, ((Component)sprite).GetComponent<Renderer>().material.color.b, 1f);
	}

	private void Start()
	{
	}

	private void Update()
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0082: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		elapsedTimeBeforeFadeStarts += Time.deltaTime;
		if (elapsedTimeBeforeFadeStarts >= timeBeforeFadeStarts)
		{
			spriteColor = new Color(((Component)sprite).GetComponent<Renderer>().material.color.r, ((Component)sprite).GetComponent<Renderer>().material.color.g, ((Component)sprite).GetComponent<Renderer>().material.color.b, Mathf.Lerp(((Component)sprite).GetComponent<Renderer>().material.color.a, 0f, Time.deltaTime * fadespeed));
			((Component)sprite).GetComponent<Renderer>().material.color = spriteColor;
			if (((Renderer)sprite).material.color.a <= 0f)
			{
				((Component)this).gameObject.SetActive(false);
			}
		}
	}
}
