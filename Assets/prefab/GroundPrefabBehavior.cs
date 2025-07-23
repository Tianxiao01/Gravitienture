using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundPrefabBehavior : MonoBehaviour
{
    public objectPool ObjectPool;
    public Transform player;


    void LateUpdate()
    {
        if (player.position.x - transform.position.x >= 3600)
        {
            ObjectPool.Return(this.gameObject);
        }
    }
}
