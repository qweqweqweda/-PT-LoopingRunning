using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Manager_Background : Singleton<Manager_Background>
{
    public Transform Tf_Parent;
    public SpriteRenderer[] Sr_Background;

    public const float width = 1280;

    private int backgroundIndex_Last;

    public void Init()
    {
        Tf_Parent.position = new Vector3(0, 0, 5000);
        Clear();
    }

    void Clear()    // 모든 배경을 초기화
    {
        for (int i = 0; i < Sr_Background.Length; i++)
        {
            Sr_Background[i].sprite = null;
            Sr_Background[i].transform.position = Vector3.zero;
        }
    }

    public void SetBackground(string fileName)  // 게임 시작하면 배경 세팅팅
    {
        Clear();

        string path = "Textures/" + fileName;
        Sprite sprite = Resources.Load(path, typeof(Sprite)) as Sprite; //Resources 폴더 안에있는 Textures 폴더 안에 fileName의 Sprite를 가져옴

        for (int i = 0; i < Sr_Background.Length; i++)
        {
            Sr_Background[i].sprite = sprite;
        }

        RenewBackgroundPos(0, true);
    }

    public void RenewBackgroundPos(float posX_Camera, bool onForce = false)
    {
        int backgroundIndex; // 현재 위치의 백그라운드인덱스

        if (posX_Camera >= 0)   // 오른쪽으로 이동 했을 때
        {
            backgroundIndex = (int)((posX_Camera + width * 0.5f) / width);
            // 영웅 위치가 아닌 Camera위치를 기준으로함 유저가 바라보고있는 화면은 플레이어의 화면이 아닌 카메라의 화면이기 때문
            // width * 0.5f는 처음 시작할 때 배경의 한 가운데에서 시작하기 때문에 배경의 절반인 width * 0.5f 를 계산해줌
            // width를 나눔으로 현재 카메라가 배경의 몇번째 넘어간건지 알 수 있음
        }
        else
        {
            backgroundIndex = (int)((posX_Camera - width * 0.5f) / width);
            // 카메라의 좌표가 -이면 왼쪽으로 이동했기에 width * 0.5f 를 - 해줘야함
        }

        if (backgroundIndex != backgroundIndex_Last || onForce) // 현재 위치가 다른 배경위치일때
        {
            int localBackgroundIndex = Sr_Background.Length / 2 * -1; // 홀수 일 때만
            // 배경 갯수 전체기준으로 가장 왼쪽의 인덱스부터 나타냄
            // 예 : Length가 5 -> -2 -1 0 1 2 중 -2를 나타냄

            for (int i = 0; i < Sr_Background.Length; i++)
            {
                int tmpBackgroundIndex = backgroundIndex + localBackgroundIndex + i;
                // 백그라운드 갯수만큼 가장 낮은 인덱스부터 나타나게됨 i의 증가에따라 인덱스 증가

                int arrayIndex = (tmpBackgroundIndex % Sr_Background.Length + Sr_Background.Length) % Sr_Background.Length;
                // + Sr_Background.Length) % Sr_Background.Length 부분은 음수값이 나오는것을 방지

                Sr_Background[arrayIndex].transform.localPosition = new Vector3(tmpBackgroundIndex * width, 0, 0);
            }
        }
        backgroundIndex_Last = backgroundIndex;
    }
}
