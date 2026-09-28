using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class experienceManager : MonoBehaviour
{

    public int levelDifference(characterStats charStats, characterStats enemyStats)
    {
        int playerLevel = charStats.classItemObj.promoted ? 20 : 0;
        playerLevel += charStats.level;

        int enemyLevel = enemyStats.classItemObj.promoted ? 20 : 0;
        return enemyLevel - playerLevel;
    }


    public int dealtDamageExperience(characterStats charStats, characterStats enemyStats, int damageDealt)
    {
        

        int exp = (int)((Mathf.Min(damageDealt, 10.0f)+10+ levelDifference(charStats, enemyStats) + (enemyStats.classItemObj.enemyExperienceBonus  + charStats.classItemObj.experienceBonus))/2);
        exp = Mathf.Clamp(exp, 1, 100); // exp cant be negative or more than 100
        Debug.Log("+" + exp + " damage experience");
        return exp;


    }
    public int killExperience(characterStats charStats, characterStats enemyStats)
    {
        int exp = 25+(int)(levelDifference(charStats, enemyStats) + enemyStats.classItemObj.enemyExperienceBonus + charStats.classItemObj.experienceBonus+ enemyStats.bossBonusExp);
        Debug.Log("+" + exp + " kill experience");
        return exp; ;
    }


}
