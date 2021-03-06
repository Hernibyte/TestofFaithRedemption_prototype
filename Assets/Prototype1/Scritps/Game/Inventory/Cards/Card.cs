using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

[Serializable]
public class NewSCard
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

    public Sprite cardImage;
    public enum CardType
    {
        HP_Plane,
        HP_Porcent,
        ATK_Plane,
        ATK_Porcent,
        DEF_Plane,
        DEF_Porcent,
        SPD_Plane,
    }
    public List<CardType> typeStats;
    public enum CardUtility { Offensive, Misc, Defensive }
    public CardUtility myUtility;
}

[CreateAssetMenu(fileName = "Card", menuName = "ScriptableObjects/Card", order = 1)]
public class Card : ScriptableObject
{
    public NewSCard sCard;
}
