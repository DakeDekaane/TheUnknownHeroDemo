using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnManager : MonoBehaviour
{
    static Dictionary<string,List<CharacterMovement>> units =  new Dictionary<string,List<CharacterMovement>>();
    static Queue<string> turnKey = new Queue<string>();
    static Queue<CharacterMovement> turnTeam = new Queue<CharacterMovement>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(turnTeam.Count == 0) {
            InitTeamTurnQueue();
        }
    }

    static void InitTeamTurnQueue() {
        List<CharacterMovement> teamlist = units[turnKey.Peek()];
        foreach (CharacterMovement unit in teamlist) {
            turnTeam.Enqueue(unit);
        }
        StartTurn();
    }

    public static void StartTurn() {
        if(turnTeam.Count > 0) {
            turnTeam.Peek().BeginTurn();
        }
    }

    public static void EndTurn() {
        CharacterMovement unit = turnTeam.Dequeue();
        unit.EndTurn();

        if(turnTeam.Count > 0) {
            StartTurn();
        }
        else {
            string team = turnKey.Dequeue();
            turnKey.Enqueue(team);
            InitTeamTurnQueue();
        }
    }

    public static void AddUnit(CharacterMovement unit) {
        List<CharacterMovement> list;
        if(!units.ContainsKey(unit.tag)) {
            list = new List<CharacterMovement>();
            units[unit.tag] = list;
            if(!turnKey.Contains(unit.tag)){
                turnKey.Enqueue(unit.tag);
            }
        }
        else {
            list = units[unit.tag];
        }
        list.Add(unit);
    }
}
