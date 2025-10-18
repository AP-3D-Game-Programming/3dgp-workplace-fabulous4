using System;
using System.Collections;
using UnityEngine;

public class NPCInteractable : MonoBehaviour
{
    public string requiredItem = "SpecialItem";
    public float fadeDuration = 0.6f;
    public float despawnDelay = 0.1f;

    public event Action onDespawn;

    public void Interact()
    {
        // vind speler (gebruik Tag "Player" op je player GameObject)
        var player = GameObject.FindWithTag("Player");
        if (player == null) return;

        var inv = player.GetComponent<PlayerInventory>();
        if (inv == null) return;

        if (inv.HasItem(requiredItem))
        {
            Debug.Log($"Interact: player has {requiredItem}, start fade+despawn");
            StartCoroutine(FadeOutAndDespawn());
        }
        else
        {
            Debug.Log("Interact: player does not have required item");
        }
    }

    IEnumerator FadeOutAndDespawn()
    {
        // verzamel renderers
        var rends = GetComponentsInChildren<Renderer>();
        // maak kopieën van materialen zodat we geen shared materials kapotmaken
        var mats = new Material[rends.Length][];
        for (int i = 0; i < rends.Length; i++)
        {
            var r = rends[i];
            var original = r.sharedMaterials;
            var instances = new Material[original.Length];
            for (int j = 0; j < original.Length; j++)
            {
                instances[j] = new Material(original[j]);
                // probeer de standaard blend mode op Transparency te zetten (works for Standard shader)
                if (instances[j].HasProperty("_Mode"))
                {
                    instances[j].SetFloat("_Mode", 3);
                    instances[j].SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                    instances[j].SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                    instances[j].SetInt("_ZWrite", 0);
                    instances[j].DisableKeyword("_ALPHATEST_ON");
                    instances[j].EnableKeyword("_ALPHABLEND_ON");
                    instances[j].DisableKeyword("_ALPHAPREMULTIPLY_ON");
                    instances[j].renderQueue = 3000;
                }
            }
            r.materials = instances;
            mats[i] = instances;
        }

        float t = 0f;
        // fade alpha from 1 -> 0
        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            float a = Mathf.Clamp01(1f - (t / fadeDuration));
            // apply alpha to all materials that have _Color
            for (int i = 0; i < mats.Length; i++)
            {
                var layer = mats[i];
                for (int j = 0; j < layer.Length; j++)
                {
                    var m = layer[j];
                    if (m == null) continue;
                    if (m.HasProperty("_Color"))
                    {
                        Color c = m.color;
                        c.a = a;
                        m.color = c;
                    }
                }
            }
            yield return null;
        }

        // korte extra wacht-tijd indien gewenst
        yield return new WaitForSeconds(despawnDelay);

        onDespawn?.Invoke();
        Destroy(gameObject);
    }
}
