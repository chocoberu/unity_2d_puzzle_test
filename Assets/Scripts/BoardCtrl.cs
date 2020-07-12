using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCtrl : MonoBehaviour
{
    // TODO : 블럭위치를 저장할 자료구조 필요

    public GameObject[,] blockBoard = new GameObject[20, 3];
    public Vector3[,] boardPos = new Vector3[20, 3];
    public GameObject block;

    int[] columnMaxValue = new int[3];
    

    public int GetColumnMaxValue(int index) { return columnMaxValue[index]; } 
    public void SetColumnMaxValue(int index, int value ) { columnMaxValue[index] += value; }

    float firstY = 9.0f;
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < 20; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                boardPos[i, j] = new Vector3(4 * j - 4.0f, firstY - i * 1.5f, 0.0f);
            }
        }
        for(int i = 0; i <5; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                blockBoard[i,j] = Instantiate(block, boardPos[i, j], Quaternion.identity);
                blockBoard[i, j].transform.parent = this.transform;
                int blockColor = Random.Range(0, (int)Define.Block.Size);
                blockBoard[i, j].GetComponent<Block>().SetBlockColor(blockColor);

                // 같은 블록이 한 열에 연속적으로 3개 존재하는지 체크, 그런 경우 색을 변경한다
                if(i >= 2)
                {
                    if (blockBoard[i - 2, j].GetComponent<Block>().BlockColor == blockBoard[i, j].GetComponent<Block>().BlockColor
                        && blockBoard[i - 1, j].GetComponent<Block>().BlockColor == blockBoard[i, j].GetComponent<Block>().BlockColor)
                    {
                        //Debug.Log($"Current pos ({i}, {j})");
                        int color = Random.Range(0, (int)Define.Block.Size);
                        //Debug.Log(color);
                        //Debug.Log((int)blockBoard[i - 1, j].GetComponent<Block>().BlockColor);
                        while (color == (int)blockBoard[i - 1, j].GetComponent<Block>().BlockColor)
                        {
                            color = Random.Range(0, (int)Define.Block.Size);
                        }
                        //Debug.Log(color);
                        blockBoard[i, j].GetComponent<Block>().SetBlockColor(color);
                        //Debug.Log("Change Color");
                    }
                }
            }
        }
        for(int i = 0; i < 3; i++)
        {
            columnMaxValue[i] = 4;
        }

    }
    
    public bool CheckColumn(int index)
    {

        return false;
    }
    public void RemoveBlock(int index)
    {

    }
    
}
