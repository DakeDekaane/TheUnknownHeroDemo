using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    public enum Faction {
        Player,
        Ally,
        Enemy
    }

    public  GameObject[] playerChars; //
    public  GameObject[] alliesChars; //
    public  GameObject[] enemyChars; //

    public  List<CharacterMovement> playerList = new List<CharacterMovement>(); //
    public  List<CharacterMovement> alliesList = new List<CharacterMovement>(); //
    public  List<CharacterMovement> enemyList = new List<CharacterMovement>(); //

    public  List<CharacterMovement> turnList = new List<CharacterMovement>(); //
    public  Faction turnTeam = Faction.Player; //
    public  bool Allies;

    public PlayerMovement activePlayer = null; //
    public CharacterMovement activeCharacter = null;

    public static TurnManager instance;

    public GameObject playerTurnPanel;
    public GameObject enemyTurnPanel;
    public GameObject playerTurnBigPanel;
    public GameObject enemyTurnBigPanel;
    public GameObject VictoryPanel;
    public GameObject DefeatPanel;

    public int enemies;
    public int players;

    public bool holdOn = false;
    public bool pause = false;

    void Start() {
        instance = this;
        activePlayer = null;
    }
    // Update is called once per frame
    void Update()
    {
        if(turnList.Count == 0) {
            if(turnTeam == Faction.Player) {
                enemyTurnPanel.SetActive(false);
                playerTurnPanel.SetActive(true);
                StartCoroutine(ShowPanel(playerTurnBigPanel));
                InitTeamTurn(playerList);
            }
            else if(turnTeam == Faction.Ally) {
                InitTeamTurn(alliesList);
            }
            else if(turnTeam == Faction.Enemy) {
                enemyTurnPanel.SetActive(true);
                playerTurnPanel.SetActive(false);
                StartCoroutine(ShowPanel(enemyTurnBigPanel));
                InitTeamTurn(enemyList);
            }
        }
        if(turnTeam == Faction.Player && (activePlayer == null || !activePlayer.selected)) {
            if(Input.GetMouseButtonDown(0)) {
                PickPlayer();
            }
        }
    }

    /*public void CollidersEnabled(bool enabled) {
        foreach(GameObject unit in playerChars) {
            if(unit != null)
                unit.GetComponent<Collider>().enabled = enabled;
        }
        foreach(GameObject unit in enemyChars) {
            if(unit != null)
                unit.GetComponent<Collider>().enabled = enabled;
        }
    }*/

    void ClearSelectableCharacter(){
        foreach (CharacterMovement obj in playerList) {
            if (obj.status != SelectStatus.Acted) {
                obj.SetStatus(SelectStatus.Unselected);
                obj.GetCurrentTile().current = false;
            }
        }
    }

    void ClearActedCharacter(){
        foreach (GameObject obj in playerChars) {
            obj.GetComponent<PlayerMovement>().SetStatus(SelectStatus.Unselected);
        }
        foreach (GameObject obj in playerChars) {
            obj.GetComponent<PlayerMovement>().SetStatus(SelectStatus.Unselected);
        }
    }
    void PickPlayer() {
        Ray viewRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit viewHit;
        if (Physics.Raycast(viewRay, out viewHit)) {
            if (viewHit.transform.tag == "Player" && viewHit.transform.GetComponent<PlayerMovement>().status != SelectStatus.Acted) {
                ClearSelectableCharacter();
                activePlayer = viewHit.transform.GetComponent<PlayerMovement>();
                activePlayer.actionUIManager.activePlayer = activePlayer;
                Debug.Log("Player Selected");
                activePlayer.GetCurrentTile();
                activePlayer.ShowCurrentTile();
                activePlayer.SetStatus(SelectStatus.Selected);
                //characterState = CharacterState.StandbyPhase1;
                //FindSelectableTiles();
            }
        }  
    }

     void FillList(List<CharacterMovement> list,GameObject[] array) {
        list.Clear();
        foreach(GameObject unit in array) {
            list.Add(unit.GetComponent<CharacterMovement>());
        }
    }

     void InitTeamTurn(List<CharacterMovement> teamlist) {
        UpdateLists();
        foreach (CharacterMovement unit in teamlist) {
            if(turnTeam == Faction.Player) {
                unit.turn = true;
                unit.selected = false;
            }
            turnList.Add(unit);
        }
        StartTurn();
    }

    public  void StartTurn() {
        ClearActedCharacter();
        if(turnTeam == Faction.Ally || turnTeam == Faction.Enemy) {
            if(turnList.Count > 0) {
                turnList[0].BeginTurn();
                activeCharacter = turnList[0];
            }
        }
    }

    public  void EndTurn(CharacterMovement player) {
        player.SetStatus(SelectStatus.Acted);
        player.EndTurn();
        turnList.Remove(player);
        if(turnList.Count == 0) {
            turnTeam = GetNextTeam();
        }
    }

    public  void EndTurn() {
        CharacterMovement unit = turnList[0];
        unit.SetStatus(SelectStatus.Acted);
        unit.EndTurn();
        activeCharacter = null;
        turnList.Remove(unit);

        if(turnList.Count > 0) {
            turnList[0].BeginTurn();
            activeCharacter = turnList[0];
        }
        if(turnList.Count == 0) {
            turnTeam = GetNextTeam();
        }

    }

    public bool CheckForEnd() {
        if(players == 0) {
            DefeatPanel.SetActive(true);
            return true;
        }
        else if (enemies == 0) {
            VictoryPanel.SetActive(true);
            return true;
        }
        return false;
    }

     Faction GetNextTeam() {
        if(turnList.Count == 0) {
            if(turnTeam == Faction.Player) {
                if(Allies) {
                    return Faction.Ally;
                }
                else {
                    Debug.Log("Enemy Turn");
                    return Faction.Enemy;
                }
            }
            else if (turnTeam == Faction.Ally) {
                return Faction.Enemy;
            }
            else if(turnTeam == Faction.Enemy) {
                Debug.Log("Player Turn");
                return Faction.Player;
            }
        }
        return turnTeam;  
    }

    void UpdateArrays() {
        playerChars = GameObject.FindGameObjectsWithTag("Player");
        if(Allies) {
            alliesChars = GameObject.FindGameObjectsWithTag("Ally");
        }
        enemyChars = GameObject.FindGameObjectsWithTag("Enemy");
        players = playerChars.Length;
        enemies = enemyChars.Length;
    }

     void UpdateLists(){
        UpdateArrays();
        FillList(playerList,playerChars);
        if(Allies) {
            FillList(alliesList,alliesChars);
        }
        FillList(enemyList,enemyChars);
    }

    public IEnumerator ShowPanel(GameObject panel) {
        Debug.Log("Coroutine");
        panel.SetActive(true);
        holdOn = true;
        yield return new WaitForSeconds(3);
        holdOn = false;
        panel.SetActive(false);

    }
}
