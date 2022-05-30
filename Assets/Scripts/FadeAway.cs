using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeAway : MonoBehaviour
{
    [SerializeField] float fadeSpeed;
    SpriteRenderer sprite;
    Color OriginalColor;
    Color OriginalColorHolder;

    private void Start()
    {
        sprite = GetComponent<SpriteRenderer>();
        OriginalColor = sprite.color;
        OriginalColorHolder = OriginalColor;

    }



    public void Fade()
    {

        StartCoroutine(DisableNPC());

    }

    public void PlayerFading()
    {
        StartCoroutine(FadeOut());
    }

    IEnumerator PlayerFade()
    {
        yield return FadeOut();
        
    }
    IEnumerator DisableNPC()
    {
        yield return FadeOut();
        gameObject.SetActive(false);
    }

    IEnumerator FadeOut()
    {
        float alpha = sprite.color.a;
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / fadeSpeed)
        {
            Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha, 0, t));
            sprite.color = newColor;
            yield return null;
        }
        
    }
    public void UnFade()
    {
        Debug.Log("unfade");
        sprite.color = new Color(1, 1, 1, 255 );
    }
}
