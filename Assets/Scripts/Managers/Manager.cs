using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manager : MonoBehaviour
{
    static Manager _instance = null;
    static Manager Instance { get { Init(); return _instance; } }

    InputManager _input = new InputManager();

    public static InputManager Input { get { return Instance._input; } }

    static void Init()
    {
        if(_instance == null)
        {
            // 하이어라키에 매니저가 있는지 확인
            GameObject obj = GameObject.Find("Manager");

            if(obj == null)
            {
                obj = new GameObject { name = "Manager" };
                obj.AddComponent<Manager>();
            }
            DontDestroyOnLoad(obj);
            _instance = obj.GetComponent<Manager>();

            // 다른 매니저들 초기화
        }
    }
    public static void Clear()
    {
        Input.Clear();
    }
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
