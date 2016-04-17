using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SpriteSetter : MonoBehaviour
{
    [SerializeField]
    private Image[] images = new Image[0];
    [SerializeField]
    private Sprite sprite = null;
    [SerializeField]
    private Color spriteColor = Color.white;

    void OnValidate()
    {
        if (images == null)
            return;

        foreach (var image in images)
        {
            if (image == null)
                continue;
            image.color = spriteColor;
            image.sprite = sprite;
        }
    }
}
