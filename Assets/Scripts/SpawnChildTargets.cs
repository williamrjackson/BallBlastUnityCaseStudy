using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnChildTargets : MonoBehaviour
{
    [SerializeField]
    public Target childTarget;
    [HideInInspector]
    public Target parentTarget; 

    public void InstantiateChildren()
    {
        StartCoroutine(Spawn(true));
        StartCoroutine(Spawn(false));
    }

    private IEnumerator Spawn(bool right)
    {
        Target t = Instantiate(childTarget);
        t.transform.parent = transform.parent;
        t.transform.position = transform.position;
        t.gameObject.SetActive(true);
        t.Damage.SetRootValue(Mathf.CeilToInt(parentTarget.Damage.GetRootVal() / 2f));
        t.Movement.isMovingRight = right;
        float outward = (right) ? 1f : -1f;
        float upward = 1f;
        float duration = .75f;
        float elapsedTime = 0f;
        Vector3 targetPos = t.transform.PosInWorldDir(up:upward, right:outward);
        while (elapsedTime < duration)
        {
            yield return new WaitForEndOfFrame();
            elapsedTime += Time.deltaTime;
            float scrub = Mathf.InverseLerp(0f, duration, elapsedTime);
            float x = Wrj.Utils.MapToCurve.EaseIn.Lerp(transform.position.x, targetPos.x, scrub);
            float y = Wrj.Utils.MapToCurve.Linear.Lerp(transform.position.y, targetPos.y, scrub);
            t.transform.position = t.transform.position.With(x:x, y:y);
        }
        t.Movement.enabled = true;
    }
}
