using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class groundGenerator : MonoBehaviour
{
    public objectPool GrndPool;
    public Transform playerTr;

    private Vector3 PositionOfLastGeneratedGround;
    private bool isGravityNormal = true;


    bool IsReached()
    {
        if (playerTr.position.x+3600 >=PositionOfLastGeneratedGround.x )
        {
            return true;
        }
        return false;
    }
    void Generate()
    {
        if (IsReached())
        {
            GameObject newGround = GrndPool.Get();
            bool ShouldChangeGravity = Random.value > 0.5f;
            int[] DeltaSpawnPostion = new int[] {};
            int DeltaSpawnPostionX = 0;
            int DeltaSpawnPostionY = 0;
            int DeltaSpawnPostionZ = 0;
     
            int newXPs = 0;
            int newYPs = 0;
            int newZPs = 0;


            DeltaSpawnPostion = randomDelta(ShouldChangeGravity);
            DeltaSpawnPostionX = DeltaSpawnPostion[0];
            DeltaSpawnPostionY = DeltaSpawnPostion[1];
            DeltaSpawnPostionZ = DeltaSpawnPostion[2];

            newXPs = (int)PositionOfLastGeneratedGround.x + DeltaSpawnPostionX;
            newYPs = (int)PositionOfLastGeneratedGround.y + DeltaSpawnPostionY;
            newZPs = (int)PositionOfLastGeneratedGround.z + DeltaSpawnPostionZ;


            
            Vector3 SpawnPostion = new Vector3(newXPs, newYPs, newZPs);
            newGround.transform.position = SpawnPostion;
            PositionOfLastGeneratedGround = SpawnPostion;
            changeNextGndGravityState(ShouldChangeGravity);
        }
    }

    int[] randomDelta(bool shouldChangeGravity)
    {
        int DeltaSpawnPostionX = Random.Range(800, 1350);
        int DeltaSpawnPostionY = 0;
        int DeltaSpawnPostionZ = 0;
        
        if (isGravityNormal)
        {
            DeltaSpawnPostionY = Random.Range(-350, 351);
            if (shouldChangeGravity)
            {
                DeltaSpawnPostionZ = Random.Range(200, 351);
            }
        }
        else
        {
            DeltaSpawnPostionZ = Random.Range(-350, 351);
            if (shouldChangeGravity)
            {
                DeltaSpawnPostionY = Random.Range(-350, -199);
            }
        }
        return new int[] {DeltaSpawnPostionX,DeltaSpawnPostionY,DeltaSpawnPostionZ};
    }

    void changeNextGndGravityState(bool shouldChangeGravity)
    {
        if (shouldChangeGravity)
        {
            isGravityNormal = !isGravityNormal;
        }
    }

    void Start()
    {
        PositionOfLastGeneratedGround = new Vector3(2450, -425, 1000);
    }


    void Update()
    {
        Generate();
    }
}
