using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Image = UnityEngine.UIElements.Image;

public class PuzzleUIScript : MonoBehaviour
{
    public List<GameObject> puzzlePieces;
    public PuzzleScript puzzleScript;

    public void SetPuzzleScript(PuzzleScript pz)
    {
        puzzleScript = pz;
    }

    public void UpdatePiece(int index)
    {
        puzzleScript.changePiece(index);
    }

    public void ChangePuzzleColour(int index, bool isCorrect)
    {
        //puzzlePieces[index].GetComponent<Image>().tintColor = isCorrect ? Color.green : Color.grey;
    }
}
