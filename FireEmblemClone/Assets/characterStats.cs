using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class characterStats : MonoBehaviour
{
    public bool isPlayer;
    public int level, exp , currentHp;
    public string characterName, characterClass;
    public characterClass classItemObj;
    public int hp, attack, speed, skill, luck, defense, resistance,
    constitution, movement;

    public int bossBonusExp;

    public string[] usableWeapons;

    public int hpGrowth, attackGrowth, speedGrowth, skillGrowth, luckGrowth, defGrowth, resGrowth,
        conGrowth, moveGrowth;

    //[HideInInspector]
    public int attackPower;
   // [HideInInspector]
    public int attackSpeed;
    //[HideInInspector]
    public int hitChance;
    //[HideInInspector]
    public int critChance;
    //[HideInInspector]
    public int avoidChance;
    //[HideInInspector]
    public int critAvoidChance;
    public int weaponMight, weaponWeight, weaponHit, weaponCrit;

    private void Start()
    {
        currentHp = hp;
    }

    void calcAttackPower()
    {
       attackPower= weaponMight + attack;
    }

    void calcAttackSpeed()
    {
        attackSpeed = Mathf.Max(speed - Mathf.Max(weaponWeight - constitution, 0), 0);
    }

    void calcAccuracy()
    {
        hitChance= weaponHit + skill;
    }
    void calcCritChance()
    {
        critChance = ((skill + skill + luck) / 3) + weaponCrit;
    }
    void calcAvoidChance()
    {
        avoidChance = (attackSpeed *3 + luck) / 2;
    }
    void calcCritAvoidChance()
    {
        critAvoidChance = luck;
    }

    public void calculateAll()
    {
        calcAttackPower();
        calcAttackSpeed();
        calcAvoidChance(); // must be after calculating attack speed
        calcAccuracy();
        calcCritChance();
        calcCritAvoidChance();
    }

    public void expCheck()
    {
        if (level == 20)
            exp = 0;
        if (exp >= 100)
        {
            exp -= 100;
            levelUp();
        }
    }


    public void levelUp()
    {
        level++;
        if (Random.Range(0, 100) < hpGrowth)
        {
            hp++;
        }
        if(Random.Range(0, 100) < attackGrowth)
        {
            attack++;
        }
        if (Random.Range(0, 100) < speedGrowth)
        {
            speed++;
        }
        if (Random.Range(0, 100) < skillGrowth)
        {
            skill++;
        }
        if (Random.Range(0, 100) < luckGrowth)
        {
            luck++;
        }
        if (Random.Range(0, 100) < defGrowth)
        {
            defense++;
        }
        if (Random.Range(0, 100) < resGrowth)
        {
            resistance++;
        }
        if (Random.Range(0, 100) < conGrowth)
        {
            constitution++;
        }
        if (Random.Range(0, 100) < moveGrowth)
        {
            movement++;
        }
    }

}
