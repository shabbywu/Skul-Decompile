using System;
using UnityEngine;

namespace TwoDLaserPack;

public class PlayerMovement : MonoBehaviour
{
	public enum PlayerMovementType
	{
		Normal,
		FreeAim
	}

	public PlayerMovementType playerMovementType;

	public bool IsMoving;

	public float aimAngle;

	[Range(1f, 5f)]
	public float freeAimMovementSpeed = 2f;

	private float SmoothSpeedX;

	private float SmoothSpeedY;

	private const float SmoothMaxSpeedX = 7f;

	private const float SmoothMaxSpeedY = 7f;

	private const float AccelerationX = 22f;

	private const float AccelerationY = 22f;

	private const float DecelerationX = 33f;

	private const float DecelerationY = 33f;

	private Animator playerAnimator;

	private void Start()
	{
		if ((Object)(object)((Component)this).gameObject.GetComponent<Animator>() != (Object)null)
		{
			playerAnimator = ((Component)this).gameObject.GetComponent<Animator>();
		}
	}

	private void moveForward(float amount)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		Vector3 position = default(Vector3);
		((Vector3)(ref position))._002Ector(((Component)this).transform.position.x, ((Component)this).transform.position.y + amount * Time.deltaTime, ((Component)this).transform.position.z);
		((Component)this).transform.position = position;
	}

	private void moveBack(float amount)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		Vector3 position = default(Vector3);
		((Vector3)(ref position))._002Ector(((Component)this).transform.position.x, ((Component)this).transform.position.y - amount * Time.deltaTime, ((Component)this).transform.position.z);
		((Component)this).transform.position = position;
	}

	private void moveRight(float amount)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		Vector3 position = default(Vector3);
		((Vector3)(ref position))._002Ector(((Component)this).transform.position.x + amount * Time.deltaTime, ((Component)this).transform.position.y, ((Component)this).transform.position.z);
		((Component)this).transform.position = position;
	}

	private void moveLeft(float amount)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		Vector3 position = default(Vector3);
		((Vector3)(ref position))._002Ector(((Component)this).transform.position.x - amount * Time.deltaTime, ((Component)this).transform.position.y, ((Component)this).transform.position.z);
		((Component)this).transform.position = position;
	}

	private void HandlePlayerToggles()
	{
	}

	private void HandlePlayerMovement()
	{
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		//IL_0314: Unknown result type (might be due to invalid IL or missing references)
		//IL_0325: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0332: Unknown result type (might be due to invalid IL or missing references)
		//IL_034e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0373: Unknown result type (might be due to invalid IL or missing references)
		//IL_0374: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e4: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0303: Unknown result type (might be due to invalid IL or missing references)
		//IL_0308: Unknown result type (might be due to invalid IL or missing references)
		//IL_030d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0312: Unknown result type (might be due to invalid IL or missing references)
		//IL_0231: Unknown result type (might be due to invalid IL or missing references)
		//IL_024e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0270: Unknown result type (might be due to invalid IL or missing references)
		//IL_0272: Unknown result type (might be due to invalid IL or missing references)
		float axis = Input.GetAxis("Horizontal");
		float axis2 = Input.GetAxis("Vertical");
		if (Mathf.Abs(axis) > 0f || Mathf.Abs(axis2) > 0f)
		{
			IsMoving = true;
			if ((Object)(object)playerAnimator != (Object)null)
			{
				playerAnimator.SetBool("IsMoving", true);
			}
		}
		else
		{
			IsMoving = false;
			if ((Object)(object)playerAnimator != (Object)null)
			{
				playerAnimator.SetBool("IsMoving", false);
			}
		}
		Vector2 facingDirection = Vector2.zero;
		switch (playerMovementType)
		{
		case PlayerMovementType.Normal:
		{
			if (axis < 0f && SmoothSpeedX > -7f)
			{
				SmoothSpeedX -= 22f * Time.deltaTime;
			}
			else if (axis > 0f && SmoothSpeedX < 7f)
			{
				SmoothSpeedX += 22f * Time.deltaTime;
			}
			else if (SmoothSpeedX > 33f * Time.deltaTime)
			{
				SmoothSpeedX -= 33f * Time.deltaTime;
			}
			else if (SmoothSpeedX < -33f * Time.deltaTime)
			{
				SmoothSpeedX += 33f * Time.deltaTime;
			}
			else
			{
				SmoothSpeedX = 0f;
			}
			if (axis2 < 0f && SmoothSpeedY > -7f)
			{
				SmoothSpeedY -= 22f * Time.deltaTime;
			}
			else if (axis2 > 0f && SmoothSpeedY < 7f)
			{
				SmoothSpeedY += 22f * Time.deltaTime;
			}
			else if (SmoothSpeedY > 33f * Time.deltaTime)
			{
				SmoothSpeedY -= 33f * Time.deltaTime;
			}
			else if (SmoothSpeedY < -33f * Time.deltaTime)
			{
				SmoothSpeedY += 33f * Time.deltaTime;
			}
			else
			{
				SmoothSpeedY = 0f;
			}
			Vector2 val = default(Vector2);
			((Vector2)(ref val))._002Ector(((Component)this).transform.position.x + SmoothSpeedX * Time.deltaTime, ((Component)this).transform.position.y + SmoothSpeedY * Time.deltaTime);
			((Component)this).transform.position = Vector2.op_Implicit(val);
			break;
		}
		case PlayerMovementType.FreeAim:
			if (axis2 > 0f)
			{
				moveForward(freeAimMovementSpeed);
			}
			else if (axis2 < 0f)
			{
				moveBack(freeAimMovementSpeed);
			}
			if (axis > 0f)
			{
				moveRight(freeAimMovementSpeed);
			}
			else if (axis < 0f)
			{
				moveLeft(freeAimMovementSpeed);
			}
			facingDirection = Vector2.op_Implicit(Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f)) - ((Component)this).transform.position);
			break;
		}
		CalculateAimAndFacingAngles(facingDirection);
		Vector3 val2 = Camera.main.WorldToViewportPoint(((Component)this).transform.position);
		val2.x = Mathf.Clamp(val2.x, 0.05f, 0.95f);
		val2.y = Mathf.Clamp(val2.y, 0.05f, 0.95f);
		((Component)this).transform.position = Camera.main.ViewportToWorldPoint(val2);
	}

	private void CalculateAimAndFacingAngles(Vector2 facingDirection)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		aimAngle = Mathf.Atan2(facingDirection.y, facingDirection.x);
		if (aimAngle < 0f)
		{
			aimAngle = (float)Math.PI * 2f + aimAngle;
		}
		((Component)this).transform.eulerAngles = new Vector3(0f, 0f, aimAngle * 57.29578f);
	}

	private void Update()
	{
		HandlePlayerMovement();
		HandlePlayerToggles();
	}
}
