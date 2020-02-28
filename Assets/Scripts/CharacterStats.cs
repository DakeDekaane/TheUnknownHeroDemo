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
    public int baseAvoid;

    public int currentHealth;
    public int currentAtk;
    public int currentDef;
    public int currentAvoid;

    public Image HPBar;
    public Color highHealth;
    public Color lowHealth;
    public float minHealthValue;

    private int damage;
    
    // Start is called before the first frame update
    void Start()
    {
        HPBar.fillAmount = 1;
        HPBar.color = highHealth;
        currentHealth = maxHealth;
        currentAtk = baseAtk;
        currentDef = baseDef;
        currentAvoid = baseAvoid;
    }

    // Update is called once per frame
    void Update()
    {
        HPBar.transform.forward = Camera.main.transform.forward;
    }

    void UpdateHPBar() {
        HPBar.fillAmount = (float)currentHealth/maxHealth;
        if (HPBar.fillAmount < minHealthValue) {
            HPBar.color = lowHealth;
        }
        else {
            HPBar.color = highHealth;
        }
    }

    public void Attack(CharacterStats enemy) {
        if(Random.Range(0,100) < 100 - enemy.currentAvoid) {
            damage = currentAtk - enemy.currentDef;
            if (damage < 0) {
                damage = 0;
            }
            enemy.GetDamage(damage);
            enemy.GetComponent<Animator>().SetTrigger("GetDamage");
        }
        else{
            Debug.Log("Miss");
            enemy.GetComponent<Animator>().SetTrigger("Dodge");
        }
    }

    void GetDamage(int damage) {
        currentHealth -= damage;
        if(currentHealth < 0 ){
            currentHealth = 0;
        }
        HPBar.fillAmount = currentHealth/maxHealth;
        if (currentHealth/maxHealth < minHealthValue) {
            HPBar.color = lowHealth;
        }
        UpdateHPBar();
    }

    public void Heal(int healAmount) {
        currentHealth += healAmount;
        if(currentHealth > maxHealth) {
            currentHealth = maxHealth;
        }
        HPBar.fillAmount = currentHealth/maxHealth;
        if (currentHealth/maxHealth < minHealthValue) {
            HPBar.color = highHealth;
        }
        UpdateHPBar();
    }
}
