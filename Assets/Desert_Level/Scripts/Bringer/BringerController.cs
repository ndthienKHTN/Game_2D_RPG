using Assets.Common.Scripts;
using Assets.Desert_Level.Scripts;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Common.Scripts;
namespace Assets.Desert_Level.Scripts
{
    public class BringerController : MonoBehaviour, IEnemyController
    {
        public int atk;
        public int maxHealth = 200;
        public int def;

        Rigidbody2D rigidbody2d;
        Animator animator;
        bool playerInCollision = false;
        bool attacking = true;
        EnemyUIHealthBar enemyUIHealthBar;

        public GameObject player;
        public GameObject deathSpellPrefabs;
        public GameObject cursorTeleport;
        public float distanceTeleport = 5.0f;
        float currentHealth;

        private List<GameObject> spawnedDeathSpells;
        public ParticleSystem sparkEffect;
        // Start is called before the first frame update
        void Start()
        {
            rigidbody2d = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            enemyUIHealthBar = GetComponentInChildren<EnemyUIHealthBar>();
            StartCoroutine(createDeathSpell());
            spawnedDeathSpells = new List<GameObject>();
            currentHealth = maxHealth;
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject == player)
            {
                Debug.Log("Player in collision with Bringer");
                playerInCollision = true;
                attacking = false;

                animator.SetBool("Tele", true);
                StartCoroutine(Teleport());
                // animator.SetBool("Attacking", true);
            }
        }

        private void OnTriggerStay2D(Collider2D collision)
        {
            if (collision.gameObject == player)
            {
                playerInCollision = true;
                attacking = false;

                //animator.SetBool("Tele", true);
                //StartCoroutine(Teleport());
            }
        }
        
        private IEnumerator createDeathSpell()
        {
           
            while (true)
            {
                if (attacking) // && !playerInCollision
                {
                    // animation attack
                    attacking = false;

                    yield return new WaitForSeconds(1.2f);
                    animator.SetBool("Idle", true);
                    Debug.Log("Creating Death Spell");
                    Vector3 gapDistance = Vector3.up * 1.45f;

                    Vector3 startPosition = transform.position;
                    Vector3 endPosition = player.transform.position;
                    Vector3 direction = (endPosition - startPosition).normalized;
                    float distance = Vector3.Distance(startPosition, endPosition);
                    float step = distance / 4;

                    for (int i = 1; i <= 3; i++)
                    {
                        Vector3 position = startPosition + direction * step * i;
                        GameObject spell = Instantiate(deathSpellPrefabs, position + gapDistance, Quaternion.identity);
                        spawnedDeathSpells.Add(spell);
                    }

                    GameObject spell2 = Instantiate(deathSpellPrefabs,  endPosition + gapDistance, Quaternion.identity);
                    spawnedDeathSpells.Add(spell2);

                    // animation spell attack
                    yield return new WaitForSeconds(1.2f);

                    //Destroy(spell2);
                    foreach (GameObject spell in spawnedDeathSpells)
                    {
                        Destroy(spell);
                    }

                    attacking = true;
                    animator.SetBool("Idle", false);
                } /*else if (playerInCollision)
                {
                    animator.SetBool("Tele", true);
                    StartCoroutine(Teleport());
                }*/
                else
                {
                    yield return new WaitForSeconds(1.2f);
                }
            }
        }

        private IEnumerator Teleport()
        {
            yield return new WaitForSeconds(1.2f);
            MoveToRandomPosition();
            animator.SetBool("Tele", false);

            attacking = false;
            yield return new WaitForSeconds(1.0f);
            attacking = true;
        }
        void MoveToRandomPosition()
        {
            Vector3 cursorPosition = cursorTeleport.transform.position;
            Vector3 currentPosition = transform.position;
            Vector3 direction = (cursorPosition - currentPosition).normalized;
            // Calculate the possible new positions
            Vector3[] possiblePositions = new Vector3[3];
            possiblePositions[0] = cursorPosition + direction * distanceTeleport; // 180 degrees
            possiblePositions[1] = cursorPosition + Quaternion.Euler(0, 0, 90) * direction * distanceTeleport; // 90 degrees
            possiblePositions[2] = cursorPosition + Quaternion.Euler(0, 0, 270) * direction * distanceTeleport; // 270 degrees

            // Choose a random position from the possible positions
            Vector3 newPosition = possiblePositions[Random.Range(0, possiblePositions.Length)];

            // Move the object to the new position
            transform.position = newPosition;
        }

        public int attack(GameObject player, int atk)
        {
            IPlayerController controller =  player.GetComponent<IPlayerController>();
            if (controller != null)
            {
                controller.beAttacked(null, atk);
                ChangeHealth(20);
            }
            return 0;
        }

        public int beAttacked(int atk)
        {
            ChangeHealth(-atk);
            return 0;
        }

        private void ChangeHealth(int amount)
        {
        
            currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);
            if (currentHealth <= 0)
            {
                StartCoroutine(DieAnimation());
            }
            Debug.Log("Bringer: " + currentHealth + "/" + maxHealth);
            enemyUIHealthBar.SetValue(currentHealth / (float)maxHealth);
        }

        private IEnumerator DieAnimation()
        {
            rigidbody2d.simulated = false;
            sparkEffect.Play();
            animator.SetBool("Idle", true);
            yield return new WaitForSeconds(1.5f);
            for (int i = spawnedDeathSpells.Count - 1; i >= 0; i--)
            {
                Destroy(spawnedDeathSpells[i]);
            }
            Destroy(gameObject);
        }

        public void DetectDeath()
        {
            return;
        }
    }
}