using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    // Start is called before the first frame update
    bool bInput = false;
    bool bPick = false;
    SpriteRenderer spriteRenderer;
    public Block pickBlock; // 플레이어가 잡은 블럭
    public BoardCtrl board; // 보드

    // 플레이어 좌표
    public Vector2Int PlayerIndex = new Vector2Int();

    // pickBlock 좌표
    public Vector2Int PickBlockIndex = new Vector2Int();

    void Start()
    {
        Manager.Input.KeyAction -= OnKeyboard;
        Manager.Input.KeyAction += OnKeyboard; // 키보드 등록
        bInput = false;
        bPick = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        pickBlock = null;

        if (board.blockBoard[4, 1] != null)
        {
            transform.position = board.blockBoard[4, 1].transform.position;
        }
        else
            Debug.Log("blockBoard[4,1] == null");

        PlayerIndex.x = 1;
        PlayerIndex.y = 4;
        
    }

    void OnKeyboard()
    {
        // TODO : 키보드에 따른 액션 
        if (Input.GetKeyDown(KeyCode.LeftArrow) && !bInput)
        {
            PlayerIndex.x = (PlayerIndex.x + 2) % 3;
            SetPlayerPos();
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) && !bInput)
        {
            PlayerIndex.x = (PlayerIndex.x + 1) % 3;
            SetPlayerPos();
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && !bInput)
        {
            board.AddRow();
            SetPlayerPos();
        }
        if (Input.GetKeyDown(KeyCode.Space) && !bInput)
        {
            //bool result;
            if (!bPick)
                board.jobSerializer.Push(PickUpBlock);
            else
                board.jobSerializer.Push(PickDownBlock);

            //if (!result)
            //Debug.Log("Game Over!");
            bInput = true;
        }
        if (!Input.anyKeyDown)
            bInput = false;
    }
    public void SetPlayerPos()
    {
        PlayerIndex.y = board.GetColumnBlockCount(PlayerIndex.x);
        if (PlayerIndex.y < 0)
            PlayerIndex.y = 0;
        transform.position = board.boardPos[PlayerIndex.y, PlayerIndex.x];
        bInput = true;
    }
    
    // 블록을 Pick up
    void PickUpBlock()
    {
        if (board.GetColumnBlockCount(PlayerIndex.x) < 0)
            return;
        
        // TODO : 특수 블럭 처리 추가
        spriteRenderer.color = Color.black;
        PickBlockIndex = PlayerIndex;
        bPick = true;


        Debug.Log($"{PickBlockIndex.x} index pick");
        return;
    }
    void PickDownBlock()
    {
        spriteRenderer.color = Color.white;

        if (PlayerIndex.x == PickBlockIndex.x)
        {
            bPick = false;
            return;
        }
            
       
        pickBlock = board.blockBoard[board.GetColumnBlockCount(PickBlockIndex.x), PickBlockIndex.x];
        int dy = board.GetColumnBlockCount(PlayerIndex.x);
        if (dy + 1 >= 13)
            return;
        if (board.blockBoard[dy + 1, PlayerIndex.x] != null)
        {
            Debug.Log("ERROR!");
            return;
        }

        board.blockBoard[dy + 1, PlayerIndex.x] = pickBlock;
        board.blockBoard[board.GetColumnBlockCount(PickBlockIndex.x), PickBlockIndex.x] = null;
        pickBlock.transform.position = board.boardPos[dy + 1, PlayerIndex.x];

        board.SetColumnBlockCount(PlayerIndex.x, 1);
        board.SetColumnBlockCount(PickBlockIndex.x, -1);

        // 블록 체크
        if (board.CheckColumn(PlayerIndex.x))
        {
            Debug.Log("3 correct, block will remove");
            board.RemoveBlock(PlayerIndex.x);

            PlayerIndex.y = Mathf.Clamp(dy - 2, 0, 21);
        }
        else
        {
            PlayerIndex.y = dy + 1;
        }
        // 플레이어 좌표를 현재 좌표로 이동
        transform.position = board.boardPos[PlayerIndex.y, PlayerIndex.x];

        pickBlock = null;
        bPick = false;
        PickBlockIndex.x = PickBlockIndex.y = -1;
        return;
    }
}
