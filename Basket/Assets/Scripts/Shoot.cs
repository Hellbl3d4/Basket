using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Shoot : MonoBehaviour
{
    private float forca = 2.0f;

    private Vector2 startPos;
    private bool tiro = false;
    private bool mirando = false;

    [SerializeField]
    private GameObject dotsGO;
    private List<GameObject> caminho;

    [SerializeField]
    private Rigidbody2D myRBody;
    [SerializeField]
    private Collider2D myCollider;

    //Variaveis aux
    [SerializeField]
    private float valorX, valorY;

    //Tipo de Jogada
    private bool bateuAro = false;
    private bool bateuTabela = false;

    //Marcou ponto
    public static bool fezPonto;

    private bool liberaSky;
    private Animator animRim, animSiwsh, animSky;

    private void Start()
    {
        animRim = GameObject.FindWithTag("Rimtxt").GetComponent<Animator>();
        animSiwsh = GameObject.FindWithTag("SwishShotTxt").GetComponent<Animator>();
        animSky = GameObject.FindWithTag("SkyHookTxt").GetComponent<Animator>();

        liberaSky = false;
        fezPonto = false;
        dotsGO = GameObject.FindWithTag("Dots");
        myRBody.isKinematic = true;
        myCollider.enabled = false;
        startPos = transform.position;
        caminho = dotsGO.transform.Cast<Transform>().ToList().ConvertAll(t => t.gameObject);

        for(int i = 0; i < caminho.Count; i++)
        {
            caminho[i].GetComponent<Renderer>().enabled = false;
        }
    }

    private void FixedUpdate()
    {
        if(GameManager.instance.jogoExecutando == true)
        {
            Vector2 wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(wp, Vector2.zero);

            if(hit.collider == null)
            {
                if (!myRBody.gameObject.CompareTag("Bolaclone"))
                {
                    Mirando();
                }
            }
        }
    }

    private void Update()
    {
        if (GameManager.instance.jogoExecutando == true)
        {
            if (!myRBody.isKinematic)
            {
                // DESAFIOS
                if (bateuTabela == false)
                {
                    //RIMSHOT
                    if (bateuAro == true && fezPonto == true && liberaSky == false)
                    {
                        GameManager.instance.rimShot = true;
                        
                        fezPonto = false;
                        GameManager.instance.desafioNum1RimShot--;
                        GameManager.instance.DesafioDeFase(OndeEstou.instance.faseN);
                        //Animação
                        animRim.Play("RimShotAnim");
                    }
                    //SWISHSHOT
                    else if (fezPonto == true && liberaSky == false)
                    {
                        GameManager.instance.swishShot = true;
                        
                        fezPonto = false;
                        GameManager.instance.desafioNum2SwishShot--;
                        GameManager.instance.DesafioDeFase(OndeEstou.instance.faseN);
                        //Animação
                        animSiwsh.Play("SwishShotAnim");
                    }
                }

                //SKYHOOK
                if (liberaSky == true && fezPonto == true)
                {
                    GameManager.instance.skyHook = true;
                    
                    fezPonto = false;
                    GameManager.instance.desafioNum3SkyHook--;
                    GameManager.instance.DesafioDeFase(OndeEstou.instance.faseN);
                    //Animação
                    animSky.Play("SkyHookAnim");
                }
            }
        }
    }

    //Metodos
    void MostraCaminho()
    {
        for(int x = 0; x < caminho.Count; x++)
        {
            caminho[x].GetComponent<Renderer>().enabled = true;
        }
    }

    void EscodeCaminho()
    {
        for(int x = 0;x < caminho.Count; x++)
        {
            caminho[x].GetComponent<Renderer>().enabled = false;
        }
    }

    Vector2 PegaForca(Vector3 mouse)
    {
        return (new Vector2(startPos.x + valorX, startPos.y + valorY) - new Vector2(mouse.x, mouse.y)) * forca;
    }

    Vector2 CaminhoPonto(Vector2 posInicial, Vector2 velInicial, float tempo)
    {
        return posInicial + velInicial * tempo + 0.5f * Physics2D.gravity * tempo * tempo;
    }

    void CalculoCaminho()
    {
        Vector2 vel = PegaForca(Input.mousePosition) * Time.fixedDeltaTime / myRBody.mass;

        for(int x = 0; x < caminho.Count; x++)
        {
            caminho[x].GetComponent<Renderer>().enabled = true;

            float t = x / 20f;
            Vector3 point = CaminhoPonto(transform.position, vel, t);
            point.z = 1.0f;
            caminho[x].transform.position = point;
        }
    }

    void Mirando()
    {
        if(tiro == true)
             return;
        
        if(Input.GetMouseButton(0))
        {
            if(GameManager.instance.PrimeiraVez == 0)
            {
                GameManager.instance.DesligaTuto();
            }

            if(mirando == false)
            {
                mirando = true;
                startPos = Input.mousePosition;
                CalculoCaminho();
                MostraCaminho();
            }
            else
            {
                CalculoCaminho();
            }
        }
        else if(mirando == true && tiro == false)
        {
            myRBody.isKinematic = false;
            myCollider.enabled = true;
            tiro = true;
            mirando = false;
            myRBody.AddForce(PegaForca(Input.mousePosition));
            EscodeCaminho();
        }
    }

    void OnBecameInvisible()
    {
        SegueBola.alvoInvisivel = true;
    }

    void OnBecameVisible()
    {
        SegueBola.alvoInvisivel = false;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if(col.gameObject.CompareTag("Aro"))
        {
            bateuAro = true;
        }

        if(col.gameObject.CompareTag("Tabela"))
        {
            bateuTabela = true;
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if(col.gameObject.CompareTag("Sky"))
        {
            liberaSky = true;
        }
    }
}
