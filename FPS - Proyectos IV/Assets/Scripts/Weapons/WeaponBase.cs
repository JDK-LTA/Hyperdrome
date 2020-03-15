using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBase : MonoBehaviour
{
    #region Weapon variables
    protected ShootingType _shootingType;
    protected WeaponType _weaponType;
    protected Changer _changer;
    protected string _name;

    protected Mesh _mesh;
    protected GameObject _bullet;
    protected Texture2D _crosshairTexture;
    protected AudioClip _fireSound;

    protected bool _canShoot;

    protected int _levelRequired;

    protected int _ammo;
    protected float _weight;
    protected float _damagePerHit;
    protected float _forceToApply;
    protected float _range;
    private float _cdBetweenShots;

    protected float _numberToChange;

    protected Transform _raycastSpot;

    protected AudioSource audioSource;
    #endregion

    private float auxTimer;
    private bool shot = false;

    protected virtual void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    // Start is called before the first frame update
    protected virtual void Start()
    {
        switch (_shootingType)
        {
            case ShootingType.LOCK:
            case ShootingType.SEMI_AUTOMATIC:
                InputManager.Instance.OnTriggerShoot += Shoot;
                break;
            case ShootingType.AUTOMATIC:
                InputManager.Instance.OnRepTriggerShoot += Shoot;
                break;
            default:
                break;
        }
        _raycastSpot = transform.Find("Shooting spot");
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        if (shot)
        {
            auxTimer += Time.deltaTime;
            if (auxTimer >= _cdBetweenShots)
            {
                shot = false;
            }
        }
    }

    protected virtual void Shoot()
    {
        if (!shot)
        {
            Ray ray = new Ray(_raycastSpot.transform.position, _raycastSpot.forward);

            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, _range))
            {
                Debug.Log("Hit" + hit.transform.name);
                if (hit.rigidbody)
                {
                    hit.rigidbody.AddForce(ray.direction * _forceToApply);
                    Debug.Log("Hit");
                }
            }

            audioSource.PlayOneShot(_fireSound);
        }
    }
}
