using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private float _walkSpeed;

    [SerializeField]
    private InputManager _input;


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
    private void Move(Vector2 axisDirection) 
    {
        // karena player akan bergerak ke sumbu x,y,z gunakan Vector3
        // untuk sumbu x berarti player akan bergerak ke samping ( axis warna merah)
        // untuk sumbu y berarti player akan bergerak ke atas ( axis warna hijau)
        // untuk sumbu z berarti player akan bergerak ke depan ( axis warna biru)
        // jadi kita masukin x = axisDirection.x , karena kita mau gerakin player ke kiri atau ke kanan berdasarkan axisDirection di sumbu x
        // jadi kita masukin y = 0 , karena player tidak menerima input gerak ke atas atau ke bawah 
        // jadi kita masukin x = axisDirection.y , karena kita mau gerakin player ke depan atau ke belakang berdasarkan axisDirection di sumbu y


        Vector3 movementDirection = new Vector3(axisDirection.x, 0 , axisDirection.y);
        Debug.Log(movementDirection);

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

        _rigidBody.AddForce(movementDirection * _walkSpeed * Time.deltaTime);

        // supaya tidak bergantung ke frame rate ( ke jumlah frame), kalikan dengan Time.deltaTime
        // jangan lupa di inspector walkspeed diatur jadi 350
        // dan di  inspector rigidbody -> constraints -> freeze rotation XYZ di centang semuanya
        // supaya player gak jatuh pas bergerak maju
        // jika player sudah dikasih force tapi masih punya rotation XYZ
        // akibatnya player jatuh karena bergerak sambil masih punya rotasi
    }
}
