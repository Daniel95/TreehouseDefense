﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SlideMenu : MonoBehaviour {

    [SerializeField]
    private GameObject selectionMenu;

    [SerializeField]
    private float slideSpeed;

    [SerializeField]
    private float changeAlphaSpeed;

    private float distanceToMove;
    private float visiblePosX;

    private Image selectionMenuImage;

    private BuyTower buyTower;

    //slidestate is false if selectionMenu is not visible
    private bool visible;

    private bool getX;

    void Awake()
    {
        selectionMenuImage = selectionMenu.GetComponent<Image>();
        distanceToMove = selectionMenuImage.rectTransform.rect.width / 4;

        //startPosX = 680;//selectionMenu.transform.position.x;

        StartCoroutine(FadeOut());

        Color color = selectionMenuImage.color;
        color.a = 0;
        selectionMenuImage.color = color;

        foreach (Transform button in selectionMenu.transform)
        {
            button.GetComponent<Image>().color = color;
            button.gameObject.GetComponent<BuyTowerSlide>().SetClickable = true;
        }
    }

    public void ChangeMenuState() {
        //when trying to get StartPosX of selectionMenu it resulted in 0. this is my solution for it since i couldnt figure out why this is happening.
        if(!getX) visiblePosX = selectionMenu.transform.position.x - selectionMenuImage.rectTransform.rect.width / 4;
        getX = true;


        //Set Visible
        if (!visible) {
            visible = true;
            Vector3 PositionToGo = new Vector3(visiblePosX, selectionMenu.transform.position.y, selectionMenu.transform.position.z);
            StartCoroutine(SlideTo(PositionToGo, true));
            StartCoroutine(FadeIn());

            foreach (Transform button in selectionMenu.transform)
            {
                button.gameObject.GetComponent<BuyTowerSlide>().SetClickable = true;
            }
        }//Set Invisible
        else {
            visible = false;

            Vector3 PositionToGo = new Vector3(visiblePosX + distanceToMove, selectionMenu.transform.position.y, selectionMenu.transform.position.z);
            StartCoroutine(SlideTo(PositionToGo, false));
            StartCoroutine(FadeOut());

            foreach (Transform button in selectionMenu.transform)
            {
                button.gameObject.GetComponent<BuyTowerSlide>().SetClickable = false;
            }
        }
    }

    IEnumerator SlideTo(Vector3 _positionToGo, bool _visible) {
        while(selectionMenu.transform.position != _positionToGo && visible == _visible) {
            selectionMenu.transform.position = Vector3.MoveTowards(selectionMenu.transform.position, _positionToGo, slideSpeed);
            yield return new WaitForFixedUpdate();         // Leave the routine and return here in the next frame
        }
    }

    IEnumerator FadeOut()
    {
        while (selectionMenuImage.color.a >= 0 && !visible)
        {
            Color color = selectionMenuImage.color;
            color.a -= changeAlphaSpeed;
            selectionMenuImage.color = color;

            foreach (Transform button in selectionMenu.transform)
            {
                button.GetComponent<Image>().color = color;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    IEnumerator FadeIn()
    {
        while (selectionMenuImage.color.a <= 1 && visible)
        {
            Color color = selectionMenuImage.color;
            color.a += changeAlphaSpeed;
            selectionMenuImage.color = color;

            foreach (Transform button in selectionMenu.transform)
            {
                button.GetComponent<Image>().color = color;
            }

            yield return new WaitForFixedUpdate();
        }
    }

    public void InstantVisible(bool _visible) {
        int IntVisible = System.Convert.ToInt32(_visible);

        Color color = selectionMenuImage.color;
        color.a = IntVisible;
        selectionMenuImage.color = color;

        foreach (Transform button in selectionMenu.transform)
        {
            button.GetComponent<Image>().color = color;
        }

        if (_visible) selectionMenu.transform.position =  new Vector3(visiblePosX - distanceToMove, selectionMenu.transform.position.y, selectionMenu.transform.position.z);
        else selectionMenu.transform.position = new Vector3(visiblePosX, selectionMenu.transform.position.y, selectionMenu.transform.position.z);

        visible = !visible;
    }
}
