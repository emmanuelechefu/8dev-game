using UnityEngine;
using System.Text;
using System.Collections;

//This is the backbone carrying everything icl. It generates the level map, spawns keys and spawns enemies

public class LevelManager : MonoBehaviour
{
    public TextAsset[] roomMaps;      // 9 maps, 15x15 each
    public MapGenerator mapGenerator; // reference to your Map object
    public GameObject keyPrefab;      // prefab to spawn  

    public GameObject enemyPrefab;
    public int enemiesPerWave = 3;
    public float timeBetweenWaves = 10f;

    // New titties health pickup variables : change 1
    [Header("Health Spawning")]
    public GameObject healthPickupPrefab; 
    public int healthPickupsToSpawn = 3;
    private const int roomSize = 15;
    private int gridSize;
    private char[,] grid;             // combined 45x45 char grid

    private void Start()
    {
        GenerateLevel();
        SpawnHealthPickups(); // call titty spawning function after levels generate : change 2
        StartCoroutine(EnemyWaveLoop());
    }

    void GenerateLevel()
    {
        if (roomMaps == null || roomMaps.Length != 9)
        {
            Debug.LogError("LevelManager: Need exactly 9 room maps.");
            return;
        }

        gridSize = roomSize * 3;
        grid = new char[gridSize, gridSize];

        // build combined char grid (3x3 rooms)
        for (int i = 0; i < 9; i++)
        {
            TextAsset room = roomMaps[i];
            if (room == null)
            {
                Debug.LogError($"LevelManager: roomMaps[{i}] is null.");
                return;
            }

            string[] rows = room.text.Replace("\r", "").Split('\n');
            if (rows.Length < roomSize)
            {
                Debug.LogError($"LevelManager: roomMaps[{i}] has wrong size.");
                return;
            }

            int roomX = i % 3;  // 0,1,2
            int roomY = i / 3;  // 0,1,2

            for (int y = 0; y < roomSize; y++)
            {
                string row = rows[roomSize - 1 - y]; // invert Y

                for (int x = 0; x < roomSize; x++)
                {
                    int gx = roomX * roomSize + x;
                    int gy = roomY * roomSize + y;

                    grid[gx, gy] = row[x];
                }
            }
        }

        // turn char grid into one big TextAsset for MapGenerator
        StringBuilder sb = new StringBuilder();
        for (int y = gridSize - 1; y >= 0; y--)
        {
            for (int x = 0; x < gridSize; x++)
                sb.Append(grid[x, y]);

            if (y > 0)
                sb.Append('\n');
        }

        TextAsset combined = new TextAsset(sb.ToString());
        mapGenerator.mapFile = combined;
        mapGenerator.Generate();

        // now spawn the key
        SpawnKeyInTopRightRooms();
    }

    //CHANGE 3: New Function to Spawn Health on Air Tiles      
    // Logic: Looks for '0' (Air) on the grid and instantiates   
    void SpawnHealthPickups()
    {
        if (healthPickupPrefab == null)
        {
            Debug.LogWarning("LevelManager: Health Pickup Prefab not assigned!");
            return;
        }

        int spawned = 0;
        int safety = 500; // prevent infinite loops if map is full

        // Loop until we spawn 5 items
        while (spawned < healthPickupsToSpawn && safety-- > 0)
        {
            // Pick ANY random spot on the entire map
            int randomX = Random.Range(0, gridSize);
            int randomY = Random.Range(0, gridSize);

            // Check our grid array to see what tile is there
            char c = grid[randomX, randomY];

            // If the tile is '0' (Air), it's a valid spawn spot
            if (c == '0') 
            {
                // Calculate world position
                Vector3 pos = new Vector3(
                    randomX * mapGenerator.tileSize,
                    randomY * mapGenerator.tileSize,
                    0f
                );

                // Spawn the prefab
                Instantiate(healthPickupPrefab, pos, Quaternion.identity);
                spawned++;
            }
        }
    }

    void SpawnKeyInTopRightRooms()
    {
        if (keyPrefab == null)
        {
            Debug.LogWarning("LevelManager: keyPrefab not assigned.");
            return;
        }

        // allowed room indices: 4, 5, 7, 8
        int[] allowedRooms = { 4, 5, 7, 8 };

        int maxTries = 200;
        while (maxTries-- > 0)
        {
            // pick one of the allowed rooms
            int roomIndex = allowedRooms[Random.Range(0, allowedRooms.Length)];
            int roomX = roomIndex % 3;
            int roomY = roomIndex / 3;

            // pick a random tile inside that 15x15 room
            int localX = Random.Range(0, roomSize);
            int localY = Random.Range(0, roomSize);

            int gx = roomX * roomSize + localX;
            int gy = roomY * roomSize + localY;

            char c = grid[gx, gy];

            // only spawn on air (0) – change/add more allowed chars if needed
            if (c != '0')
                continue;

            Vector3 pos = new Vector3(
                gx * mapGenerator.tileSize,
                gy * mapGenerator.tileSize,
                0f
            );

            Instantiate(keyPrefab, pos, Quaternion.identity);
            Debug.Log($"Key spawned at grid ({gx},{gy}) in room {roomIndex}.");
            return;
        }

        Debug.LogWarning("LevelManager: Failed to find a valid key position in top-right rooms.");
    }



    IEnumerator EnemyWaveLoop()
    {
        // wait a moment before first wave if you want
        yield return new WaitForSeconds(2f);

        while (true)
        {
            SpawnEnemyWave();
            yield return new WaitForSeconds(timeBetweenWaves);
        }
    }

    void SpawnEnemyWave()
    {
        if (enemyPrefab == null)
        {
            Debug.LogWarning("LevelManager: enemyPrefab not assigned.");
            return;
        }

        // rooms where enemies are allowed: all except room 0
        int[] allowedRooms = { 1, 2, 3, 4, 5, 6, 7, 8 };

        int spawned = 0;
        int safety = 500; // avoid infinite loops

        while (spawned < enemiesPerWave && safety-- > 0)
        {
            int roomIndex = allowedRooms[Random.Range(0, allowedRooms.Length)];
            int roomX = roomIndex % 3;
            int roomY = roomIndex / 3;

            int localX = Random.Range(0, roomSize);
            int localY = Random.Range(0, roomSize);

            int gx = roomX * roomSize + localX;
            int gy = roomY * roomSize + localY;

            char c = grid[gx, gy];

            // only spawn on air tiles
            if (c != '0')
                continue;

            Vector3 pos = new Vector3(
                gx * mapGenerator.tileSize,
                gy * mapGenerator.tileSize,
                0f
            );

            Instantiate(enemyPrefab, pos, Quaternion.identity);
            spawned++;
        }

        Debug.Log($"Spawned {spawned} enemies in this wave.");
    }

}
