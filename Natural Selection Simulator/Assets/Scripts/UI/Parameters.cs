using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Parameters : MonoBehaviour
{
    private InputField r_number;
    private InputField r_variability;
    private InputField r_speed;
    private InputField r_size;
    private InputField r_efficiency;
    private InputField r_fear_coefficient;
    private float[] r_attributes;

    public void R_number(InputField x) { r_number = x; Debug.Log(r_number.text); }
    public void R_variability(InputField x) { r_variability = x; }
    public void R_speed(InputField x) { r_speed = x; }
    public void R_size(InputField x) { r_size = x; }
    public void R_efficiency(InputField x) { r_efficiency = x; }
    public void R_fear_coefficient(InputField x) { r_fear_coefficient = x; }
    public float[] R_attributes() { return r_attributes; }


    private InputField t_number;
    private InputField t_variability;
    private InputField t_speed;
    private InputField t_size;
    private InputField t_efficiency;
    private float[] t_attributes;

    public void T_number(InputField x) { t_number = x; }
    public void T_variability(InputField x) { t_variability = x; }
    public void T_speed(InputField x) { t_speed = x; }
    public void T_size(InputField x) { t_size = x; }
    public void T_efficiency(InputField x) { t_efficiency = x; }
    public float[] T_attributes() { return t_attributes; }

    private Text InvalidMessage;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        InvalidMessage = GameObject.Find("InvalidMessage").GetComponent<Text>();
    }

    public void PressedStart()
    {
        try
        {
            try
            {
                int.Parse(r_number.text);
                int.Parse(t_number.text);
            }
            catch
            {
                 InvalidMessage.text = "\"Number\" must be a positive integer.";
            }

            r_attributes = new float[] { float.Parse(r_number.text), float.Parse(r_variability.text), float.Parse(r_speed.text),
            float.Parse(r_size.text), float.Parse(r_efficiency.text), float.Parse(r_fear_coefficient.text) };

            t_attributes = new float[] { float.Parse(t_number.text), float.Parse(t_variability.text), float.Parse(t_speed.text),
            float.Parse(t_size.text), float.Parse(r_efficiency.text)};

            SceneManager.LoadScene("Simulation", LoadSceneMode.Single);

            Debug.Log("Inputs Valid");

        }
        catch
        {
            Debug.Log("Invalid Argument Dumbass");
        }
    }

}
