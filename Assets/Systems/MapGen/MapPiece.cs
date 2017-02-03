using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPiece : MonoBehaviour {

    public bool walkable;
    private SpriteRenderer sprite;

    void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        SetUnwalkable();
    }

	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
		
	}

    public int GetX()
    {
        return (int)gameObject.transform.position.x;
    }

    public int GetY()
    {
        return (int)gameObject.transform.position.y;
    }

    public void SetWalkable()
    {
        walkable = true;
        sprite.color = Color.blue;
    }

    internal void SetUnwalkable()
    {
        walkable = false;
        sprite.color = Color.green;
    }
}
