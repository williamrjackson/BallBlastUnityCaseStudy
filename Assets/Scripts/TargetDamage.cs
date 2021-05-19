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
    private int upperGradientValue => GameManager.Instance.bps * (GameManager.Instance.firePower / 100);
    private Wrj.Utils.MapToCurve.Manipulation manipulation;
    private bool hasExploded = false;

    public Target parentTarget;
    void Start()
    {
        if (_value < 0)
            Value = initialValue;
    }

    private int _value = -1;
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
    public void SetRootValue(int rootVal)
    {
        initialValue = rootVal;
        Value = rootVal;
    }
    public int GetRootVal()
    {
        return initialValue;
    }

    private void SetColor(int value)
    {
        value = Mathf.Clamp(value, 1, upperGradientValue);
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
        parentTarget.enabled = false;
        tintSprite.enabled = false;
        valueReadout.enabled = false;
        explosionParticles.Play();
        if (parentTarget.ChildSpawner != null)
        {
            parentTarget.ChildSpawner.InstantiateChildren();
        }
        
        //Schedule Destroy
        Destroy(gameObject, 2f);
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
