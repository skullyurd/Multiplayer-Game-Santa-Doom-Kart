using System;
using System.Collections;
using System.Collections.Generic;
using Lean.Gui;
using UnityEngine;

public class ModalAnim : MonoBehaviour
{
    [SerializeField] private LeanWindow leanWindow;

    private void OnEnable()
    {
        leanWindow.TurnOn();
    }
    private void OnDisable()
    {
        leanWindow.TurnOff();
    }
}
