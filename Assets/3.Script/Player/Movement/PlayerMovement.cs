using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float Movespeed = 5f;
    //[SerializeField] private float RotateSpeed = 180f;
    [SerializeField] private PlayerInput player_input;

    private Rigidbody player_r;
    private Animator player_ani;



    private void Start()
    {
        TryGetComponent(out player_input);
        TryGetComponent(out player_r);
        TryGetComponent(out player_ani);
    }

    private void FixedUpdate()
    {
        Move();
        Rotate();
        player_ani.SetFloat("Move", player_input.Move_Value);
    }

    private void Move()
    {
        Vector3 moveDirection =
            player_input.Move_Value * transform.forward * Movespeed * Time.deltaTime;

        player_r.MovePosition(player_r.position + moveDirection);
    }
    private void Rotate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Vector3 mouseDir = new Vector3(hit.point.x, transform.position.y, hit.point.z) - transform.position;
            transform.forward = mouseDir;
        }
    }

}
