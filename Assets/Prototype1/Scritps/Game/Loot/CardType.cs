using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardType : MonoBehaviour
{
    public enum CardUtility { Offensive, Misc, Defensive}
    public CardUtility cardUtility;
    public List<Sprite> cardsType;
    public Image mainImage;

    void Update()
    {
        switch (cardUtility)
        {
            case CardUtility.Offensive:
                mainImage.sprite = cardsType[0];
                break;
            case CardUtility.Misc:
                mainImage.sprite = cardsType[1];
                break;
            case CardUtility.Defensive:
                mainImage.sprite = cardsType[2];
                break;
        }
    }
    public void SetCardUtility(CardUtility utility)
    {
        cardUtility = utility;
    }
}
