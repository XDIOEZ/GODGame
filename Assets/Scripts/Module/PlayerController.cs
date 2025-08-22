using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem; // ������ϵͳ
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public PlayerInput playerInput; // ������ϵͳ����
    public InputActionAsset InputActions { get => playerInput.actions; set => playerInput.actions = value; }
    public Rigidbody2D rb;


    private void Awake()
    {
        rb = GetComponentInParent<Rigidbody2D>();
    }

    private void Update()
    {
    }
}
