using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class TriggerPassthrough : MonoBehaviour
{
   [SerializeField] private storyManager sM;
   [SerializeField] private string storyID;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        sM.collisionTag = storyID;
        sM.OnTriggerEnter2DGateway(collision);
    }


}
