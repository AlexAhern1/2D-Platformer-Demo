using System.Collections;
using UnityEngine;

public class DamageFlashController : MonoBehaviour
{
    [SerializeField] protected Color flashColor = Color.white;
    [SerializeField] protected float flashTime = 0.25f;

    protected SpriteRenderer[] renderers;
    protected Material[] materials;

    //Coroutine damageFlashCoroutine;
    void Awake()
    {
        renderers = GetComponentsInChildren<SpriteRenderer>();
        SetupMaterials();
    }

    protected void SetupMaterials()
    {
        materials = new Material[renderers.Length];

        for (int i = 0; i < renderers.Length; i++)
        {
            materials[i] = renderers[i].material;
        }
    }

    protected virtual void CallDamageFlash()
    {
        //damageFlashCoroutine = StartCoroutine(DoDamageFlash());
        StartCoroutine(DoDamageFlash());
    }

    protected virtual IEnumerator DoDamageFlash()
    {
        foreach (Material mat in materials)
        {
            mat.SetColor("_FlashColor", flashColor);
        }

        float currentFlashAmount = 0f;
        float elapsedTime = 0f;

        while (elapsedTime < flashTime)
        {
            //elapse time
            elapsedTime += Time.deltaTime;

            //lerp through the flash color
            currentFlashAmount = Mathf.Lerp(1f, 0f, elapsedTime / flashTime);

            //iterating through the materials array and updating the flash color
            foreach (Material mat in materials)
            {
                mat.SetFloat("_FlashAmount", currentFlashAmount);
            }

            yield return null;
        }   
    }
}