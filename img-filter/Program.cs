using System;

// Step 1 - Define Component Interface
public interface IImage
{
    void Display(); // Metode untuk menampilkan gambar (atau efek filter)
}

// Step 2 - Buat ConcreteComponent, sebagai objek dasar yang akan didekorasi.
public class BaseImage : IImage
{
    private string _filename;

    public BaseImage(string filename)
    {
        _filename = filename;
        Console.WriteLine($"Memuat gambar asli: {_filename}");
    }

    public void Display()
    {
        Console.WriteLine($"Menampilkan gambar dasar: {_filename}");
    }
}

// Step 3 - Buat Abstract Decorator, sebagai abstract class yang mengimplementasikan antarmuka Component dan menyimpan referensi ke objek Component.
public abstract class ImageFilterDecorator : IImage
{
    protected IImage _wrappedImage; // Objek IImage yang dibungkus

    // Konstruktor menerima objek IImage untuk dibungkus
    protected ImageFilterDecorator(IImage image)
    {
        _wrappedImage = image;
    }

    // Mendelegasikan pemanggilan ke objek yang dibungkus. Class turunan (ConcreteDecorator) akan menambahkan fungsionalitasnya di sini.
    public virtual void Display()
    {
        _wrappedImage.Display();
    }
}

// Step 4 - Buat ConcreteDecorators, untuk menambahkan fungsionalitas spesifik ke Component.

// Contoh 1: Dekorator untuk filter Hitam Putih
public class BlackAndWhiteFilter : ImageFilterDecorator
{
    public BlackAndWhiteFilter(IImage image) : base(image) { }

    public override void Display()
    {
        base.Display(); // Pertama, tampilkan gambar yang dibungkus (atau hasil filter sebelumnya)
        ApplyBlackAndWhiteFilter(); // Kemudian, terapkan filter spesifik ini
    }

    private void ApplyBlackAndWhiteFilter()
    {
        Console.WriteLine("Menerapkan filter Hitam Putih.");
    }
}

// Contoh 2: Dekorator untuk filter Sepia
public class SepiaFilter : ImageFilterDecorator
{
    public SepiaFilter(IImage image) : base(image) { }

    public override void Display()
    {
        base.Display();
        ApplySepiaFilter();
    }

    private void ApplySepiaFilter()
    {
        Console.WriteLine("Menerapkan filter Sepia.");
    }
}

// Contoh 3: Dekorator untuk filter Blur
public class BlurFilter : ImageFilterDecorator
{
    public BlurFilter(IImage image) : base(image) { }

    public override void Display()
    {
        base.Display();
        ApplyBlurFilter();
    }

    private void ApplyBlurFilter()
    {
        Console.WriteLine("Menerapkan filter Blur.");
    }
}

// Step 5 - Panggil yang telah dibuat tadi dalam method utama Main
public class ImageProcessorClient
{
    public static void Main(string[] args)
    {
        Console.WriteLine("--- Contoh Penggunaan Pola Decorator untuk Filter Gambar ---");

        // Membuat gambar dasar
        IImage myPhoto = new BaseImage("FotoLiburan.jpg");
        Console.WriteLine("\n[Menampilkan gambar asli]");
        myPhoto.Display();
        Console.WriteLine("----------------------------------------------------------");

        // Menerapkan filter Hitam Putih ke gambar asli
        IImage photoWithBW = new BlackAndWhiteFilter(myPhoto);
        Console.WriteLine("\n[Menampilkan gambar dengan filter Hitam Putih]");
        photoWithBW.Display();
        Console.WriteLine("----------------------------------------------------------");

        // Menerapkan filter Sepia ke gambar yang sudah diberi filter Hitam Putih
        IImage photoWithBWSepia = new SepiaFilter(photoWithBW);
        Console.WriteLine("\n[Menampilkan gambar dengan filter Hitam Putih lalu Sepia]");
        photoWithBWSepia.Display();
        Console.WriteLine("----------------------------------------------------------");

        // Menerapkan filter Blur dan Sepia langsung ke gambar asli secara berurutan
        // Urutan pembungkusan penting: myPhoto -> dibungkus BlurFilter -> dibungkus SepiaFilter
        IImage photoWithBlurAndSepia = new SepiaFilter(new BlurFilter(myPhoto));
        Console.WriteLine("\n[Menampilkan gambar dengan filter Blur lalu Sepia]");
        photoWithBlurAndSepia.Display();
        Console.WriteLine("----------------------------------------------------------");

        // Contoh lain: myPhoto -> dibungkus SepiaFilter -> dibungkus BlurFilter
        IImage photoWithSepiaAndBlur = new BlurFilter(new SepiaFilter(myPhoto));
        Console.WriteLine("\n[Menampilkan gambar dengan filter Sepia lalu Blur]");
        photoWithSepiaAndBlur.Display();
        Console.WriteLine("----------------------------------------------------------");
    }
}

/** 
Penjelasan Kode:
1. IImage, ini adalah antarmuka component kita. Semua gambar, baik yang asli maupun yang sudah diberi filter, akan mengimplementasikan antarmuka ini. 
    Method Display() digunakan untuk mensimulasikan penampilan gambar beserta efek filternya.
2. BaseImage, sebagai ConcreteComponent, yaitu class gambar dasar kita. class ini mengimplementasikan IImage dan merupakan objek yang akan kita hias atau tambahkan filter.
3. ImageFilterDecorator, sebagai class abstract decorator. Class ini juga mengimplementasikan IImage dan memiliki referensi ke objek IImage lain (_wrappedImage). 
    Konstruktornya menerima objek IImage yang akan dibungkus. Method Display()-nya secara default hanya memanggil Display() pada objek yang dibungkus.
4. BlackAndWhiteFilter, SepiaFilter, BlurFilter, adalah ConcreteDecorator. Masing-masing mewarisi ImageFilterDecorator dan menambahkan fungsionalitas filter spesifik. 
    Dalam method Display() akan dilakukan hal berikut:
    - Memanggil base.Display() terlebih dahulu. Ini memastikan bahwa Display() dari objek yang dibungkus (yang bisa jadi BaseImage atau Decorator lain) dipanggil.
    - Setelah itu, mereka "menerapkan" filter mereka sendiri (disimulasikan dengan Console.WriteLine).
5. ImageProcessorClient sebagai class untuk method utama Main yang menunjukkan cara penggunaan pola Decorator.
   - Kita mulai dengan objek BaseImage.
   - Kemudian, kita "membungkus" objek BaseImage tersebut dengan berbagai ConcreteDecorator (filter).
   - Penting untuk diperhatikan bahwa kita bisa membungkus sebuah decorator dengan decorator lain, menciptakan rantai filter. 
     Urutan pembungkusan menentukan urutan penerapan filter.

Cara Kerja:
Ketika Anda memanggil Display() pada objek decorator paling luar (misalnya, photoWithBWSepia.Display()), 
panggilan tersebut akan diteruskan ke bawah melalui rantai decorator hingga mencapai BaseImage. 
Setelah BaseImage.Display() dieksekusi, setiap decorator dalam rantai (dalam urutan terbalik dari pembungkusan) akan menambahkan perilakunya sendiri.

Output dari program ini akan menunjukkan urutan pemuatan gambar dan penerapan filter secara berlapis.
**/