using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Canva : MonoBehaviour
{
    public GameManager GM;
    public void restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        GM.isGameEnded = false;
    }
}
