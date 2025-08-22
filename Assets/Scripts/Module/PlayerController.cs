using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem; // 新输入系统
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public PlayerInput playerInput; // 新输入系统引用
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
