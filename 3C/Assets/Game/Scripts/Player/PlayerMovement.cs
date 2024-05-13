using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _walkSpeed;  // kecepatan Player saat walk

    [SerializeField]
    private float _sprintSpeed; // kecepatan Player saat sprint

    private float _speed;   // current speed , akan dipakai untuk disesuaikan dengan _walkSpeed atau _sprintSpeed

    [SerializeField]
    private float _walkSprintTransition;


    [SerializeField]
    private InputManager _input;

    [SerializeField]
    private float _rotationSmoothTime = 0.1f;

    private float _rotationSmoothVelocity;

    private Rigidbody _rigidBody;

        
    [SerializeField]
    private float _jumpForce;

    private bool _isGrounded;    // field yang menentukan apakah player di tanah atau tidak

    [SerializeField]
    private Transform _groundDetector; // ini untuk menentukan posisi lower Step ( berguna untuk check isGrounded)

    [SerializeField]
    private float _detectorRadius;

    [SerializeField]
    private LayerMask _groundLayer;

    [SerializeField]
    private Vector3 _upperStepOffset; // ini untuk offset , jadi posisi _upperStep  adalah penjumlahan lower step + offset

    [SerializeField]
    private float _stepCheckerDistance; // ini untuk hitung jarak maksimum dari detector ke step
                                        // jika sudah melebihi jarak _stepCheckerDistance, maka step tidak akan terdeteksi
                                        // detector hanya akan mendeteksi step dalam range _stepCheckterDistance        
    
    [SerializeField]
    private float _stepForce; // ini adalah besar daya mengangkat Player jika ada step ( anak tangga ) di depan Player

    private PlayerStance _playerStance;

    [SerializeField]
    private Transform _climbDetector;

    [SerializeField]
    private float _climbCheckDistance;

    [SerializeField]
    private LayerMask _climbableLayer;
    
    [SerializeField]
    private Vector3 _climbOffset;

    [SerializeField]
    private float _climbSpeed;

    [SerializeField]
    private Transform _cameraTransform;

    [SerializeField]
    private CameraManager _cameraManager;

    private Animator _animator;



    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();   // akses RigidBody via script di objek yang sama yaitu Player

        _speed = _walkSpeed;        // speed awal saat start Game

        _playerStance = PlayerStance.Stand;

        _animator = GetComponent<Animator>();

    }


    private void Start()
    {
        _input.OnMoveInput += Move;   // PROCESS SUBSCRIBE Move

        _input.OnSprintInput += Sprint; // SUBSCRIBE Sprint

        _input.OnJumpInput += Jump;

        _input.OnClimbInput += StartClimb;

        _input.OnCancelClimb += CancelClimb;

        _cameraManager.OnChangePerspective += ChangePerspective;
    }



    private void OnDestroy()
    {
        _input.OnMoveInput -= Move; // PROCESS UNSUBSCRIBE Move

        _input.OnSprintInput -= Sprint; // UNSUBSCRIBE Sprint

        _input.OnJumpInput -= Jump;

        _input.OnClimbInput -= StartClimb;

        _input.OnCancelClimb -= CancelClimb;

        _cameraManager.OnChangePerspective -= ChangePerspective;
    }

    private void Update()
    {
        CheckIsGrounded();
        CheckStep();
    }

    private void Move(Vector2 axisDirection)   // sekarang buat camera , bikin dulu field _cameraManager utk direferensikan di Inspector

    {  
        Vector3 movementDirection = Vector3.zero;
        
        bool isPlayerStanding = _playerStance == PlayerStance.Stand;
        bool isPlayerClimbing = _playerStance == PlayerStance.Climb;

                   
        if (isPlayerStanding)
        {
            
      

            // tentukan terlebih dahulu apakah camera 1st person atau 3rd person
            // pake switch
            
            switch(_cameraManager.CameraState)
            {
                case CameraState.ThirdPerson:

                    if (axisDirection.magnitude >= 0.1)
                    {
                        float rotationAngle = Mathf.Atan2(axisDirection.x, axisDirection.y) * Mathf.Rad2Deg + _cameraTransform.eulerAngles.y;

                        float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationAngle, ref _rotationSmoothVelocity, _rotationSmoothTime);

                        transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);

                        movementDirection = Quaternion.Euler(0f, rotationAngle, 0f) * Vector3.forward;

                        _rigidBody.AddForce(movementDirection * _speed * Time.deltaTime);
                    }

                    break;
                
                case CameraState.FirstPerson:

                    transform.rotation = Quaternion.Euler(0f, _cameraTransform.eulerAngles.y, 0f);

                    Vector3 verticalDirection = axisDirection.y * transform.forward;

                    Vector3 horizontalDirection = axisDirection.x * transform.right;

                    movementDirection = verticalDirection + horizontalDirection;

                    _rigidBody.AddForce(movementDirection * _speed * Time.deltaTime);

                    break;
                
                default:
                    break;

            }

            // atur animasi idle walking run untuk 3rd person and 1st person 
            Vector3 velocity = new Vector3(_rigidBody.velocity.x, 0, _rigidBody.velocity.z);

            _animator.SetFloat("Velocity", velocity.magnitude * axisDirection.magnitude); //3rd person pake parameter Velocity
            _animator.SetFloat("VelocityZ", velocity.magnitude * axisDirection.y); // 1st person pake parameter VelocityZ ke sumbu Z untuk sumbu y
            _animator.SetFloat("VelocityX", velocity.magnitude * axisDirection.x); // 1st person pake parameter VelocityX ke sumbu X untuk sumbu x



        }
        else if (isPlayerClimbing)
        {
            Vector3 horizontal = axisDirection.x * transform.right;

            Vector3 vertical = axisDirection.y * transform.up;

            movementDirection = horizontal + vertical;

            _rigidBody.AddForce(movementDirection * _speed * Time.deltaTime);
        }
    }


    private void Jump()
    {
        if (_isGrounded)
        {
            Vector3 jumpDirection = Vector3.up; // new Vector3(0,1,0) = Vector3.up

            _rigidBody.AddForce(jumpDirection * _jumpForce * Time.deltaTime);
        }
    }
    
    private void CheckIsGrounded()
    {
        _isGrounded = Physics.CheckSphere(_groundDetector.position, _detectorRadius, _groundLayer);
    }

    private void CheckStep()
    {
        bool isHitLowerStep = Physics.Raycast(_groundDetector.position,
                                                transform.forward,
                                                _stepCheckerDistance);

        bool isHitUpperStep = Physics.Raycast(_groundDetector.position +
                                                _upperStepOffset,
                                                transform.forward,
                                                _stepCheckerDistance);

        if (isHitLowerStep && !isHitUpperStep)
        {
            _rigidBody.AddForce(0, _stepForce * Time.deltaTime, 0);
        }
    }

    private void Sprint(bool isSprint)
    {
        if (isSprint)
        {
            if (_speed < _sprintSpeed)
            {
                _speed = _speed + _walkSprintTransition * Time.deltaTime;
            }
        }
        else
        {
            if (_speed > _walkSpeed)
            {
                _speed = _speed - _walkSprintTransition * Time.deltaTime;
            }
        }
    }

    private void StartClimb()
    {
        bool isInfrontofClimbingWall = Physics.Raycast(_climbDetector.position,
                                                        transform.forward,
                                                        out RaycastHit hit,
                                                        _climbCheckDistance,
                                                        _climbableLayer);

        bool isNotClimbing = _playerStance != PlayerStance.Climb;

        if ( isInfrontofClimbingWall && _isGrounded && isNotClimbing)
        {
            Vector3 offset = (transform.forward * _climbOffset.z) + (Vector3.up * _climbOffset.y);

            transform.position = hit.point - offset;

            _playerStance = PlayerStance.Climb;

            _rigidBody.useGravity = false;

            _speed = _climbSpeed;

            // panggil SetFPS dari class CameraManager

            _cameraManager.SetFPSClampedCamera(true, transform.rotation.eulerAngles);

            // atur Field Of View menjadi 70 ketika memanjat

            _cameraManager.setTPSFieldofView(70);
        }
    }
    

    private void CancelClimb()
    {
        if (_playerStance == PlayerStance.Climb)
        {
            _playerStance = PlayerStance.Stand;

            _rigidBody.useGravity = true;

            _speed = _walkSpeed;

            transform.position -= transform.forward;

            _cameraManager.SetFPSClampedCamera(false, transform.rotation.eulerAngles);

            _cameraManager.setTPSFieldofView(40);

        }
    }

    private void ChangePerspective()
    {
        _animator.SetTrigger("ChangePerspective");
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawRay(_climbDetector.position,transform.forward);
    }

