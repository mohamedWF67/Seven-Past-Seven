using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class PuzzleScript : MonoBehaviour
{
    public List<bool> correctPuzzleStates;
    public List<bool> puzzlePieceStates;
    public bool isSolved;
    public GameObject puzzlePiecePrefab;
    public PuzzleUIScript puzzleUIScript;
    
    public GameObject keyPrefab;

    public void changePiece(int index)
    {
        puzzlePieceStates[index] = !puzzlePieceStates[index];
        puzzleUIScript.ChangePuzzleColour(index, puzzlePieceStates[index] == correctPuzzleStates[index]);
        CheckValidity();
    }
    
    void CheckValidity()
    {
        if (puzzlePieceStates.SequenceEqual(correctPuzzleStates))
        {
            isSolved = true;
            OnSolved();
        }
    }

    void OnSolved()
    {
        Debug.Log("Solved!");
        //Instantiate(keyPrefab, transform.position, Quaternion.identity);
        Destroy(puzzleUIScript.gameObject);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("Player")) return;
        puzzleUIScript = Instantiate(puzzlePiecePrefab).GetComponent<PuzzleUIScript>();
        puzzleUIScript.SetPuzzleScript(this);
    }
}
