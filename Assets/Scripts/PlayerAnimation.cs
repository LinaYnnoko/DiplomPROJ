using System.Collections;
using UnityEngine;


public class PlayerAnimation : MonoBehaviour
{
    [Header("Character settings")]
    [SerializeField] int maxHealth = 100;
    [SerializeField] float maxStamina = 100f;
    public float staminaConsumption = .1f;
    public int damage = 50;


    [Header("Movement settings")]
    [SerializeField] float runSpeed = 7f;
    [SerializeField] float walkSpeed = 4f;
    [SerializeField] float carrySpeed = 2f;
    [SerializeField] float gravity = -9f;
    [SerializeField] float turnSmoothTime = 0.6f;

    [Header("Objects")]
    public GameObject pivot;
    [SerializeField] Transform characterCamera;
    [SerializeField] GameObject inventory;
    [SerializeField] GameObject bag;
    [SerializeField] GameObject journal;
    [SerializeField] Transform carryPivot;
    [SerializeField] NotificationManager notifyManager;
    [SerializeField] HealthManager healthManager;
    [SerializeField] StaminaController staminaController;

    public bool isMoveLocked = false;
    bool bagUsed = false;
    bool carrying = false;
    int currentHealth;
    float currentStamina;
    float movementSpeed;
    float turnSmoothVelocity;
    public MovementState state;

    Vector3 velocity;
    Vector3 direction;
    Animator anim;
    InventoryManager inventoryManager;
    GameObject carryingObject;
    GameObject collisionObject;
    CharacterController controller;

    private void Awake()
    {
        StartCutSceneManager.startGameEvent.AddListener(SpawnPlayer);
        gameObject.SetActive(false);
    }

    void Start()
    {
        currentHealth = maxHealth;
        currentStamina = maxStamina;
        healthManager.SetMaxHealth(currentHealth);
        staminaController.SetMaxStamina(currentStamina);
        anim = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();
        inventoryManager = GetComponent<InventoryManager>();

        inventory.SetActive(false);
        bag.SetActive(false);
    }
    void Update()
    {
        if (!isMoveLocked)
        {
            MovementSystem();
            MovementStateHandler();

            if (!controller.isGrounded)
            {
                velocity.y = gravity * Time.deltaTime;
                controller.Move(velocity);
            }
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            if (carryingObject != null)
            {
                carrying = false;
                anim.SetBool("carry", false);
                carryingObject.transform.SetParent(null);
                carryingObject.GetComponent<Rigidbody>().isKinematic = false;
                carryingObject.GetComponent<SphereCollider>().enabled = true;
                carryingObject.GetComponent<BoxCollider>().enabled = true;
                carryingObject = null;
            }
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (collisionObject != null)
            {
                if (collisionObject.CompareTag("item") && bagUsed)
                {
                    anim.Play("take");
                    notifyManager.Notify(collisionObject, true);
                    inventoryManager.AddItem(collisionObject.GetComponent<Item>().itemScriptableObject, collisionObject.GetComponent<Item>().amount);
                    Destroy(collisionObject, .7f);
                }
                else if (collisionObject.CompareTag("questItem") && bagUsed)
                {
                    notifyManager.Notify(collisionObject, true);
                    anim.Play("take");
                    Destroy(collisionObject, .7f);
                }
                else if (collisionObject.CompareTag("bag"))
                {
                    bagUsed = true;
                    notifyManager.Notify(collisionObject, true);
                    bag.SetActive(true);
                    inventory.SetActive(true);
                    Destroy(collisionObject);
                }
                else if (collisionObject.CompareTag("itemForTransfer") && !carrying)
                {
                    carrying = true;
                    anim.SetBool("carry", true);
                    anim.Play("take");
                    carryingObject = collisionObject;
                    StartCoroutine(ObjectIsCarrying());
                }
                else if (collisionObject.CompareTag("door"))
                {
                    collisionObject.GetComponent<OpenedDoor>().Open();
                    anim.Play("Opening");
                }
                else
                {
                    notifyManager.Notify(collisionObject, false);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Debug.Log("Сумочка активирована");
            bagUsed = true;
        }
    }

    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        healthManager.SetHealth(currentHealth);
        if (currentHealth <= 0)
        {
            Destroy(gameObject);
        }
    }

    public void Heal(int healCount)
    {
        if (currentHealth < maxHealth)
        {
            currentHealth += healCount;
            healthManager.SetHealth(currentHealth);
        }
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }
    }
    public void Stamina(float staminaCount)
    {
        if (currentStamina < maxStamina)
        {
            currentStamina += staminaCount;
            staminaController.SetStamina(currentStamina);
        }
        if (currentStamina > maxStamina)
        {
            currentStamina = maxStamina;
        }
    }

    private void MovementStateHandler()
    {
        if (Input.GetKey(KeyCode.LeftShift) && direction.magnitude >= 0.1f && !carrying)
        {
            state = MovementState.sprinting;
            movementSpeed = runSpeed;
            anim.SetBool("run", true);
            currentStamina -= staminaConsumption;
            staminaController.SetStamina(currentStamina);
            if (currentStamina <= 0)
            {
                anim.SetTrigger("isFalling");
            }
        }
        else if (direction.magnitude >= 0.1f && anim.GetBool("carry") == false)
        {
            state = MovementState.walking;
            movementSpeed = walkSpeed;
            StaminaRecovery();
            anim.SetBool("run", false);
            anim.SetBool("walk", true);
        }
        else if (carrying && carryingObject != null)
        {
            state = MovementState.carrying;
            carrying = true;
            movementSpeed = carrySpeed;
            anim.SetBool("walk", false);
            anim.SetBool("run", false);
        }
        else
        {
            state = MovementState.idle;
            movementSpeed = 0;
            StaminaRecovery();
            anim.SetBool("walk", false);
            anim.SetBool("run", false);
        }
    }

    void StaminaRecovery()
    {
        if (currentStamina != maxStamina)
        {
            currentStamina += staminaConsumption;
            staminaController.SetStamina(currentStamina);
        }
    }

    public enum MovementState
    {
        walking,
        sprinting,
        idle,
        carrying
    }

    private void MovementSystem()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        direction = new Vector3(horizontal, 0f, vertical).normalized;
        if (direction.magnitude >= 0.1f)
        {
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + characterCamera.eulerAngles.y;
            float agle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, agle, 0f);

            Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
            controller.Move(moveDir.normalized * movementSpeed * Time.deltaTime);
        }
    }

    private void SpawnPlayer()
    {
        gameObject.SetActive(true);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.GetComponent<Item>() != null || other.gameObject.CompareTag("itemForTransfer") || other.gameObject.CompareTag("bag") || other.gameObject.CompareTag("door") || other.gameObject.CompareTag("questItem") || other.gameObject.CompareTag("item"))
        {
            collisionObject = other.gameObject;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        collisionObject = null;
    }

    IEnumerator ObjectIsCarrying()
    {
        yield return new WaitForSeconds(0.8f);
        carryingObject.transform.SetParent(carryPivot);
        carryingObject.transform.position = carryPivot.position;
        if (carryingObject.GetComponent<Rigidbody>())
        {
            carryingObject.GetComponent<Rigidbody>().isKinematic = true;
        }
        if (carryingObject.GetComponent<SphereCollider>())
        {
            carryingObject.GetComponent<SphereCollider>().enabled = false;
        }
        if (carryingObject.GetComponent<BoxCollider>())
        {
            carryingObject.GetComponent<BoxCollider>().enabled = false;
        }
    }
}