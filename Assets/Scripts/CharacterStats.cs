using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterStats : MonoBehaviour
{

    public string characterName;
    public string characterClass;

    public int maxHealth;
    public int baseAtk;
    public int baseDef;
    public int baseAccuracy;

    public int currentHealth;
    public int currentAtk;
    public int currentDef;
    public int currentAccuracy;

    public Image HPBar;
    public Color highHealth;
    public Color lowHealth;
    public float minHealthValue;

    public Transform cameraOrientation;
    private int damage;
    // Start is called before the first frame update
    void Start()
    {
        HPBar.fillAmount = 1;
        HPBar.color = highHealth;
        currentHealth = maxHealth;
        currentAtk = baseAtk;
        currentDef = baseDef;
        currentAccuracy = baseAccuracy;
    }

    // Update is called once per frame
    void Update()
    {
        HPBar.transform.forward = cameraOrientation.forward;
        HPBar.fillAmount = (float)currentHealth/maxHealth;
        if (HPBar.fillAmount < minHealthValue) {
            HPBar.color = lowHealth;
        }
        else {
            HPBar.color = highHealth;
        }
    }

    public void Attack(CharacterStats enemy) {
        if(Random.Range(0,100) < currentAccuracy) {
            damage = baseAtk - enemy.baseDef;
            if (damage < 0) {
                damage = 0;
            }
            enemy.GetDamage(damage);
        }
        else{
            Debug.Log("Miss");
        }
    }

    void GetDamage(int damage) {
        currentHealth -= damage;
        HPBar.fillAmount = currentHealth/maxHealth;
        if (currentHealth/maxHealth < minHealthValue) {
            HPBar.color = lowHealth;
        }
    }

    void Heal(int healAmount) {
        currentHealth += healAmount;
        HPBar.fillAmount = currentHealth/maxHealth;
        if (currentHealth/maxHealth < minHealthValue) {
            HPBar.color = highHealth;
        }
    }
}
