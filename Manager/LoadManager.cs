using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
public class LoadManager : SingletonBase<LoadManager>
{

    // --- 게임 데이터 파일이름 설정 ---
    public string GameDataFileName = "GameData.json";

    private Player thePlayer;


    // "원하는 이름(영문).json"
    public GameData _gameData;
    public GameData gameData
    {
        get
        {
            // 게임이 시작되면 자동으로 실행되도록
            if (_gameData == null)
            {
                LoadGameData();
                SaveGameData();
            }
            return _gameData;
        }
    }

    private void Start()
    {
        thePlayer = FindObjectOfType<Player>();
        LoadGameData();
        SaveGameData();
    }

    // 저장된 게임 불러오기
    public void LoadGameData()
    {
        string filePath = Application.persistentDataPath + GameDataFileName;

        // 저장된 게임이 있다면
        if (File.Exists(filePath))
        {
      
            print("불러오기 성공");
            string FromJsonData = File.ReadAllText(filePath);
            _gameData = JsonUtility.FromJson<GameData>(FromJsonData);
            thePlayer.transform.position = gameData.playerPos;
            thePlayer.transform.eulerAngles = gameData.playerRot;
        }

        // 저장된 게임이 없다면
        else
        {
            print("새로운 파일 생성");
            _gameData = new GameData();
        }
    }

    // 게임 저장하기
    public void SaveGameData()
    {

        // 올바르게 저장됐는지 확인 (자유롭게 변형)
        gameData.playerPos = thePlayer.transform.position;
        gameData.playerRot = thePlayer.transform.rotation.eulerAngles;
        string ToJsonData = JsonUtility.ToJson(gameData);
        string filePath = Application.persistentDataPath + GameDataFileName;

  

        // 이미 저장된 파일이 있다면 덮어쓰기
        File.WriteAllText(filePath, ToJsonData);
    }

    // 게임을 종료하면 자동저장되도록
    private void OnApplicationQuit()
    {
        SaveGameData();
    }
}