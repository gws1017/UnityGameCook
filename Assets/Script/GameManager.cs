using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public Player player;
    public MonsterSpawner MobSpawner;
    public GameObject MenuPannel;
    public GameObject GamePannel;

    public Text Level;
    public RectTransform HPImg;
    public RectTransform ExpImg;


    public void GameStart()
    {
        MenuPannel.SetActive(false);
        GamePannel.SetActive(true);

        player.gameObject.SetActive(true);
        MobSpawner.gameObject.SetActive(true);
    }
    
    

    void LateUpdate()
    {
        Level.text = string.Format("{0}", player.Level);
        HPImg.localScale = new Vector3(player.CurHealth / player.MaxHealth,1,1);
        ExpImg.localScale = new Vector3(player.Exp / player.MaxExp,1,1);
    }
}
