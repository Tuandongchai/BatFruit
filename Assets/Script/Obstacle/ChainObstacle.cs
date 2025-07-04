using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChainObstacle : Obstacle
{
    [Header("Setup")]
    [SerializeField] private GameObject chainBroke_VFX;


    public override void DestroyThis()
    {
        Instantiate(chainBroke_VFX, gameObject.transform.parent.position, Quaternion.identity);
        obstacleBroken?.Invoke(ObstacleCellType.chain);
        //transform.GetComponent<FruitCell>().chainObject = null;
        Destroy(gameObject, 0.1f);           
    }
    
}