//    private void OnCollisionExit(Collision collision)
//    {
//        _rigidBody.useGravity = true;   // butuh perbaikan , isTrigger = true, kalo udah climb malah jatuh lagi jadinya
//    }

}


////private void Move(Vector2 axisDirection)  // ini method without rotation, jangan dihapus, harus dipelajari dulu sebelum rotation
////{
// karena player akan bergerak ke sumbu x,y,z gunakan Vector3
// untuk sumbu x berarti player akan bergerak ke samping ( axis warna merah)
// untuk sumbu y berarti player akan bergerak ke atas ( axis warna hijau)
// untuk sumbu z berarti player akan bergerak ke depan ( axis warna biru)
// jadi kita masukin x = axisDirection.x , karena kita mau gerakin player ke kiri atau ke kanan berdasarkan axisDirection di sumbu x
// jadi kita masukin y = 0 , karena player tidak menerima input gerak ke atas atau ke bawah 
// jadi kita masukin x = axisDirection.y , karena kita mau gerakin player ke depan atau ke belakang berdasarkan axisDirection di sumbu y


////    Vector3 movementDirection = new Vector3(axisDirection.x, 0, axisDirection.y);
////    Debug.Log(movementDirection);

// langkah selanjutnya menambahkan method Move ke method OnMoveInput di Class InputManager
// gunakan field sebagai referensi dari Class PlayerMovement ke Class InputManager
// nama fieldnya _input dengan tipe Class yang hendak di akses yaitu InputManager
// jangan lupa direferensikan di Unitynya
//  drag InputManager dari Hierarcy ke Input di bagian Inspector PlayerMovement Script

