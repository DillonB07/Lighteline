using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Platform : MonoBehaviour
{
    private PlayerController player;
    void Awake()
    {
        player = FindObjectOfType<PlayerController>();
    }

    private IEnumerator DestroyTile(Tilemap tilemap, Vector3Int cellPos, float delay)
    {
        yield return new WaitForSeconds(delay);
        tilemap.SetTile(cellPos, null);
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (col.gameObject.name =="Player")
        {
            var tilemap = GetComponent<Tilemap>();
            var grid = tilemap.layoutGrid;
            var cellPos = grid.WorldToCell(col.transform.position);
            var tile = tilemap.GetTile(cellPos);
            player.OnIce = false;

            if (tile != null)
            {
                Debug.Log($"Player is on platform of type {tile.name}");
                switch (tile.name) {
                    case "safe":
                        Debug.Log("Normal platform");
                        break;
                    case "crumble":
                        Debug.Log("Falling platform");
                        StartCoroutine(DestroyTile(tilemap, cellPos, 1f));
                        break;
                    case "cold":
                        Debug.Log("Ice platform");
                        player.OnIce = true;
                        break;
                    case "victory":
                        Debug.Log("Victory platform");
                        player.Victory();
                        break;
                    case "lightning":
                        Debug.Log("Lightning platform");
                        player.score ++;
                        Debug.Log($"Score: {player.score}");
                        tilemap.SetTile(cellPos, null);
                        break;
                }
            }
        }
    }
}
