using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class playerScript : MonoBehaviour
{
    private Vector2 moveInput;
    public float moveSpeed;
    public Rigidbody2D rb2d;
    public Animator playerAnim;

    public int direction;

    public float attackingCoolDown;

    public GameObject sword1;
    public GameObject bow1;
    public int arrowCount;
    public GameObject arrowPrefab;
    public int weaponInUse;

    public bool hurting;
    public GameObject playerSprite;
    public bool stillInEnemyRange;

    public int playerHealth;
    public Animator gameOver;
    public GameObject heart1;
    public GameObject heart2;
    public GameObject heart3;

    public TextMeshProUGUI inGameCoinText;
    public int coinCount;

    public TextMeshProUGUI inGameHealthPotionText;
    public int healthPotionCount;

    public TextMeshProUGUI inGameArrowText;

    public GameObject shopButtons;

    // Start is called before the first frame update
    void Start()
    {
        playerHealth = 3;
    }

    // Update is called once per frame
    void Update()
    {
        //////////////////////////

        //attacking cool down if
        if (attackingCoolDown <= 0 && playerHealth > 0)
        {
            rb2d.constraints = RigidbodyConstraints2D.None;
            rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;


            //movement
            moveInput.x = Input.GetAxisRaw("Horizontal");
            moveInput.y = Input.GetAxisRaw("Vertical");
            moveInput.Normalize();

            if (Mathf.Abs(moveInput.x) > Mathf.Abs(moveInput.y))
            {
                moveInput.y = 0;
                rb2d.velocity = moveInput * moveSpeed;
            }
            else
            {
                moveInput.x = 0;
                rb2d.velocity = moveInput * moveSpeed;
            }

            //walking anims
            if (moveInput.y < 0)
            {
                playerAnim.Play("playerWalkD");
                direction = 0;
            }
            else if (moveInput.x > 0)
            {
                playerAnim.Play("playerWalkR");
                direction = 1;
            }
            else if (moveInput.x < 0)
            {
                playerAnim.Play("playerWalkL");
                direction = 2;
            }
            else if (moveInput.y > 0)
            {
                playerAnim.Play("playerWalkU");
                direction = 3;
            }

            //idle anims
            if (moveInput.y == 0 && moveInput.x == 0)
            {
                if (direction == 0)
                {
                    playerAnim.Play("playerIdleD");
                }
                if (direction == 1)
                {
                    playerAnim.Play("playerIdleR");
                }
                if (direction == 2)
                {
                    playerAnim.Play("playerIdleL");
                }
                if (direction == 3)
                {
                    playerAnim.Play("playerIdleU");
                }
            }

            //attacking
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (direction == 0)
                {
                    playerAnim.Play("playerAttackD");
                    attackingCoolDown = 0.4f;
                }
                if (direction == 1)
                {
                    playerAnim.Play("playerAttackR");
                    attackingCoolDown = 0.4f;
                }
                if (direction == 2)
                {
                    playerAnim.Play("playerAttackL");
                    attackingCoolDown = 0.4f;
                }
                if (direction == 3)
                {
                    playerAnim.Play("playerAttackU");
                    attackingCoolDown = 0.4f;
                }

                //shooting arrows
                if (arrowCount > 0 && weaponInUse == 1)
                {
                    PlayerPrefs.SetInt("ArrowCount", arrowCount - 1);
                    if (direction == 0)
                    {
                        Instantiate(arrowPrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, 270)));
                    }
                    if (direction == 1)
                    {
                        Instantiate(arrowPrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, 0)));
                    }
                    if (direction == 2)
                    {
                        Instantiate(arrowPrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, 180)));
                    }
                    if (direction == 3)
                    {
                        Instantiate(arrowPrefab, transform.position, Quaternion.Euler(new Vector3(0, 0, 90)));
                    }
                }
            }
        }
        else
        {
            rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        //changing weapons
        if (Input.GetKey(KeyCode.Alpha1))
        {
            sword1.SetActive(true);
            bow1.SetActive(false);
            weaponInUse = 0;
        }
        if (Input.GetKey(KeyCode.Alpha2))
        {
            sword1.SetActive(false);
            bow1.SetActive(true);
            weaponInUse = 1;
        }
        if (Input.GetKeyDown(KeyCode.H))
        {
            if (healthPotionCount > 0 && playerHealth < 3)
            {
                playerHealth++;
                PlayerPrefs.SetInt("HealthPotionCount", healthPotionCount - 1);
            }
        }

        //attacking cool down timer
        if (attackingCoolDown > 0)
        {
            attackingCoolDown -= Time.deltaTime;
        }

        //loosing health
        if (playerHealth == 3)
        {
            heart1.SetActive(true);
            heart2.SetActive(true);
            heart3.SetActive(true);
        }
        if (playerHealth == 2)
        {
            heart1.SetActive(false);
            heart2.SetActive(true);
            heart3.SetActive(true);
        }
        if (playerHealth == 1)
        {
            heart1.SetActive(false);
            heart2.SetActive(false);
            heart3.SetActive(true);
        }
        if (playerHealth <= 0)
        {
            heart1.SetActive(false);
            heart2.SetActive(false);
            heart3.SetActive(false);
            gameOver.Play("gameOverAnim");
            gameObject.GetComponent<Animator>().speed = 0;
        }

        coinCount = PlayerPrefs.GetInt("ScoreCount");
        inGameCoinText.text = coinCount.ToString();

        arrowCount = PlayerPrefs.GetInt("ArrowCount");
        inGameArrowText.text = arrowCount.ToString();

        healthPotionCount = PlayerPrefs.GetInt("HealthPotionCount");
        inGameHealthPotionText.text = healthPotionCount.ToString();

        /////////////////////////
    }

    //enemy contanct / hurting
    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && hurting == false && playerHealth > 0)
        {
            playerSprite.GetComponent<SpriteRenderer>().color = Color.red;
            playerHealth--;
            StartCoroutine(whitecolor());
            if (playerHealth > 0)
            {
                transform.position = Vector2.MoveTowards(transform.position, collision.gameObject.transform.position, -70 * Time.deltaTime);
            }
            hurting = true;

        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Heart") && playerHealth < 3)
        {
            playerHealth++;
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Coin"))
        {
            PlayerPrefs.SetInt("ScoreCount", coinCount + 1);
            Destroy(collision.gameObject);
        }
        if (collision.gameObject.CompareTag("Shop"))
        {
            shopButtons.SetActive(true);
        }
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Shop"))
        {
            shopButtons.SetActive(false);
        }
    }

    IEnumerator whitecolor()
    {

        yield return new WaitForSeconds(2);
        if (playerHealth > 0)
        {
            playerSprite.GetComponent<SpriteRenderer>().color = Color.white;
        }
        hurting = false;
        GetComponent<BoxCollider2D>().enabled = false;
        GetComponent<BoxCollider2D>().enabled = true;

    }

    public void playAgain()
    {
        SceneManager.LoadScene(0);
    }

    public void mainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void buyArrow()
    {
        if (coinCount >= 5)
        {
            PlayerPrefs.SetInt("ScoreCount", coinCount - 5);
            PlayerPrefs.SetInt("ArrowCount", arrowCount + 1);
        }
    }
    public void buyHealthPotion()
    {
        if (coinCount >= 5)
        {
            PlayerPrefs.SetInt("ScoreCount", coinCount - 5);
            PlayerPrefs.SetInt("HealthPotionCount", healthPotionCount + 1);
        }
    }
}
