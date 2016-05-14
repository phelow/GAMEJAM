using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class DialogSystem : MonoBehaviour {
	public enum LevelEnum{
		Day1,
		Night1,
		Day2,
		Night2,
		Day3,
		Night3,
		Day4,
		Night4,
		Day5,
		Night5





	}
    //Game objects for each day and night dialogue
    [SerializeField]
    GameObject Day1;
    [SerializeField]
    GameObject Night1;
    [SerializeField]
    GameObject Day2;
    [SerializeField]
    GameObject Night2;
    [SerializeField]
    GameObject Day3;
    [SerializeField]
    GameObject Night3;
    [SerializeField]
    GameObject Day4;
    [SerializeField]
    GameObject Night4;
    [SerializeField]
    GameObject Day5;
    [SerializeField]
    GameObject Night5;


    public static DialogSystem Main;

    void Awake()
    {
        Main = this.GetComponent<DialogSystem>();
    }


    public void SetLevel(LevelEnum _level)
    {
        switch (_level)
        {
            case (LevelEnum.Day1):
                Day1.gameObject.SetActive(true);
                break;
            case (LevelEnum.Night1):
                Day1.gameObject.SetActive(false);
                Night1.gameObject.SetActive(true);
                break;
            case (LevelEnum.Day2):
                Night1.gameObject.SetActive(false);
                Day2.gameObject.SetActive(true);
                break;
            case (LevelEnum.Night2):
                Day2.gameObject.SetActive(false);
                Night2.gameObject.SetActive(true);
                break;
            case (LevelEnum.Day3):
                Night2.gameObject.SetActive(false);
                Day3.gameObject.SetActive(true);
                break;
            case (LevelEnum.Night3):
                Day3.gameObject.SetActive(false);
                Night3.gameObject.SetActive(true);
                break;
            case (LevelEnum.Day4):
                Night3.gameObject.SetActive(false);
                Day4.gameObject.SetActive(true);
                break;
            case (LevelEnum.Night4):
                Day4.gameObject.SetActive(false);
                Night4.gameObject.SetActive(true);
                break;
            case (LevelEnum.Day5):
                Night4.gameObject.SetActive(false);
                Day5.gameObject.SetActive(true);
                break;
            case (LevelEnum.Night5):
                Day5.gameObject.SetActive(false);
                Night5.gameObject.SetActive(true);
                break;
        }
    }
   
}
