using UnityEngine;

public class Root_Activation : MonoBehaviour
{
    public GameObject startup;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // startup.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Bootup()
    {
        startup.SetActive(true);
    }
}
