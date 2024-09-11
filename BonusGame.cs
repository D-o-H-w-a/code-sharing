using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class BonusGame : MonoBehaviour
{
    public enum Fanname
    {
        Hayong,
        HoneyBee,
        Adam,
        Mochi,
        Milk,
        Turtle,
    }

    public enum Honeyz
    {
        Ohwayo,
        HoneyChurros,
        Damyui,
        DDDDragon,
        AyaUke,
        Mangnae,
    }

    // 정수형 으로 점수를 저장할 변수명 score 로 선언
    private int score;
    // 텍스트 변수 scoreText 로 선언
    private Text scoreText;
    // enum 변수 fanname 선언
    private Fanname fanname;
    // enum 변수 honeyz 선언
    private Honeyz honeyz;
    // 프리팹 생성 스폰위치를 잡을 가져올 public 형태의 변수 spawnPoint 선언
    public GameObject spawnPoint;

    // 생성한 캐릭터를 보관할 변수
    public List<GameObject> charList = new List<GameObject>();
    // 업그레이드 대기중인 캐릭터를 보관할 변수
    public List<GameObject> upgradeList = new List<GameObject>(); 

    private void Awake()
    {
        // 자식 개체 중 Text 컴포넌트를 찾아서 scoreText 변수에 대입
        scoreText = GetComponentInChildren<Text>();
    }

    public void Update()
    {
        if (upgradeList.Count >= 2)
        {
            UpgradeChar();
        }
    }

    public void ScoreUpdate()
    {
        // 현재 점수 10점 추가
        score += 10;

        // 만약 score 를 50으로 나눴을 때 나머지가 0이면
        if (score % 10 == 0)
        {
            // enum 중 랜덤한 하나의 값을 들고오기
            fanname = GetRandomEnumValue<Fanname>();
            // 캐릭터 생성
            createChar();
        }

        // text 에 score 가 가진 점수 표기
        scoreText.text = " 점수: " + score;
    }

    // 캐릭터 생성
    public void createChar()
    {
        // 비활성화 된 오브젝트가 있는지 체크하고 거짓이(오브젝트 가 없음) 반환되면
        if (!FindChar())
        {
            // 리소스 주소를 통해서 오브젝트를 생성
            GameObject _char = Instantiate(Resources.Load("Prefabs/Character/" + fanname)) as GameObject;
            // 생성된 캐릭터 오브젝트를 charList 에 대입
            _char.name = _char.name.Replace("(Clone)","").Trim();
            charList.Add(_char);
        }
    }

    public bool FindChar()
    {
        // charList 에 있는 오브젝트 계속 체크하기
        foreach (GameObject obj in charList)
        {
            // 들고온 오브젝트 가 이름이 같은지 체크
            if (obj.name == fanname.ToString())
            {
                // 오브젝트가 비활성화 인지 체크
                if (!obj.activeSelf)
                {
                    // 오브젝트 활성화
                    obj.SetActive(true);
                    // 참을 반환
                    return true;
                }
            }
        }

        // 활성화 된 오브젝트가 없다면 거짓을 반환
        return false;
    }

    public T GetRandomEnumValue<T>()
    {
        // enum 타입의 값들을 배열로 가져온다
        T[] values = (T[])System.Enum.GetValues(typeof(T));

        // 랜덤한 인덱스를 선택하여 해당 값을 반환
        return values[UnityEngine.Random.Range(0, values.Length)];
    }

    // 같은 캐릭터 충돌 시 캐릭터 업그레이드 및 비활성화
    public void UpgradeChar()
    {
        if (upgradeList[0].name == upgradeList[1].name)
        {
            string _Name = upgradeList[0].name;
            upgradeList.RemoveAt(0);
            upgradeList.RemoveAt(1);

            // 해당 이름과 일치하는 Enum을 찾고 그 다음 값을 처리
            ProcessNextEnumValue(_Name);
        }
    }
    private void ProcessNextEnumValue(string name)
    {
        // Fanname Enum에서 처리
        if (TryGetNextEnumValue<Fanname>(name, out Fanname nextFanname))
        {
            Debug.Log($"Next Fanname enum value: {nextFanname}");
            // 여기에서 다음 Fanname enum 값으로 원하는 작업 수행
        }

        // Honeyz Enum에서 처리
        if (TryGetNextEnumValue<Honeyz>(name, out Honeyz nextHoneyz))
        {
            Debug.Log($"Next Honeyz enum value: {nextHoneyz}");
            // 여기에서 다음 Honeyz enum 값으로 원하는 작업 수행
        }
    }

    private bool TryGetNextEnumValue<TEnum>(string name, out TEnum nextEnum) where TEnum : struct, Enum
    {
        nextEnum = default;

        if (Enum.TryParse(name, out TEnum currentEnum) && Enum.IsDefined(typeof(TEnum), currentEnum))
        {
            TEnum[] enumValues = (TEnum[])Enum.GetValues(typeof(TEnum));

            int currentIndex = Array.IndexOf(enumValues, currentEnum);

            if (currentIndex >= 0 && currentIndex < enumValues.Length - 1)
            {
                nextEnum = enumValues[currentIndex + 1];
                return true;
            }
        }

        return false;
    }
}