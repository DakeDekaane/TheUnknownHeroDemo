using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionUIManager : MonoBehaviour
{
    // Start is called before the first frame update

    public PlayerMovement activePlayer = null;
    public GameObject actionPanel;

    void Start(){
        actionPanel.transform.Find("ItemButton").GetComponent<Button>().interactable = false;
        actionPanel.transform.Find("ClockWorkManButton").GetComponent<Button>().interactable = false;
    }
    
    void Update() {
        if(activePlayer != null) {
            if (activePlayer.characterState == CharacterState.Idle || activePlayer.characterState == CharacterState.StandbyPhase1 || activePlayer.characterState == CharacterState.StandbyPhase2) {
                actionPanel.SetActive(true);
            }
            if(activePlayer.characterState == CharacterState.Idle) {
               actionPanel.transform.Find("MoveButton").GetComponent<Button>().interactable = true;
                
            }
            if(activePlayer.attackableTiles.Count > 0){
               actionPanel.transform.Find("AttackButton").GetComponent<Button>().interactable = true;

            }
            else {
               actionPanel.transform.Find("AttackButton").GetComponent<Button>().interactable = false;
            }
            if(activePlayer.characterState == CharacterState.Move) {
                actionPanel.SetActive(false);
            }
            if(activePlayer.characterState == CharacterState.StandbyPhase2) {
               actionPanel.transform.Find("MoveButton").GetComponent<Button>().interactable = false;
            }
        }
        else {
            actionPanel.SetActive(false);
        }
        
    }
    public void Move() {
        if(activePlayer.characterState == CharacterState.Idle) {
            activePlayer.selected = true;
            activePlayer.characterState = CharacterState.StandbyPhase1;
        }
    }

    public void Attack() {
        if(activePlayer.characterState == CharacterState.Idle || activePlayer.characterState == CharacterState.StandbyPhase2) {
            activePlayer.selected = true;
            activePlayer.characterState = CharacterState.StandbyPhase2;
            activePlayer.ShowAttackableTiles();
        }
        
    }

    public void Wait() {
        if(activePlayer.characterState == CharacterState.Idle || activePlayer.characterState == CharacterState.StandbyPhase1 || activePlayer.characterState == CharacterState.StandbyPhase2) {
            activePlayer.characterState = CharacterState.End;
            //TurnManager.instance.EndTurn(activePlayer);
        }
    }
 }
