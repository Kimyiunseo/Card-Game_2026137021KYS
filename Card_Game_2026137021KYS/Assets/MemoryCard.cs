using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MemoryCard : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Image cardImage;

    private MemoryGameManager manager;
    private Sprite faceSprite;
    private Sprite backSprite;

    public int PairId { get; private set; }
    public bool IsOpen { get; private set; }
    public bool IsMatched { get; private set; }

    private void Awake()
    {
        if (cardImage == null)
            cardImage = GetComponent<Image>();
    }

    public void Setup(MemoryGameManager gameManager, int pairId, Sprite face, Sprite back)
    {
        manager = gameManager;
        PairId = pairId;
        faceSprite = face;
        backSprite = back;

        IsOpen = false;
        IsMatched = false;

        cardImage.sprite = backSprite;
        transform.localScale = Vector3.one;
    }

    // 👇 버튼 대신 이걸로 클릭 감지
    public void OnPointerClick(PointerEventData eventData)
    {
        if (IsOpen || IsMatched) return;
        manager.OnCardClicked(this);
    }

    public void Reveal()
    {
        StartCoroutine(FlipTo(faceSprite));
        IsOpen = true;
    }

    public void Hide()
    {
        StartCoroutine(FlipTo(backSprite));
        IsOpen = false;
    }

    public void MatchAndDisappear()
    {
        IsMatched = true;
        StartCoroutine(DisappearRoutine());
    }

    private IEnumerator DisappearRoutine()
    {
        yield return new WaitForSeconds(0.15f);
        gameObject.SetActive(false);
    }

    private IEnumerator FlipTo(Sprite targetSprite)
    {
        Vector3 original = Vector3.one;
        Vector3 middle = new Vector3(0f, 1f, 1f);

        float t = 0f;
        float duration = 0.08f;

        while (t < duration)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(original, middle, t / duration);
            yield return null;
        }

        cardImage.sprite = targetSprite;

        t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            transform.localScale = Vector3.Lerp(middle, original, t / duration);
            yield return null;
        }

        transform.localScale = original;
    }
}