using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{

    public GameObject selectedZombie;

    public GameObject[] zombies;

    private InputAction next, previous, push;
    private int selectedID = 0;
    public Vector3 selectedScale, pushForce;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Debug.Log(zombies[2].name);
        
        
        next = InputSystem.actions.FindAction("Player/Next");
        previous = InputSystem.actions.FindAction("Player/Previous");
        push = InputSystem.actions.FindAction("Player/Push");
        
        SelectZombie(0);
    }

    private void SelectZombie(int id)
    {

        selectedID = id;

        if (selectedID != null)
            selectedZombie.transform.localScale = Vector3.one;
        
        selectedZombie = zombies[id];
        selectedZombie.transform.localScale = selectedScale;
        Debug.Log("selected " + selectedZombie.name);
    }

    // Update is called once per frame
    void Update()
    {
        if (push.WasPressedThisFrame())

        {
            Debug.Log("space pressed");

            Rigidbody rb = selectedZombie.GetComponent<Rigidbody>();
            if (rb != null)
                 rb.AddForce(pushForce, ForceMode.Impulse);
        }

        if (next.WasPressedThisFrame())

        {
           
            selectedID++;
            if (selectedID >= zombies.Length)
                selectedID = 0;
            SelectZombie(selectedID);
        }
        
        if (previous.WasPressedThisFrame())

        {
            
            selectedID--;
            if (selectedID < 0)
                selectedID = zombies.Length - 1; 
            SelectZombie(selectedID);
        }
    }
}
