using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    public GameObject losePanel;


    #region Singleton and Awake
    public static UIManager instance;
    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(this.gameObject);
        } else
        {
            instance = this;
        }
    }

    #endregion
    public void showLosePanel()
    {
        losePanel.SetActive(true);
    }

    public void hideLosePanel()
    {
        losePanel.SetActive(false);
    }
}
