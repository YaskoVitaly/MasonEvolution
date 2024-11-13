using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager Instance;

    [SerializeField]
    private PlayerData playerData;
    
    [SerializeField]
    private PlayerController playerController;
    
    [SerializeField]
    private ObjectScheme objectScheme;
    
    [SerializeField]
    private ObjectCreator objectCreator;

    [SerializeField]
    private CameraController cameraController;

    [SerializeField]
    private UpgradeSystem upgradeSystem;

    [SerializeField]
    private CoreUI coreUI;

    [SerializeField]
    private GameObject quarkPrefab;

    public int productSizeX;
    public int productSizeY;
    public int productSizeZ;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("InstanceCreate");
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("Gameobject destoed " + gameObject.name);
            SceneManager.activeSceneChanged += SceneCheck;
        }
    }

    private void CoreInitializeSystems()
    {
        Quark quark = quarkPrefab.GetComponent<Quark>();

        
        LoadData();
        //playerData = new PlayerData();
        playerController = gameObject.AddComponent<PlayerController>();
        objectScheme = gameObject.AddComponent<ObjectScheme>();
        objectCreator = gameObject.AddComponent<ObjectCreator>();
        cameraController = gameObject.AddComponent<CameraController>();
        upgradeSystem = gameObject.AddComponent<UpgradeSystem>();
        coreUI = FindObjectOfType<CoreUI>();

        coreUI.Init(playerController, playerData, objectCreator, upgradeSystem);
        objectScheme.Init(quarkPrefab, productSizeX, productSizeY, productSizeZ); //переработать схему. Должны быть схемы на выбор.
        playerController.Init(playerData, objectCreator, objectScheme);
        objectCreator.Init(playerController, objectScheme, coreUI, quarkPrefab);
        upgradeSystem.Init(playerData, objectCreator, coreUI);
        cameraController.Init(Camera.main, new Vector3(productSizeX/2 * quark.size, productSizeY/2 * quark.size, productSizeZ/2 * quark.size));//Откорректировать фокус камеры. Добавить управление камерой (вращение вокруг объекта, приближение/отдаление).
        Debug.Log("CoreInit");
    }

    public void SaveData()
    {
        string json = JsonUtility.ToJson(playerData);
        File.WriteAllText(Application.persistentDataPath + "/playerData.json", json);
    }

    public void LoadData()
    {
        string path = Application.persistentDataPath + "/playerData.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            playerData = JsonUtility.FromJson<PlayerData>(json);
            Debug.Log("PlayerData loaded");
        }
        else
        {
            playerData = new PlayerData();
            Debug.Log("PlayerData new");
        }
    }
    public void LoadCoreScene()
    {
        SceneManager.LoadScene("CoreGamePlayScene");
    }
    private void SceneCheck(Scene current, Scene next)
    {
        if (SceneManager.GetActiveScene().name == "CoreGamePlayScene")
        {
            CoreInitializeSystems();
        }
        else
        {
            Debug.LogWarning("This scene is not a CoreGmaPlayScene");
        }
    }
}
