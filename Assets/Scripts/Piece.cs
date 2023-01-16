using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    float secondsCounter = 0;
    float secondsToCount = 1;
    // Start is called before the first frame update
    void Start()
    {
        // Default position not valid? Then it's game over
        if (!IsValidBoard())
		{
            Debug.Log("GAME OVER");
            Destroy(gameObject);
        }
    }

    // Update is called once per frame.
    // Implements all piece movements: right, left, rotate and down.
    void Update()
    {
        // Move Left
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // Modify position
            transform.position += new Vector3(-1, 0, 0);

            // See if it's valid
            if (IsValidBoard())
				// It's valid. Update grid.
				UpdateBoard();
            else
                // Its not valid. revert.
                transform.position += new Vector3(1, 0, 0);
        }
        // Implement Move Right (key RightArrow)
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);
            if (IsValidBoard())
                UpdateBoard();
            else
                transform.position += new Vector3(-1, 0, 0);
        }

        // Implement Rotate, rotates the piece 90 degrees (Key UpArrow)
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            transform.Rotate(0,0,90);
            if (IsValidBoard())
                UpdateBoard();
            else
                transform.Rotate(0,0,0);
        }

        // Implement move Downwards and Fall (each second)
        
        if (Input.GetKeyDown(KeyCode.DownArrow) || (secondsCounter += Time.deltaTime) >= secondsToCount)
        {
            
            transform.position += new Vector3(0, -1, 0);
            if (IsValidBoard())
            {
                UpdateBoard();
            }
            else{
                transform.position += new Vector3(0, 1, 0);
                Board.DeleteFullRows();
                FindObjectOfType<Spawner>().SpawnNext();
                enabled = false;
            }
            secondsCounter = 0;
        }
    }

    // TODO: Updates the board with the current position of the piece. 
    void UpdateBoard()
    {
        // First you have to loop over the Board and make current positions of the piece null.
        for (int y = 0; y < Board.h; y++)
        {
            for (int x = 0; x < Board.w; x++)
            {
                if (Board.grid[x, y] != null && Board.grid[x, y].transform.parent == transform)
                {
                    Board.grid[x, y] = null;
                }
            }
        }
        // Then you have to loop over the blocks of the current piece and add them to the Board.
        foreach (Transform a in transform)
        {
            Vector2 v = Board.RoundVector2(a.position);
            Board.grid[(int)v.x, (int)v.y] = a.gameObject;
        }
    }

    // Returns if the current position of the piece makes the board valid or not
    bool IsValidBoard()
    {
        foreach (Transform child in transform)
        {
            Vector2 v = Board.RoundVector2(child.position);

            // Not inside Border?
            if (!Board.InsideBorder(v))
                return false;

            // Block in grid cell (and not part of same group)?
            if (Board.grid[(int)v.x, (int)v.y] != null &&
                Board.grid[(int)v.x, (int)v.y].transform.parent != transform)
                return false;
        }
        return true;
    }
}
