﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Level", menuName = "Level")]
public class Level: ScriptableObject
{
    public int orderItemSize;
    public int levelItemSize;
    public float conveyorBeltSpeed;
    public Level(int _orderItemSize, int _levelItemSize, int _conveyorBeltSpeed)
    {
        this.orderItemSize = _orderItemSize;
        this.levelItemSize = _levelItemSize;
        this.conveyorBeltSpeed = _conveyorBeltSpeed;
    }
}