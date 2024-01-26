using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "New Jewel Type", menuName = "Jewel")]
public class JewelSO : ScriptableObject
{
    public JewelType JewelType;
    public Sprite Sprite;
}