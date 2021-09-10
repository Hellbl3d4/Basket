using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [System.Serializable]
    public class Level
    {
        public string levelText;
        public bool habilitado;
        public int desbloqueado;
        public bool txtAtivo;
    }

    public GameObject botao;
    public Transform localBtn;
    public List<Level> levelList;

    void ListaAdd()
    {
        foreach (Level level in levelList)
        {
            GameObject btnnovo = Instantiate(botao) as GameObject;
            BotaoLevel btnNew = btnnovo.GetComponent<BotaoLevel>();
            btnNew.leveltxtBtn.text = level.levelText;

            if (PlayerPrefs.GetInt("Level" + btnNew.leveltxtBtn.text) == 1)
            {
                level.desbloqueado = 1;
                level.habilitado = true;
                level.txtAtivo = true;
            }

            btnNew.desbloqueadoBtn = level.desbloqueado;
            btnNew.GetComponent<Button>().interactable = level.habilitado;
            btnNew.GetComponentInChildren<Text>().enabled = level.txtAtivo;
            btnNew.GetComponent<Button>().onClick.AddListener(() => ClickLevel("Level" + btnNew.leveltxtBtn.text));

            btnnovo.transform.SetParent(localBtn, false);
        }
    }

    void ClickLevel(string level)
    {
        SceneManager.LoadScene(level);
    }

    private void Start()
    {
        ListaAdd();
    }
}
