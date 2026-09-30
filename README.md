# Data Mahasiswa

Aplikasi desktop sederhana untuk mengelola data mahasiswa menggunakan
**C# Windows Forms**.\
Project ini dibuat menggunakan **Visual Studio 2022**.

## Deskripsi

**Data Mahasiswa** merupakan aplikasi Student Management System yang
digunakan untuk mengelola informasi mahasiswa seperti:

-   NIM
-   Nama lengkap
-   Program studi
-   IPK

Aplikasi menyediakan beberapa fungsi utama seperti menambahkan data,
mengedit data, menghapus data, mencari data mahasiswa, serta menampilkan
ringkasan data pada dashboard.

## Fitur

-   Dashboard data mahasiswa
-   Menambahkan data mahasiswa
-   Validasi data kosong
-   Validasi NIM agar tidak duplikat
-   Validasi IPK dengan rentang 0.00--4.00
-   Menampilkan data mahasiswa dalam tabel
-   Mencari mahasiswa berdasarkan NIM, nama, atau program studi
-   Mengedit data mahasiswa
-   Menghapus data mahasiswa dengan konfirmasi
-   Menampilkan jumlah mahasiswa
-   Menampilkan jumlah program studi
-   Menampilkan rata-rata IPK
-   Menampilkan data mahasiswa dengan IPK tertinggi pada dashboard
-   Tombol refresh dan keluar aplikasi

## Teknologi yang Digunakan

-   **Language:** C#
-   **Framework:** Windows Forms
-   **IDE:** Visual Studio 2022
-   **.NET:** Sesuai target framework pada project Visual Studio

------------------------------------------------------------------------

# Dokumentasi Penggunaan

## 1. Dashboard

Saat aplikasi dijalankan, pengguna dapat melihat halaman dashboard yang
berisi ringkasan data mahasiswa.

Dashboard menampilkan:

-   Total mahasiswa
-   Jumlah program studi
-   Rata-rata IPK
-   Tabel data mahasiswa

<img width="2557" height="1437" alt="01-dashboard" src="https://github.com/user-attachments/assets/bcb57a66-1cd7-409e-bef0-a69325686df4" />

------------------------------------------------------------------------

## 2. Pengisian Data Mahasiswa

Untuk menambahkan mahasiswa baru, pengguna dapat memilih menu
**Tambah**.

Form yang tersedia terdiri dari:

-   NIM
-   Nama Lengkap
-   Program Studi
-   IPK

Setelah seluruh data diisi, klik tombol **Simpan Data**.

<img width="2551" height="1437" alt="02-tambah-mahasiswa" src="https://github.com/user-attachments/assets/8eb5564b-9e8d-4f33-9880-1ca6d2271a2a" />

------------------------------------------------------------------------

## 3. Validasi Data Kosong

Aplikasi melakukan validasi sebelum data disimpan.

Jika terdapat field yang belum diisi, aplikasi akan menampilkan pesan:

> Semua data harus diisi.

<img width="2557" height="1437" alt="03-validasi-kosong" src="https://github.com/user-attachments/assets/cb6b81bc-641d-4697-8ff1-5093977e1a2b" />

------------------------------------------------------------------------

## 4. Validasi IPK

IPK harus berada pada rentang **0.00 sampai 4.00**.

Jika pengguna memasukkan nilai di luar rentang tersebut, aplikasi akan
menampilkan pesan validasi.

<img width="2557" height="1405" alt="04-validasi-ipk" src="https://github.com/user-attachments/assets/fc44aa9b-1b7f-41b1-8293-82430b0b3ef1" />

Validasi tersebut diterapkan ketika data mahasiswa akan ditambahkan.

------------------------------------------------------------------------

## 5. Data Berhasil Ditambahkan

Jika seluruh data valid, aplikasi akan menampilkan notifikasi bahwa data
mahasiswa berhasil ditambahkan.

<img width="2546" height="1437" alt="05-data-berhasil-ditambahkan" src="https://github.com/user-attachments/assets/bc4bc210-0d51-425c-b282-fe92019c3696" />

Data kemudian ditampilkan pada halaman **Data Mahasiswa**.

------------------------------------------------------------------------

