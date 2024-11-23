using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEditor;
using Unity.VisualScripting;
using System;

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
    private ResearchSystem researchSystem;

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
        metaUI.Init(globalData);

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

        if(researchSystem == null)
        {
            researchSystem = gameObject.AddComponent<ResearchSystem>();
            researchSystem.Init(metaUI, globalData);
        }
        else
        {
            researchSystem.Init(metaUI, globalData);
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

        playerData.forceCur = 0;
        playerData.expCur = 0;
        playerData.expTotal = 0;
        playerData.completedObjects = 0;

        playerData.upgradeLevels["EnergyLimit"] = globalData.researchLevels["EnergyLimit"];
        playerData.upgradeLevels["EnergyRegeneration"] = globalData.researchLevels["EnergyRegeneration"];
        playerData.upgradeLevels["EnergySpend"] = globalData.researchLevels["EnergySpend"];
        playerData.upgradeLevels["ForceProduction"] = globalData.researchLevels["ForceProduction"];
        playerData.upgradeLevels["ForceGeneration"] = globalData.researchLevels["ForceGeneration"];
        playerData.upgradeLevels["ForceSpend"] = globalData.researchLevels["ForceSpend"];
        playerData.upgradeLevels["ProductionSpeed"] = globalData.researchLevels["ProductionSpeed"];
        playerData.upgradeLevels["ProductionCount"] = globalData.researchLevels["ProductionCount"];
        playerData.upgradeLevels["ExperienceMult"] = globalData.researchLevels["ExperienceMult"];

        playerData.energyMax = playerData.energyMaxBasic * (playerData.upgradeLevels["EnergyLimit"] + 1);
        playerData.energyCur = playerData.energyMax;
        playerData.energyReg = playerData.energyRegBasic + playerData.upgradeLevels["EnergyRegeneration"];

        if(playerData.upgradeLevels["EnergySpend"] > 0)
            playerData.energySpend = playerData.energySpendBasic * (float)Math.Pow(0.9f, playerData.upgradeLevels["EnergySpend"]);
        else 
            playerData.energySpend = playerData.energySpendBasic;
        
        if(playerData.upgradeLevels["ForceProduction"] > 0)
        {
            playerData.forceProd = playerData.forceProdBasic + playerData.upgradeLevels["ForceProduction"];
            playerData.workCost = (playerData.workCostBasic * playerData.forceProd) * playerData.energySpend;
        }
        


        if (playerData.upgradeLevels["ForceGeneration"] == 0)
            playerData.forceReg = 0;
        else if (playerData.upgradeLevels["ForceGeneration"] == 1)
            playerData.forceReg = playerData.forceRegBasic;
        else
            playerData.forceReg = playerData.forceRegBasic * (float)Math.Pow(0.9f, playerData.upgradeLevels["ForceGeneration"]);

        if (playerData.upgradeLevels["ForceSpend"] > 0)
            playerData.forceSpend = playerData.forceSpendBasic * (float)Math.Pow(0.9f, playerData.upgradeLevels["ForceSpend"]);
        else
            playerData.forceSpend = playerData.forceSpendBasic;

        

        if (playerData.upgradeLevels["ProductionSpeed"] > 0)
            playerData.productionTime = playerData.productionTimeBasic * (float)Math.Pow(0.9f, playerData.upgradeLevels["ProductionSpeed"]);
        else
            playerData.productionTime = playerData.productionTimeBasic;

        playerData.productionCount = playerData.productionCountBasic + playerData.upgradeLevels["ProductionCount"];

        if (playerData.upgradeLevels["ExperienceMult"] > 0)
            playerData.experienceMult = playerData.experienceMultBasic * (float)Math.Pow(1.2f, playerData.upgradeLevels["ExperienceMult"]);
        else
            playerData.experienceMult = playerData.experienceMultBasic;
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
