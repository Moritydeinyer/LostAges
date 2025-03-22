using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using HeneGames.DialogueSystem;
using System.Globalization;

namespace Cainos.PixelArtTopDown_Basic
{
    public class TopDownCharacterController : MonoBehaviour
    {
        public float speed;
        public float sprint = 1;

        private Gamepad pad;
        
        public float attackDamage = 0.1f;
        public float damageRange = 10;

        public bool act = false;
        [SerializeField] public Image lifebar;
        [SerializeField] public Sprite lfb0;
        [SerializeField] public Sprite lfb1;
        [SerializeField] public Sprite lfb2;
        [SerializeField] public Sprite lfb3;
        [SerializeField] public Sprite lfb4;
        [SerializeField] public Transform demoEnemyTransform;
 
        private Animator animator;

        public float dashSpeed = 20f;
        public float dashDuration = 0.3f;
        public float dashCooldown = 1f;
        public bool allowDirectionChangeDuringDash = false;

        public ParticleSystem dashParticles;
        public AudioSource dashSound;
        public TrailRenderer trailRenderer;
        public bool hit = false;
        private Rigidbody2D rb;
        private Vector2 movementInput;
        public bool isDashing = false;
        private bool canDash = true;

        [SerializeField] private Camera minimapCam1;
        [SerializeField] public escMenuController escMC;
        [SerializeField] private DialogueUI ui;
        [SerializeField] private PlayerInput player_input;



        [Header("Map Input Settings")]
        [Header("Zoom Settings")]
        public float zoomSpeed = 5f;
        public float minZoom = 5f;
        public float maxZoom = 50f;

        [Header("Pan Settings")]
        public float panSpeed = 10f;

        [SerializeField] public Camera mapCamera;
        private Vector2 panInput;
        private float zoomInput;
        [Header("Waypoint Settings")]
        [SerializeField] private GameObject waypointPrefab; 
        [SerializeField] private Transform waypointParent;
        [SerializeField] private float baseScale = 5f;
        public List<GameObject> waypoints = new List<GameObject>();

        public float sensitivity = 0.5f;
        public float smoothFactor = 0.1f;
        public float screenEdgeBuffer = 100f;
        private Vector2 rightStickInput;
        private Vector3 targetMousePosition;
        private Vector3 currentMousePosition;
        private int direction;
        String dire = "South";

        


        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            if (trailRenderer != null)
            {
                trailRenderer.emitting = false;
            }
            animator = GetComponent<Animator>();
            currentMousePosition = Mouse.current.position.ReadValue();
            targetMousePosition = currentMousePosition;
            StartCoroutine(regenerateHealth());
        }

        private IEnumerator regenerateHealth()
        {
            while (true)
            {
                yield return new WaitForSeconds(1);
                if (escMC.gameData.health < 400)
                {
                    escMC.gameData.health += 1;
                }
            }
        }

        IEnumerator gameOverFadeOut()
        {
            escMC.gameOverScreenPanel.gameObject.SetActive(true);
            yield return new WaitForSeconds(1);
            while (escMC.gospTxt1.alpha > 0)
            {
                escMC.gospTxt1.alpha -= 0.02f;
                escMC.gospTxt2.alpha -= 0.02f;
                yield return new WaitForSeconds(0.001f);
            }
            escMC.gameOverScreenPanel.gameObject.SetActive(false);
        }
        IEnumerator respawnFadeOut()
        {
            escMC.resawnScreenPanel.gameObject.SetActive(true);
            yield return new WaitForSeconds(1);
            while (escMC.rpspTxt1.alpha > 0)
            {
                escMC.rpspTxt1.alpha -= 0.02f;
                escMC.rpspTxt2.alpha -= 0.02f;
                yield return new WaitForSeconds(0.001f);
            }
            escMC.resawnScreenPanel.gameObject.SetActive(false);
        }
        IEnumerator afterWorldFadeOut()
        {
            escMC.afterWorldScreenPanel.gameObject.SetActive(true);
            yield return new WaitForSeconds(1);
            while (escMC.gospTxt1.alpha > 0)
            {
                escMC.awspTxt1.alpha -= 0.02f;
                escMC.awspTxt2.alpha -= 0.02f;
                yield return new WaitForSeconds(0.001f);
            }
            escMC.afterWorldScreenPanel.gameObject.SetActive(false);
        }

