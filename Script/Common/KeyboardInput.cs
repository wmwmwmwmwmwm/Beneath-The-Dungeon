using UnityEngine;

public class KeyboardInput : MonoBehaviour
{
#if UNITY_EDITOR
    void Start()
	{
        Player.Instance.UsePendingMove = false;
	}

	void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            Player.Instance.Move(Vector2Int.up);
        }
        else if (Input.GetKey(KeyCode.S))
        {
            Player.Instance.Move(Vector2Int.down);
        }
        else if (Input.GetKey(KeyCode.A))
        {
            Player.Instance.Move(Vector2Int.left);
        }
        else if (Input.GetKey(KeyCode.D))
		{
            Player.Instance.Move(Vector2Int.right);
        }
    }
#endif
}
