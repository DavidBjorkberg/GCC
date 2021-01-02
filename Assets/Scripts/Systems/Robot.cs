using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public class Robot : ComponentSystem
{
    private Goliath goliath;

    protected override void OnUpdate()
    {
        
    }


    //public void MoveTo(Vector3 pos, Vector3 normal)
    //{
    //    StartCoroutine(MoveToCoroutine(pos, normal));
    //}

    //IEnumerator MoveToCoroutine(Vector3 pos, Vector3 normal)
    //{
    //    float lerpValue = 0;
    //    Vector3 startPos = transform.position;
    //    Vector3 startRotation = transform.localRotation.eulerAngles;
    //    while (lerpValue < 1)
    //    {
    //        transform.position = Vector3.Lerp(startPos, pos, lerpValue);
    //        transform.LookAt(transform.position + Vector3.Lerp(startRotation, normal, lerpValue));
    //        lerpValue += Time.deltaTime;
    //        yield return new WaitForEndOfFrame();
    //    }
    //}
}
