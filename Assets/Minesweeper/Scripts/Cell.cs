using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Cell : MonoBehaviour
{
    public TMP_Text text = default;
    public Image img = default;
    public Sprite mineSprite = default;
    public Sprite normalSprite = default;

    public void Setup(int t)
    {
        transform.localPosition = Vector3.zero;
        transform.localScale = Vector3.one;

        if (t != 0 && t != -1)
        {
            text.SetText(t.ToString());
        }
        if (t == -1)
        {
            img.sprite = mineSprite;
        }
        else
        {
            img.sprite = normalSprite;
        }
    }
}