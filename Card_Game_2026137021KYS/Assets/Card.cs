using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [Header("UI Images")]
    [SerializeField] private Image frontImage;
    [SerializeField] private Image backImage;

    [Header("Button")]
    [SerializeField] private Button button;

    private int cardId;
    private bool isFlipped;
    private bool isMatched;

    private CardGameManager gameManager;

    public int CardId => cardId;
    public bool IsFlipped => isFlipped;
    public bool IsMatched => isMatched;

    private void Awake()
    {
        if (button == null)
        {
            button = GetComponent<Button>();
        }

        button.onClick.AddListener(OnCardClicked);
    }

    public void Initialize(int id, Sprite frontSprite, Sprite backSprite, CardGameManager manager)
    {
        cardId = id;
        gameManager = manager;

        frontImage.sprite = frontSprite;
        backImage.sprite = backSprite;

        isFlipped = false;
        isMatched = false;

        button.interactable = true;

        ShowBack();
    }

    private void OnCardClicked()
    {
        if (gameManager == null)
        {
            return;
        }

        gameManager.SelectCard(this);
    }

    public void FlipToFront()
    {
        if (isMatched)
        {
            return;
        }

        isFlipped = true;
        ShowFront();
    }

    public void FlipToBack()
    {
        if (isMatched)
        {
            return;
        }

        isFlipped = false;
        ShowBack();
    }

    public void SetMatched()
    {
        isMatched = true;
        isFlipped = true;

        ShowFront();

        button.interactable = false;
    }

    private void ShowFront()
    {
        frontImage.gameObject.SetActive(true);
        backImage.gameObject.SetActive(false);
    }

    private void ShowBack()
    {
        frontImage.gameObject.SetActive(false);
        backImage.gameObject.SetActive(true);
    }
}