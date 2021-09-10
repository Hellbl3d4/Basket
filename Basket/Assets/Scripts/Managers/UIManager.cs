using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public Text numBolas;

    public Text desafio1, desafio2, desafio3;

    public Toggle desafio1T, desafio2T, desafio3T;

    public Text moedasUI;

    public Button entendiBtn;

    public Text desafio1Ap, desafio2Ap, desafio3Ap;

    //Painel Win e Lose
    public Text txtWL;
    public Button avancarBtn, voltarBtn, Novamentebtn;

    //Loja
    public List<int> Bolas;
    public Sprite[] imagemSp;
    public int aux = 0;
    public Button[] compraBtn;
    public Image menuImg;

    private Button sobe, desce;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
            //PlayerPrefs.DeleteAll();
        }
        else
        {
            Destroy(gameObject);
        }

        //Loja
        Bolas = new List<int>();
        Bolas.Add(0);

        if (!PlayerPrefs.HasKey("Bola0"))
        {
            PlayerPrefs.SetInt("Bola0", Bolas[0]);
            Debug.Log("Salvo");
        }

        for (int i = 1; i < PlayerPrefs.GetInt("list_Count"); i++)
        {
            Bolas.Add(PlayerPrefs.GetInt("Bola" + i));
        }

        if(OndeEstou.instance.faseN != 0 && OndeEstou.instance.faseN != 1)
        {
            menuImg = GameObject.FindWithTag("ImgBolaLoja").GetComponent<Image>();

            moedasUI = GameObject.FindWithTag("CoinNum").GetComponent<Text>();

            //DESAFIOS
            desafio1T = GameObject.FindWithTag("tog1").GetComponent<Toggle>();
            desafio2T = GameObject.FindWithTag("tog2").GetComponent<Toggle>();
            desafio3T = GameObject.FindWithTag("tog3").GetComponent<Toggle>();

            desafio1 = GameObject.FindWithTag("d1").GetComponent<Text>();
            desafio2 = GameObject.FindWithTag("d2").GetComponent<Text>();
            desafio3 = GameObject.FindWithTag("d3").GetComponent<Text>();
        }

        SceneManager.sceneLoaded += Carrega;
    }

    void Carrega(Scene cena, LoadSceneMode modo)
    {
        if(OndeEstou.instance.faseN != 0 && OndeEstou.instance.faseN != 1 && OndeEstou.instance.faseN != 2)
        {
            // PAINEL WIN E LOSE
            txtWL = GameObject.FindWithTag("TxtWL").GetComponent<Text>();
            avancarBtn = GameObject.FindWithTag("btnAvancar").GetComponent<Button>();

            voltarBtn = GameObject.FindWithTag("btnVoltar").GetComponent<Button>();
            Novamentebtn = GameObject.FindWithTag("btnNovamente").GetComponent<Button>();


            //BUTTON DA APRESENTAÇÃO DE DESAFIOS
            entendiBtn = GameObject.FindWithTag("btnEntendi").GetComponent<Button>();

            numBolas = GameObject.FindWithTag("numBolas").GetComponent<Text>();
            numBolas.text = GameManager.instance.numJogadas.ToString();

            //DESAFIOS
            desafio1T = GameObject.FindWithTag("tog1").GetComponent<Toggle>();
            desafio2T = GameObject.FindWithTag("tog2").GetComponent<Toggle>();
            desafio3T = GameObject.FindWithTag("tog3").GetComponent<Toggle>();

            desafio1 = GameObject.FindWithTag("d1").GetComponent<Text>();
            desafio2 = GameObject.FindWithTag("d2").GetComponent<Text>();
            desafio3 = GameObject.FindWithTag("d3").GetComponent<Text>();

            moedasUI = GameObject.FindWithTag("CoinNum").GetComponent<Text>();

            desafio1Ap = GameObject.FindWithTag("Desafio1Ap").GetComponent<Text>();
            desafio2Ap = GameObject.FindWithTag("Desafio2Ap").GetComponent<Text>();
            desafio3Ap = GameObject.FindWithTag("Desafio3Ap").GetComponent<Text>();

            //Loja
            menuImg = GameObject.FindWithTag("ImgBolaLoja").GetComponent<Image>();
            menuImg.sprite = imagemSp[PlayerPrefs.GetInt("Bola" + Bolas[0])];

            //Sobe e Desce btn
            sobe = GameObject.FindWithTag("btnCima").GetComponent<Button>();
            desce = GameObject.FindWithTag("btnBaixo").GetComponent<Button>();
            //Eventos
            sobe.onClick.AddListener(CimaBolas);
            desce.onClick.AddListener(BaixoBolas);
            aux = 0;
        }

        if(OndeEstou.instance.faseN == 2)
        {
            moedasUI = GameObject.FindWithTag("CoinNum").GetComponent<Text>();
            moedasUI.text = ScoreManager.Instance.LoadDados().ToString("c");
        }

        AtualizaBtnBola();
    }

    private void Start()
    {
        if(OndeEstou.instance.faseN != 0 && OndeEstou.instance.faseN != 1)
        {
            //Numero de Bolas
            numBolas.text = GameManager.instance.numJogadas.ToString();

            menuImg.sprite = imagemSp[PlayerPrefs.GetInt("Bola" + Bolas[0])];
        }

        //ScoreManager.Instance.SalvarDados(0);
       // PlayerPrefs.DeleteAll();
    }

    public void Compra(int ID)
    {
        if (ID == 1)
        {
            if (ScoreManager.Instance.LoadDados() >= 1000)
            {
                ChamaCompra(1);
                ScoreManager.Instance.PerdeMoedas(1000);
                moedasUI.text = ScoreManager.Instance.LoadDados().ToString("c");
            }
            else
            {
                Debug.Log("Sem dinheiro!");
            }
        }
        else if(ID == 2)
        {
            if (ScoreManager.Instance.LoadDados() >= 3000)
            {
                ChamaCompra(2);
                ScoreManager.Instance.PerdeMoedas(3000);
                moedasUI.text = ScoreManager.Instance.LoadDados().ToString("c");
            }
            else
            {
                Debug.Log("Sem Money!");
            }
        }
    }

    void ChamaCompra(int id)
    {
        Bolas.Add(id);

        PlayerPrefs.SetInt("list_Count", Bolas.Count);
        PlayerPrefs.SetInt("Bola" + id, id);
        compraBtn[id - 1].interactable = false;

        if(id != 2)
        {
            compraBtn[id].interactable = true;
        }

        if(Bolas.Contains(id))
        {
            compraBtn[id - 1].GetComponentInChildren<Text>().text = "Comprado";
            compraBtn[id - 1].GetComponentInChildren<Text>().color = new Color(0, 1, 0, 1);
        }
    }

    void AjustaBolasBtn(int x)
    {
        compraBtn[x].interactable = false;
        compraBtn[x].GetComponentInChildren<Text>().text = "Comprado!";
        compraBtn[x].GetComponentInChildren<Text>().color = new Color(0, 1, 0, 1);
    }

    void BaixoBolas()
    {
        if(aux < Bolas.Count -1)
        {
            aux++;
            menuImg.sprite = imagemSp[PlayerPrefs.GetInt("Bola" + aux)];
        }
    }

    void CimaBolas()
    {
        if(aux >= 1)
        {
            aux--;
            menuImg.sprite = imagemSp[PlayerPrefs.GetInt("Bola" + aux)];
        }
    }

    void AtualizaBtnBola()
    {
        if(OndeEstou.instance.faseN == 2)
        {
            compraBtn = new Button[2];
            compraBtn[0] = GameObject.FindWithTag("btnCompra1").GetComponent<Button>();
            compraBtn[1] = GameObject.FindWithTag("btnCompra2").GetComponent<Button>();

            compraBtn[0].onClick.AddListener(() => Compra(1));
            compraBtn[1].onClick.AddListener(() => Compra(2));

            if(Bolas.Contains(1))
            {
                AjustaBolasBtn(0);

                if(!Bolas.Contains(2))
                {
                    compraBtn[1].interactable = true;
                }
            }

            if(Bolas.Contains(2))
            {
                AjustaBolasBtn(1);
            }
        }
    }
}
