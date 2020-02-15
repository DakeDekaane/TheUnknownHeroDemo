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
            if (activePlayer.characterState == CharacterState.Begin || activePlayer.characterState == CharacterState.Idle || activePlayer.characterState == CharacterState.Idle2 || activePlayer.characterState == CharacterState.StandbyPhase1 || activePlayer.characterState == CharacterState.StandbyPhase2) {
                actionPanel.SetActive(true);
            }
            if(activePlayer.characterState == CharacterState.Idle) {
               actionPanel.transform.Find("MoveButton").GetComponent<Button>().interactable = true;
            }
            else {
               actionPanel.transform.Find("MoveButton").GetComponent<Button>().interactable = false;
            }
            if(activePlayer.attackableTiles.Count > 0){
               actionPanel.transform.Find("AttackButton").GetComponent<Button>().interactable = true;
            }
            else{
               actionPanel.transform.Find("AttackButton").GetComponent<Button>().interactable = false;
            }
            if(activePlayer.characterState == CharacterState.Move || activePlayer.characterState == CharacterState.Attack) {
                actionPanel.SetActive(false);
            }
            if(activePlayer.currentTile.itemTiles.Count > 0) {
               actionPanel.transform.Find("ItemButton").GetComponent<Button>().interactable = true;
            }
            else {
               actionPanel.transform.Find("ItemButton").GetComponent<Button>().interactable = false;
            }
        }
        else {
            actionPanel.SetActive(false);
        }
        
    }
    public void Move() {
        if(activePlayer.characterState == CharacterState.Idle) {
            activePlayer.selected = true;
            Debug.Log(activePlayer.name + ": Idle->PSB1");
            activePlayer.characterState = CharacterState.PreStandbyPhase1;
        }
    }

    public void Attack() {
        if(activePlayer.characterState == CharacterState.Idle || activePlayer.characterState == CharacterState.Idle2) {
            activePlayer.selected = true;
            Debug.Log(activePlayer.name + ": IdleX->PSB2");
            activePlayer.characterState = CharacterState.PreStandbyPhase2;
            activePlayer.ShowAttackableTiles();
        }
        
    }

    public void Item() {
        if(activePlayer.characterState == CharacterState.Idle || activePlayer.characterState == CharacterState.Idle2) {
            activePlayer.selected = true;
            Debug.Log(activePlayer.name + ": IdleX->End");
            activePlayer.characterState = CharacterState.End;
            activePlayer.UseItem();
        }
    }

    public void Wait() {
        if(activePlayer.characterState == CharacterState.Idle || activePlayer.characterState == CharacterState.Idle2) {
            Debug.Log(activePlayer.name + ": IdleX->End");
            activePlayer.characterState = CharacterState.End;
            //TurnManager.instance.EndTurn(activePlayer);
        }
    }
 }
