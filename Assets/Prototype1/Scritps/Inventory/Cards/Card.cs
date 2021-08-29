using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class SCard
{
    public string name;
    public float hp;
    public float defense;
    public int damage;
    public float knockback;

    public float attackColdown; // ambos
    public float attackSpeed; // player

    public float attackDelay; // enemy

    public float movementSpeed; // ambos
}

[CreateAssetMenu(fileName = "Card", menuName = "ScriptableObjects/Card", order = 1)]
public class Card : ScriptableObject
{ 
    public SCard sCard;
}
