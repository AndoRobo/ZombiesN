using UnityEngine;
using UnityEngine.InputSystem;
public class MazeController : MonoBehaviour
{

    public GameObject maze;
    public float rotationSpeed = 5;
    private InputAction rotateInput;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rotateInput = InputSystem.actions.FindAction("Player/Move");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 rotation = rotateInput.ReadValue<Vector2>();
        Debug.Log("rotation x: " + rotation.x + "y" + rotation.y);
        maze.transform.Rotate(rotation.y * rotationSpeed*Time.deltaTime,0,-rotation.x * rotationSpeed*Time.deltaTime);
    }
}
