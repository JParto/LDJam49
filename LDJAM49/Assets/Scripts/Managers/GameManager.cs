using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    PlayerController playerController => PlayerController.instance;
    public LayerMask lightCameraCullingMask;
    public Animator fadeAnimator;
    private IEnumerator coroutine;

    void Awake(){
        instance = this;
    }

    public void RespawnPlayer(){
        coroutine = RespawnCoroutine();
        StartCoroutine(coroutine);
    }

    IEnumerator RespawnCoroutine(){
        fadeAnimator.SetTrigger("Fade");
        yield return new WaitForSeconds(1.5f);
        Camera.main.cullingMask = lightCameraCullingMask;
        playerController.Respawn();
        yield return new WaitForSeconds(0.5f);

        fadeAnimator.SetTrigger("Fade");

    }

    public void EndGame(){
        coroutine = CompleteGameCo();
        StartCoroutine(coroutine);
    }

    IEnumerator CompleteGameCo(){
        fadeAnimator.SetTrigger("SlowFade");
        yield return new WaitForSeconds(8f);
        SceneManager.LoadScene("EndScene");
    }
}
