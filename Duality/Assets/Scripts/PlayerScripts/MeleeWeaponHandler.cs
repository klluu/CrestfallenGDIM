using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponHandler : MonoBehaviour
{
    [SerializeField] Player_Controller playerController;
    public float WeaponSpeed = 0;

    [SerializeField] private float _damage = 75f;
    [SerializeField] private float _attackRange = 5f;
    [SerializeField] private float _weaponDistance = 4f;
    [SerializeField] private float _weaponSpeedMin = .05f;
    [SerializeField] private float _weaponSpeedMax = .75f;
    [SerializeField] private float _knockback = 15f;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            if (WeaponSpeed > _weaponSpeedMin)
            {
                EntityManager entity = collision.GetComponent<EntityManager>();
                float t = Mathf.Clamp((WeaponSpeed - _weaponSpeedMin) / (_weaponSpeedMax - _weaponSpeedMin), 0, 1);

                entity.TakeDamage(_damage * t);
                entity.KnockBack = (Vector2)entity.transform.position - collision.ClosestPoint((Vector2)entity.transform.position).normalized * _knockback * t;

                print(_damage * t);
            }
        }
    }

    private void UpdateWeaponPosition()
    {
        Vector3 oldPos = transform.position;

        Vector2 positionOnScreen = Camera.main.WorldToViewportPoint(playerController.transform.position);
        Vector2 mouseOnScreen = (Vector2)Camera.main.ScreenToViewportPoint(Input.mousePosition);
        float angle = AngleBetweenTwoPoints(positionOnScreen, mouseOnScreen);

        transform.rotation = Quaternion.Euler(0, 0, angle);

        Vector3 newPos = playerController.transform.position + (Vector3)(mouseOnScreen - positionOnScreen).normalized * _weaponDistance;

        transform.position = new Vector3(newPos.x, newPos.y, -1);

        WeaponSpeed = (transform.position - oldPos).magnitude;
    }

    float AngleBetweenTwoPoints(Vector3 a, Vector3 b)
    {
        return Mathf.Atan2(a.y - b.y, a.x - b.x) * Mathf.Rad2Deg;
    }

    private void Update()
    {
        UpdateWeaponPosition();
    }
}