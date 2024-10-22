using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    #region Inspector variables
    [SerializeField]
    Grid m_grid;
    [SerializeField]
    GroupBlocks groupBlocks_prefab;
    #endregion

    #region Singleton and Awake
    public static BoardManager instance;
    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    #endregion

    bool loseGame = false;
    bool isFalling = true;
    GroupBlocks currentGroup;

    Dictionary<Vector3Int,Block> blocks_dictionary = new Dictionary<Vector3Int,Block>();

    Vector3 currentGroupTarget = Vector3.zero;



    public void playGame()
    {
        setupTestGame();
        createRandomGroup();
        StartCoroutine(startLoopGame());


    }

    void createRandomGroup(){
        currentGroup = GameObject.Instantiate(groupBlocks_prefab, new Vector3(UnityEngine.Random.Range(1, 8), 10, 0), Quaternion.identity);
        currentGroupTarget = calculateCurrentGroupTarget(currentGroup.transform.position.x);
    }
    IEnumerator startLoopGame() {
        while (loseGame == false)
        {
            yield return dropGroup();
        }
        //yield return calculateScore();
        //yield return reorderBlocksAndScore();
    }

    IEnumerator dropGroup() {

        while(currentGroup.transform.position.y > currentGroupTarget.y)
        {
            currentGroup.displaceDown(1.8f);
            yield return null;
        }
        currentGroup.transform.position = currentGroupTarget;

        Vector3Int p = worldToGrid(currentGroup.transform.position);
        blocks_dictionary.Add(p,currentGroup.block1);
        blocks_dictionary.Add(p+Vector3Int.up, currentGroup.block2);
        blocks_dictionary.Add(p+Vector3Int.up+Vector3Int.up, currentGroup.block3);

        currentGroup.destroyGroup();

        if (loseGame)
        {
            UIManager.instance.showLosePanel();
        } else
        {
            createRandomGroup();
        }

    }

    public void moveCurrentGroupYAxis(float pos)
    {
        pos = checkAvailableHorizontalRoute(currentGroup.transform.position, pos);
        if (currentGroup.transform.position.x != pos)
        {
            
            currentGroup.moveHorizontal(pos);
            currentGroupTarget = calculateCurrentGroupTarget(pos);
        } else
        {
            currentGroup.rotateBlocks();
        }
        Debug.Log("Target: " + currentGroupTarget);
    }

    float checkAvailableHorizontalRoute(Vector3 fromPosition, float toYPosition)
    {
        Vector3Int fromInt = worldToGrid(fromPosition);
        int YtargetInt = (int)toYPosition - 1;

        //Debug.Log("Verificando para fromInt: " + fromInt + " con un target: " + YtargetInt);
        if(fromInt.x < YtargetInt)
        {
            while (fromInt.x <= YtargetInt)
            {
                //Debug.Log("Checkeando posiciones libres");
                fromInt += Vector3Int.right;
                if (blocks_dictionary.ContainsKey(fromInt))
                {
                    return fromInt.x;
                }
            }


        } else
        {
            while (fromInt.x > YtargetInt)
            {
                //Debug.Log("Checkeando posiciones libres");
                fromInt -= Vector3Int.right;
                if (blocks_dictionary.ContainsKey(fromInt))
                {
                    return fromInt.x+2;
                }
            }
        }

        return toYPosition;
    }

    public void setupTestGame()
    {

        Block[] blocks_test = GameObject.FindObjectsOfType<Block>();
        foreach (Block b in blocks_test)
        {
            blocks_dictionary.Add(worldToGrid(b.transform.position), b);
            b.setPosInGrid(worldToGrid(b.transform.position));
        }

    }

    Vector3 calculateCurrentGroupTarget(float pos) {
        Debug.Log("Calculando target a partir de un pos: " + pos);
        Vector3Int intp = worldToGrid(new Vector3(pos,currentGroup.transform.position.y,0));
        intp *= new Vector3Int(1,0,1);

        while (blocks_dictionary.ContainsKey(intp))
        {
            intp += Vector3Int.up;
        }
        if(intp.y >= 12)
        {
            gameOver();
        }
        return intp+new Vector3Int(1,1,0);

    }
    
    void gameOver()
    {
        loseGame = true;
    }
    
    public Vector3Int worldToGrid(Vector3 pos)
    {
        return m_grid.WorldToCell(pos);
    }

    IEnumerator calculateScore()
    {
        yield return null;
    }

    IEnumerator reorderBlocksAndScore()
    {
        yield return null;
    }
    IEnumerator checkScore()
    {
        yield return null;

        createRandomGroup();
    }

}
