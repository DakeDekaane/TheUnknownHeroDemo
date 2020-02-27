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
    public TextMeshProUGUI rangeLabel;

    // Update is called once per frame
    void Update()
    {
        if (selectedCharacter != null) {
            nameLabel.text = "Name: " + selectedCharacter.characterName;
            classLabel.text = "Class: " + selectedCharacter.characterClass;
            HPLabel.text = "HP: " + selectedCharacter.currentHealth + "/" + selectedCharacter.maxHealth;
            atkLabel.text = "ATK: " + selectedCharacter.baseAtk;
            defLabel.text = "DEF: " + selectedCharacter.baseDef;
            if(selectedCharacter.currentDef != selectedCharacter.baseDef) {
                defLabel.text += "+" + (selectedCharacter.currentDef - selectedCharacter.baseDef);
            }
            rangeLabel.text = "Range: " + selectedCharacter.GetComponent<CharacterMovement>().attackRange;
        }
    }
}
