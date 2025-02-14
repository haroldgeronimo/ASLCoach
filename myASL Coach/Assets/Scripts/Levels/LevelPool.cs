﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "LevelPool", menuName = "myASL Coach/LevelPool", order = 0)]
public class LevelPool : ScriptableObject {

    public Level[] levels;
    public Difficulty difficultyType;

    public bool isLast (int index) {
        return (index == levels.Length - 1) ? true : false;
    }

}