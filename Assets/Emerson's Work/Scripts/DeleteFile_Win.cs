using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteFile_Win : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void DeleteFileAfterWin()
    {
        DataPersistenceManager.instance.DeleteProfileData();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
