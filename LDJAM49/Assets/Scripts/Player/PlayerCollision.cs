using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    PlayerController playerController => PlayerController.instance;
    GameManager gameManager => GameManager.instance;
    [SerializeField] private string deathTag = "DeathZone";
    [SerializeField] private string checkPointTag = "CheckPoint";

    void OnCollisionEnter2D(Collision2D col){
        if (col.gameObject.tag == deathTag){
            gameManager.RespawnPlayer();
        } 
        // else if (col.gameObject.tag == checkPointTag){
        //     playerController.respawnPoint = col.transform;
        // }
    }

    void OnTriggerEnter2D(Collider2D col){
        if (col.CompareTag(checkPointTag)){
            playerController.respawnPoint = col.transform;
        }
    }
}
