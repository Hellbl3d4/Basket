using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
                                                        //////////////////////// DESAFIOS ////////////////////////////////////
    [System.Serializable]
    public class DesafiosTxt
    {
        public int ondeEstou;
        public string desafioRim, desafioSwish, desafioSky;
        public int desafioInt1RimShot = 0, desafioInt2SwichShot = 0, desafioInt3SkyHook = 0;
        public int numeroJogadas;
    }

    public List<DesafiosTxt> desafiosList;

    void ListaAdd()
    {
        foreach (DesafiosTxt desaf in desafiosList)
        {
            if (desaf.ondeEstou == OndeEstou.instance.faseN)
            {
                UIManager.instance.desafio1.text = desaf.desafioRim;
                UIManager.instance.desafio2.text = desaf.desafioSwish;
                UIManager.instance.desafio3.text = desaf.desafioSky;

                desafioNum1RimShot = desaf.desafioInt1RimShot;
                desafioNum2SwishShot = desaf.desafioInt2SwichShot;
                desafioNum3SkyHook = desaf.desafioInt3SkyHook;

                UIManager.instance.desafio1Ap.text = desaf.desafioRim;
                UIManager.instance.desafio2Ap.text = desaf.desafioSwish;
                UIManager.instance.desafio3Ap.text = desaf.desafioSky;

                numJogadas = desaf.numeroJogadas;
                break;
            }
        }
    }

                                                    /////////////////////// GAMEMANAGER ///////////////////////////////////////
    public static GameManager instance;

    public bool bolaEmCena;
    public int numJogadas;
    public GameObject[] bolaPrefab;
    private Transform posDireita, posEsquerda, posCima, posBaixo;
    public bool jogoExecutando = true, win = false, lose = false;

    //Mão Bolinhas
    public GameObject mao, bolinhas;
    private Animator maoAnim, bolinhasAnim;
    public int PrimeiraVez;

    //Desafios
    public bool rimShot = false, swishShot = false, skyHook = false;
    public int desafioNum1RimShot, desafioNum2SwishShot, desafioNum3SkyHook;

    //Identifica Ponto
    public int Pontos = 0;

    //Salvar moedas
    public int moedasIntSave;

    //Tela desafios
    [SerializeField]
    private GameObject fundo, tela, telaWL;
    [SerializeField]
    private Animator animCont;

    public void LiberaContagem()
    {
        fundo.gameObject.SetActive(false);
        tela.gameObject.SetActive(false);
        telaWL.SetActive(false);

        animCont.Play("ContadorAnim");
    }

    private void Awake()
    {
        //Verificação de tutorial
        if(PlayerPrefs.HasKey("PrimeiraVez") == false)
        {
            PlayerPrefs.SetInt("PrimeiraVez", 0);
        }
        else
        {
            PrimeiraVez = PlayerPrefs.GetInt("PrimeiraVez");
        }

        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        SceneManager.sceneLoaded += Carrega;
    }

    void Start()
    {
        StartGame();
        ListaAdd();

        bolaEmCena = true;
        //PlayerPrefs.DeleteAll();
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            OndeEstou.instance.faseN++;
            SceneManager.LoadScene(OndeEstou.instance.faseN);
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(OndeEstou.instance.faseN);
        }

        //Vencer ou Perder
        if(OndeEstou.instance.faseN != 0 && OndeEstou.instance.faseN !=1 && OndeEstou.instance.faseN != 2)
        {
            if (numJogadas <= 0 && desafioNum1RimShot > 0 && desafioNum2SwishShot > 0 && desafioNum3SkyHook > 0)
            {
                YouLose();
            }
            else if(numJogadas <=0 && desafioNum1RimShot > 0 && desafioNum2SwishShot > 0)
            {
                YouLose();
            }
            else if(numJogadas <=0 && desafioNum1RimShot > 0 && desafioNum3SkyHook > 0)
            {
                YouLose();
            }
            else if(numJogadas <= 0 && desafioNum3SkyHook > 0 && desafioNum2SwishShot > 0)
            {
                YouLose();
            }
            else if(numJogadas <= 0 && desafioNum1RimShot > 0)
            {
                YouLose();
            }
            else if(numJogadas <= 0 && desafioNum2SwishShot > 0)
            {
                YouLose();
            }
            else if(numJogadas <= 0 && desafioNum3SkyHook > 0)
            {
                YouLose();
            }
            else if (numJogadas > 0 && desafioNum1RimShot <= 0 && desafioNum2SwishShot <= 0 && desafioNum3SkyHook <= 0)
            {
                YouWin();
                int temp = OndeEstou.instance.faseN;
                PlayerPrefs.SetInt("Level" + temp, 1);
            }
        }
    }

    void Carrega(Scene cena, LoadSceneMode modo)
    {
        if (OndeEstou.instance.faseN != 0 && OndeEstou.instance.faseN != 1 && OndeEstou.instance.faseN != 2)
        {
            StartGame();
            ListaAdd();

            posDireita = GameObject.FindWithTag("Direita_pos").GetComponent<Transform>();
            posEsquerda = GameObject.FindWithTag("Esquerda_pos").GetComponent<Transform>();
            posCima = GameObject.FindWithTag("Cima_pos").GetComponent<Transform>();
            posBaixo = GameObject.FindWithTag("Baixo_pos").GetComponent<Transform>();

            fundo = GameObject.FindWithTag("FundoC");
            tela = GameObject.FindWithTag("Teladesafio");
            animCont = GameObject.FindWithTag("Contador").GetComponent<Animator>();

            telaWL = GameObject.FindWithTag("TelaWL");

            maoAnim = GameObject.FindWithTag("Mao").GetComponent<Animator>();
            bolinhasAnim = GameObject.FindWithTag("Bolinhas").GetComponent<Animator>();

            PrimeiraVez = PlayerPrefs.GetInt("PrimeiraVez");

            if (PrimeiraVez == 0 || PrimeiraVez == 1)
            {
                PegaSpritesTutorial();

                if (PrimeiraVez == 1)
                {
                    Matador(mao.gameObject, bolinhas.gameObject);
                }
            }
        }
    }

    public void NascBolas()
    {
        Instantiate(bolaPrefab[UIManager.instance.aux], new Vector2(Random.Range(posEsquerda.position.x, posDireita.position.x), Random.Range(posCima.position.y, posBaixo.position.y)), Quaternion.identity);
        bolaEmCena = true;
    }

    public void DesligaTuto()
    {
        Matador(mao.gameObject, bolinhas.gameObject);
        PlayerPrefs.SetInt("PrimeiraVez",1);
    }

    void Matador(GameObject obj, GameObject obj2)
    {
        Destroy(obj);
        Destroy(obj2);
    }

    void PegaSpritesTutorial()
    {
        mao = GameObject.FindWithTag("Mao");
        bolinhas = GameObject.FindWithTag("Bolinhas");
    }

    void StartGame()
    {
        if (OndeEstou.instance.faseN != 0 && OndeEstou.instance.faseN != 1 &&OndeEstou.instance.faseN != 2)
        {
            jogoExecutando = false;
            Pontos = 0;
            win = false;
            lose = false;

            moedasIntSave = ScoreManager.Instance.LoadDados();
            UIManager.instance.moedasUI.text = moedasIntSave.ToString("c");

            //BTN OK
            UIManager.instance.entendiBtn.onClick.AddListener(LiberaContagem);
            //BTN JOGAR NOVAMENTE
            UIManager.instance.Novamentebtn.onClick.AddListener(Novamente);
            //BTN AVANCAR
            UIManager.instance.avancarBtn.onClick.AddListener(Avancar);
            //BTN VOLTAR
            UIManager.instance.voltarBtn.onClick.AddListener(Voltar);
        }
    }

    public void DesafioDeFase(int fase)
    {
        if(OndeEstou.instance.faseN == fase)
        {
            if(desafioNum1RimShot == 0)
            {
                UIManager.instance.desafio1T.isOn = true;
                Debug.Log("RimShot Completo");
            }

            if(desafioNum2SwishShot == 0)
            {
                UIManager.instance.desafio2T.isOn = true;
                Debug.Log("SwishShot Completo");
            }

            if(desafioNum3SkyHook == 0)
            {
                UIManager.instance.desafio3T.isOn = true;
                Debug.Log("SkyHook Completo");
            }

        }
    }

    void YouWin()
    {
        if(jogoExecutando == true)
        {
            win = true;
            jogoExecutando = false;
            Debug.Log("You Win");
            ScoreManager.Instance.SalvarDados (moedasIntSave);

            telaWL.SetActive(true);
            UIManager.instance.txtWL.text = "YOU WIN!";
        }
    }

    void YouLose()
    {
        lose = true;
        jogoExecutando = false;
        Debug.Log("You Lose");

        telaWL.SetActive(true);
        UIManager.instance.txtWL.text = "YOU LOSE!";
        UIManager.instance.avancarBtn.gameObject.SetActive(false);
    }

    void Novamente()
    {
        SceneManager.LoadScene(OndeEstou.instance.faseN);
    }

    void Avancar()
    {
        if (win == true)
        {
            int temp = OndeEstou.instance.faseN + 1;
            SceneManager.LoadScene(temp);
        }
    }

    void Voltar()
    {
        SceneManager.LoadScene("MenuFases");
    }

    public void PrimeriraJogada()
    {
        if(jogoExecutando == true && PrimeiraVez == 0)
        {
            if(mao != null && bolinhas != null)
            {
                maoAnim.Play("Mao");
                bolinhasAnim.Play("Bolinhas");
            }
        }
    }
}
