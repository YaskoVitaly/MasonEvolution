using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectScheme : MonoBehaviour
{
    private GameObject quark;
    private int sizeX = 0;
    private int sizeY = 0;
    private int sizeZ = 0;

    //private Quark[,,] quarks;

    public List<Quark> quarksList;

    public float quarkSize;
    private bool incomplited = true;

    private GameObject productScheme;
    
    public void Init(GameObject _quark, int _sizeX, int _sizeY, int _sizeZ)
    {
        sizeX = _sizeX;
        sizeY = _sizeY;
        sizeZ = _sizeZ;
        quark = _quark;
        //quarks = new Quark[sizeX, sizeY, sizeZ];
        quarksList = new List<Quark>();
        QuarkGen();
        Debug.Log("ObjectScheme init");
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
        int quarkX = 0;
        int quarkY = 0;
        int quarkZ = 0;
        Quark[,,] quarks = new Quark[sizeX, sizeY, sizeZ];
        productScheme = new GameObject("Product");
        Vector3 curPos = new Vector3();
        while (incomplited)
        {
            if (quarkX < sizeX && quarkZ < sizeZ && quarkY < sizeY)
            {
                Quark temp = Instantiate(quark, productScheme.transform).GetComponent<Quark>();
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
                        Debug.Log("Scheme completed: " + quarks.Length);
                        incomplited = false;
                    }
                }
            }
        }
    }
}
