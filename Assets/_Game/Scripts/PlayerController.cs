using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
[SerializeField] private Vector2 _joystickSize = new Vector2(200, 200);
[SerializeField] private float _speed = 7f;
[SerializeField] private float rotationSpeed;
[SerializeField] private Rigidbody _rigidbody;
[SerializeField] private FixedJoystick _joystick;
[SerializeField] private Animator _animator;
[SerializeField] private float _fireRate = 1;
[SerializeField] private float _nextFire = 1;
[SerializeField] private float _range = 1000000;
[SerializeField] private GameObject _muzzleFlash;
[SerializeField] private Transform _bulletSpawn;
[SerializeField] private AudioClip _shotSFX;
[SerializeField] private AudioSource _audioSource;
[SerializeField] private Camera _cam;

    void Start()
    {
        _animator = GetComponent<Animator>();
    }
    public void Shoot()
    {
        if (Time.time > _nextFire)
        {
            _nextFire = Time.time + _fireRate;
            _audioSource.PlayOneShot(_shotSFX);
            Destroy(Instantiate(_muzzleFlash, _bulletSpawn.position, _bulletSpawn.rotation), 0.1f);
           
              if (Physics.Raycast(_cam.transform.position, _cam.transform.forward, out var hit, _range))
              {
                  Quaternion hitRotation = Quaternion.FromToRotation(Vector3.up, hit.normal);
                  if (hit.collider != null)
                  {
                     Destroy(Instantiate(_muzzleFlash, hit.point + (hit.normal * 0.001f), hitRotation), 0.1f);
                  }
                  Debug.Log("Bang " + hit.collider);
              }
        }
    }

    private void Move()
    {
        float angleA = Mathf.Atan2(_joystick.Horizontal, _joystick.Vertical) * Mathf.Rad2Deg * Time.fixedDeltaTime * rotationSpeed;
        transform.rotation *= Quaternion.Euler(0f, angleA, 0f);
        _rigidbody.velocity = transform.forward * _joystick.Vertical * _speed;
    }
    private void FixedUpdate()
    {
        float maxMovement = _joystickSize.x / 2f;

        if (_joystick.Horizontal != 0 || _joystick.Vertical != 0)
        {
            Move();
        }
        else
        {
            _rigidbody.velocity = Vector3.zero;
        }
        
        _animator.SetFloat("moveX", _joystick.Vertical);
        _animator.SetFloat("moveZ", _joystick.Horizontal);  
    }
}



