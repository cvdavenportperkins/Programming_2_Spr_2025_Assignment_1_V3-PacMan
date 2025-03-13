using UnityEngine;
using System.Collections.Generic;
public class Node : MonoBehaviour
{
    public List<Vector2> availableDirections;
    public LayerMask obstacleLayer;

    private Start()
    {
        this.availableDirections = new List<Vector2>();

        CheckAvailableDirections(Vector2.up);
        CheckAvailableDirections(Vector2.down); 
        CheckAvailableDirections(Vector2.left);
        CheckAvailableDirections(Vector2.right);
    }
    

    private void CheckAvailableDirections(Vector2 direction)
    {
        RaycastHit2D hit = Physics2D.BoxCast(this.transform.position, Vector2.one * 0.5f, 0.0f, direction, 1f, this.obstacleLayer);
        Debug.Log("Blocked" + (hit.collider != null) + " in direction: " + direction);
        return hit.collider != null;
    }

}