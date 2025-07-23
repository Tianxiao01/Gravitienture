using System.Collections;
using UnityEngine;

public class chaserBehavior : MonoBehaviour
{
    public GameObject Manager;
    public Transform player;
    private float chasingSpeed = 300f;


    void Start()
    {
        StartCoroutine(SpeedInc());
    }

    private IEnumerator SpeedInc()
    {
        while (chasingSpeed < 800f)
        {
            yield return new WaitForSeconds(5f);
            chasingSpeed += 50f;
            chasingSpeed = Mathf.Clamp(chasingSpeed, 100f, 800f);
        }
    }


    void Update()
    {
        Vector3 newPostion = transform.position;
        newPostion.x += chasingSpeed * Time.deltaTime;
        newPostion.y = player.position.y+200;
        newPostion.z = player.position.z;

        transform.position = newPostion;
        //Debug.Log(chasingSpeed);
    }
}
