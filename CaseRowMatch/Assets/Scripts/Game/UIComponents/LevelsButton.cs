using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Threading.Tasks;

public class LevelsButton : Button
{
    private InitSceneManager firstSceneManager;

    void Start()
    {
        firstSceneManager = FindObjectOfType<InitSceneManager>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Debug.Log("Pressed left click, casting ray.");
            _ = CastRayAsync();
        }
    }


    public new async Task CastRayAsync()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
        if (hit)
        {
            await ButtonAnimation();
            Debug.Log(hit.collider.gameObject.name);
            firstSceneManager.gameStartTrigger = true;
        }
    }
}
