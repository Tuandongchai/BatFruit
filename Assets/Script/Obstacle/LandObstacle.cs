using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandObstacle : Obstacle
{
    [Header("Setup")]
    [SerializeField] private SpriteRenderer spriteRen;
    [SerializeField] private Sprite[] sprites;
    [SerializeField] private GameObject landBroke_VFX;


    public override void DestroyThis()
    {
        switch(this.type)
        {
            case ObstacleCellType.LandType3:
                Instantiate(landBroke_VFX, gameObject.transform.parent.position, Quaternion.identity);
                ChangedType(ObstacleCellType.LandType2);
                break;
            case ObstacleCellType.LandType2:
                Instantiate(landBroke_VFX, gameObject.transform.parent.position, Quaternion.identity);
                ChangedType(ObstacleCellType.LandType1);

                break;
            case ObstacleCellType.LandType1:
                Instantiate(landBroke_VFX, gameObject.transform.parent.position, Quaternion.identity);
                ChangedType(ObstacleCellType.LandType0);
                break;
            case ObstacleCellType.LandType0:
                Instantiate(landBroke_VFX, gameObject.transform.parent.position, Quaternion.identity);
                obstacleBroken?.Invoke(ObstacleCellType.LandType3);
                Destroy(gameObject, 0.02f);
                break;
        }
    }
    protected void ChangedType(ObstacleCellType typeOb)
    {
        switch (typeOb)
        {
            case ObstacleCellType.LandType2:
                this.type = typeOb;
                spriteRen.sprite = sprites[1];
                break;
            case ObstacleCellType.LandType1:
                this.type = typeOb;
                spriteRen.sprite = sprites[2];
                break;
            case ObstacleCellType.LandType0:
                this.type = typeOb;
                spriteRen.sprite = sprites[3];
                spriteRen.sortingOrder = 2;
                break;
        }
    }
}
