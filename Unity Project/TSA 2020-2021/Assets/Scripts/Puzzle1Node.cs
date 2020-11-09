using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Puzzle1Node : MonoBehaviour
{
    public int id;
    public Image image;
    public Vector3 pos;
    public List<Puzzle1Node> neighbours = new List<Puzzle1Node>();
}
