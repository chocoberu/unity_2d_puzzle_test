using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardCtrl : MonoBehaviour
{
    public const int MaxY = 13;

    public Block[,] blockBoard = new Block[MaxY + 1, 3];
    public Vector3[,] boardPos = new Vector3[MaxY + 1, 3];
    public GameObject blockPrefab;

    float[] updateTime = new float[4] { 5.0f, 4.0f, 3.5f, 2.5f };

    public Dictionary<Define.Block, Stack<Block>> pool = new Dictionary<Define.Block, Stack<Block>>();

    //
    public JobSerializer jobSerializer = new JobSerializer();

    public bool IsWorking { get; set; } = false;

    int[] columnMaxValue = new int[3] { -1, -1, -1 };
    
    // 해당 열의 블록 개수를 반환
    public int GetColumnBlockCount(int index) { return columnMaxValue[index]; } 

    // 해당 열의 블록 개수를 설정
    public void SetColumnBlockCount(int index, int value ) { columnMaxValue[index] += value; }

    float firstY = 9.0f;

    Coroutine coUpdateBoard;
    
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < MaxY; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                boardPos[i, j] = new Vector3(4 * j - 4.0f, firstY - i * 1.5f, 0.0f);
            }
        }
        for(int i = 0; i < (int)Define.Block.Size; i++)
        {
            Stack<Block> stack = new Stack<Block>();
            CreateBlock(stack, i, 50);
            pool.Add((Define.Block)i, stack);
        }
        for(int i = 0; i <5; i++)
        {
            for(int j = 0; j < 3; j++)
            {
                AddBlock(i, j, Start : true);
                columnMaxValue[j] = i;
            }
        }
        coUpdateBoard = StartCoroutine(UpdateBoard());
    }

    void CreateBlock(Stack<Block> stack, int blockColor,  int blockCount = 10)
    {
        for(int i = 0; i < blockCount; i++)
        {
            GameObject go = Instantiate(blockPrefab);
            Block block = go.GetComponent<Block>();
            block.SetBlockColor(blockColor);
            go.SetActive(false);
            stack.Push(block);
        }
        
    }
    
    // index 열에 같은 블록이 3개 연속으로 나열했는지 체크
    public bool CheckColumn(int index)
    {
        if(GetColumnBlockCount(index) < 2)
            return false;

        int conti = 1;
        int start = GetColumnBlockCount(index);
        Define.Block color = blockBoard[start, index].blockColor;

        for(int i = 1; i < 3; i++)
        {
            if(blockBoard[start - i, index] == null)
            {
                Debug.Log("ERROR!");
            }
            if (blockBoard[start - i, index].blockColor == color)
                conti++;
            else
                break;
        }
        if (conti == 3)
            return true;
        return false;
    }

    public void AddBlock(int row, int index, bool Start = false)
    {
        // 블록 게임 오브젝트 생성 및 초기화
        //GameObject go = Instantiate(block, boardPos[row, index], Quaternion.identity);
        int randIndex = Random.Range(0, (int)Define.Block.Size);
        Stack<Block> stack;
        pool.TryGetValue((Define.Block)randIndex, out stack);

        if(stack != null && stack.Count == 0)
        {
            // stack에 추가 생성
            CreateBlock(stack, randIndex, 10);
        }
        Block block = stack.Pop();

        blockBoard[row, index] = block;
        blockBoard[row, index].transform.parent = this.transform;
        blockBoard[row, index].transform.position = boardPos[row, index];
        //int blockColor = Random.Range(0, (int)Define.Block.Size);
        //blockBoard[row, index].GetComponent<Block>().SetBlockColor(blockColor);
        blockBoard[row, index].gameObject.SetActive(false);

        // 처음 보드를 초기화 할 때 옵션
        if (Start)
        {
            if(row >= 2)
            {
                if (blockBoard[row - 2, index].GetComponent<Block>().BlockColor == blockBoard[row, index].GetComponent<Block>().BlockColor
                        && blockBoard[row - 1, index].GetComponent<Block>().BlockColor == blockBoard[row, index].GetComponent<Block>().BlockColor)
                {
                    int color = Random.Range(0, (int)Define.Block.Size);
                    while (color == (int)blockBoard[row - 1, index].GetComponent<Block>().BlockColor)
                    {
                        color = Random.Range(0, (int)Define.Block.Size);
                    }
                    blockBoard[row, index].GetComponent<Block>().SetBlockColor(color);
                }
            }
        }
        else
        {
            // 같은 블록이 한 열에 연속적으로 3개 존재하는지 체크, 그런 경우 색을 변경한다
            if (GetColumnBlockCount(index) >= 2)
            {
                if (blockBoard[row, index].GetComponent<Block>().BlockColor == blockBoard[row + 1, index].GetComponent<Block>().BlockColor
                    && blockBoard[row + 1, index].GetComponent<Block>().BlockColor == blockBoard[row + 2, index].GetComponent<Block>().BlockColor)
                {
                    int color = Random.Range(0, (int)Define.Block.Size);
                    while (color == (int)blockBoard[1, index].GetComponent<Block>().BlockColor)
                    {
                        color = Random.Range(0, (int)Define.Block.Size);
                    }
                    blockBoard[row, index].GetComponent<Block>().SetBlockColor(color);
                }
            }
        }
        blockBoard[row, index].gameObject.SetActive(true);
    }

    // 연속으로 나열된 블록 제거
    public void RemoveBlock(int index)
    {
        // TODO : 폭탄 등의 블록 제거 필요
        int start = GetColumnBlockCount(index);
        for(int i = 0; i < 3; i++)
        {
            //Destroy(blockBoard[start - i, index].gameObject);
            blockBoard[start - i, index].gameObject.SetActive(false);
            Stack<Block> stack;
            pool.TryGetValue(blockBoard[start - i, index].blockColor, out stack);
            stack.Push(blockBoard[start - i, index]);
            blockBoard[start - i, index] = null;
            columnMaxValue[index]--;
        }

        // 점수 추가
        Manager.Game.AddScore();

    }

    public void AddRow()
    {
        IsWorking = true;
        for (int j = 0; j < 3; j++)
        {
            if(GetColumnBlockCount(j) == 12)
            {
                Debug.Log("Game Over!");
                StopCoroutine(coUpdateBoard);
                Manager.Game.EndGame();
                return;
            }
        }

        for(int j = 0; j < 3; j++)
        {
            // 1. 기존 블록 위치 이동
            for (int i = GetColumnBlockCount(j); i >= 0; i--)
            {
                if(blockBoard[i,j] == null)
                {
                    Debug.Log($"({i}, {j}) is null");
                    continue;
                }
                blockBoard[i + 1, j] = blockBoard[i, j]; 
                blockBoard[i + 1, j].transform.position = boardPos[i + 1, j];
                blockBoard[i, j] = null;
            }
            SetColumnBlockCount(j, 1);

            // 2. 블록 추가
            AddBlock(0, j);
        }
        IsWorking = false;

    }

    IEnumerator UpdateBoard()
    {
        while (true)
        {
            yield return new WaitForSeconds(updateTime[Manager.Game.Level - 1]);
            //AddRow();
            jobSerializer.Push(AddRow);
            Debug.Log("AddRow jobSerializer push");
        }
    }
    
}
