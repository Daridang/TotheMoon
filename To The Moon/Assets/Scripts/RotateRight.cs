﻿using UnityEngine;
using UnityEngine.EventSystems;

public class RotateRight : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField]
    private Rocket _rocket;

    private bool _pointerDown = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        _pointerDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _pointerDown = false;
    }

    private void Update()
    {
        if(_rocket != null)
        {
            _rocket.RotateRight(_pointerDown);
        }
    }

    public void SetRocket(Rocket rocket)
    {
        _rocket = rocket;
    }
}