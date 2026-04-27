using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardGameManager : MonoBehaviour
{
    [Header("Game Setting")]
    public int pairCount = 10;

    [Header("Prefab / Parent")]
    public Card cardPrefab;
    public Transform cardParent;

    [Header("UI")]
    public TMP_Text clearText;

    private Card firstCard;
    private Card secondCard;

    private bool isChecking = false;
    private int matchedPairCount = 0;

    private void Start()
    {
        StartGame();
    }

    public void StartGame()
    {
        clearText.gameObject.SetActive(false);

        matchedPairCount = 0;
        firstCard = null;
        secondCard = null;
        isChecking = false;

        DeleteOldCards();

        List<int> cardValues = CreateCardValues();
        Shuffle(cardValues);

        CreateCards(cardValues);
    }

    private void DeleteOldCards()
    {
        for (int i = cardParent.childCount - 1; i >= 0; i--)
        {
            Destroy(cardParent.GetChild(i).gameObject);
        }
    }

    private List<int> CreateCardValues()
    {
        List<int> values = new List<int>();

        for (int i = 1; i <= pairCount; i++)
        {
            values.Add(i);
            values.Add(i);
        }

        return values;
    }

    private void Shuffle(List<int> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);

            int temp = list[i];
            list[i] = list[randomIndex];
            list[randomIndex] = temp;
        }
    }

    private void CreateCards(List<int> values)
    {
        for (int i = 0; i < values.Count; i++)
        {
            Card newCard = Instantiate(cardPrefab, cardParent);
            newCard.Setup(values[i], this);
        }
    }

    public void SelectCard(Card selectedCard)
    {
        if (isChecking) return;

        selectedCard.FlipOpen();

        if (firstCard == null)
        {
            firstCard = selectedCard;
            return;
        }

        secondCard = selectedCard;
        StartCoroutine(CheckCards());
    }

    private IEnumerator CheckCards()
    {
        isChecking = true;

        yield return new WaitForSeconds(0.7f);

        if (firstCard.cardValue == secondCard.cardValue)
        {
            firstCard.MatchAndRemove();
            secondCard.MatchAndRemove();

            matchedPairCount++;

            if (matchedPairCount >= pairCount)
            {
                GameClear();
            }
        }
        else
        {
            firstCard.FlipClose();
            secondCard.FlipClose();
        }

        firstCard = null;
        secondCard = null;

        isChecking = false;
    }

    private void GameClear()
    {
        clearText.gameObject.SetActive(true);
        clearText.text = "CLEAR!";
    }
}