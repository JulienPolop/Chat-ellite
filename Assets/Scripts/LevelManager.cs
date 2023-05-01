using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance = null;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            SetUpMissingElement();
        }
        else
        {
            Debug.LogError("Two gameManager, this is a problem.");
            Destroy(this.gameObject);
        }
    }

    private void Start()
    {
        customers = FindObjectsOfType<Customer>().ToList();
        ChoseCustomerToFeed();
    }


    public PlayerController player;
    public CameraController cam;
    public AlienSpawner spawner;
    public UIManager ui_man;
    public MusicManager music;
    public TimerManager timer;
    public List<Customer> customers;

    public bool loose = false;

    
    private void SetUpMissingElement()
    {
        if(player == null)
        {
            player = FindObjectOfType<PlayerController>();
        }
        if (cam == null)
        {
            cam = FindObjectOfType<CameraController>();
        }
        if (spawner == null)
        {
            spawner = FindObjectOfType<AlienSpawner>();
        }
        if (ui_man == null)
        {
            ui_man = FindObjectOfType<UIManager>();
        }
        if (music == null)
        {
            music = FindObjectOfType<MusicManager>();
        }
        if (timer == null)
        {
            timer = FindObjectOfType<TimerManager>();
        }
    }

    public void ChoseCustomerToFeed()
    {
        Customer customer = customers[Random.Range(0, customers.Count -1)];
        customer.StartAlerte();
        customer.NeedToBeFed = true;
    }
}
