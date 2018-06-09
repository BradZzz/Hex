using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleGui : MonoBehaviour {

  private Vector3 originPosition;
  private Quaternion originRotation;
  public float shake_decay = 0.05f;
  public float shake_intensity = .3f;

  private float temp_shake_intensity = 0;

  public void isHit(){
    StartCoroutine(AnimateHit());
  }

  private IEnumerator AnimateHit()
  {
    originPosition = transform.position;
    originRotation = transform.rotation;
    temp_shake_intensity = shake_intensity;

    while (temp_shake_intensity > 0){
      transform.position = originPosition + Random.insideUnitSphere * temp_shake_intensity;
      transform.rotation = new Quaternion(
        originRotation.x + Random.Range (-temp_shake_intensity,temp_shake_intensity) * .2f,
        originRotation.y + Random.Range (-temp_shake_intensity,temp_shake_intensity) * .2f,
        originRotation.z + Random.Range (-temp_shake_intensity,temp_shake_intensity) * .2f,
        originRotation.w + Random.Range (-temp_shake_intensity,temp_shake_intensity) * .2f);
      temp_shake_intensity -= shake_decay;

      yield return null;
    }

    yield return 0;


//    Vector3 startPos = transform.position;
//    float t = 0;
//    //Vector3 endPos = new Vector3(startPos.x * startPos.y + System.Math.Sign(5), startPos.z);
//    float factor = 1f;
//    float moveSpeed = 1f;
//
//    while (t < 1f)
//    {
//      t += Time.deltaTime * moveSpeed;
//      float x = 0;
//      if (t < .5f) {
//        x = transform.position.x + t;
//      } else {
//        x = transform.position.x + (t - 1);
//      }
//      Vector3 pos = new Vector3(x, transform.position.y, transform.position.z);
//      transform.position = pos;
//
//      yield return null;
//    }
//
//    while(t < .5f){
//      t += Time.deltaTime * moveSpeed;
//      float x = transform.position.x + (.5f - t);
//      Vector3 pos = new Vector3(x, transform.position.y, transform.position.z);
//      transform.position = pos;
//
//      yield return null;
//    }
//
//    yield return 0;
  }
}

