using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public enum TargetSize {Large, Medium, Small};
    public TargetSize targetSize;
    TargetMovement _targetMovement;
    TargetDamage _targetDamage;
    SpawnChildTargets _childSpawner;

    private static List<Target> targets = new List<Target>();
    public static int ActiveTargetCount => targets.Count;

    public TargetDamage Damage 
    {
        get
        {
            if (_targetDamage == null)
            {
                _targetDamage = GetComponent<TargetDamage>();
                _targetDamage.parentTarget = this;
            }
            return _targetDamage;
        }
    }
    public TargetMovement Movement
    {
        get
        {
            if (_targetMovement == null)
            {
                _targetMovement = GetComponent<TargetMovement>();
                _targetMovement.parentTarget = this;
            }
            return _targetMovement;
        }
    }
    public SpawnChildTargets ChildSpawner
    {
        get
        {
            if (_childSpawner == null)
            {
                _childSpawner = GetComponent<SpawnChildTargets>();
            }
            if (_childSpawner != null)
            {
                _childSpawner.parentTarget = this;
            }
            return _childSpawner;
        }
    }
    public int GetTotalValue()
    {
        if (targetSize == TargetSize.Large)
        {
            return _targetDamage.GetRootVal() * 3;
        }
        else if (targetSize == TargetSize.Medium)
        {
            return _targetDamage.GetRootVal() * 2;
        }
        else
        {
            return _targetDamage.GetRootVal();
        }
    }
    private void OnEnable()
    {
        targets.Add(this);
    }
    private void OnDisable()
    {
        targets.Remove(this);
    }
}
