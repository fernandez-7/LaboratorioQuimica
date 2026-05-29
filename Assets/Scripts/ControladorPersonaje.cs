using UnityEngine;

public class ControladorPersonaje : MonoBehaviour
{
    [Header("Movimiento")]
    public float velocidadCaminar = 3f;
    public float velocidadCorrer = 6f;

    [Header("Camara")]
    public float sensibilidadMouse = 2f;
    public Camera camaraPrimeraPersona;
    public Camera camaraTerceraPersona;

    [Header("Tercera Persona")]
    public Vector3 offsetTerceraPersona = new Vector3(0, 2f, -2f);

    private Animator animator;
    private CharacterController controller;
    private bool esTerceraPersona = false;
    private float rotacionVertical = 0f;

    void Start()
    {
        animator = GetComponentInChildren<Animator>();
        controller = GetComponent<CharacterController>();

        // Inicia en primera persona
        camaraPrimeraPersona.gameObject.SetActive(true);
        camaraTerceraPersona.gameObject.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        ManejarCambioVista();
        ManejarMovimiento();

        if (!esTerceraPersona)
            ManejarCamaraPrimeraPersona();
        else
            ManejarCamaraTerceraPersona();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }

    void ManejarCambioVista()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            esTerceraPersona = !esTerceraPersona;
            camaraPrimeraPersona.gameObject.SetActive(!esTerceraPersona);
            camaraTerceraPersona.gameObject.SetActive(esTerceraPersona);
        }
    }

    void ManejarMovimiento()
    {
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        bool corriendo = Input.GetKey(KeyCode.LeftShift);

        Vector3 direccion = transform.right * h + transform.forward * v;
        float velocidad = corriendo ? velocidadCorrer : velocidadCaminar;
        controller.Move(direccion * velocidad * Time.deltaTime);

        // Gravedad
        if (!controller.isGrounded)
            controller.Move(Vector3.down * 9.8f * Time.deltaTime);

        // Animaciones
        float speed = direccion.magnitude * velocidad;
        animator.SetFloat("Speed", speed);
        animator.SetBool("IsRunning", corriendo && direccion.magnitude > 0.1f);
    }

    void ManejarCamaraPrimeraPersona()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensibilidadMouse;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidadMouse;

        rotacionVertical -= mouseY;
        rotacionVertical = Mathf.Clamp(rotacionVertical, -80f, 80f);

        camaraPrimeraPersona.transform.localRotation =
            Quaternion.Euler(rotacionVertical, 0f, 0f);
        transform.Rotate(Vector3.up * mouseX);
    }

    void ManejarCamaraTerceraPersona()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensibilidadMouse;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidadMouse;

        rotacionVertical -= mouseY;
        rotacionVertical = Mathf.Clamp(rotacionVertical, -30f, 60f);

        transform.Rotate(Vector3.up * mouseX);

        // Posición cámara tercera persona
        Vector3 posDeseada = transform.position +
            transform.TransformDirection(offsetTerceraPersona);
        camaraTerceraPersona.transform.position = posDeseada;
        camaraTerceraPersona.transform.LookAt(
            transform.position + Vector3.up * 1.5f);
    }
}