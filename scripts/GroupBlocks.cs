using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupBlocks : MonoBehaviour
{
    public Block block1;
    public Block block2;
    public Block block3;

    public Block firstBlock;

    public void setFirstBlock(Block b)
    {
        block1 = b;
    }


    public void setSecondBlock(Block b)
    {
        block2 = b;
    }


    public void setThirdBlock(Block b)
    {
        block3 = b;
    }

    private void Start()
    {
        setRandomColors();
        firstBlock = block1;
    }

    void setRandomColors()
    {
        block1.setBlockColor((BlockColor)Random.Range(0,4));
        block2.setBlockColor((BlockColor)Random.Range(0, 4));
        block3.setBlockColor((BlockColor)Random.Range(0, 4));
    }

    public void rotateBlocks()
    {
        Vector3 aux_pos = block1.transform.position;
        block1.transform.position = block2.transform.position;
        block2.transform.position = block3.transform.position;
        block3.transform.position = aux_pos;

    }



    public void displaceDown(float speed)
    {
        transform.position -= Vector3.up*Time.deltaTime * speed;
    }

    public void destroyGroup()
    {
        block1.transform.SetParent(null);
        block2.transform.SetParent(null);
        block3.transform.SetParent(null);

        GameObject.Destroy(gameObject);
    }
}