using System.Collections;
using System.Collections.Generic;
using FMOD.Studio;
using UnityEngine;
[DisallowMultipleComponent]
public class PlayerJumpInstant : MonoBehaviour
{
   
    [Min(0)][SerializeField] float minJumpHeight;
    [Min(0)][SerializeField] float maxJumpHeight;
    [Min(0)][SerializeField] float jumpChargeTime;
    [SerializeField] GravitySO gravitySo;
    [SerializeField] FMODUnity.EventReference JumpSound;

    EventInstance jumpInstance;
   
    float jumpCharge;
    bool reversedGravitySettings;
    
    PlayerInputs playerInputs;
    Rigidbody2D rigidbody2D;
    GroundChecker groundChecker;
    
    void Awake(){
        playerInputs = GetComponent<PlayerInputs>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        groundChecker = GetComponent<GroundChecker>();
        jumpInstance = FMODUnity.RuntimeManager.CreateInstance(JumpSound);

        reversedGravitySettings = gravitySo.gravityIsReversed;
        if (reversedGravitySettings){
            rigidbody2D.gravityScale = -rigidbody2D.gravityScale;
        }
    }

    void Update(){
        //Jump Charging
        if (playerInputs.JumpInput){
            //This value increases as long as player holds space
            jumpCharge += Time.deltaTime / jumpChargeTime;
        }

        if (playerInputs.JumpInput){
            if (groundChecker.IsGrounded){
                //Lerps between min and max jump height.
                var jumpForce = Mathf.Lerp(minJumpHeight, maxJumpHeight, jumpCharge);
                PlayJumpSound();
                if (reversedGravitySettings){
                    rigidbody2D.AddForce(Vector2.up * -jumpForce);
                }
                else if(!reversedGravitySettings){
                    rigidbody2D.AddForce(Vector2.up * jumpForce); 
                }
                
            }
            jumpCharge = 0f;
        }
    }

    void PlayJumpSound(){
        jumpInstance.getPlaybackState(out var playbackState);
        if (playbackState == PLAYBACK_STATE.STOPPED){
            jumpInstance.start();
        }
    }
}