// lalu balik lagi ke vs setelah sudah direferensikan di Unity
// berarti sekarang field _input sudah bisa akses OnMoveInput di Class InputManager
// jadi tambahkan _input dari OnMoveInput ke method Move di Start
// kenapa di Start ? karena method Move harus ditambahkan ke event OnMoveInput ketika game baru pertama kali dijalankan

// jangan lupa juga untuk menghapus method Move dari event OnMoveInput ketika game berhenti
// jadi gunakan OnDestroy()

//proses menambahkan method ke dalam event itu disebut SUBSCRIBE
//proses menghapus method dari dalam event itu disebut UNSUBSCRIBE

// kita akan menggerakan Player menggunakan physics ( ada force, ada gravitasi)
// jadi pake RigidBody
// di inspector Unity tambahkan component RigidBody
// lalu referensikan dengan cara Serialize atau dengan akses langsung di script
// kita pake yang script , taruh di method Awake 
// _rigidBody = GetComponent<Rigidbody>();

// lalu balik lagi ke method Move
// _rigidBody.AddForce 

////   _rigidBody.AddForce(movementDirection * _walkSpeed * Time.deltaTime);

// supaya tidak bergantung ke frame rate ( ke jumlah frame), kalikan dengan Time.deltaTime
// jangan lupa di inspector walkspeed diatur jadi 350
// dan di  inspector rigidbody -> constraints -> freeze rotation XYZ di centang semuanya
// supaya player gak jatuh pas bergerak maju
// jika player sudah dikasih force tapi masih punya rotation XYZ
// akibatnya player jatuh karena bergerak sambil masih punya rotasi
////}

//*******************************************************************************

////private void Move(Vector2 axisDirection)   // ini method with rotation
////{
// kita mau cari sudut rotasi ( rotation angle )
// caranya pake rumus segitiga trigonometri tan alfa = AB / BC
// alfa adalah sudutnya
// karena mau cari sudut kita pake arc tan 
// jadi kita pake Matf.Atan2
// karena sudut satuannya derajat
// jadi supaya hasilnya derajat , kita kalikan dengan pake Mathf.Rad2Deg
// Atan = Arcus tangen , Rad2Deg = Radius to degree
// hasil sudutnya simpan ke variabel rotationAngle dengan tipe float
////    float rotationAngle = Mathf.Atan2(axisDirection.x, axisDirection.y) * Mathf.Rad2Deg;

//masukkan nilai sudut rotasi tsb ( yaitu rotationAngle) yang 
// ke transform
//khususnya transform.rotation ( yang adalah nilai Vector, bukan sudut)
// dengan cara memakai Quaternion.Euler , kita bisa memasukkan nilai sudut menjadi nilai vector
// untuk pemakaian nilai terkait rotasi( sudut)  akan selalu memakai Quaternion
// karena nilai sumbu y yang mau dirotasi maka nilai sumbu x dan sumbu z bernilai 0 float
////    transform.rotation = Quaternion.Euler(0f, rotationAngle, 0f);

