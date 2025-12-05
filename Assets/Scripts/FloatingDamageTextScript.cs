using UnityEngine;

public class FloatingDamageTextScript : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] float destroyTime = 1f;
    
    [SerializeField] Vector2 RandomOffset = new Vector2(0.5f,0);
    void Start()
    {
        Destroy(gameObject,destroyTime);
        transform.localPosition += new Vector3(Random.Range(-RandomOffset.x,RandomOffset.x),1f,0);
        
        LeanTween.value(gameObject, transform.localScale.x, 0, destroyTime)
            .setEase(LeanTweenType.easeInOutCubic)
            .setLoopPingPong(0) // -1 = infinite loop
            .setOnUpdate( val=>
            {
                transform.localScale = new Vector2(val,val);
            });
        
        float modifer = transform.localPosition.y;
        modifer += 1;
        
        LeanTween.value(gameObject, transform.localPosition.y, modifer, destroyTime)
            .setEase(LeanTweenType.easeInOutCubic)
            .setLoopPingPong(0) // -1 = infinite loop
            .setOnUpdate( val=>
            {
                transform.localPosition = new Vector2(transform.localPosition.x,val);
            });
    }
}
