using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _walkSpeed;

    [SerializeField]
    private InputManager _input;

    [SerializeField]
    private float _rotationSmoothTime = 0.1f;

    private float _rotationSmoothVelocity;

    private Rigidbody _rigidBody;

    private void Awake()
    {
        _rigidBody = GetComponent<Rigidbody>();   // akses RigidBody via script di objek yang sama yaitu Player
    }


    private void Start()
    {
        _input.OnMoveInput += Move;   // PROCESS SUBSCRIBE
    }

    private void OnDestroy()
    {
        _input.OnMoveInput -= Move; // PROCESS UNSUBSCRIBE
    }

    // buat method Move untuk gerakin player
    // method Move akan menerima data dari Class InputManager
    // jadi sediakan parameter dengan tipe Vector2 , namanya axisDirection

    private void Move(Vector2 axisDirection) // method Move with rotation dan cek tombol
    {
       if (axisDirection.magnitude >= 0.1) 
        {
            float rotationAngle = Mathf.Atan2(axisDirection.x, axisDirection.y) * Mathf.Rad2Deg;

            // supaya lebih halus rotasinya kalikan dengan Mathf.SmoothDampAngle
            // nilai rotasi awal dari player = transform.eulerAngles.y 
            // nilai sudut rotasi tujuan = rotationAngle
            float smoothAngle = Mathf.SmoothDampAngle(transform.eulerAngles.y, rotationAngle, ref _rotationSmoothVelocity, _rotationSmoothTime);

            // berikutnya ganti rotationAngle jadi smoothAngle
            //transform.rotation = Quaternion.Euler(0f, rotationAngle, 0f);
            transform.rotation = Quaternion.Euler(0f, smoothAngle, 0f);


            Vector3 movementDirection = Quaternion.Euler(0f, rotationAngle, 0f) * Vector3.forward;

            _rigidBody.AddForce(movementDirection * Time.deltaTime * _walkSpeed);                
        }
            
    }

    

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
