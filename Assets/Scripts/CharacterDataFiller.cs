using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class CharacterDataFiller : MonoBehaviour
{
    public CharacterStats selectedCharacter;
    public TextMeshProUGUI atkLabel;
    public TextMeshProUGUI defLabel;
    public TextMeshProUGUI HPLabel;
    public TextMeshProUGUI nameLabel;
    public TextMeshProUGUI classLabel;

    // Update is called once per frame
    void Update()
    {
        if (selectedCharacter != null) {
            nameLabel.text = "Name: " + selectedCharacter.characterName;
            classLabel.text = "Class: " + selectedCharacter.characterClass;
            HPLabel.text = "HP: " + selectedCharacter.currentHealth;
            atkLabel.text = "ATK: " + selectedCharacter.currentAtk;
            defLabel.text = "DEF: " + selectedCharacter.currentDef;

        }
    }
}
