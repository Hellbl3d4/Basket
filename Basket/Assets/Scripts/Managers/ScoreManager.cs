using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SalvarDados(int moeda)
    {
        BinaryFormatter b = new BinaryFormatter();
        FileStream f = File.Create(Application.persistentDataPath + "/dadoscoinDatab.txt");

        SaveDados coin = new SaveDados();
        coin.moedas = moeda;

        b.Serialize(f, coin);
        f.Close();
    }

    public int LoadDados()
    {
        int moeda = 0;

        if(File.Exists(Application.persistentDataPath + "/dadoscoinDatab.txt"))
        {
            BinaryFormatter b = new BinaryFormatter();
            FileStream f = File.Open(Application.persistentDataPath + "/dadoscoinDatab.txt", FileMode.Open);

            SaveDados coin = (SaveDados)b.Deserialize(f);
            f.Close();

            moeda = coin.moedas;
        }

        return moeda;
    }

    public void PerdeMoedas(int moeda)
    {
        int tempMoeda;
        int novoVal;

        tempMoeda = LoadDados();

        novoVal = tempMoeda - moeda;

        SalvarDados(novoVal);
    }

    [Serializable]
    class SaveDados
    {
        public int moedas;
    }

}