        void Update()
        {
            if (escMC.gameData.health <= 0) 
            {
                if (!escMC.storyManager.checkStoryID("187"))                                    // DEBUG
                {
                    if (escMC.storyManager.checkStoryID("1008")) {escMC.storyManager.resetEndFight();}
                    if (escMC.storyManager.checkStoryID("1019")) {escMC.storyManager.resetEndFight();}
                    StartCoroutine(afterWorldFadeOut());
                    escMC.gameData.health = 100;
                    escMC.storyManager.addStoryID("187");                                       // DEBUG
                    escMC.playerTransform.transform.position = new Vector3(-34.4f, 55.1f, 0);   // DEBUG
                } else {
                    StartCoroutine(gameOverFadeOut());
                    escMC.quitSaveListener(false);
                    escMC.deleteGame(escMC.gameData.id, false);
                }
                lifebar.sprite = lfb0;
            }
            if (escMC.gameData.health <= 100 && escMC.gameData.health >= 1) {lifebar.sprite = lfb1;}
            if (escMC.gameData.health <= 200 && escMC.gameData.health >= 101) {lifebar.sprite = lfb2;}
            if (escMC.gameData.health <= 300 && escMC.gameData.health >= 201) {lifebar.sprite = lfb3;}
            if (escMC.gameData.health <= 400 && escMC.gameData.health >= 301) {lifebar.sprite = lfb4;}
            
            HandleZoom();
            if (escMC.mapPanel.activeSelf)
            {
                Cursor.visible = true;
                UpdateWaypointScale();
            }

            // DEBUG <start>
            if (Input.GetKeyDown(KeyCode.B))
            {
                if (animator.GetBool("swerd") == true)
                {
                    animator.SetBool("swerd", false);
                }
                else
                {
                    animator.SetBool("swerd", true);
                }
        }
            // DEBUG <end>
        }

        private void UpdateWaypointScale()
        {
            float zoomFactor = mapCamera.orthographicSize / 4f; 
            foreach (GameObject waypoint in waypoints)
            {
                if (waypoint != null)
                {
                    waypoint.transform.localScale = Vector3.one * zoomFactor;
                }
            }
        }

        private Vector3 GetWorldPositionFromMapClick(Vector2 screenPosition)
        {
            Ray ray = mapCamera.ScreenPointToRay(screenPosition);
            Plane groundPlane = new Plane(Vector3.forward, Vector3.zero);
            if (groundPlane.Raycast(ray, out float distance))
            {
                return ray.GetPoint(distance); 
            }
            return Vector3.zero;
        }

        public void DeleteWaypoint(GameObject waypoint)
        {
            waypoints.Remove(waypoint);
            Destroy(waypoint);
        }

        private GameObject GetWaypointAtPosition(Vector3 position)
        {
            foreach (GameObject waypoint in waypoints)
            {
                if (Vector3.Distance(waypoint.transform.position, position) < 1f) 
                {
                    return waypoint;
                }
            }
            return null;
        }

        public void OnZoom(InputAction.CallbackContext context)
        {
            zoomInput = context.ReadValue<float>();
        }


        private void HandleZoom()
        {
            if (zoomInput != 0f)
            {
                float newZoom = Mathf.Clamp(mapCamera.orthographicSize - zoomInput * zoomSpeed * Time.deltaTime, minZoom, maxZoom);
                mapCamera.orthographicSize = newZoom;
            }
        }


        private void HandlePan()
        {
            if (panInput != Vector2.zero)
            {
                Vector3 panMovement = new Vector3(panInput.x, panInput.y, 0f) * panSpeed * Time.deltaTime;
                mapCamera.transform.Translate(panMovement, Space.World);
            }
        }

        public void Dash(InputAction.CallbackContext context)
        {
            if (act) 
            {
                if (context.performed && canDash && !isDashing)
                {
                    escMC.storyManager.checkDashKampfTutorialEnemy = true;
                    StartCoroutine(Dash());
                }
            }
        }

