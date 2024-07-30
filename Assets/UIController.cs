using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public static UIController instance;
    public TextMeshProUGUI textWrn;
    public TMP_InputField X;
    public TMP_InputField Y;
    public TMP_InputField Z;

    public TMP_InputField Sv;
    public TMP_InputField Br;
    public TMP_InputField St;

    public Button pause;
    public Button rs;

    public Button recharge;
    public Button rech;

    private void Start()
    {
        instance = this;
    }

    public void ComprX(string c)
    {
        if (c != null)
        {
            Brain.Instance.SetX(c);
            X.enabled = false;
        }
        else
        {
            X.GetComponent<Image>().color = Color.red;
            X.enabled = true;
        }
    }
    
    public void ComprY(string c)
    {
        if (c != null)
        {
            Brain.Instance.SetY(c);
            Y.enabled = false;
        }
        else
        {
            Y.GetComponent<Image>().color = Color.red;
            Y.enabled = true;
        }
    }
    
    public void ComprZ(string c)
    {
        if (c != null)
        {
            Brain.Instance.SetZ(c);
            Z.enabled = false;
        }
        else
        {
            Z.GetComponent<Image>().color = Color.red;
            Z.enabled = true;
        }
    }
    
    public void ComprSv(string c)
    {
        if (c != null)
        {
            Brain.Instance.SetSurvival(c);
            Sv.enabled = false;
        }
        else
        {
            Sv.GetComponent<Image>().color = Color.red;
            Sv.enabled = true;
        }
    }
    
    public void ComprBr(string c)
    {
        if (c != null)
        {
            Brain.Instance.SetBirth(c);
            Br.enabled = false;
        }
        else
        {
            Br.GetComponent<Image>().color = Color.red;
            Br.enabled = true;
        }
    }
    
    public void ComprSt(string c)
    {
        if (c != null)
        {
            Brain.Instance.SetState(c);
            St.enabled = false;
        }
        else
        {
            St.GetComponent<Image>().color = Color.red;
            St.enabled = true;
        }
    }

    public void Clear()
    {
        textWrn.gameObject.SetActive(true);
        Br.enabled = true;
        Sv.enabled = true;
        St.enabled = true;
        X.enabled = true;
        Z.enabled = true;
        Y.enabled = true;
    }
    public void Play()
    {
        textWrn.gameObject.SetActive(false);
        Br.gameObject.transform.parent.gameObject.SetActive(false);
        Sv.gameObject.transform.parent.gameObject.SetActive(false);
        St.gameObject.transform.parent.gameObject.SetActive(false);
         X.gameObject.transform.parent.gameObject.SetActive(false);
         Z.gameObject.transform.parent.gameObject.SetActive(false);
         Y.gameObject.transform.parent.gameObject.SetActive(false);

        pause.gameObject.SetActive(true);
    }

    public void Reload()
    {
        SceneManager.LoadScene(0);
    }
    public void Dead()
    {
        recharge.gameObject.SetActive(false);
        rech.gameObject.SetActive(true);
    }

}
