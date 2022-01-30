using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class Trap : MonoBehaviour{
    [SerializeField] float trapResetTime = 5;
    [SerializeField] CinemachineImpulseSource impulseSource;
    [SerializeField] PlayerAnimationSwitcher playerAnimationSwitcher;
    [SerializeField] CharacterHolderSO characterHolderSo;
    GravitySwap gravitySwap;
    WorldSwitcher worldSwitcher;

    bool canTrigger = true;
    

    void Start(){
        gravitySwap = FindObjectOfType<GravitySwap>();
        worldSwitcher = FindObjectOfType<WorldSwitcher>();
        playerAnimationSwitcher = FindObjectOfType<PlayerAnimationSwitcher>();
        impulseSource = GetComponent<CinemachineImpulseSource>();
        characterHolderSo = FindObjectOfType<CharacterHolderSO>();
    }


    void OnCollisionEnter2D(Collision2D collision){
        if (collision.transform.CompareTag("Player") && canTrigger){
            characterHolderSo.ChangeLifeState();
            gravitySwap.SwitchGravity();
            worldSwitcher.SwitchWorld();
            impulseSource.GenerateImpulse();
            playerAnimationSwitcher.ChangeRunningAnimationController();
            StartCoroutine(ResetTrap());
        }
    }

    void OnTriggerEnter2D(Collider2D col){
        if (col.transform.CompareTag("Player") && canTrigger){
            characterHolderSo.ChangeLifeState();
            gravitySwap.SwitchGravity();
            worldSwitcher.SwitchWorld();
            impulseSource.GenerateImpulse();
            playerAnimationSwitcher.ChangeRunningAnimationController();
            StartCoroutine(ResetTrap());
        }
    }

    IEnumerator ResetTrap(){
        canTrigger = false;
        yield return new WaitForSeconds(trapResetTime);
        canTrigger = true;
    }
}
