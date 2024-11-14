using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flash : MonoBehaviour
{
    [SerializeField] private Material whileFlashMat;
    [SerializeField] private float restoreDefaulMaTime = .2f;
    private Material defaultMat;
    private SpriteRenderer spriteRenderer;
    private void Awake ()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultMat = spriteRenderer.material;
    }
    public float GetRestoreMatTime()
    {
        return restoreDefaulMaTime;
    }
    public IEnumerator FlashRountine()
    {
        spriteRenderer.material = whileFlashMat;
        yield return new WaitForSeconds(restoreDefaulMaTime);
        spriteRenderer.material = defaultMat;
    }

    
}
