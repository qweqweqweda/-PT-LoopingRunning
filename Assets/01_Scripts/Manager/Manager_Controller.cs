using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Manager_Controller : Singleton<Manager_Controller>
{
    public UnitBase unit_Target;
    public Camera game_Camare;

    int touchCount;
    int touchCount_Last;
    Vector3 touchPos_Enter;
    Vector3 dir_Last;
    public int popupCount;

    public void Init()
    {

    }

    public void Update()
    {
        if (unit_Target == null || popupCount > 0)
            return;

        touchCount = 0;
        Vector3 dir = Vector3.zero;

        bool onKeyboard = false;

        //키보트로 조작하는 경우
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            onKeyboard = true;
            touchCount = 1;
        }

        //조작을 금지했을때 터치를 입력받지 않는다.
        if (touchCount > 0)
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                touchCount = 0;
            }
        }

        if (touchCount > 0)
        {
            Vector3 touchPos_Current = Vector3.zero;

            if (onKeyboard) // 키보드 이동일 시
            {
                if (dir_Last.y >= 0)
                {
                    if (Input.GetKey(KeyCode.W))
                    {
                        touchPos_Current.y += 1;
                    }
                    else if (Input.GetKey(KeyCode.S))
                    {
                        touchPos_Current.y -= 1;
                    }
                }
                else
                {
                    if (Input.GetKey(KeyCode.S))
                    {
                        touchPos_Current.y -= 1;
                    }
                    else if (Input.GetKey(KeyCode.W))
                    {
                        touchPos_Current.y += 1;
                    }
                }

                if (dir_Last.x >= 0)
                {
                    if (Input.GetKey(KeyCode.D))
                    {
                        touchPos_Current.x += 1;
                    }
                    else if (Input.GetKey(KeyCode.A))
                    {
                        touchPos_Current.x -= 1;
                    }
                }
                else
                {
                    if (Input.GetKey(KeyCode.A))
                    {
                        touchPos_Current.x -= 1;
                    }
                    else if (Input.GetKey(KeyCode.D))
                    {
                        touchPos_Current.x += 1;
                    }
                }

                touchPos_Current = touchPos_Current.normalized;
            }
            else    // 터치 이동일 때 마우스 터치위치로
            {
                touchPos_Current = Input.mousePosition;
            }

            if (!onKeyboard)
            {
                touchPos_Enter = touchPos_Current;
            }

            dir = touchPos_Current - touchPos_Enter;

            if (onKeyboard)
            {
                dir = dir.normalized;

                // Player 이동
                unit_Target.MoveDirection(dir);
            }
            else
            {
                if (dir.magnitude >= 10)    // 10픽셀 이상 
                {
                    dir = dir.normalized;

                    // Player 이동
                    unit_Target.MoveDirection(dir);
                }
            }
        }
        else
        {
            // Player 이동종료 액션

        }


        SetCameraPos();
    }

    void SetCameraPos()
    {
        Vector3 tmp = unit_Target.transform.position;
        tmp.z = -100;
        tmp.y = 0;
        game_Camare.transform.position = tmp;

        Manager_Background.Instance.RenewBackgroundPos(tmp.x);
    }
}
