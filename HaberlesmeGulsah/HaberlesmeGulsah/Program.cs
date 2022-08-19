using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using Newtonsoft.Json; //Install-Package Newtonsoft.Json -Version 11.0.2 consola kurdum

namespace HaberlesmeGulsah
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("m for multicaster, r for receiver");
            string klavye = Console.ReadLine();

            //instance çağırınca fonskiyonlarımın static olmasına gerek kalmıyor
            //ama eger static olmalarını istiyorsam instancedan çağırmamam gerekecektir.
            var instanceım = new Program();

            if(klavye == "m")
            {
                instanceım.Multicaster();
            }
            else if(klavye == "r")
            {
                instanceım.Recevier();
            }
        }

        void Multicaster()
        {
            //Burada multicast yayın yapacak olan programımın ip addresini ve portunu belirttim.
            //Bu bilgiler senderEndPoint isimli endpoitte tutuluyor şu an.
            string multicasterIpAddress = "239.255.255.255";
            int multicastPort = 8758;
            IPEndPoint senderEndPoint = new IPEndPoint(IPAddress.Parse(multicasterIpAddress), Convert.ToInt32(multicastPort));

            //senderIP isimli endpointe paket gönderme işlemi yapmam lazım.Yani Send işlemi.
            //Bu yüzden bir UDPClient nesnesi oluşturmalı ve bunu endpointe bağlayıp send fonksiyonu çağırmalıyım.
            UdpClient senderUDPClient = new UdpClient();
            senderUDPClient.Connect(senderEndPoint);
            
            
            //Deneme amaçlı dictionary tipinde bir paket oluşturalım.
            //Bu paket pil ve irtifa modelimizden gelecek olan verilerimizi içersin.
            Modellerim.PilModel pil = new Modellerim.PilModel();
            Modellerim.IrtifaModel irtifa = new Modellerim.IrtifaModel();

            while (true)
            {
                Console.WriteLine("Irtifa için yeni değer: ");
                int yeniIrtifa = 1; yeniIrtifa = Convert.ToInt32(Console.ReadLine());
                Console.WriteLine("Pil için yeni değer: ");
                int yeniPil = 1;  yeniPil = Convert.ToInt32(Console.ReadLine());
                

                uint bir = (uint)yeniPil;
                uint iki = (uint)yeniIrtifa;

                while ((bir!=1) && (iki!=1))
                {
                    Console.WriteLine("Döngüye girdi.");
                    //Bu paketin devamlı güncel hali alması için Unityde Update() fonksiyonun içinde tanımlanması gerekiyor diye düşünüyorum.
                    Dictionary<int, uint> paketim = new Dictionary<int, uint>();
                    paketim.Add(pil.ID, bir);
                    paketim.Add(irtifa.ID, iki);

                    //Paketim byte array tipinde olmak zorunda!!!
                    byte[] ByteaCevir(Dictionary<int, uint> paket)
                    {
                        BinaryFormatter bf = new BinaryFormatter();
                        MemoryStream ms = new MemoryStream();
                        bf.Serialize(ms, paket);
                        return ms.ToArray();
                    }

                    Byte[] byteVerim = ByteaCevir(paketim);

                    //Send methoduna parametre olarak gönderilecek olan bytler ve sayısını giriyorum. 
                    //Aslında endpoint değerleri yani ip address ve port numda girilebilir parameter olarak.
                    //Ama biz çoktan oluşturdugmuz objeyi senderin endpointine connect ettik. Burda gerek yok.
                    senderUDPClient.Send(byteVerim, byteVerim.Length);
                    Console.WriteLine(Encoding.UTF8.GetString(byteVerim));

                    //1000 demek 1 saniyeliğine askıya alması demek.
                    Thread.Sleep(1000);
                    break;
                }
            }

            //Close() ile işlem sonrası kapatabiliyoruz.
            senderUDPClient.Close();
            Console.WriteLine("Döngüden çıktı.");
            Console.ReadLine();

        }

        void Recevier()
        {
            //ONCE SUBSCRİBE OLMAM LAZIM!! Ben bir clientım.
            //Bu yüzden multicast yayın yapan servera (IP adressi ve porta) bağlanarak subscribe oluyoru.
            //Daha sonrasında oraya düşen paketleri receive() ile alıyorum.
            //Subscribe olurken JoinMulticastGroup fonk ile oluyorum.

            IPAddress multicasterIPADRESS = IPAddress.Parse("239.255.255.255");
            UdpClient listener = new UdpClient(8758);
            listener.JoinMulticastGroup(multicasterIPADRESS);
            
            IPEndPoint receivedIPInfo = new IPEndPoint(multicasterIPADRESS, 8758);
            Console.WriteLine("Alım başlıyor.");

            //Aldığım BYTE tipindeki verileri sonuc ismini verdiğim fonksiyon aracılığı ile dictionary tipine dönüştürüyorum.
            Dictionary<int, uint> sonuc(byte[] arrBytes)
            {
                MemoryStream memStream = new MemoryStream();
                BinaryFormatter binForm = new BinaryFormatter();
                memStream.Write(arrBytes, 0, arrBytes.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                Object obj = (Object)binForm.Deserialize(memStream);

                var json = JsonConvert.SerializeObject(obj);
                var dictionary = JsonConvert.DeserializeObject<Dictionary<int, uint>>(json);
                return dictionary;
            }

            PilClass pilObje = new PilClass();
            IrtifaClass irtifaObje = new IrtifaClass();
            Modellerim.PilModel pil = new Modellerim.PilModel();
            Modellerim.IrtifaModel irtifa = new Modellerim.IrtifaModel();

            
            while (true)
            {
                //verim adını verdiğim Byte array tipindeki değişkenim multicasterın attıgı verileri dinleyen listener UDPClient objemin aldığı verileri tutuyor.
                Byte[] verim = listener.Receive(ref receivedIPInfo);


                Console.WriteLine("\nilk pil degerim = " + pil.Deger);
                Console.WriteLine("ilk irtifa degerim = " + irtifa.Deger);

                //verim'i sonuc fonskyionuma atıyorum ve dictionary tipine dönüştürmüş oluyorum. Sonra bunları konsola yazırıyorum.
                Dictionary<int, uint> topluVerim = (sonuc(verim));
                foreach (var group in topluVerim)
                {
                    Console.WriteLine("\nKey: {0} Value: {1}", group.Key, group.Value);

                    //Pildegeri veya irtifa değerini arayüzde değiştirmek istiyorum.
                    if (group.Key == 0)
                    {
                        pilObje.PilYuzdesiDegistir(group.Value);
                        Console.WriteLine("Guncel pil degerim = " + pil.Deger);
                    }
                    else if (group.Key == 1)
                    {
                        irtifaObje.IrtifaDegeriDegistir(group.Value);
                        Console.WriteLine("Guncel irtifa degerim = " + irtifa.Deger + "\n\n--------------------------------------");
                    }
                }
            }

            Console.WriteLine("Alım bitti.");
            Console.ReadLine();
        }
    }
}
