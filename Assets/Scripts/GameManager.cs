using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEditor;
using Unity.VisualScripting;

public class GameManager : MonoBehaviour
{
    private static GameManager Instance;

    [SerializeField]
    private GlobalData globalData;

    [SerializeField]
    private PlayerData playerData;

    [SerializeField]
    private ContractData contractData;

    [SerializeField]
    private ContractManager contractManager;

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
    private MetaUI metaUI;

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
            SceneManager.activeSceneChanged += SceneCheck;
            Debug.Log("InstanceCreate");
        }
        else
        {
            Destroy(gameObject);
            Debug.Log("Gameobject destoed " + gameObject.name);
        }
    }

    private void MetaInit()
    {
        metaUI = FindObjectOfType<MetaUI>();
        metaUI.Init(globalData.money, globalData.totalExperience);

        if (contractManager == null)
        {
            contractManager = gameObject.AddComponent<ContractManager>();
            contractManager.OnContractSelected += LoadCoreScene;
            contractManager.Init(metaUI, globalData);
        }
        else
        {
            contractManager.Init(metaUI, globalData);
        }
    }

    private void CoreInit()
    {
        Quark quark = quarkPrefab.GetComponent<Quark>();

        //LoadData();

        PlayerDataInit();

        playerController = gameObject.AddComponent<PlayerController>();
        objectScheme = gameObject.AddComponent<ObjectScheme>();
        objectCreator = gameObject.AddComponent<ObjectCreator>();
        cameraController = gameObject.AddComponent<CameraController>();
        upgradeSystem = gameObject.AddComponent<UpgradeSystem>();
        coreUI = FindObjectOfType<CoreUI>();

        coreUI.Init(playerController, playerData, objectCreator, upgradeSystem);
        objectScheme.Init(quarkPrefab, productSizeX, productSizeY, productSizeZ); //переработать схему. Должны быть схемы на выбор.
        playerController.Init(playerData, objectCreator, objectScheme, contractData);
        objectCreator.Init(playerController, objectScheme, coreUI, quarkPrefab);
        upgradeSystem.Init(playerData, objectCreator, coreUI);
        cameraController.Init(Camera.main, new Vector3(productSizeX/2 * quark.size, productSizeY/2 * quark.size, productSizeZ/2 * quark.size));//Откорректировать фокус камеры. Добавить управление камерой (вращение вокруг объекта, приближение/отдаление).

        playerController.OnContractCompleated += LoadMetaScene;
        
        Debug.Log("CoreInit");
    }

    private void PlayerDataInit()
    {
        playerData = new PlayerData();
        playerData.energyLimitUpgradeLevel = globalData.energyLimitResearchLevel;
        playerData.energyRegenerationUpgradeLevel = globalData.energyRegenResearchLevel;
        playerData.energySpendUpgradeLevel = globalData.energySpendResearchLevel;
        playerData.forceProductionUpgradeLevel = globalData.forceProductionResearchLevel;
        playerData.forceGenerationUpgradeLevel = globalData.forceAutogenResearchLevel;
        playerData.forceSpendUpgradeLevel = globalData.forceSpendResearchLevel;
        playerData.productionTimeUpgradeLevel = globalData.productionSpeedResearchLevel;
        playerData.productionCountUpgradeLevel = globalData.productionCountResearchLevel;
        playerData.expIncomeUpgradeLevel = globalData.experienceMultResearchLevel;
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
    public void LoadCoreScene(ContractData currentContract)
    {
        Destroy(metaUI);

        contractData = currentContract;
        SceneManager.LoadScene("CoreGamePlayScene");
    }

    public void LoadMetaScene(float exp)
    {

        globalData.totalExperience += exp;
        globalData.money += contractData.reward;

        Destroy(playerController);
        Destroy(objectScheme);
        Destroy(objectCreator);
        Destroy(cameraController);
        Destroy(upgradeSystem);
        Destroy(coreUI);



        /*
        playerController = null;
        objectScheme = null;
        objectCreator = null;
        cameraController = null;
        upgradeSystem = null;
        coreUI = null;
        */

        SceneManager.LoadScene("MetaGamePlayScene");
    }
    private void SceneCheck(Scene current, Scene next)
    {
        if (SceneManager.GetActiveScene().name == "CoreGamePlayScene")
        {
            CoreInit();
            Debug.LogWarning("This scene is a CoreGamePlayScene" + gameObject.name);

        }
        else if (SceneManager.GetActiveScene().name == "MetaGamePlayScene")
        {
            MetaInit();
            Debug.LogWarning("This scene is a MetaGamePlayScene" + gameObject.name);
        }
    }
}
