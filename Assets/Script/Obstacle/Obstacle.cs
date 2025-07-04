using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObstacleCellType {chain ,LandType0, LandType1, LandType2, LandType3 , none };
public abstract class Obstacle : MonoBehaviour
{
    public ObstacleCellType type;
    public static Action<ObstacleCellType> obstacleBroken;
    public abstract void DestroyThis();
}