        void FixedUpdate()
        {
            if (!isDashing)
            {
                rb.velocity = movementInput * speed * sprint;
            }
            if (escMC.mapPanel.activeSelf)
            {
                HandlePan();
            }

            if (hit) {animator.SetBool("dmg", true); StartCoroutine(hitAnimation()); hit = false;}
        }

        IEnumerator hitAnimation()
        {
            yield return new WaitForSeconds(0.5f);
            animator.SetBool("dmg", false);
        }

        private IEnumerator Dash()
        {
            isDashing = true;
            canDash = false;

            if (dashParticles != null) dashParticles.Play();
            if (dashSound != null) dashSound.Play();
            if (trailRenderer != null) trailRenderer.emitting = true;

            Vector2 dashDirection = movementInput == Vector2.zero ? Vector2.up : movementInput;

            float elapsedTime = 0f;
            while (elapsedTime < dashDuration)
            {
                if (allowDirectionChangeDuringDash)
                {
                    dashDirection = movementInput != Vector2.zero ? movementInput : dashDirection;
                }

                rb.velocity = dashDirection * dashSpeed;
                elapsedTime += Time.deltaTime;
                yield return null;
            }

            rb.velocity = Vector2.zero;

            if (trailRenderer != null) trailRenderer.emitting = false;
            isDashing = false;

            yield return new WaitForSeconds(dashCooldown);
            canDash = true;
        }

        public void Attack(InputAction.CallbackContext context)
        {
            if (act) 
            {
                attack();
                // DEBUG add delay
            }
            if (context.performed && escMC.mapPanel.activeSelf)
            {
                Vector2 mousePosition = Mouse.current.position.ReadValue();
                Vector3 worldPosition = GetWorldPositionFromMapClick(mousePosition);
                GameObject clickedWaypoint = GetWaypointAtPosition(worldPosition);
                if (clickedWaypoint != null)
                {
                    DeleteWaypoint(clickedWaypoint);
                }
                else
                {
                    CreateWaypoint(worldPosition);
                }
            }
        }


        public GameObject CreateWaypoint(Vector3 position)
        {
            GameObject newWaypoint = Instantiate(waypointPrefab, position, Quaternion.identity);
            newWaypoint.layer = LayerMask.NameToLayer("minimapOnly");
            if (waypointParent != null)
            {
                newWaypoint.transform.SetParent(waypointParent, true); 
            }
            waypoints.Add(newWaypoint);
            UpdateWaypointScale();
            return newWaypoint;
        }

        public void MinimapMinus(InputAction.CallbackContext context)
        {
            if (act) 
            {
                if (minimapCam1.orthographicSize <= 15)
                {
                    minimapCam1.orthographicSize += 1.075f;
                }
            }
            else if (escMC.mapPanel.activeSelf)
            {
                mapCamera.orthographicSize = Mathf.Clamp(mapCamera.orthographicSize - zoomSpeed * Time.deltaTime, minZoom, maxZoom);
            }
        }

        public void MinimapPlus(InputAction.CallbackContext context)
        {
            if (act) 
            {
                if (minimapCam1.orthographicSize >= 1)
                {
                    minimapCam1.orthographicSize -= 1.075f;
                }
            }
            else if (escMC.mapPanel.activeSelf)
            {
                mapCamera.orthographicSize = Mathf.Clamp(mapCamera.orthographicSize + zoomSpeed * Time.deltaTime, minZoom, maxZoom);
            }
        }

        public void Move(InputAction.CallbackContext context)
        {
            if (act) 
            {
                if (!isDashing)
                {
                    movementInput = context.ReadValue<Vector2>();
                    UpdateAnimation();
                }
            }
            else if (escMC.mapPanel.activeSelf) {panInput = context.ReadValue<Vector2>();}
        }

        private void UpdateAnimation()
        {
            Vector2 dir = movementInput;
            if (dir.x < 0)
            {
                animator.SetBool("west", true);
                animator.SetBool("east", false);
                dire = "West";
            }
            else if (dir.x > 0)
            {
                animator.SetBool("east", true);
                animator.SetBool("west", false);
                dire = "East";
            }
            if (dir.y > 0)
            {
                animator.SetBool("noth", true);
                animator.SetBool("south", false);
                dire = "North";
            }
            else if (dir.y < 0)
            {
                animator.SetBool("south", true);
                animator.SetBool("noth", false);
                dire = "South";
            } 
            if (dir.x == 0 && dir.y == 0)
            {
                animator.SetBool("idle", true);
                animator.SetBool("west", false);
                animator.SetBool("east", false);
                animator.SetBool("noth", false);
                animator.SetBool("south", false);
                dire = "South";
            }
        }

