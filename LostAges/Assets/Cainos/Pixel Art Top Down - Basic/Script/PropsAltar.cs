using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Cainos.PixelArtTopDown_Basic
{

    public class PropsAltar : MonoBehaviour
    {
        public List<SpriteRenderer> runes;
        public float lerpSpeed;

        private Color curColor;
        private Color targetColor;

        [Header("Target Data")]

        public PropsAltar targetAltar;
        public bool isTargetAltar = false;

        [Header("Player Data")]

        public Transform playerTransform;
        public SpriteRenderer playerRenderer;
        public SpriteRenderer playerShadow;
        
        private void Awake()
        {
            targetColor = runes[0].color;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("enter");
            targetColor.a = 1.0f;
            if (isTargetAltar == false) 
            {
                targetAltar.isTargetAltar = true;
                Invoke("teleporter", 2);
            }
        }

        public void teleporter() 
        {
            float x = GetComponent<SpriteRenderer>().transform.position.x - playerTransform.position.x;
            float y = GetComponent<SpriteRenderer>().transform.position.y - playerTransform.position.y;

            playerTransform.position = new Vector3(targetAltar.gameObject.transform.position.x - x, targetAltar.gameObject.transform.position.y - y, 0);

            playerTransform.gameObject.layer = targetAltar.gameObject.layer;
            playerRenderer.sortingLayerID = targetAltar.GetComponent<SpriteRenderer>().sortingLayerID;
            playerShadow.sortingLayerID = targetAltar.GetComponent<SpriteRenderer>().sortingLayerID;
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            Debug.Log("exit");
            isTargetAltar = false;
            targetColor.a = 0.0f;
        }


        private void Update()
        {
            curColor = Color.Lerp(curColor, targetColor, lerpSpeed * Time.deltaTime);

            foreach (var r in runes)
            {
                r.color = curColor;
            }
        }
    }
}
