using System.Collections;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour
{    
    [SerializeField] private Transform _resourcesParent;
    
    private ICollectable[] _resources;

    private void Awake()
    {        
        _resources = new ICollectable[_resourcesParent.childCount];

        for (int i = 0; i < _resources.Length; i++)        
            _resources[i] = _resourcesParent.GetChild(i).GetComponent<ICollectable>();
    }

    private void OnEnable()
    {        
        foreach (var resource in _resources)
            resource.Collected += RespawnResource;
    }

    private void OnDisable()
    {        
        foreach (var resource in _resources)
            resource.Collected -= RespawnResource;
    }

    private void RespawnResource(ICollectable resource, float respawnTime)
    {        
        resource.SetActive(false);        

        StartCoroutine(SpawnWithDelay(resource, respawnTime));
    }

    private IEnumerator SpawnWithDelay(ICollectable resource, float delay)
    {
        yield return new WaitForSeconds(delay);
        
        resource.SetActive(true);
    }
}