        public void Talk(InputAction.CallbackContext context)
        {
            if (act) 
            {
                ui.talk = true;
            }
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Item") && collision.gameObject.activeSelf)
            {
                collision.gameObject.SetActive(false);

                if (collision.gameObject.name.Contains("KugelDEBUG")  && escMC.storyManager.checkStoryIDgraterThan("1001") && escMC.storyManager.checkStoryIDsmallerThan("1004"))
                {
                    escMC.gameData.story_id = "" + (int.Parse(escMC.gameData.story_id) + 1);
                }
                if (collision.gameObject.name.Contains("PortalZEUS"))
                {
                    escMC.storyManager.kampfTutorialEnemy.player.position = new Vector3(-57.38f, 55.7f, 0);
                    escMC.gameData.story_id = "1007";
                }
                if (collision.gameObject.name.Contains("PortalAfterworld"))
                {
                    escMC.storyManager.kampfTutorialEnemy.player.position = new Vector3(-34.4f, 55.1f, 0);
                    escMC.gameData.story_id = "1015";
                }
                if (collision.gameObject.name.Contains("TaschenuhrAfterworld"))
                {
                    escMC.gameData.story_id = "1017";
                }
                // DEBUG change story ID
                // escMC.gameData.story_id = int.Parse(collision.gameObject.name);
            }
        }

        IEnumerator attackAnimation()
        {
            String tempDir = dire;
            animator.SetBool("attack"+tempDir, true);
            animator.SetBool("attack", true);
            Debug.Log("Attack"+dire);
            yield return new WaitForSeconds(0.25f);
            animator.SetBool("attack"+tempDir, false);
            animator.SetBool("attack", false);
        }

        private void attack()
        {
            Debug.Log("Attack");
        StartCoroutine(attackAnimation());
        Enemy[] allEnemies = FindObjectsOfType<Enemy>();
        foreach (var enemy in allEnemies)
        {
            if (enemy.active)
                {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy <= damageRange)
                {
                    enemy.health -= attackDamage;
                    enemy.hit = true;  
                    if (enemy.health <= 0)
                    {
                        if (!(escMC.storyManager.checkStoryID("11") || escMC.storyManager.checkStoryID("10")))
                        {
                            if (!escMC.storyManager.checkStoryID("187"))
                            {
                                if (escMC.storyManager.checkStoryID("1019"))
                                {
                                    enemy.active = false;
                                    Debug.Log("DEBUG Sieg über AfterworldEnemy0");
                                } 
                                else 
                                {
                                    if (!escMC.gameData.story_id.Contains("demo"))
                                    {
                                        Destroy(enemy.gameObject);
                                    }
                                    else
                                    {
                                        enemy.health = enemy.maxHealth;
                                        enemy.transform.position = new Vector3(32.55f, -7.09f, 0);
                                    }
                                }
                            } 
                            else 
                            {
                                enemy.health = enemy.maxHealth;
                            } 
                        }
                    }     
                    if (escMC.storyManager.checkStoryID("187"))                                   // DEBUG
                    {
                        if (enemy.health <= 0)
                        {
                                                                                                    //DEBUG
                            StartCoroutine(respawnFadeOut());
                            escMC.gameData.health = 400;
                            escMC.storyManager.rmStoryID("187");
                            float x = float.Parse(escMC.gameData.respw.Split(';')[0].Replace(",", "."), CultureInfo.InvariantCulture);
                            float y = float.Parse(escMC.gameData.respw.Split(';')[1].Replace(",", "."), CultureInfo.InvariantCulture);                                               // DEBUG
                            escMC.playerTransform.transform.position = new Vector3(x, y, 0);          // DEBUG
                        }
                    }
                }
            }
        }
        }
        

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.magenta;
            Gizmos.DrawWireSphere(transform.position, damageRange);
        }
    }
}
