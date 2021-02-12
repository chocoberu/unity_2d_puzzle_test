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
            bool result;
            if (!bPick)
                result = PickUpBlock();
            else
                result = PickDownBlock();

            if (!result)
                Debug.Log("Game Over!");
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
    bool PickUpBlock()
    {
        if (board.GetColumnBlockCount(PlayerIndex.x) < 0)
            return true;
        
        spriteRenderer.color = Color.black;
        pickBlock = board.blockBoard[PlayerIndex.y, PlayerIndex.x];
        PickBlockIndex = PlayerIndex;
        bPick = true;

        return true;
    }
    bool PickDownBlock()
    {
        spriteRenderer.color = Color.white;
        if(PickBlockIndex == PlayerIndex)
        {
            pickBlock = null;
            PickBlockIndex.x = PickBlockIndex.y = -1;
            bPick = false;
            return true;
        }
        int dy = board.GetColumnBlockCount(PlayerIndex.x);
        if (dy < 13 && PlayerIndex.x != PickBlockIndex.x)
        {
            // 블록을 옮길 좌표 계산
            float posX, posY, posZ;
            if (dy < 0)
            {
                posY = board.boardPos[0, PlayerIndex.x].y;
                posX = board.boardPos[0, PlayerIndex.x].x;
                posZ = board.boardPos[0, PlayerIndex.x].z;
            }
            else
            {
                posY = board.boardPos[dy, PlayerIndex.x].y - 1.5f;
                posX = board.boardPos[dy, PlayerIndex.x].x;
                posZ = board.boardPos[dy, PlayerIndex.x].z;
            }
            // 잡은 블록을 해당 위치로 옮긴다
            board.blockBoard[dy + 1, PlayerIndex.x] = pickBlock;
            board.SetColumnBlockCount(PlayerIndex.x, 1);
            pickBlock.transform.position = new Vector3(posX, posY, posZ);

            board.blockBoard[PickBlockIndex.y, PickBlockIndex.x] = null; // 이전 블록 위치에 null로

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

            board.SetColumnBlockCount(PickBlockIndex.x, -1);
            // 플레이어 좌표를 현재 좌표로 이동
            transform.position = board.boardPos[PlayerIndex.y, PlayerIndex.x];
        }
        else
            return false;

        pickBlock = null;
        bPick = false;
        PickBlockIndex.x = PickBlockIndex.y = -1;
        return true;
    }
}