// supaya bisa bergerak ke sumbu x,y,z 
// maka gunakan Vector3, buat variabel bernama movementDirection dengan tipe Vector3
// yang nilainya adalah hasil perkalian antara transform.rotation dengan arah maju 
// arah maju adalah Vector3.forward ( 0,0,1)
// kenapa dikalikan arah maju ( x = 0, y = 0, z = 1 ) ?
// karena supaya sesuai dengan tombol keyboard yang ditekan , panah atas buat maju, bukan buat mundur
// kalo pake Vector3.back , akibatnya mau tekan tombol panah atas supaya maju malah jadi mundur
////    Vector3 movementDirection = Quaternion.Euler(0f, rotationAngle, 0f) * Vector3.forward;

// player sudah punya arah input dari keyboard tapi belum bisa gerak karena belum ada forcenya
// berikutnya memberikan physics ( ada force, ada gravitasi ) 
// menggunakan rigidbody yaitu AddForce
// dengan parameter movementDirection dikali Time.deltaTime dikali variabel _walkSpeed;
////    _rigidBody.AddForce(movementDirection * Time.deltaTime * _walkSpeed);

// kita juga harus memastikan ada input( tombol ) yang ditekan
// gunakan nilai axisDirection
// jika axisDirection.magnitude lebih besar dari 0,1
// berarti ada input atau tombol yang ditekan , baru proses movementDirection
// jika tidak ada, jangan proses 
// kalo tidak ada proses periksa ini, akibatnya Player akan selalu bergerak
//meskipun tidak ada tombol yang ditekan


////}

//=========================================================

////    private void Move(Vector2 axisDirection) // method Move with rotation dan cek tombol
//    {
//       if (axisDirection.magnitude >= 0.1) 
//        {
//            float rotationAngle = Mathf.Atan2(axisDirection.x, axisDirection.y) * Mathf.Rad2Deg;

// supaya lebih halus rotasinya kalikan dengan Mathf.SmoothDampAngle
// nilai rotasi awal dari player = transform.eulerAngles.y 
// nilai sudut rotasi tujuan = rotationAngle
//            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationAngle, ref _rotationSmoothVelocity, _rotationSmoothTime);

// berikutnya ganti rotationAngle jadi smoothAngle
//transform.rotation = Quaternion.Euler(0f, rotationAngle, 0f);
//            transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);


//Vector3 movementDirection = Quaternion.Euler(0f, rotationAngle, 0f) * Vector3.forward;

// ganti _walkSpeed menjadi _speed
//_rigidBody.AddForce(movementDirection * _walkSpeed * Time.deltaTime);                
//            _rigidBody.AddForce(movementDirection * _speed * Time.deltaTime);                
//        }
//    }

// buat method Move untuk gerakin player
// method Move akan menerima data dari Class InputManager
// jadi sediakan parameter dengan tipe Vector2 , namanya axisDirection

////    private void Move(Vector2 axisDirection) // method Move with isPlayerClimbing checking  // lalu ditambah _cameraTransform.eulerAngles.y ke rotationAngle
//    {
// buat 2 variabel untuk check lagi berdiri atau lagi manjat 
//        bool isPlayerStanding = _playerStance == PlayerStance.Stand;
//        bool isPlayerClimbing = _playerStance == PlayerStance.Climb;

//        Vector3 movementDirection = Vector3.zero;


// lalu cek dulu lagi berdiri atau lagi manjat

//     if (isPlayerStanding)
//     {
//            if (axisDirection.magnitude >= 0.1) 
//            {
//                float rotationAngle = Mathf.Atan2(axisDirection.x, axisDirection.y) * Mathf.Rad2Deg + _cameraTransform.eulerAngles.y;

//                float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationAngle, ref _rotationSmoothVelocity, _rotationSmoothTime);

//                transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);

//                movementDirection = Quaternion.Euler(0f, rotationAngle, 0f) * Vector3.forward;

//                _rigidBody.AddForce(movementDirection * _speed * Time.deltaTime);


//            }

//     }
//     else if (isPlayerClimbing)
//     {
//            Vector3 horizontal = axisDirection.x * transform.right;

//            Vector3 vertical = axisDirection.y * transform.up;

//            movementDirection = horizontal + vertical;

//            _rigidBody.AddForce(movementDirection * _speed * Time.deltaTime);

//     }
// }

// atur animasi Third person movement only
//   Vector3 velocity = new Vector3(_rigidBody.velocity.x, 0, _rigidBody.velocity.z);
//   _animator.SetFloat("Velocity", velocity.magnitude * axisDirection.magnitude);
