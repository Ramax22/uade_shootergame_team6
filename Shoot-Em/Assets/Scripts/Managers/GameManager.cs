using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] List<Transform> _spawnPoints;
    [SerializeField] GameObject _enemyPrefab;
    [SerializeField] List<Node> _nodeList;
    [SerializeField] Transform _playerTransform;
    [SerializeField] EntityModel _playerModel;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject.Instantiate(_enemyPrefab, _spawnPoints[0].position, _spawnPoints[0].rotation);
        }
    }

    public Transform PlayerTransform { get => _playerTransform; }
    public EntityModel PlayerModel { get => _playerModel; }
    public List<Node> NodeList { get => _nodeList; }
}
