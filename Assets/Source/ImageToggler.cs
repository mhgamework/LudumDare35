using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ImageToggler : MonoBehaviour
{
    [SerializeField]
    private Image targetImage = null;
    [SerializeField]
    private Sprite[] sprites = new Sprite[0];

    public void SetSprite(int index)
    {
        targetImage.sprite = sprites[index];
    }
}
