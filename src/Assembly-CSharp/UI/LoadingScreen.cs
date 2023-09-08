using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI;

public class LoadingScreen : MonoBehaviour
{
	public struct LoadingScreenData
	{
		public readonly Sprite background;

		public readonly AnimationClip walkClip;

		public readonly string stageName;

		public readonly string description;

		public readonly bool displayTime;

		public readonly string currentTime;

		public readonly string bestTime;

		public readonly bool bestTimeUpdated;

		public LoadingScreenData(Sprite background, AnimationClip walkClip, string stageName, string description, string currentTime, string bestTime, bool bestTimeUpdated)
		{
			this.background = background;
			this.walkClip = walkClip;
			this.stageName = stageName;
			this.description = description;
			displayTime = true;
			this.currentTime = currentTime;
			this.bestTime = bestTime;
			this.bestTimeUpdated = bestTimeUpdated;
		}

		public LoadingScreenData(Sprite background, AnimationClip walkClip, string stageName, string description)
		{
			this.background = background;
			this.walkClip = walkClip;
			this.stageName = stageName;
			this.description = description;
			displayTime = false;
			currentTime = string.Empty;
			bestTime = string.Empty;
			bestTimeUpdated = false;
		}
	}

	public const float minimumDisplayingTime = 3f;

	public const string walkAnimationName = "Walk";

	private static readonly Color _bestTimeColor = new Color(0.635f, 0.509f, 0.407f);

	private static readonly Color _updatedBestTimeColor = new Color(1f, 0.823f, 0f);

	[SerializeField]
	private CanvasGroup _canvasGroup;

	[SerializeField]
	[Space]
	private Image _background;

	[SerializeField]
	[Header("Loading Animation")]
	private Image _loadingAnimation;

	[SerializeField]
	private Image _loadingAnimation_AlphaAddtion1;

	[SerializeField]
	private Image _loadingAnimation_AlphaAddtion2;

	[Header("Loading Animator")]
	[SerializeField]
	private Animator _loadingAnimator;

	[SerializeField]
	private Animator _loadingAnimator_AlphaAddtion1;

	[SerializeField]
	private Animator _loadingAnimator_AlphaAddtion2;

	[Space]
	[SerializeField]
	private AnimationClip _defaultWalkClip;

	[SerializeField]
	private TMP_Text _name;

	[SerializeField]
	private TMP_Text _description;

	[SerializeField]
	[Space]
	private GameObject _currentTimeContainer;

	[SerializeField]
	private TMP_Text _currentTime;

	[Space]
	[SerializeField]
	private GameObject _bestTimeContainer;

	[SerializeField]
	private TMP_Text _bestTime;

	[Space]
	[SerializeField]
	private GameObject _difficultyContainer;

	[SerializeField]
	private TMP_Text _difficulty;

	private AnimatorOverrideController _loadingAnimatorOverrider;

	private SpriteRenderer[] spriteRenderer = (SpriteRenderer[])(object)new SpriteRenderer[3];

	private void Awake()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Expected O, but got Unknown
		_loadingAnimatorOverrider = new AnimatorOverrideController(_loadingAnimator.runtimeAnimatorController);
		_loadingAnimator.runtimeAnimatorController = (RuntimeAnimatorController)(object)_loadingAnimatorOverrider;
		_loadingAnimator_AlphaAddtion1.runtimeAnimatorController = (RuntimeAnimatorController)(object)_loadingAnimatorOverrider;
		_loadingAnimator_AlphaAddtion2.runtimeAnimatorController = (RuntimeAnimatorController)(object)_loadingAnimatorOverrider;
		spriteRenderer[0] = ((Component)_loadingAnimation).gameObject.AddComponent<SpriteRenderer>();
		spriteRenderer[1] = ((Component)_loadingAnimation_AlphaAddtion1).gameObject.AddComponent<SpriteRenderer>();
		spriteRenderer[2] = ((Component)_loadingAnimation_AlphaAddtion2).gameObject.AddComponent<SpriteRenderer>();
		_canvasGroup.alpha = 0f;
	}

	public IEnumerator CShow(LoadingScreenData loadingScreenData)
	{
		float t = 0f;
		_background.sprite = loadingScreenData.background;
		if ((Object)(object)loadingScreenData.walkClip == (Object)null)
		{
			_loadingAnimatorOverrider["Walk"] = _defaultWalkClip;
		}
		else
		{
			_loadingAnimatorOverrider["Walk"] = loadingScreenData.walkClip;
		}
		_loadingAnimator.Update(0f);
		_loadingAnimator_AlphaAddtion1.Update(0f);
		_loadingAnimator_AlphaAddtion2.Update(0f);
		((Graphic)_loadingAnimation).SetNativeSize();
		((Graphic)_loadingAnimation_AlphaAddtion1).SetNativeSize();
		((Graphic)_loadingAnimation_AlphaAddtion2).SetNativeSize();
		_name.text = loadingScreenData.stageName;
		_description.text = loadingScreenData.description;
		if (loadingScreenData.displayTime)
		{
			_currentTimeContainer.SetActive(true);
			_bestTimeContainer.SetActive(true);
			_currentTime.text = loadingScreenData.currentTime;
			_bestTime.text = loadingScreenData.bestTime;
			((Graphic)_bestTime).color = (loadingScreenData.bestTimeUpdated ? _updatedBestTimeColor : _bestTimeColor);
		}
		else
		{
			_currentTimeContainer.SetActive(false);
			_bestTimeContainer.SetActive(false);
		}
		_canvasGroup.alpha = 0f;
		yield return null;
		for (; t < 1f; t += Time.unscaledDeltaTime * 2f)
		{
			_canvasGroup.alpha = t;
			yield return null;
		}
		_canvasGroup.alpha = 1f;
	}

	private void Update()
	{
		if (_canvasGroup.alpha != 0f)
		{
			_loadingAnimation.sprite = spriteRenderer[0].sprite;
			((Graphic)_loadingAnimation).SetNativeSize();
			_loadingAnimation_AlphaAddtion1.sprite = spriteRenderer[1].sprite;
			((Graphic)_loadingAnimation_AlphaAddtion1).SetNativeSize();
			_loadingAnimation_AlphaAddtion2.sprite = spriteRenderer[2].sprite;
			((Graphic)_loadingAnimation_AlphaAddtion2).SetNativeSize();
		}
	}

	public IEnumerator CHide()
	{
		float t = 0f;
		_canvasGroup.alpha = 1f;
		yield return null;
		for (; t < 1f; t += Time.unscaledDeltaTime * 2f)
		{
			_canvasGroup.alpha = 1f - t;
			yield return null;
		}
		_loadingAnimatorOverrider["Walk"] = null;
		_canvasGroup.alpha = 0f;
	}
}
