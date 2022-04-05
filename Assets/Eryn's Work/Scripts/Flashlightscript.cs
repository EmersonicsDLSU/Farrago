using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flashlightscript : MonoBehaviour
{
    bool isFlickering = false;
    bool startFlicker = false;
    float flickerDelay = 0;
    float delay = 0;
    private Renderer parentMaterial = null;


    void Start()
    {
        this.GetComponent<Light>().enabled = true;
        this.parentMaterial = this.transform.parent.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (delay > 3.0f)
        {
            if (!isFlickering && delay > 3.0f)
            {
                StartCoroutine(FlickeringLight());
            }
            if (delay > 4.5f)
                delay = 0;
        }
    }

    IEnumerator FlickeringLight()
    {
        isFlickering = true;
        this.gameObject.GetComponent<Light>().enabled = false;
        this.parentMaterial.material.SetVector("_EmissionColor", new Vector4(1.0f,1.0f,1.0f, 1f) * 1f);
        flickerDelay = Random.Range(0.01f, 0.2f);
        yield return new WaitForSeconds(flickerDelay);
        this.gameObject.GetComponent<Light>().enabled = true;
        this.parentMaterial.material.SetVector("_EmissionColor", new Vector4(1.0f,1.0f,1.0f, 1f) * 5f);
        flickerDelay = Random.Range(0.01f, 0.2f);
        yield return new WaitForSeconds(flickerDelay);
        isFlickering = false;
    }

    void FixedUpdate()
    {
            delay += Time.deltaTime;
    }
}
