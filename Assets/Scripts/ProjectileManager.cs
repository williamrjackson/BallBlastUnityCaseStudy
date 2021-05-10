using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileManager : MonoBehaviour
{
    Coroutine shootingRoutine;
    void Update()
    {
        if (Input.GetMouseButton(0) && shootingRoutine == null)
        {
            shootingRoutine = StartCoroutine(Shoot());
        }
        else if (!Input.GetMouseButton(0) && shootingRoutine != null)
        {
            StopCoroutine(shootingRoutine);
            shootingRoutine = null;
        }
    }

    private IEnumerator Shoot()
    {
        while(true)
        {
            Projectile shotProjectile = Projectile.GetFromPool();
            shotProjectile.gameObject.SetActive(true);
            shotProjectile.SetCount(Random.Range(4, 6));
            yield return new WaitForSeconds(.1f);
        }
    }
}
