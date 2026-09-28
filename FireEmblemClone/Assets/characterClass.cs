using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/characterClassObject", order = 1)]
public class characterClass : ScriptableObject
{
    public string className;
    public float experienceBonus;
    public float enemyExperienceBonus;
    public bool promoted;
    public string[] weaponTypes;

    public int hpBase, attackBase, skillBase, speedBase, luckBase, defenseBase, resistanceBase, constitutionBase, moveBase;

    public int hpPromoBonus, attackPromoBonus, skillPromoBonus, speedPromoBonus, luckPromoBOnus, defensePromoBOnus, resistancePromoBonus, constitutionPromoBonus, movePromoBonus;

    //set up terrain penalties here
}
