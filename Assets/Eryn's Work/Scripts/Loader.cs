using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Loader : MonoBehaviour
{
    [SerializeField]
    public GameObject loadingPanel;
    public Slider loadingProgress;
    public Text loaderText;
    public Image loadingBG;
    public List<Sprite> loadingImages;

    public static Loader loadinstance;

    private void Awake()
    {
        if (loadinstance != null && loadinstance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        loadinstance = this;

    }

    public void LoadLevel(int SceneIndex)
    {
        Time.timeScale = 1;
        Debug.Log("Click");
        StartCoroutine(loadAsynchronously(SceneIndex));
    }

    IEnumerator loadAsynchronously (int sceneIndex)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        loadingBG.sprite = loadingImages[(int)(Random.Range(0, loadingImages.Count))];
        loadingPanel.SetActive(true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            loadingProgress.value = progress;
            loaderText.text = Mathf.Round(progress * 100) + "%";
            Debug.Log(loaderText.text);

            yield return null;
        }
    }
}
