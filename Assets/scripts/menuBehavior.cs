using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class menuBehavior : MonoBehaviour
{
    public Animator animator;
    public void StartGame()
    {
        animator.SetTrigger("startGame");
        StartCoroutine(WaitAndLoadScene());
    }
       
    private IEnumerator WaitAndLoadScene()
    {
        yield return new WaitForSeconds(2f); 
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}

