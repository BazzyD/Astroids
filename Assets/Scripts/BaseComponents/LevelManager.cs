using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    private float _minX, _maxX, _minY, _maxY;
    private float _timer =0f;
    private float _timeBetweenWaves = 5f;
    private List<GameObject> astroids = new();
    private Camera _camera;
    private bool didSpawn =false;
    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Start()
    {
        // get the sprite size in world units to use as a buffer for wrapping
        //buffer = GetComponent<SpriteRenderer>().bounds.extents.x;
        
        // Get the screen corners in World Space
        // (0,0) is bottom-left, (Screen.width, Screen.height) is top-right
        Vector3 bottomLeft = _camera.ScreenToWorldPoint(new Vector3(0, 0, 0));
        Vector3 topRight = _camera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0));

        _minX = bottomLeft.x;
        _maxX = topRight.x;
        _minY = bottomLeft.y;
        _maxY = topRight.y;
        for (int i = 1; i <= 5; i++)
        {
            SpawnAstroid(i);
        }
    }
    private void SpawnAstroid(int level)
    {
        float randomX = Random.Range(_minX, _maxX);
        float randomY = Random.Range(_minY, _maxY);
        float randomZ = Random.Range(0f, 360f);
        Quaternion randomRotation = Quaternion.Euler(0, 0, randomZ);
        Debug.Log($"Spawning Astroid lvl {level}");
        GameObject astroid = ObjectPool.Instance.Spawn($"Astroid_lvl{level}", new Vector3(randomX, randomY, 0), randomRotation);
        astroids.Add(astroid);
    }

    // Update is called once per frame
    void Update()
    {
        _timer += Time.deltaTime;
        if (_timer >= _timeBetweenWaves && !didSpawn)
        {
            _timer = 0f;
            didSpawn = true;
            foreach (var astroid in astroids)
            {
                if (astroid.TryGetComponent(out Astroid astroidComponent))
                {
                    astroidComponent.TakeDamage(1000f);
                }
            }
        }

    }
}
