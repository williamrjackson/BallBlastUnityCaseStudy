using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField]
    private Collider2D collider2d;
    [SerializeField]
    private SpriteRenderer spriteRenderer;
    public void Hit()
    {
        collider2d.enabled = false;
        spriteRenderer.enabled = false;
    }
    public void Restore()
    {
        collider2d.enabled = true;
        spriteRenderer.enabled = true;        
    }
}
