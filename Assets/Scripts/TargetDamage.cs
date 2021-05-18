using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TargetDamage : MonoBehaviour
{
    [SerializeField]
    private int initialValue;
    [SerializeField]
    private TextMeshPro valueReadout;
    [SerializeField]
    private Gradient gradient;
    [SerializeField]
    private SpriteRenderer tintSprite;
    [SerializeField]
    private ParticleSystem explosionParticles;
    [SerializeField]
    private TargetMovement targetMovement;
    [SerializeField]
    private CircleCollider2D circleCollider2D;
    private int upperGradientValue => GameManager.Instance.bps * (GameManager.Instance.firePower / 100) * 2;
    private Wrj.Utils.MapToCurve.Manipulation manipulation;
    private bool hasExploded = false;

    void Start()
    {
        Value = initialValue;
    }

    private int _value;
    public int Value
    {
        get => _value;
        set
        {
            _value = value;
            valueReadout.text = FormatNumericReadout(value);
            SetColor(_value);
        }
    }
    private void SetColor(int value)
    {
        value = Mathf.Clamp(value, 0, upperGradientValue);
        tintSprite.color = gradient.Evaluate(Mathf.InverseLerp(0f, upperGradientValue, value));
    }
    public void DecValue()
    {
        Value -= GameManager.Instance.firePower / 100;
        if (Value < 1 && !hasExploded)
        {
            Explode();
        }
    }

    private void Explode()
    {
        hasExploded = true;
        circleCollider2D.enabled = false;
        targetMovement.enabled = false;
        tintSprite.enabled = false;
        valueReadout.enabled = false;
        explosionParticles.Play();
        
        //Schedule Destroy
        Destroy(gameObject, 3f);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Bullet bullet = other.GetComponent<Bullet>();
        if (bullet != null)
        {
            bullet.Hit();
            DecValue();
            if (manipulation == null || !manipulation.IsRunning)
            {
                manipulation = Wrj.Utils.MapToCurve.EaseOut.Scale(transform, transform.localScale * 1.1f, .15f, mirrorPingPong:1);
            }
        }
    }
    private string FormatNumericReadout(int value)
    {
        if (value >= 1000)
        {
            return (value / 1000f).ToString("0.#") + "k";
        }
        return value.ToString("#,0");
    }
}
