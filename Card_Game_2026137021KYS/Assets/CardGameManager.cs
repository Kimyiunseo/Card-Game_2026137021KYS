using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardGameManager : MonoBehaviour
{
    [Header("Game Setting")]
    [SerializeField] private int pairCount = 10;

    [Header("Card Prefab")]
    [SerializeField] private Card cardPrefab;

    [Header("Card Parent")]
    [SerializeField] private Transform cardParent;

    [Header("Card Sprites")]
    [SerializeField] private Sprite cardBackSprite;
    [SerializeField] private Sprite[] cardFrontSprites;

    [Header("Delay")]
    [SerializeField] private float wrongMatchDelay = 0.7f;

    private Card firstSelectedCard;
    private Card secondSelectedCard;

    private bool isChecking;
    private int matchedPairCount;

    private void Start()
    {
        CreateCards();
    }

    private void CreateCards()
    {
        ClearOldCards();

        if (pairCount <= 0)
        {
            Debug.LogError("Pair Count는 1 이상이어야 합니다.");
            return;
        }

        if (cardPrefab == null)
        {
            Debug.LogError("Card Prefab이 연결되지 않았습니다.");
            return;
        }

        if (cardParent == null)
        {
            Debug.LogError("Card Parent가 연결되지 않았습니다.");
            return;
        }

        if (cardBackSprite == null)
        {
            Debug.LogError("Card Back Sprite가 연결되지 않았습니다.");
            return;
        }

        if (cardFrontSprites == null || cardFrontSprites.Length < pairCount)
        {
            Debug.LogError("앞면 카드 이미지 개수가 Pair Count보다 적습니다.");
            return;
        }

        List<int> cardIdList = new List<int>();

        for (int i = 0; i < pairCount; i++)
        {
            cardIdList.Add(i);
            cardIdList.Add(i);
        }

        Shuffle(cardIdList);

        for (int i = 0; i < cardIdList.Count; i++)
        {
            int id = cardIdList[i];

            Card newCard = Instantiate(cardPrefab, cardParent);
            newCard.Initialize(id, cardFrontSprites[id], cardBackSprite, this);
        }

        matchedPairCount = 0;
        firstSelectedCard = null;
        secondSelectedCard = null;
        isChecking = false;
    }

    private void ClearOldCards()
    {
        if (cardParent == null)
        {
            return;
        }

        for (int i = cardParent.childCount - 1; i >= 0; i--)
        {
            Destroy(cardParent.GetChild(i).gameObject);
        }
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

    public void SelectCard(Card selectedCard)
    {
        if (isChecking)
        {
            return;
        }

        if (selectedCard == null)
        {
            return;
        }

        if (selectedCard.IsMatched)
        {
            return;
        }

        if (selectedCard.IsFlipped)
        {
            return;
        }

        selectedCard.FlipToFront();

        if (firstSelectedCard == null)
        {
            firstSelectedCard = selectedCard;
            return;
        }

        secondSelectedCard = selectedCard;

        StartCoroutine(CheckMatch());
    }

    private IEnumerator CheckMatch()
    {
        isChecking = true;

        if (firstSelectedCard.CardId == secondSelectedCard.CardId)
        {
            firstSelectedCard.SetMatched();
            secondSelectedCard.SetMatched();

            matchedPairCount++;

            if (matchedPairCount >= pairCount)
            {
                Debug.Log("게임 클리어!");
            }
        }
        else
        {
            yield return new WaitForSeconds(wrongMatchDelay);

            firstSelectedCard.FlipToBack();
            secondSelectedCard.FlipToBack();
        }

        firstSelectedCard = null;
        secondSelectedCard = null;

        isChecking = false;
    }

    public void RestartGame()
    {
        CreateCards();
    }
}