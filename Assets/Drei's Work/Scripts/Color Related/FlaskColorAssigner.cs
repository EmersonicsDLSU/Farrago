using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlaskColorAssigner : MonoBehaviour
{
    public GameObject potionHolder;
    public ColorPropertyPool _colorPropertyPool;
    public string colorName;
    public Color colorProperty;
    private Color finalColor;
    private Color baseColor;

    // Start is called before the first frame update
    void Start()
    {
        assignColorToFlask(colorName);
    }

    // Update is called once per frame
    void Update()
    {
        assignColorToFlask(colorName);
    }

    void assignColorToFlask(string colorn)
    {
        switch(colorn)
        {
            case "red":
                baseColor = Color.red;
                finalColor = baseColor * Mathf.LinearToGammaSpace(1.5f);
                potionHolder.GetComponent<MeshRenderer>().material.color = baseColor;
                potionHolder.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", finalColor);
                potionHolder.GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
                GetComponent<Light>().color = baseColor;
                colorProperty = Color.red;
                break;

            case "yellow":
                baseColor = Color.yellow;
                finalColor = baseColor * Mathf.LinearToGammaSpace(1.5f);
                potionHolder.GetComponent<MeshRenderer>().material.color = baseColor;
                potionHolder.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", finalColor);
                potionHolder.GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
                colorProperty = Color.yellow;
                GetComponent<Light>().color = baseColor;
                break;
            case "blue":
                baseColor = Color.blue;
                finalColor = baseColor * Mathf.LinearToGammaSpace(1.5f);
                potionHolder.GetComponent<MeshRenderer>().material.color = baseColor;
                potionHolder.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", finalColor);
                potionHolder.GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
                colorProperty = Color.blue;
                GetComponent<Light>().color = baseColor;
                break;
        }
    }
}
