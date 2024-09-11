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

    // ������ ���� ������ ������ ������ score �� ����
    private int score;
    // �ؽ�Ʈ ���� scoreText �� ����
    private Text scoreText;
    // enum ���� fanname ����
    private Fanname fanname;
    // enum ���� honeyz ����
    private Honeyz honeyz;
    // ������ ���� ������ġ�� ���� ������ public ������ ���� spawnPoint ����
    public GameObject spawnPoint;

    // ������ ĳ���͸� ������ ����
    public List<GameObject> charList = new List<GameObject>();
    // ���׷��̵� ������� ĳ���͸� ������ ����
    public List<GameObject> upgradeList = new List<GameObject>(); 

    private void Awake()
    {
        // �ڽ� ��ü �� Text ������Ʈ�� ã�Ƽ� scoreText ������ ����
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
        // ���� ���� 10�� �߰�
        score += 10;

        // ���� score �� 50���� ������ �� �������� 0�̸�
        if (score % 10 == 0)
        {
            // enum �� ������ �ϳ��� ���� ������
            fanname = GetRandomEnumValue<Fanname>();
            // ĳ���� ����
            createChar();
        }

        // text �� score �� ���� ���� ǥ��
        scoreText.text = " ����: " + score;
    }

    // ĳ���� ����
    public void createChar()
    {
        // ��Ȱ��ȭ �� ������Ʈ�� �ִ��� üũ�ϰ� ������(������Ʈ �� ����) ��ȯ�Ǹ�
        if (!FindChar())
        {
            // ���ҽ� �ּҸ� ���ؼ� ������Ʈ�� ����
            GameObject _char = Instantiate(Resources.Load("Prefabs/Character/" + fanname)) as GameObject;
            // ������ ĳ���� ������Ʈ�� charList �� ����
            _char.name = _char.name.Replace("(Clone)","").Trim();
            charList.Add(_char);
        }
    }

    public bool FindChar()
    {
        // charList �� �ִ� ������Ʈ ��� üũ�ϱ�
        foreach (GameObject obj in charList)
        {
            // ���� ������Ʈ �� �̸��� ������ üũ
            if (obj.name == fanname.ToString())
            {
                // ������Ʈ�� ��Ȱ��ȭ ���� üũ
                if (!obj.activeSelf)
                {
                    // ������Ʈ Ȱ��ȭ
                    obj.SetActive(true);
                    // ���� ��ȯ
                    return true;
                }
            }
        }

        // Ȱ��ȭ �� ������Ʈ�� ���ٸ� ������ ��ȯ
        return false;
    }

    public T GetRandomEnumValue<T>()
    {
        // enum Ÿ���� ������ �迭�� �����´�
        T[] values = (T[])System.Enum.GetValues(typeof(T));

        // ������ �ε����� �����Ͽ� �ش� ���� ��ȯ
        return values[UnityEngine.Random.Range(0, values.Length)];
    }

    // ���� ĳ���� �浹 �� ĳ���� ���׷��̵� �� ��Ȱ��ȭ
    public void UpgradeChar()
    {
        if (upgradeList[0].name == upgradeList[1].name)
        {
            string _Name = upgradeList[0].name;
            upgradeList.RemoveAt(0);
            upgradeList.RemoveAt(1);

            // �ش� �̸��� ��ġ�ϴ� Enum�� ã�� �� ���� ���� ó��
            ProcessNextEnumValue(_Name);
        }
    }
    private void ProcessNextEnumValue(string name)
    {
        // Fanname Enum���� ó��
        if (TryGetNextEnumValue<Fanname>(name, out Fanname nextFanname))
        {
            Debug.Log($"Next Fanname enum value: {nextFanname}");
            // ���⿡�� ���� Fanname enum ������ ���ϴ� �۾� ����
        }

        // Honeyz Enum���� ó��
        if (TryGetNextEnumValue<Honeyz>(name, out Honeyz nextHoneyz))
        {
            Debug.Log($"Next Honeyz enum value: {nextHoneyz}");
            // ���⿡�� ���� Honeyz enum ������ ���ϴ� �۾� ����
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