## 6. Menampilkan dan Mencari Data Mahasiswa

Halaman **Data Mahasiswa** menampilkan data dalam bentuk tabel dengan
informasi:

  No   NIM          Nama     Program Studi        IPK    Aksi
  ---- ------------ -------- -------------------- ------ --------------
  1    5025241048   Candra   Teknik Informatika   3.98   Edit / Hapus
  2    5025241037   Budi     Teknik Informatika   3.66   Edit / Hapus

Pengguna juga dapat menggunakan kolom pencarian untuk mencari
berdasarkan:

-   NIM
-   Nama
-   Program Studi

<img width="2382" height="418" alt="06-data-mahasiswa" src="https://github.com/user-attachments/assets/6e3f63bf-40af-4a96-a49f-0c07b55b436e" />

Fitur pencarian pada kode melakukan pencocokan terhadap NIM, nama, dan
program studi.

------------------------------------------------------------------------

## 7. Edit Data Mahasiswa

Pengguna dapat memilih tombol **Edit** pada data mahasiswa.

Form edit akan menampilkan data mahasiswa yang dipilih dan memungkinkan
pengguna memperbarui:

-   Nama
-   Program Studi
-   IPK

NIM ditampilkan sebagai identitas data yang sedang diedit.

<img width="2557" height="1437" alt="07-edit-mahasiswa" src="https://github.com/user-attachments/assets/d5d7b72d-fb5c-4fd9-af36-93909dbc84fc" />

------------------------------------------------------------------------

## 8. Pencarian Data

Pengguna dapat memasukkan kata kunci pada kolom pencarian.

Pencarian dapat dilakukan berdasarkan:

-   NIM
-   Nama
-   Program Studi

Contohnya, memasukkan sebagian NIM akan menampilkan mahasiswa yang
sesuai dengan kata kunci tersebut.

<img width="2557" height="1437" alt="08-pencarian" src="https://github.com/user-attachments/assets/d8ae1d09-b2de-4c08-a5a5-4a6e24771ba3" />

------------------------------------------------------------------------

## 9. Menghapus Data Mahasiswa

Tombol **Hapus** digunakan untuk menghapus data mahasiswa.

Sebelum data dihapus, aplikasi akan meminta konfirmasi kepada pengguna.

<img width="2557" height="1437" alt="Screenshot 2026-09-30 125836" src="https://github.com/user-attachments/assets/cc9073b8-9196-42cc-a8de-13eba7ecb3bf" />

Jika pengguna memilih **Yes**, data mahasiswa akan dihapus dari daftar.

<img width="2557" height="1437" alt="Screenshot 2026-09-30 125844" src="https://github.com/user-attachments/assets/a96e08cf-0f85-4113-a645-4ee25b13a3e6" />

------------------------------------------------------------------------

## 10. Keluar dari Aplikasi

Tombol **Keluar** digunakan untuk menutup aplikasi.

Aplikasi menampilkan dialog konfirmasi terlebih dahulu sehingga pengguna
dapat memilih apakah ingin benar-benar keluar dari aplikasi.

<img width="2557" height="1437" alt="09-konfirmasi-keluar" src="https://github.com/user-attachments/assets/4b120a2d-16c3-4830-81b9-51b13a52dbbe" />

------------------------------------------------------------------------

# Cara Menjalankan Project

## Menggunakan Visual Studio 2022

1.  Clone atau download repository.
2.  Buka file:

``` text
DataMahasiswa.sln
```

3.  Project akan terbuka di **Visual Studio 2022**.
4.  Pastikan project `DataMahasiswa` menjadi **Startup Project**.
5.  Jalankan aplikasi menggunakan:

``` text
F5
```

atau tombol **Start** pada Visual Studio.

------------------------------------------------------------------------

# Catatan

Project ini menggunakan Windows Forms sehingga aplikasi dijalankan
sebagai aplikasi desktop Windows.

Data mahasiswa dikelola menggunakan object `Mahasiswa` yang memiliki
properti:

-   `NIM`
-   `Nama`
-   `Prodi`
-   `IPK`

Class tersebut digunakan sebagai model data mahasiswa dalam aplikasi.
