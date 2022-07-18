using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CareerStatsHandler : MonoBehaviour, ICareerDataPersistence
{
    public static CareerStatsHandler instance { get; private set; }
    //Player properties:
    public CareerStats _careerProperty = new CareerStats();

    private void Awake() 
    {
        if (instance != null) 
        {
            Debug.LogError("Found more than one Career Stats Handler in the scene.");
        }
        instance = this;
        DontDestroyOnLoad(this);
    }

    // Start is called before the first frame update
    void Start()
    {
        _careerProperty.total_deaths = 0;
        _careerProperty.total_visit = 1;
        _careerProperty.total_time_played = 0;
    }

    // Update is called once per frame
    void Update()
    {
        _careerProperty.total_time_played += Time.deltaTime;
    }

    private int visit_count = 0;
    void OnApplicationQuit()
    {
        visit_count++;
        DataPersistenceManager.instance.SaveCareerGame();
    }
    
    public void LoadData(CareerData data)
    {
        _careerProperty.total_deaths = data.total_deaths;
        _careerProperty.total_visit = data.total_visit;
        _careerProperty.total_time_played = data.total_time_played;
    }
    
    public void SaveData(CareerData data)
    {
        data.total_deaths = _careerProperty.total_deaths;
        data.total_visit += visit_count;
        data.total_time_played = _careerProperty.total_time_played;
    }
}
