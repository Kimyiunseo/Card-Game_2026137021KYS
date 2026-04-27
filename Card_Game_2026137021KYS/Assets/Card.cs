using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Card : MonoBehaviour
{
    [Header("UI")]
    public Image backImage;
    public TMP_Text frontText;
    public Button button;

    [Header("Card Data")]
    public int cardValue;

    private CardGameManager gameManager;
    private bool isFlipped = false;
    private bool isMatched = false;

    public void Setup(int value, CardGameManager manager)
    {
        cardValue = value;
        gameManager = manager;

        frontText.text = value.ToString();

        isFlipped = false;
        isMatched = false;

        ShowBack();

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(OnClickCard);
    }

    private void OnClickCard()
    {
        if (isFlipped) return;
        if (isMatched) return;

        gameManager.SelectCard(this);
    }

    public void FlipOpen()
    {
        isFlipped = true;

        backImage.gameObject.SetActive(false);
        frontText.gameObject.SetActive(true);
    }

    public void FlipClose()
    {
        if (isMatched) return;

        isFlipped = false;

        ShowBack();
    }

    private void ShowBack()
    {
        backImage.gameObject.SetActive(true);
        frontText.gameObject.SetActive(false);
    }

    public void MatchAndRemove()
    {
        isMatched = true;
        gameObject.SetActive(false);
    }
}