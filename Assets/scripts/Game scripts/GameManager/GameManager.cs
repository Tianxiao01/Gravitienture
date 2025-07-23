using UnityEngine;
using System.Collections;
using System;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    public Transform chaserTransformation;
    public Rigidbody playerRB;
    public Transform Camera;
    public Transform playerTr;
    public cameraBehavior cameraOfst;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI survivalTimeT;
    public TextMeshProUGUI timesGravityChange;
    public TextMeshProUGUI scorePerSecond;
    public GameObject gameEndPanel;
    public GameObject instructionPanel;
    public DataBaseConnection DB;
    

    private bool GravityStateNormal = true;
    private float CameraRotationTime = 1f;
    private int baseScore=0;
    private int bonusScore = 0;
    private int score = 0;
    private float survivalTime = 0;
    private int currentDistance = 0;
    private int MaxDistance = 0;
    private int TimesGracityChange = 0;

    [HideInInspector]
    public bool isGameEnded = false;

    void gravityChange()
    {
        GravityStateNormal = !GravityStateNormal;
        if (GravityStateNormal)
        {
            Physics.gravity = new Vector3(0f, -981f, 0f);
            playerRB.constraints = RigidbodyConstraints.None;
            playerRB.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionZ;
            StartCoroutine(SmoothRotate(Camera, Quaternion.Euler(90f, 0f, 0f), Quaternion.Euler(0f, 0f, 0f), CameraRotationTime));
            StartCoroutine(SmoothRotate(chaserTransformation, Quaternion.Euler(90f, 0f, 0f), Quaternion.Euler(0f, 0f, 0f), CameraRotationTime));
            cameraOfst.offset = new Vector3(0f, 0f, -800f);


        }
        else
        {
            Physics.gravity = new Vector3(0f, 0f, 981f);
            playerRB.constraints = RigidbodyConstraints.None;
            playerRB.constraints = RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
            cameraOfst.offset = new Vector3(0f, 800f, 0f);
            StartCoroutine(SmoothRotate(Camera, Quaternion.Euler(0f, 0f, 0f), Quaternion.Euler(90f, 0f, 0f), CameraRotationTime));
            StartCoroutine(SmoothRotate(chaserTransformation, Quaternion.Euler(0f, 0f, 0f), Quaternion.Euler(90f, 0f, 0f), CameraRotationTime));
        }
        //Debug.Log(Physics.gravity);
    }

    private IEnumerator SmoothRotate(Transform target, Quaternion from, Quaternion to, float duration)
    {
        float elapsed = 0f;
        while (elapsed < CameraRotationTime)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / CameraRotationTime);
            target.rotation = Quaternion.Slerp(from, to, t);
            yield return null;
        }
        target.rotation = to;
    }

    void calculateScore()
    {
        currentDistance = (int)playerTr.position.x + 1000;
        if (MaxDistance < currentDistance)
        {
            MaxDistance = currentDistance;
            baseScore = MaxDistance / 10;
        }
        if (Input.GetKeyDown(KeyCode.C) && Time.timeScale==1)
        {
            bonusScore += 10;
        }
        score = baseScore + bonusScore;
    }

    public void EndGame()
    {
        if (isGameEnded)
        {
            return;
        }

        isGameEnded = true;
        survivalTimeT.text = survivalTime.ToString("0") + "s";
        timesGravityChange.text = TimesGracityChange.ToString();
        scorePerSecond.text = (score / survivalTime).ToString("F1");
        gameEndPanel.SetActive(true);
        Time.timeScale = 0;
        Physics.gravity = new Vector3(0f, -981f, 0f);
        float result = (float)score / survivalTime;
        float SPS = (float)Math.Round(result, 1);
        float ST = (float)Math.Round(survivalTime, 1);
        FindAnyObjectByType<DataBaseConnection>().InsertGameDataAsync(ST,score,SPS,TimesGracityChange);
    }



    void Start()
    {
        Time.timeScale = 0;
    }


    void LateUpdate()
    {
        if (Input.GetMouseButtonDown(0) && !isGameEnded)
        {
            instructionPanel.SetActive(false);
            Time.timeScale = 1;
        }
        if (Input.GetKeyDown(KeyCode.C) && Time.timeScale==1)
            {
                Vector3 gravityDir = Physics.gravity.normalized;
                Vector3 velocity = playerRB.velocity;

                // 将速度向原重力方向投影并减去
                Vector3 gravityVelocity = Vector3.Project(velocity, gravityDir);
                playerRB.velocity = velocity - gravityVelocity;
                gravityChange();
                TimesGracityChange++;
            }
        calculateScore();
        survivalTime += Time.deltaTime;
        scoreText.text = score.ToString();
    }
}
