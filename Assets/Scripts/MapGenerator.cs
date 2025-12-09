using UnityEngine;

/* Another backbone, turns the txt files into maps and then the levle manager/safeArea code
 turns it into the levels/safeAreas good code if you want to add like more levels or places idk
*/

public enum TileType
{
    Air      = 0,
    Wall     = 1,
    Door     = 2,
    Key      = 3,
    ExitDoor = 4,
    BossDoor = 5
}

public class MapGenerator : MonoBehaviour
{
    public TextAsset mapFile;
    public float tileSize = 1f;

    public GameObject airPrefab;
    public GameObject wallPrefab;
    public GameObject doorPrefab;
    public GameObject keyPrefab;
    public GameObject exitDoorPrefab;
    public GameObject bossDoorPrefab;

    private GameObject[,] tiles;

    public void Generate()
    {
        if (mapFile == null)
        {
            Debug.LogError("No map file assigned!");
            return;
        }

        string[] rows = mapFile.text.Replace("\r", "").Split('\n');
        int height = rows.Length;
        int width  = rows[0].Length;

        tiles = new GameObject[width, height];

        for (int y = 0; y < height; y++)
        {
            // invert Y so bottom row of the text is y = 0 in world space
            string row = rows[height - 1 - y];

            for (int x = 0; x < width; x++)
            {
                char ch = row[x];
                if (ch < '0' || ch > '9') continue;   // ignore anything weird

                int val = ch - '0';
                TileType type = (TileType)val;

                Vector3 origin = transform.position;
                Vector3 pos = new Vector3(
                    origin.x + x * tileSize,
                    origin.y + y * tileSize,
                    origin.z
                );
                GameObject tile = null;

                switch (type)
                {
                    case TileType.Air:
                        if (airPrefab != null)
                            tile = Instantiate(airPrefab, pos, Quaternion.identity, transform);
                        break;

                    case TileType.Wall:
                        if (wallPrefab != null)
                            tile = Instantiate(wallPrefab, pos, Quaternion.identity, transform);
                        break;

                    case TileType.Door:
                        if (doorPrefab != null)
                            tile = Instantiate(doorPrefab, pos, Quaternion.identity, transform);
                        break;

                    case TileType.Key:
                        // Always have a floor under the key
                        if (airPrefab != null)
                            tile = Instantiate(airPrefab, pos, Quaternion.identity, transform);

                        // Spawn the key object on top
                        if (keyPrefab != null)
                            Instantiate(keyPrefab, pos, Quaternion.identity, transform);
                        break;

                    case TileType.ExitDoor:
                        if (exitDoorPrefab != null)
                            tile = Instantiate(exitDoorPrefab, pos, Quaternion.identity, transform);
                        break;

                    case TileType.BossDoor:
                        if (bossDoorPrefab != null)
                            tile = Instantiate(bossDoorPrefab, pos, Quaternion.identity, transform);
                        break;
                }

                if (tile != null)
                {
                    tiles[x, y] = tile;
                }
            }
        }
    }

    private void Start()
    {
        Generate();
    }
}
