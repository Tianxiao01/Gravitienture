using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraBehavior : MonoBehaviour
{
    public Transform playerTransforom;
    public float smoothSpeed = 0.1f;
    public Vector3 offset = new Vector3(0f,0f,-800f);

    void LateUpdate()
    {
        Vector3 desiredPostion = playerTransforom.position + offset;
        Vector3 smoothPostion = Vector3.Lerp(transform.position, desiredPostion, smoothSpeed);
        transform.position = smoothPostion;
    }
}
