using DG.Tweening;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Threading.Tasks;

public class Button : MonoBehaviour
{
    // Update is called once per frame


    public async Task CastRayAsync()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction, Mathf.Infinity);
        if (hit)
        {
            await ButtonAnimation();
            Debug.Log(hit.collider.gameObject.name);
        }
    }

    //Değişicek
    protected async Task ButtonAnimation()
    {
        var buttonLarger = DOTween.Sequence();
        buttonLarger.Join(this.transform.DOScale(new Vector3(3f, 3f, 1f), 4f));
        await buttonLarger.Play().AsyncWaitForCompletion();
    }

}
