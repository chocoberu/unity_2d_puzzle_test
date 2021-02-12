using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour
{
    public Define.Block blockColor;
    public SpriteRenderer spriteRenderer;

    public Define.Block BlockColor { get { return blockColor; } }
    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        //blockColor = (Define.Block)Random.Range(0, (int)Define.Block.Size);

        //SetBlockColor((int)blockColor);
    }
    private void Update()
    {
        switch (BlockColor)
        {
            case Define.Block.Red:
                spriteRenderer.color = Color.red;
                break;
            case Define.Block.Green:
                spriteRenderer.color = Color.green;
                break;
            case Define.Block.Blue:
                spriteRenderer.color = Color.blue;
                break;
        }
    }

    public void SetBlockColor(int newBlockColor)
    {
        blockColor = (Define.Block)newBlockColor;

        if (spriteRenderer == null)
            return;
        switch (blockColor)
        {
            case Define.Block.Red:
                spriteRenderer.color = Color.red;
                break;
            case Define.Block.Green:
                spriteRenderer.color = Color.green;
                break;
            case Define.Block.Blue:
                spriteRenderer.color = Color.blue;
                break;
        }
    }
}
