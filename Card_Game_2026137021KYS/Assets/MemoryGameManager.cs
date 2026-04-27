using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MemoryGameManager : MonoBehaviour
{
    [Header("Board")]
    [SerializeField] private MemoryCard cardPrefab;
    [SerializeField] private Transform boardParent;
    [SerializeField] private Sprite cardBackSprite;

    [Header("Face Images")]
    [SerializeField] private List<Sprite> pairSprites = new List<Sprite>();

    [Header("Board Size")]
    [SerializeField] private int columns = 4;

    [Header("UI")]
    [SerializeField] private GameObject clearPanel;
    [SerializeField] private Button restartButton;

    [Header("Timing")]
    [SerializeField] private float mismatchDelay = 0.6f;

    private MemoryCard firstCard;
    private MemoryCard secondCard;
    private bool inputLocked;
    private int matchedPairs;

    private void Start()
    {
        if (restartButton != null)
            restartButton.onClick.AddListener(CreateBoard);

        if (clearPanel != null)
            clearPanel.SetActive(false);

        CreateBoard();
    }

    public void CreateBoard()
    {
        StopAllCoroutines();

        foreach (Transform child in boardParent)
            Destroy(child.gameObject);

        firstCard = null;
        secondCard = null;
        inputLocked = false;
        matchedPairs = 0;

        int pairCount = pairSprites.Count;
        if (pairCount <= 0)
        {
            Debug.LogError("pairSprites에 카드 앞면 이미지가 하나도 없습니다.");
            return;
        }

        List<int> ids = new List<int>();
        for (int i = 0; i < pairCount; i++)
        {
            ids.Add(i);
            ids.Add(i);
        }

        Shuffle(ids);

        for (int i = 0; i < ids.Count; i++)
        {
            int pairId = ids[i];
            Sprite faceSprite = pairSprites[pairId];

            MemoryCard card = Instantiate(cardPrefab, boardParent);
            card.Setup(this, pairId, faceSprite, cardBackSprite);
        }

        if (clearPanel != null)
            clearPanel.SetActive(false);
    }

    public void OnCardClicked(MemoryCard card)
    {
        if (inputLocked) return;
        if (card == firstCard) return;
        if (card.IsMatched) return;

        card.Reveal();

        if (firstCard == null)
        {
            firstCard = card;
            return;
        }

        secondCard = card;
        StartCoroutine(CheckMatch());
    }

    private IEnumerator CheckMatch()
    {
        inputLocked = true;

        yield return new WaitForSeconds(mismatchDelay);

        if (firstCard.PairId == secondCard.PairId)
        {
            firstCard.MatchAndDisappear();
            secondCard.MatchAndDisappear();

            matchedPairs++;

            if (matchedPairs >= pairSprites.Count)
            {
                if (clearPanel != null)
                    clearPanel.SetActive(true);
            }
        }
        else
        {
            firstCard.Hide();
            secondCard.Hide();
        }

        firstCard = null;
        secondCard = null;
        inputLocked = false;
    }

    private void Shuffle<T>(List<T> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int randomIndex = Random.Range(i, list.Count);
            (list[i], list[randomIndex]) = (list[randomIndex], list[i]);
        }
    }
}