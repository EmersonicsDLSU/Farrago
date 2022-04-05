using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//General Identification of the enemies type
public enum Enemy_Type { RAT = 0, SPIDER };

public class EnemyStatistics : MonoBehaviour
{
    [SerializeField] private Enemy_Type enemyType;
    public Enemy_Type EnemyType
    {
        get { return this.enemyType; }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
