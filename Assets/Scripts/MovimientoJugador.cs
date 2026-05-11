using UnityEngine;

public class MovimientoJugador : MonoBehaviour
{
    [Header("Velocidad")]
    public float velocidad = 5f;
    public float sensibilidadMouse = 2f;

    private Rigidbody rb;
    private Camera camara;
    private float rotacionVertical = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        camara = GetComponentInChildren<Camera>();
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        MoverJugador();
        RotarConMouse();
    }

    void MoverJugador()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 direccion = transform.right * horizontal +
                           transform.forward * vertical;
        direccion = direccion.normalized * velocidad;
        direccion.y = rb.velocity.y;

        rb.velocity = direccion;
    }

    void RotarConMouse()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensibilidadMouse;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidadMouse;

        transform.Rotate(0, mouseX, 0);

        rotacionVertical -= mouseY;
        rotacionVertical = Mathf.Clamp(rotacionVertical, -80f, 80f);
        camara.transform.localRotation =
            Quaternion.Euler(rotacionVertical, 0, 0);
    }
}