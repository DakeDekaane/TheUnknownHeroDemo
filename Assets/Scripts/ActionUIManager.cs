using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionUIManager : MonoBehaviour
{
    // Start is called before the first frame update

    public PlayerMovement activePlayer = null;
    public GameObject actionPanel;

    void Update() {
        if(activePlayer != null || !(activePlayer.characterState == CharacterState.Move)) {
            actionPanel.SetActive(true);
        }
        else {
            actionPanel.SetActive(false);
        }
    }
    public void Move() {
        if(activePlayer.characterState == CharacterState.Idle) {
            activePlayer.characterState = CharacterState.StandbyPhase1;
            activePlayer.FindSelectableTiles();
        }
    }

    public void Attack() {
        if(activePlayer.characterState == CharacterState.Idle || activePlayer.characterState == CharacterState.StandbyPhase2) {
            activePlayer.characterState = CharacterState.StandbyPhase2;
            activePlayer.FindAttackableTiles();
        }
        
    }

    public void Wait() {
        if(activePlayer.characterState == CharacterState.Idle || activePlayer.characterState == CharacterState.StandbyPhase1 || activePlayer.characterState == CharacterState.StandbyPhase2) {
            activePlayer.characterState = CharacterState.End;
        }
    }
 }
