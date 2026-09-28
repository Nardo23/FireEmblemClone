using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class combatManager : MonoBehaviour
{
    public characterStats initiator, defender;
    bool initiatorDoubles, defenderDoubles;
    public TMP_Text playerText, opponentText;
    public GameObject battleForcast;
    public GameObject px2, ox2;
    experienceManager expManager;
    private void Start()
    {
        expManager = GetComponent<experienceManager>();
    }


    int initiatorHit, defenderHit, initiatorDmg, defenderDmg, initiatorCrit, defenderCrit;
    public void prepFight()
    {

        battleForcast.SetActive(true);
        initiatorDoubles = false; defenderDoubles = false;
        ox2.SetActive(false); px2.SetActive(false);
        initiator.calculateAll();
        defender.calculateAll();
        if (initiator.attackSpeed - defender.attackSpeed >= 3)
            initiatorDoubles = true;
        else if (defender.attackSpeed - initiator.attackSpeed >= 3)
            defenderDoubles = true;

        initiatorHit = Mathf.Max(initiator.hitChance - defender.avoidChance,0);
        defenderHit = Mathf.Max(defender.hitChance - initiator.avoidChance,0);
        initiatorDmg = Mathf.Max(initiator.attackPower - defender.defense,0);
        defenderDmg = Mathf.Max(defender.attackPower - initiator.defense,0);
        initiatorCrit = Mathf.Max(initiator.critChance - defender.critAvoidChance,0);
        defenderCrit = Mathf.Max(defender.critChance - initiator.critAvoidChance,0);

        playerText.text = "" + initiator.currentHp + "<br>" + initiatorHit + "<br>" + initiatorDmg + "<br>" + initiatorCrit;
        opponentText.text = "" + defender.currentHp + "<br>" + defenderHit + "<br>" + defenderDmg + "<br>" + defenderCrit;

        if (initiatorDoubles)
            px2.SetActive(true);
        else if (defenderDoubles)
            ox2.SetActive(true);

    }
    bool hitTemp = false;
    bool critTemp = false;
    int damageTemp = 0;
    
    private void resetTempValues()
    {
        hitTemp = false;
        critTemp = false;
        damageTemp = 0;
        
    }
    void strike(int hitRate, int damageDealt, int critRate, characterStats target)
    {
        resetTempValues();
        int rn = Random.Range(0, 100);// 0 is included 100 is excluded so you can get 0-99
        
        if (rn <= hitRate)
        {
            hitTemp = true;
            damageTemp = damageDealt;
        }
        if (rn <= critRate && hitTemp)
        {
            damageTemp = Mathf.Max(damageDealt * 3, 3); // crits always deal at least 3 damage
            critTemp = true;
        }
        target.currentHp -= damageTemp;
       
        

    }
    int playertotalDamage;
    public void playbattle()
    {
        playertotalDamage = 0;
        //first hit
        strike(initiatorHit, initiatorDmg, initiatorCrit, defender);
        if (initiator.isPlayer)
            playertotalDamage += damageTemp;
        battleText(initiator, defender);
        if (bothLive())
        {
            //counter attack
            strike(defenderHit, defenderDmg, defenderCrit, initiator);
            if (defender.isPlayer)
                playertotalDamage += damageTemp;
            battleText(defender, initiator);

        }
        else
        {
            awardBattleExp();
            return;
        }
            
        if (bothLive())
        {
            //check for followup attack
            if (initiatorDoubles)
            {
                strike(initiatorHit, initiatorDmg, initiatorCrit, defender);
                if (initiator.isPlayer)
                    playertotalDamage += damageTemp;
                battleText(initiator, defender);
            }
            else if (defenderDoubles)
            {
                strike(defenderHit, defenderDmg, defenderCrit, initiator);
                if (defender.isPlayer)
                    playertotalDamage += damageTemp;
                battleText(defender, initiator);
            }
        }

        awardBattleExp();

    }
    void awardBattleExp()
    {
        if (initiator.isPlayer)
        {
            if (initiator.currentHp > 0)
            {
                initiator.exp += expManager.dealtDamageExperience(initiator, defender, playertotalDamage);
                if (defender.currentHp <= 0)
                    initiator.exp += expManager.killExperience(initiator, defender);
                initiator.expCheck();
            }

        }
        else if (defender.isPlayer)
        {
            if (defender.currentHp > 0)
            {
                defender.exp += expManager.dealtDamageExperience(defender, initiator, playertotalDamage);
                if (initiator.currentHp <= 0)
                    defender.exp += expManager.killExperience(defender, initiator);
                defender.expCheck();
            }
        }
    }
    bool bothLive()
    {
        if (initiator.currentHp > 0 && defender.currentHp > 0)
            return true;
        else
            return false;
    }

    void battleText(characterStats attackerChar, characterStats defenderChar)
    {
        if (hitTemp)
        {
            if (critTemp)
            {
                Debug.Log("Critical hit!");
            }
                

            Debug.Log(attackerChar.characterName + " dealt " + damageTemp + " to " + defenderChar.characterName+".");
            if (!bothLive())
            {
                Debug.Log(defenderChar.characterName + " fell.");
            }
               

        }
        else
        {
            Debug.Log(attackerChar.characterName + " missed.");
        }
    }
}
