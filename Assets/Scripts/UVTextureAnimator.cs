using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UVTextureAnimator : MonoBehaviour
{
    private Renderer uvRenderer;

    float offsetTimer = 0; // timer for frame animation
    float offsetDelay = .2f; // seconds between each frame
    float offsetSpeed = .2f; // slide speed (units per second)

    private void Start()
    {
        uvRenderer = this.GetComponent<Renderer>();
    }

    private void Update()
    {
        AnimateTextureUV();
    }

    private void AnimateTextureUV()
    {
        if (uvRenderer == null) return;

        offsetTimer -= Time.deltaTime;
        Vector2 offset = uvRenderer.sharedMaterial.mainTextureOffset;
        // offset x
        if (offsetTimer <= 0)
        {
            offsetTimer = offsetDelay;
            offset.x += .25f;
            if (offset.x >= 1f) { offset.x = 0; } // cap
        }
        // offset y
        offset.y += offsetSpeed * Time.deltaTime;
        if (offset.y >= 1f) { offset.y -= 1; } // cap
        // apply
        uvRenderer.sharedMaterial.mainTextureOffset = offset;
    }
}
