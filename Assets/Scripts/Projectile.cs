using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private float maxY = 10f;
    [SerializeField]
    private int initialPoolSize = 50;
    [SerializeField]
    private Transform projectilePoolParent;
    [SerializeField]
    Bullet[] subProjectiles;

    private float maxSpacing = .18f;
    private float yPosForFullWidth = -.65f;
    private Wrj.LayoutGroup3d layoutGroup;
    private static Projectile prototypeProjectile;
    private static List<Projectile> projectilePool = new List<Projectile>();
    // Start is called before the first frame update
    void Awake()
    {
        if (prototypeProjectile == null)
        {
            prototypeProjectile = this;
            gameObject.SetActive(false);

            for (int i = 0; i < initialPoolSize; i++)
            {
                CreateNewProjectile();
            }
        }
    }

    private void CreateNewProjectile()
    {
        Projectile newProjectile = Instantiate(prototypeProjectile);
        newProjectile.layoutGroup = newProjectile.GetComponent<Wrj.LayoutGroup3d>();
        newProjectile.transform.parent = projectilePoolParent;
        newProjectile.AddToPool();
    }

    private void AddToPool()
    {
        layoutGroup.horizontalSpacing = 0f;
        layoutGroup.Refresh();
        gameObject.SetActive(false);
        transform.position = prototypeProjectile.transform.position;
        transform.rotation = prototypeProjectile.transform.rotation;
        projectilePool.Add(this);
    }

    public static Projectile GetFromPool()
    {
        if (projectilePool.Count < 2)
        {
            prototypeProjectile.CreateNewProjectile();
        }
        Projectile toServe = projectilePool[0];
        foreach(Bullet bullet in toServe.subProjectiles)
        {
            bullet.Restore();
        }
        projectilePool.Remove(toServe);
        toServe.transform.position = prototypeProjectile.transform.position;
        toServe.transform.rotation = prototypeProjectile.transform.rotation;
        return toServe;
    }

    public void SetCount(int count)
    {
        count = Mathf.Clamp(count, 1, subProjectiles.Length);
        int i = 0;
        foreach (Bullet item in subProjectiles)
        {
            item.gameObject.SetActive(i < count);
            i++;
        }
        layoutGroup.Refresh();
    }

    void Update()
    {
        transform.position = transform.PosInWorldDir(up: GameManager.Instance.projectileSpeed * Time.deltaTime);
        float y = transform.position.y;
        if (y > maxY)
        {
            AddToPool();
        }
        else if (y < yPosForFullWidth)
        {
            layoutGroup.horizontalSpacing = y.Remap(prototypeProjectile.transform.position.y, yPosForFullWidth, 0f, maxSpacing);
            layoutGroup.Refresh();
        }
        else
        {
            layoutGroup.horizontalSpacing = maxSpacing;
            layoutGroup.Refresh();
        }
    }
}
