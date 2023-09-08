using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class Light2DLookupTexturePreloader : MonoBehaviour
{
	private void Start()
	{
		Light2DLookupTexture.CreateLookupTextures();
		Object.Destroy((Object)(object)this);
	}
}
