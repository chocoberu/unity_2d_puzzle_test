using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCtrl : MonoBehaviour
{
    // Start is called before the first frame update
    bool input = false;
    bool pick = false;
    SpriteRenderer spriteRenderer;
    public GameObject pickBlock; // 플레이어가 잡은 블럭
    public BoardCtrl board; // 보드

    // 플레이어 좌표
    public int x;
    public int y;

    // pickBlock 좌표
    public int pickX;
    public int pickY;

    void Start()
    {
        Manager.Input.KeyAction -= OnKeyboard;
        Manager.Input.KeyAction += OnKeyboard; // 키보드 등록
        input = false;
        pick = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
        pickBlock = null;

        if (board.blockBoard[4, 1] != null)
        {
            transform.position = board.blockBoard[4, 1].transform.position;
        }
        else
            Debug.Log("blockBoard[4,1] == null");

        x = 1; y = 4;

    }

    void OnKeyboard()
    {
        // TODO : 키보드에 따른 액션 
        if (Input.GetKeyDown(KeyCode.LeftArrow) && !input)
        {
            x = (x + 2) % 3;
            y = board.GetColumnMaxValue(x);
            if (y < 0) y = 0;
            transform.position = board.boardPos[y, x];
            input = true;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow) && !input)
        {
            x = (x + 1) % 3;
            y = board.GetColumnMaxValue(x);
            if (y < 0) y = 0;
            transform.position = board.boardPos[y, x];
            input = true;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && !input)
        {
           
        }
        if (Input.GetKeyDown(KeyCode.Space) && !input)
        {
            if (!pick)
            {
                if (board.GetColumnMaxValue(x) >= 0)
                {
                    spriteRenderer.color = Color.black;
                    pickBlock = board.blockBoard[y, x];
                    pickX = x;
                    pickY = y;
                    pick = true;
                }
            }
            else
            {    
                spriteRenderer.color = Color.white;
                int dy = board.GetColumnMaxValue(x);
                if (dy <= 20 && x != pickX)
                {
                    // 블록을 옮길 좌표 계산
                    float posX, posY, posZ;
                    if (dy < 0)
                    {
                        posY = board.boardPos[0, x].y;
                        posX = board.boardPos[0, x].x;
                        posZ = board.boardPos[0, x].z;
                    }
                    else
                    {
                        posY = board.boardPos[dy, x].y - 1.5f;
                        posX = board.boardPos[dy, x].x;
                        posZ = board.boardPos[dy, x].z;
                    }
                    // 잡은 블록을 해당 위치로 옮긴다
                    board.blockBoard[dy + 1, x] = pickBlock;
                    board.SetColumnMaxValue(x, 1);
                    pickBlock.transform.position = new Vector3(posX, posY, posZ);

                    board.blockBoard[pickY, pickX] = null; // 이전 블록 위치에 null로
                    board.SetColumnMaxValue(pickX, -1);
                    y = dy + 1;

                    // 플레이어 좌표를 현재 좌표로 이동
                    transform.position = board.boardPos[y, x];
                }
                pickBlock = null;
                pick = false;
                pickX = pickY = -1;
            }
            input = true;
        }
        if (!Input.anyKeyDown)
            input = false;
    }
}
