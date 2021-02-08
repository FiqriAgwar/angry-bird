using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Owl : Bird
{
    public float _power = 10f;
    public float _radius = 5f;
    public float _upForce = 1f;

    public override void OnCollisionEnter2D(Collision2D collision){
        SetState(BirdState.HitSomething);
        Explode();
        Destroy(gameObject);
    }

    void Explode(){
        Vector3 position = transform.position;
        Vector2 explosionPosition = new Vector2(position.x, position.y);

        Collider2D[] colliders = Physics2D.OverlapCircleAll(explosionPosition, _radius);

        foreach(Collider2D collider in colliders){
            Debug.Log(collider.name);

            Rigidbody2D rb = collider.GetComponent<Rigidbody2D>();
            if(rb != null){
                AddExplosionForce(rb, _power, explosionPosition, _radius, _upForce, ForceMode2D.Impulse);
            }
        }
    }

    void AddExplosionForce(Rigidbody2D rb, float power, Vector2 explosionPosition, float explosionRadius, float upwardsModifier, ForceMode2D mode){
        Vector2 explosionDirection = rb.position - explosionPosition;
        float explosionDistance = explosionDirection.magnitude;

        if(upwardsModifier == 0){
            explosionDirection /= explosionDistance;
        }
        else{
            explosionDirection.y += upwardsModifier;
            explosionDirection.Normalize();
        }

        rb.AddForce(Mathf.Lerp(0, power, (1-explosionDistance)) * explosionDirection, mode);
    } 
}
