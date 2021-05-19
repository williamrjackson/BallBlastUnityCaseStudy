using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetSpawner : MonoBehaviour
{
    public int levelScoreTotal = 35;
    public int levelBallCount = 35;
    public int simultaneousBalls = 5;
    public float spawnRate = 5f;

    [SerializeField]
    Target largePrototype;
    [SerializeField]
    Target mediumPrototype;
    [SerializeField]
    Target smallPrototype;
    [SerializeField]
    Wall[] walls;

    private int ballValuesDeployed = 0;
    private void Start() {
        StartCoroutine(RunLevel());
    }
    private Target GetPrototypeBySize(int size)
    {
        size = Mathf.Clamp(size, 0, 2);
        switch (size)
        {
            case 0: return largePrototype;
            case 1: return mediumPrototype;
            default: return smallPrototype;
        }

    }
    private IEnumerator RunLevel()
    {
        float timeSinceLastSpawn = spawnRate;
        while(ballValuesDeployed < levelScoreTotal)
        {
            yield return new WaitForEndOfFrame();
            timeSinceLastSpawn += Time.deltaTime;
            if ((timeSinceLastSpawn > spawnRate && simultaneousBalls > Target.ActiveTargetCount)
                || Target.ActiveTargetCount == 0)
            {     
                timeSinceLastSpawn = 0f;           
                int ballSize = Random.Range(0, 3);
                Target newTarget = Instantiate(GetPrototypeBySize(ballSize));
                newTarget.gameObject.SetActive(true);
                newTarget.transform.parent = transform;
                int rootScore = levelScoreTotal / levelBallCount;
                newTarget.Damage.SetRootValue(rootScore);
                while (ballValuesDeployed + newTarget.GetTotalValue() > levelScoreTotal)
                {
                    rootScore--;
                    newTarget.Damage.SetRootValue(rootScore);
                }
                ballValuesDeployed += newTarget.GetTotalValue();
                Transform wallTransform = walls[0].transform;
                float targetXModification = newTarget.transform.lossyScale.x; 
                newTarget.Movement.isMovingRight = true;
                if (Wrj.Utils.CoinFlip)
                {
                    wallTransform = walls[1].transform;
                    targetXModification = -targetXModification;
                    newTarget.Movement.isMovingRight = false;
                } 
                newTarget.transform.position = newTarget.transform.position.With(
                    x: wallTransform.position.x,
                    y: Random.Range(0f, 4.25f)
                );
                newTarget.transform.EaseMove(newTarget.transform.PosInWorldDir(right: targetXModification), 1f);
                Wrj.Utils.DeferredExecution(1f, ()=> newTarget.Movement.enabled = true);
            }
        }        
    }

}
