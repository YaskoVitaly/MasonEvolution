using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;
using static UnityEditor.Experimental.AssetDatabaseExperimental.AssetDatabaseCounters;

public class ObjectScheme : MonoBehaviour
{
    private ObjectCreator _objectCreator;

    public int sizeX = 0;
    public int sizeY = 0;
    public int sizeZ = 0;

    public Quark[,,] quarks;

    public int quarkX = 0;
    public int quarkY = 0;
    public int quarkZ = 0;

    public List<Quark> quarksList;

    public float quarkSize;
    private bool incomplited = true;

    public GameObject productScheme;

    private void Awake()
    {
        
    }
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void Init(ObjectCreator objectCreator)
    {
        _objectCreator = objectCreator;
        quarks = new Quark[sizeX, sizeY, sizeZ];
        quarksList = new List<Quark>();
        QuarkGen();
    }
    private void QuarkInstantiate(Quark q, Vector3 pos)
    {
        q.Init(pos, 1);
        quarksList.Add(q);
    }
    public List<Quark> CurrentQuarks(int quarkCount, int currentQuark)
    {
        List<Quark> result = new List<Quark>();
        if(currentQuark + quarkCount < quarksList.Count)
        {
            for (int i = currentQuark; i < currentQuark + quarkCount; i++)
            {
                result.Add(quarksList[i]);
            }
        }
        else
        {
            for (int i = currentQuark; i < quarksList.Count; i++)
            {
                result.Add(quarksList[i]);
            }
            Debug.LogWarning("Last Quarks");
        }
        return result;
    }
    public void QuarkGen()
    {
        productScheme = new GameObject("Product");
        Vector3 curPos = new Vector3();
        while (incomplited)
        {
            if (quarkX < sizeX && quarkZ < sizeZ && quarkY < sizeY)
            {

                Quark temp = Instantiate(_objectCreator.quark, productScheme.transform).AddComponent<Quark>();
                quarks[quarkX, quarkY, quarkZ] = temp;
                curPos = new Vector3(quarkX * temp.size, quarkY * temp.size, quarkZ * temp.size);
                QuarkInstantiate(temp, curPos);
                quarkX++;
            }
            else
            {
                if (quarkZ < sizeZ)
                {
                    quarkX = 0;
                    quarkZ++;
                }
                else
                {
                    if (quarkY < sizeY)
                    {
                        quarkZ = 0;
                        quarkX = 0;
                        quarkY++;
                    }
                    else
                    {
                        Debug.Log("Scheme completed");
                        incomplited = false;
                    }
                }
            }
        }
    }
}
