using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using UnityEngine.Events;

public class ArdunioThink : MonoBehaviour
{





    public static ArdunioThink instance;


    
    SerialPort sp = new SerialPort("COM6", 9600);
    bool isStreaming = false;


    bool ledOn;

    float inputcd = 0f;


    public UnityEvent<float> OnWeightRecieved;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
   public void ChangeLedState(int state)
    {
        Debug.Log("L" + state);
        sp.WriteLine("L" + state);
        ledOn=state==1 ? true : false;
    }

    void SwitchLedState()
    {
        if (ledOn)
        {
            ChangeLedState(0);
        }
        else
        {
            ChangeLedState(1);
        }
    }
        void OpenConnection()
    {
        isStreaming = true;
        sp.ReadTimeout = 100;
        sp.Open();
        sp.DtrEnable = true;
    }

    void Start()
    {
        OpenConnection();
    }

    string counter = "0";
    string counterPing = "0";
    private void Update()
    {
       // SwitchLedState();
        if (isStreaming)
        {


            counter = ReadSerialPort(50);
            Debug.Log(ReadSerialPort(50));

            if (counter != null)
            {
                WeightRecieved(float.Parse(counter));
            }
         

            if (inputcd < Time.time && counter!=counterPing)
            {
                inputcd=Time.time+ 0.5f;
                counterPing = counter;
            }

            

        }
    }
    void OnDisable()
    {
        sp.Close();
    }

    public void WeightRecieved(float weight)
    {
        OnWeightRecieved?.Invoke(weight);
    }

    string ReadSerialPort(int timeout = 50)
    {
        string message;
        sp.ReadTimeout = timeout;
        try
        {
            message = sp.ReadLine();
            return message;
        }
        catch (System.Exception)
        {
            return null;
        }
    }
    }
