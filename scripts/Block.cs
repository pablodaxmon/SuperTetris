using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class Block : MonoBehaviour
{


    [SerializeField]
    BlockColor blockColor;

    [SerializeField]
    Vector3Int posInGrid;

    SpriteRenderer spriteRenderer;

    public bool willBeDestroyed = false;
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void setBlockColor(BlockColor color)
    {
        blockColor = color;

        switch (color) {
            case BlockColor.ROJO:
                spriteRenderer.color = new Color(0.8867924f, 0.2135372f, 0.1464044f,1f);
                return;
            case BlockColor.AMARILLO:
                spriteRenderer.color = new Color(0.8862745f, 0.6931456f, 0.145098f, 1f);
                return;
            case BlockColor.VERDE:
                spriteRenderer.color = new Color(0.4736959f, 0.7264151f, 0.07195621f, 1f);
                return;
            case BlockColor.AZUL:
                spriteRenderer.color = new Color(0.07943218f, 0.3016083f, 0.8018868f, 1f);
                return;
        }
    }

    
    public void setPosInGrid(Vector3Int order)
    {
        posInGrid = order;
    }

    public Vector3Int getPosInGrid()
    {
        return posInGrid;
    }


    public BlockColor getColor()
    {
        return blockColor;
    }

    public void displaceOneBlock()
    {
        transform.position -= Vector3.up;
    }
}

public enum BlockColor
{
    ROJO,AMARILLO,VERDE,AZUL
}