using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlocksController : MonoBehaviour
{
    [SerializeField]
    GroupBlocks groupBlocks_prefab;

    [SerializeField]
    Grid m_grid;

    [Header("Game loop")]
    [SerializeField]
    float intervalLoop;


    // private variables

    GroupBlocks currentGroup = null;

    public Block tercerblockToTest;

    Dictionary<Vector3Int, Block> blocks_dictionary = new Dictionary<Vector3Int, Block>();

    public void createGroup()
    {
        currentGroup = GameObject.Instantiate(groupBlocks_prefab,new Vector3(Random.Range(1,8),10,0), Quaternion.identity);
    }


    public void startGameLoop()
    {
        StartCoroutine(loopGame());
    }

    IEnumerator loopGame()
    {
        while (true)
        {
            if(currentGroup == null)
            {
                createGroup();
                continue;
            }

            if (isGroupOverGround())
            {
                //yield return checkLevelAfterGroup();

                currentGroup.destroyGroup();
                createGroup();
            } else
            {
                currentGroup.displaceDown(1);
            }
            yield return new WaitForSeconds(intervalLoop);
        }

    }

    IEnumerator checkLevelAfterGroup()
    {
        HashSet<Block> blocksToDelete = new HashSet<Block>();
        Vector3Int p = tercerblockToTest.getPosInGrid();
        int countV = 0;
        for (int i = 0; i < 5; i++)
        {
            if (isSameColor(p, p - Vector3Int.up))
            {
                Debug.Log("Checkeando " + p + " y " + (p-Vector3Int.up));
                countV++;

                if(countV == 2)
                {
                    blocksToDelete.Add(blocks_dictionary[p-Vector3Int.up]); 
                    blocksToDelete.Add(blocks_dictionary[p+Vector3Int.up]);
                    blocksToDelete.Add(blocks_dictionary[p]);

                    blocks_dictionary[p - Vector3Int.up].willBeDestroyed = true;
                    blocks_dictionary[p + Vector3Int.up].willBeDestroyed = true;
                    blocks_dictionary[p].willBeDestroyed = true;
                } else if(countV > 2)
                {
                    blocksToDelete.Add(blocks_dictionary[p - Vector3Int.up]);
                    blocks_dictionary[p].willBeDestroyed = true;
                }
            } else
            {
                countV = 0;
            }


            p -= Vector3Int.up;
        }

        //ahora verificamos los laterales
        Debug.LogWarning("Checkeando horizontalmente");
        int countH = 0;
        for (int j = 0; j < 3; j++)
        {
            p = new Vector3Int(tercerblockToTest.getPosInGrid().x - 2, tercerblockToTest.getPosInGrid().y - j, 0);
            for (int i = 0; i < 5; i++)
            {
                if (isSameColor(p, p + Vector3Int.right))
                {
                    countH++;

                    if (countH == 2)
                    {
                        if (blocks_dictionary[p].willBeDestroyed == false)
                        {
                            blocksToDelete.Add(blocks_dictionary[p]);
                            moveUpperObjects(p);
                        }
                        if (blocks_dictionary[p - Vector3Int.right].willBeDestroyed == false)
                        {
                            blocksToDelete.Add(blocks_dictionary[p - Vector3Int.right]);
                            moveUpperObjects(p - Vector3Int.right);
                        }
                        if (blocks_dictionary[p + Vector3Int.right].willBeDestroyed == false)
                        {
                            blocksToDelete.Add(blocks_dictionary[p + Vector3Int.right]);
                            moveUpperObjects(p + Vector3Int.right);
                        }
                    }
                    else if (countH > 2)
                    {
                        if (blocks_dictionary[p + Vector3Int.right].willBeDestroyed == false)
                        {
                            blocksToDelete.Add(blocks_dictionary[p + Vector3Int.right]);
                            moveUpperObjects(p + Vector3Int.right);
                        }
                    }
                } else
                {
                    countH = 0;
                }

                p += Vector3Int.right;
            }


            countH = 0;
        }

        Debug.LogWarning(blocksToDelete.Count + " bloques eliminados.");
        foreach (Block b in blocksToDelete)
        {
            GameObject.Destroy(b.gameObject);
        }

        yield return new WaitForSeconds(intervalLoop);
    }

    void moveUpperObjects(Vector3Int objectPos)
    {
        blocks_dictionary.Remove(objectPos);

        bool existNext = true;

        while (existNext)
        {
            objectPos += Vector3Int.up;
            if(blocks_dictionary.ContainsKey(objectPos))
            {
                Block aux = blocks_dictionary[objectPos];
                aux.displaceOneBlock();
                blocks_dictionary.Remove(objectPos);
                blocks_dictionary.Add(objectPos - Vector3Int.up,aux);
            }else {
                checkPointsAfterScore(objectPos-Vector3Int.up);
                existNext = false;
            }
        }
    }

    void checkPointsAfterScore(Vector3Int objectPos)
    {

    }


    bool isSameColor(Vector3Int posA, Vector3Int posB)
    {
        if(blocks_dictionary.ContainsKey(posA) && blocks_dictionary.ContainsKey(posB))
        {
            if (blocks_dictionary[posA].getColor() == blocks_dictionary[posB].getColor())
            {
                return true;
            } else
            {
                return false;
            }

        }
        return false; 
    }
    /// <summary>
    /// Verifica si el grupo actual esta sobre otro bloque o si esta sobre el suelo
    /// </summary>
    /// <returns></returns>
    public bool isGroupOverGround()
    {
        //el block1 es el que esta mas abajo
        Vector3Int pos = worldToGrid(currentGroup.transform.position);
        if (blocks_dictionary.ContainsKey(pos-Vector3Int.up) || pos.y <= 1)
        {

            blocks_dictionary.Add(pos,currentGroup.block1);
            blocks_dictionary.Add(pos + Vector3Int.up, currentGroup.block2);
            blocks_dictionary.Add(pos + Vector3Int.up + Vector3Int.up, currentGroup.block3);

            currentGroup.block1.setPosInGrid(pos);
            currentGroup.block2.setPosInGrid(pos + Vector3Int.up);
            currentGroup.block3.setPosInGrid(pos + Vector3Int.up + Vector3Int.up);

            return true;
        
        } 
        return false;
    }


    public void setupTestGame()
    {
        Block[] blocks_test = GameObject.FindObjectsOfType<Block>();
        foreach (Block b in blocks_test)
        {
            blocks_dictionary.Add(worldToGrid(b.transform.position),b);
            b.setPosInGrid(worldToGrid(b.transform.position));
        }

        Debug.Log("Checkeando puntaje");
        StartCoroutine(checkLevelAfterGroup());
    }

    public Vector3Int worldToGrid(Vector3 pos)
    {
        return m_grid.WorldToCell(pos);
    }

    public void moveCurrentGroupYaxis(float pos)
    {
        if(currentGroup.transform.position.x != pos)
        {
            currentGroup.transform.position = new Vector3(pos,
                currentGroup.transform.position.y, 0);

        } else
        {
            currentGroup.rotateBlocks();
        }
    }

}
