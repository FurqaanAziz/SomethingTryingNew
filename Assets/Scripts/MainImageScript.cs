using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MainImageScript : MonoBehaviour
{
    [SerializeField] private GameObject image_unknown;
    [SerializeField] private GameControllerScript gameController;

    public void OnMouseDown()
    {
        if (image_unknown.activeSelf && gameController.canOpen)
        {
            image_unknown.GetComponent<DOTweenAnimation>().DOPlay();
            gameController.imageOpened(this);
        }
    }

    private int _spriteId;
    public int spriteId
    {
        get { return _spriteId; }
    }

    public void ChangeSprite(int id,Sprite image, float spriteWidth, float spriteHeight)
    {
        _spriteId = id;
        GetComponent<SpriteRenderer>().size = new Vector2(spriteWidth, spriteHeight);
        GetComponent<SpriteRenderer>().sprite = image; //Gets the sprite renderer component to change the sprite.
    }
    
    public void Close()
    {
        image_unknown.SetActive(true); // Hide image
        image_unknown.GetComponent<DOTweenAnimation>().DORewind();
    }
}
