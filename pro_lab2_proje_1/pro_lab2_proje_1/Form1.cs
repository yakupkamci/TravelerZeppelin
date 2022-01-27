using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;/*Dosya İşlemleri için Kullanılmıştır...*/
using System.Reflection;/*Dosya Adresleri için Kullanılmıştır.*/
using System.Diagnostics;/*Kodun Çalışma Süresini Bulmak için Gereken İsim Uzayı...*/
namespace pro_lab2_proje_1
{
    /*160201135 YAKUP KAMÇI - 160201141 CENGİZ ATİLA*/
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        public struct iller
        {
           public double enlem;
           public double boylam;
           public int plaka;
           public int rakim;
           public int[] komsuluk;          
        };        /*İl Bilgileri...*/
        iller[] il = new iller[81];   /*Struct Nesnesi...*/  
        double[,] komsuluk_m_matrisi = new double[81, 81]; /*Komşuluk Maliyet Matrisi*/
        int dunya_r = 6373;/*Mesafe Hesaplarken Gerekli Değişken(dünyanın y_çapı)*/
        int buton_say = 0;/*Tekrar İşlemler için Butonu Sayan Değişken*/
        int sabit_bilet_fiyati = 100;/*Maliyet için Gerekli Değişken*/
        
        /*Kontrol Değişkenleri*/
        int xx=0,xy=0;
        bool cntrl = false;
        bool cntrl2 = false;       

        /*Dosyalama için Değişkenler/*/
        int p1, p2;
        int[] d_egim = new int[45];
        string[] gzrgh=new string[45];
        string[] list2=new string[45];

        /*Çalışma Süresi için Değişkenler*/
        Stopwatch gecen_zaman = new Stopwatch();
        TimeSpan total_zaman;

        /*Çizim için Değişkenler*/
        Graphics grafik;

        string[] il_isimleri = {"ADANA","ADIYAMAN","AFYON","AĞRI","AMASYA","ANKARA","ANTALYA","ARTVİN","AYDIN","BALIKESİR",
            "BİLECİK","BİNGÖL","BİTLİS","BOLU","BURDUR","BURSA","ÇANAKKALE","ÇANKIRI","ÇORUM","DENİZLİ",
            "DİYARBAKIR","EDİRNE","ELAZIĞ","ERZİNCAN","ERZURUM","ESKİŞEHİR","GAZİANTEP","GİRESUN","GÜMÜŞANE","HAKKARİ",
            "HATAY","ISPARTA","MERSİN","İSTANBUL","İZMİR","KARS","KASTAMONU","KAYSERİ","KIRKLARELİ","KIRŞEHİR",
            "KOCAELİ","KONYA","KÜTAHYA","MALATYA","MANİSA","K. MARAŞ","MARDİN","MUĞLA","MUŞ","NEVŞEHİR",
            "NİĞDE","ORDU","RİZE","SAKARYA","SAMSUN","SİİRT","SİNOP","SİVAS","TEKİRDAĞ","TOKAT",
            "TRABZON","TUNCELİ","ŞANLIURFA","UŞAK","VAN","YOZGAT","ZONGULDAK","AKSARAY","BAYBURT","KARAMAN",
            "KIRIKKALE","BATMAN","ŞIRNAK","BARTIN","ARDAHAN","IĞDIR","YALOVA","KARABÜK","KİLİS","OSMANİYE","DÜZCE"
        };

        int[,] koordinat = { {590,530}, {740,460}, {300,360}, {1055,240}, {600,175}, {425,240}, {325,525}, {940,110}, {145,445}, {130,230},
            {265,215}, {900,310}, {990,370}, {380,160}, {255,485}, {220,205}, {80,200}, {475,180}, {550,185}, {200,450},
            {885,410}, {78,50}, {810,360}, {810,250}, {950,230}, {310,255}, {720,530}, {760,165}, {820,190}, {1100,440},
            {645,575}, {315,475}, {535,535}, {210,115}, {115,400}, {1040,140}, {480,110}, {605,370}, {115,45}, {505,325},
            {255,135}, {405,430}, {245,290}, {755,400}, {130,360}, {675,475}, {915,490}, {150,505}, {950,325}, {540,380},
            {535,445}, {700,160}, {890,120}, {290,155}, {625,110}, {985,420}, {550,80}, {675,260}, {100,100}, {650,205},
            {830,130}, {830,315}, {795,510}, {215,360}, {1095,340}, {540,260}, {375,105}, {495,405}, {865,195}, {455,525},
            {470,255}, {940,430}, {1020,455}, {405,80}, {1005,95}, {1095,205}, {215,170}, {415,120}, {695,560}, {640,535}, {335,155}
        };

        private void il_bilgileri()
        {
            string klasor_yolu, gerekli_yol, ilk, cumle;
            string[] kelime;
            int cumle_say =0;
            gecen_zaman.Start();
            klasor_yolu = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);/*Programın .exe Dosyasının Adresini Bulur...*/
            DirectoryInfo di = new DirectoryInfo(klasor_yolu);/*Belirtilen Yoldaki Klasör Bilgilerini Edinir...*/
              if (File.Exists(klasor_yolu + "\\latlong.txt"))/*Belirtilen İsimde Dosya Olup Olmadığını Sorar...*/
                 {
                    gerekli_yol = klasor_yolu + "\\latlong.txt";
                    StreamReader oku = new StreamReader(gerekli_yol);
                    ilk = oku.ReadLine();
                    cumle = oku.ReadLine();
                    while (cumle != null)
                    {
                       kelime = cumle.Split(',');                   
                       il[cumle_say].enlem = double.Parse(kelime[0],System.Globalization.CultureInfo.InvariantCulture);
                       il[cumle_say].boylam = double.Parse(kelime[1], System.Globalization.CultureInfo.InvariantCulture);
                       il[cumle_say].plaka = Convert.ToInt32(kelime[2]);             
                       il[cumle_say].rakim = Convert.ToInt32(kelime[3]);   
                       cumle_say++;
                       cumle = oku.ReadLine();                  
                    }
                    oku.Close();
               
                }
                else
                {
                    MessageBox.Show("Lat Long Dosyası Okumasında Bir Hata Tespit Edildi...");
                }

                klasor_yolu = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);/*Programın .exe Dosyasının Adresini Bulur...*/
                DirectoryInfo di2 = new DirectoryInfo(klasor_yolu);/*Belirtilen Yoldaki Klasör Bilgilerini Edinir...*/
                if (File.Exists(klasor_yolu + "\\komsular.txt"))/*Belirtilen İsimde Dosya Olup Olmadığını Sorar...*/
                {
                    gerekli_yol = klasor_yolu + "\\komsular.txt";
                    StreamReader oku = new StreamReader(gerekli_yol);
                    ilk = oku.ReadLine();
                    cumle = oku.ReadLine();
                    cumle_say = 0;
                    while (cumle != null)
                    {
                        kelime = cumle.Split(',');
                        il[cumle_say].komsuluk = new int[kelime.Length-1];
                        for(int i = 1; i < kelime.Length; i++)
                        {
                            il[cumle_say].komsuluk[i-1] = Convert.ToInt32(kelime[i]);
                            
                        }
                    cumle_say++;
                    cumle = oku.ReadLine();
                    if (cumle_say == 81) break;                   
                    }
                    oku.Close();
                }
                else
                {
                    MessageBox.Show("Komşuluklar Dosyası Okumasında Bir Hata Tespit Edildi...");
                }
            gecen_zaman.Stop();
            total_zaman += gecen_zaman.Elapsed;       
            
        }/*Komşuluk ve Lat Long gibi Bilgileri Structa Aktarım Yapmaktadır...*/
        
        private void komsuluk_maliyet_matrisi()
        {
            int komsu_sayisi, say = 0;
            gecen_zaman.Start();
            for (int i = 0; i < 81; i++)
            {
                komsu_sayisi = il[i].komsuluk.Length;
               for(int j = 0; j < 81; j++)
                {
                    if (i == j) komsuluk_m_matrisi[i, j] = 0;
                    else
                    {
                        for(int k = 0; k < komsu_sayisi; k++)
                        {
                            if (il[i].komsuluk[k] == il[j].plaka) say++;
                        }
                        if (say == 0) komsuluk_m_matrisi[i,j] = 0;
                        else
                        {
                            komsuluk_m_matrisi[i, j] = mesafe_hesapla(il[i].enlem, il[i].boylam,il[j].enlem,il[j].boylam,il[i].rakim,il[j].rakim);
                        }
                        say = 0;
                    }
                }
            }
            gecen_zaman.Stop();
            total_zaman += gecen_zaman.Elapsed;
        }/*İller Arası Komşuluk Maliyetini Hesaplar...*/

        private double mesafe_hesapla(double g_lat1,double g_long1,double g_lat2,double g_long2,double rkm1,double rkm2)
        {
            double lat1, long1, lat2, long2, fark_lat, fark_long, a, b,d,snc,km;
            gecen_zaman.Start();
            lat1 = g_lat1 * Math.PI / 180;
            long1 = g_long1 * Math.PI / 180;
            lat2 = g_lat2 * Math.PI / 180;
            long2 = g_long2 * Math.PI / 180;

            fark_lat = lat2 - lat1;
            fark_long = long2 - long1;

            a = Math.Pow(Math.Sin(fark_lat / 2), 2) + Math.Cos(lat1) * Math.Cos(lat2) * Math.Pow(Math.Sin(fark_long / 2), 2);
            b = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            d = b * dunya_r;
            snc = Math.Round(d, 3);
            km =Math.Round(Math.Sqrt(Math.Pow((Math.Abs(rkm1-rkm2+50)/1000),2)+Math.Pow(snc,2)),2);
            gecen_zaman.Stop();
            total_zaman += gecen_zaman.Elapsed;
            return km;
        } /*Şehirler Arası Mesafe Bulmaktadır...*/

        private int min_egim_hasaplama(double mesafe,int rkm1,int rkm2)
        {
            int egm = 0;
            double a;
            gecen_zaman.Start();
            a = Math.Atan(Math.Abs(rkm1 - rkm2+50) / mesafe);
            egm = Convert.ToInt32(a*180/Math.PI);
            gecen_zaman.Stop();
            total_zaman += gecen_zaman.Elapsed;
            return egm;
        } /*İller Arası Eğimi Bulan Fonk.*/

        private int yolcu_sayisina_g_egim(int sayi)
        {
            return 80-sayi;
        }/*Zeplin Manevrası Hesaplayan Fonk.*/

        private int min_mesafe(double[] mesafe, bool[] kontrol)
        {
            double min = double.MaxValue;
            int minIndex = 0;
            gecen_zaman.Start();
            for (int v = 0; v < 81; ++v)
            {
                if (kontrol[v] == false && mesafe[v] <= min)
                {
                    min = mesafe[v];
                    minIndex = v;
                }
            }
            gecen_zaman.Stop();
            total_zaman += gecen_zaman.Elapsed;
            return minIndex;
        }/*Yol Bulma Algoritmasında Kullanılan Fonk.*/
        
        public void sonuc(string[]R,double[] mesafe,int bitis_sehri)
        {
            int kap=0,ind1,ind2,kisi_sayisi;
            double maliyet, total_kazanc, kar, kar_orani;
            string[] tut;
            grafik= pictureBox1.CreateGraphics();
            Pen kalem = new Pen(System.Drawing.Color.Blue, 7);
            Graphics grafikk = pictureBox1.CreateGraphics();
            SolidBrush ciz = new SolidBrush(Color.Red);
            gecen_zaman.Start();
            
            tut = R[bitis_sehri].Split(',');//Metin içindeki Virgüllere Göre Ayırma Yapar...
                if(tut[tut.Length - 1] != " ")
                {
                kap =Convert.ToInt32(tut[tut.Length - 1]);
                
                    if (bitis_sehri == kap)
                    {
                        if (radioButton1.Checked == true)
                        {
                            listBox2.Items.Add(R[bitis_sehri]);
                            list2[xx] = mesafe[bitis_sehri].ToString();
                            gzrgh[xx]= R[bitis_sehri];
                            xx++;
                            cntrl = true;
                        gecen_zaman.Stop();
                        total_zaman += gecen_zaman.Elapsed;
                        }
                        else if (radioButton2.Checked == true)
                        {
                            listBox2.Items.Add(R[bitis_sehri]);
                            list2[xy] = mesafe[bitis_sehri].ToString();
                            gzrgh[xy] = R[bitis_sehri];
                            xy++;
                            cntrl2 = true;
                        gecen_zaman.Stop();
                        total_zaman += gecen_zaman.Elapsed;
                        }
                        else
                        {
                            for (int j = 0; j < tut.Length; j++)
                            {
                                listBox1.Items.Add("     " + il_isimleri[Convert.ToInt32(tut[j])]);
                            }

                            for (int j = 0; j < tut.Length - 1; j++)
                            {
                                ind1 = Convert.ToInt32(tut[j]);
                                Point nokta1 = new Point(koordinat[ind1, 0], koordinat[ind1, 1]);
                                ind2 = Convert.ToInt32(tut[j + 1]);
                                Point nokta2 = new Point(koordinat[ind2, 0], koordinat[ind2, 1]);
                                grafikk.FillEllipse(ciz, new Rectangle(koordinat[ind1, 0], koordinat[ind1, 1], 10, 10));
                                grafikk.FillEllipse(ciz, new Rectangle(koordinat[ind2, 0], koordinat[ind2, 1], 10, 10));
                                grafik.DrawLine(kalem, nokta1, nokta2);
                                listBox2.Items.Add(min_egim_hasaplama(komsuluk_m_matrisi[ind1, ind2], il[ind1].rakim, il[ind2].rakim));
                            }
                            label14.Text = mesafe[bitis_sehri].ToString() + " Km";
                        kisi_sayisi = 80 - Convert.ToInt32(textBox3.Text);
                        maliyet = Math.Round((mesafe[bitis_sehri] * 10.0), 2);
                        total_kazanc = Convert.ToDouble(kisi_sayisi * sabit_bilet_fiyati);
                        kar = Convert.ToDouble(kisi_sayisi * sabit_bilet_fiyati) - Math.Round((mesafe[bitis_sehri] * 10.0), 2);
                        kar_orani = Math.Round((kar / maliyet) * 100, 1);
                        label21.Text = maliyet + " TL";
                        label23.Text = total_kazanc + " TL";
                        label25.Text = kar + " TL";
                        label27.Text = "% "+ kar_orani;
                        gecen_zaman.Stop();
                        total_zaman += gecen_zaman.Elapsed;
                    }
                        
                    }
                    else
                    {
                       if(radioButton1.Checked==false && radioButton2.Checked==false) MessageBox.Show("Girilen Yolcu Sayısıyla Hedefe Ulaşılamıyor, Şehirler Arası Eğim Zeplin Manevrasından Fazladır!!!", "Gezgin Zeplin", MessageBoxButtons.OK);                     
                    }      
                
                }
                else
                {
                    if (radioButton1.Checked == false && radioButton2.Checked==false)
                    {
                    MessageBox.Show("Girilen Yolcu Sayısıyla Hedefe Ulaşılamıyor, Şehirler Arası Eğim Zeplin Manevrasından Fazladır!!!", "Gezgin Zeplin", MessageBoxButtons.OK);
                    textBox3.Clear();
                    }
                   
                }
           
            
        }/*Haritaya Yolu Çizen ve Sonucu Gösteren Fonk.*/

        private void yol_bul_algoritmasi(int baslangic_sehri,int bitis_sehri)
        {
            double[] mesafe = new double[81];
            bool[] kontrol = new bool[81];
            string[] R = new string[81];
            gecen_zaman.Start();
            for (int i = 0; i < 81; i++)
            {
                R[i] = " ";
            }


            for (int i = 0; i < 81; ++i)
            {
                mesafe[i] = double.MaxValue;
                kontrol[i] = false;
            }

            mesafe[baslangic_sehri] = 0;

            for (int k = 0; k < 81; ++k)
            {
                int g_ind = min_mesafe(mesafe, kontrol);
                kontrol[g_ind] = true;

                for (int ind = 0; ind < 81; ++ind)
                {
                    if (!kontrol[ind] && Convert.ToBoolean(komsuluk_m_matrisi[g_ind, ind]) && mesafe[g_ind] != double.MaxValue && mesafe[g_ind] + komsuluk_m_matrisi[g_ind, ind] < mesafe[ind])
                    {
                        mesafe[ind] = mesafe[g_ind] + komsuluk_m_matrisi[g_ind, ind];
                        R[ind] = string.Copy(R[g_ind]);
                        R[ind] += g_ind + ",";
                    }
                }
            }
            for (int i = 0; i < 81; i++)
            {
                if (mesafe[i] != double.MaxValue)
                {
                    R[i] += i.ToString();
                }                           
            }
            gecen_zaman.Stop();
            total_zaman += gecen_zaman.Elapsed;
           sonuc(R, mesafe, bitis_sehri);
        }/*Dijkstra Algoritması*/

        private void Form1_Load(object sender, EventArgs e)
        {            
            gecen_zaman.Start();
            il_bilgileri();
            komsuluk_maliyet_matrisi();
            textBox3.Enabled = false;
            label3.Enabled = false;
            label8.Enabled = false;            
            gecen_zaman.Stop();
            total_zaman += gecen_zaman.Elapsed;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            basla:
            int plk_1 = 0, plk_2 = 0,kisi_sayisi = 0,zeplin_manevrasi,olasi_egim,v=0,v2=0;
            if (buton_say == 0)
            { 
                try
               {
                    int  i = 0;
                    gecen_zaman.Start();buton_say++;
                    for (i = 0; i < 81; i++) if (il_isimleri[i] == textBox1.Text.ToUpper()) { p1=plk_1 = i; break; }
                    if(i==81) { MessageBox.Show("Başlangıç Şehri Mevcut Değildir!!!", "Gezgin Zeplin", MessageBoxButtons.OK);textBox1.Clear();}
                    for (i = 0; i < 81; i++) if (il_isimleri[i] == textBox2.Text.ToUpper()) { p2=plk_2 = i; break; }
                    if (i == 81) { MessageBox.Show("Bitiş Şehri Mevcut Değildir!!!", "Gezgin Zeplin", MessageBoxButtons.OK); textBox2.Clear(); }
                    if (checkBox1.Checked == true)
                    {
                        kisi_sayisi = Convert.ToInt32(textBox3.Text); if (kisi_sayisi < 5 && kisi_sayisi > 50) { MessageBox.Show("Kişi Sayısı 5 ile 50 Arasında Olmalıdır!!!", "Gezgin Zeplin", MessageBoxButtons.OK); textBox3.Clear(); }
                        zeplin_manevrasi = yolcu_sayisina_g_egim(kisi_sayisi);
                        label15.Text = zeplin_manevrasi + " C";
                        for (i = 0; i < 81; i++)
                        {
                            for (int j = 0; j < 81; j++)
                            {
                                if (komsuluk_m_matrisi[i, j] != 0)
                                {
                                    olasi_egim = min_egim_hasaplama(komsuluk_m_matrisi[i, j], il[i].rakim, il[j].rakim);
                                    if (zeplin_manevrasi < olasi_egim) komsuluk_m_matrisi[i, j] = 0;
                                }
                            }
                        }
                        yol_bul_algoritmasi(plk_1, plk_2);
                        gecen_zaman.Stop();
                        total_zaman += gecen_zaman.Elapsed;
                    }
                    else
                    {
                        if (radioButton1.Checked == true)
                        {
                            for (int x = 50; x >= 5; x--)
                            {
                                zeplin_manevrasi = yolcu_sayisina_g_egim(x);
                                for (i = 0; i < 81; i++)
                                {
                                    for (int j = 0; j < 81; j++)
                                    {
                                        if (komsuluk_m_matrisi[i, j] != 0)
                                        {
                                            olasi_egim = min_egim_hasaplama(komsuluk_m_matrisi[i, j], il[i].rakim, il[j].rakim);
                                            if (zeplin_manevrasi < olasi_egim) komsuluk_m_matrisi[i, j] = 0;
                                        }
                                    }
                                }
                                yol_bul_algoritmasi(plk_1, plk_2);
                                if (cntrl == true) { d_egim[v] = zeplin_manevrasi;v++;cntrl = false; }                                
                                komsuluk_maliyet_matrisi();
                            }
                            if (v == 0) MessageBox.Show("Malesef İki Şehir Arasında Rota Oluşturulamadı !!! (!!Şehirler Arası Eğim, Zeplin Manevrasından Fazla!!)", "Gezgin Zeplin", MessageBoxButtons.OK);
                            label13.Text = "---Tüm Rotalar---";
                            label13.Text +="("+listBox2.Items.Count+")";                          
                            gecen_zaman.Stop();
                            total_zaman += gecen_zaman.Elapsed;
                        }
                        else if (radioButton2.Checked == true)
                        {
                            for (int x = 10; x <= 50; x+=10)
                            {
                                zeplin_manevrasi = yolcu_sayisina_g_egim(x);
                                for (i = 0; i < 81; i++)
                                {
                                    for (int j = 0; j < 81; j++)
                                    {
                                        if (komsuluk_m_matrisi[i, j] != 0)
                                        {
                                            olasi_egim = min_egim_hasaplama(komsuluk_m_matrisi[i, j], il[i].rakim, il[j].rakim);
                                            if (zeplin_manevrasi < olasi_egim) komsuluk_m_matrisi[i, j] = 0;
                                        }
                                    }
                                }
                                yol_bul_algoritmasi(plk_1, plk_2);
                                if (cntrl2 == true) { d_egim[v2] = zeplin_manevrasi; v2++; cntrl2 = false; }
                                komsuluk_maliyet_matrisi();
                            }
                            if (v2 == 0) MessageBox.Show("Malesef İki Şehir Arasında Rota Oluşturulamadı !!! (!!Şehirler Arası Eğim, Zeplin Manevrasından Fazla!!)", "Gezgin Zeplin", MessageBoxButtons.OK);
                            label13.Text = "---Tüm Rotalar---";
                            label13.Text += "(" + listBox2.Items.Count + ")";
                            gecen_zaman.Stop();
                            total_zaman += gecen_zaman.Elapsed;
                        }
                   }
                    label5.Text = il_isimleri[plk_1];
                    label7.Text = il_isimleri[plk_2];
                } catch {MessageBox.Show("Girişler Boş veya Sayısal Dışı Girilemez!!!", "Gezgin Zeplin", MessageBoxButtons.OK);textBox1.Clear();textBox2.Clear();textBox3.Clear();}
                
                             
                label10.Text = total_zaman.ToString();  
            }
            else
            {
                pictureBox1.Refresh();
                listBox1.Items.Clear();
                listBox2.Items.Clear();
                total_zaman=TimeSpan.Zero;
                gecen_zaman = new Stopwatch();
                v = 0;
                v2 = 0;
                xx = 0;
                xy = 0;
                p1 = 0;
                p2 = 0;
                d_egim = new int[45];
                gzrgh = new string[45];
                list2 = new string[45];
                cntrl = false;
                cntrl2 = false;
                komsuluk_maliyet_matrisi();
                buton_say = 0; 
                label10.Text = "";goto basla;           
            }
        }/*Hesaplatma Butonu*/

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBox1.Checked == true)
            {
                textBox3.Enabled = true;
                label3.Enabled = true;
                label8.Enabled = true;
                radioButton1.Enabled = false;
                radioButton2.Enabled = false;
                
                label13.Text = "--Şehirler Arası Eğimler--";
                label17.Visible = false;
                label18.Visible = false;
                label20.Visible = false;
                label19.Visible = false;
            }
            else
            {
                textBox3.Enabled = false;
                label3.Enabled = false;
                label8.Enabled = false;
                radioButton1.Enabled = true;
                radioButton2.Enabled = true;               
               
                label17.Visible = true;
                label18.Visible = true;
                label20.Visible = true;
                label19.Visible = true;
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            label13.Text = "---Tüm Rotalar---";
            label28.Text = "Kar Oranı:";
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            label13.Text = "---Tüm Rotalar---";
            label28.Text = "Bilet Fiyatı:";
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {            
            gecen_zaman.Start();
            if (radioButton1.Checked == true)
            {
                int ind1, ind2,kisi_sayisi=0;
                double maliyet, total_kazanc, kar, kar_orani;
                string[] tut=listBox2.GetItemText(listBox2.SelectedItem).Split(',');
                grafik = pictureBox1.CreateGraphics();
                Pen kalem = new Pen(System.Drawing.Color.Blue, 7);
                Graphics grafikk = pictureBox1.CreateGraphics();
                SolidBrush ciz = new SolidBrush(Color.Red);
                listBox1.Items.Clear();
                pictureBox1.Refresh();
                label19.Text = "";
                for (int j = 0; j < tut.Length; j++)
                {
                    listBox1.Items.Add("     " + il_isimleri[Convert.ToInt32(tut[j])]);
                }

                for (int j = 0; j < tut.Length - 1; j++)
                {
                    ind1 = Convert.ToInt32(tut[j]);
                    Point nokta1 = new Point(koordinat[ind1, 0], koordinat[ind1, 1]);
                    ind2 = Convert.ToInt32(tut[j + 1]);
                    Point nokta2 = new Point(koordinat[ind2, 0], koordinat[ind2, 1]);
                    grafikk.FillEllipse(ciz, new Rectangle(koordinat[ind1, 0], koordinat[ind1, 1], 10, 10));
                    grafikk.FillEllipse(ciz, new Rectangle(koordinat[ind2, 0], koordinat[ind2, 1], 10, 10));
                    grafik.DrawLine(kalem, nokta1, nokta2);
                   label19.Text+= min_egim_hasaplama(komsuluk_m_matrisi[ind1, ind2], il[ind1].rakim, il[ind2].rakim)+",";
                }
                label14.Text = list2[listBox2.SelectedIndex] + " Km";
                kisi_sayisi = 80 - d_egim[listBox2.SelectedIndex];
                label17.Text = kisi_sayisi+" Kişi";
                label15.Text = d_egim[listBox2.SelectedIndex].ToString();
                maliyet = Math.Round((Convert.ToDouble(list2[listBox2.SelectedIndex]) * 10.0), 2);
                label21.Text = maliyet +" TL";
                total_kazanc = Convert.ToDouble((80 - d_egim[listBox2.SelectedIndex]) * sabit_bilet_fiyati);
                label23.Text =total_kazanc+" TL";
                kar = Convert.ToDouble((80 - d_egim[listBox2.SelectedIndex]) * sabit_bilet_fiyati) - Math.Round((Convert.ToDouble(list2[listBox2.SelectedIndex]) * 10.0), 2);
                label25.Text = kar + " TL";
                kar_orani = Math.Round((kar/maliyet)*100, 1);
                label27.Text ="% "+kar_orani;
                gecen_zaman.Stop();
                total_zaman += gecen_zaman.Elapsed;
            }
            else if (radioButton2.Checked == true)
            {
                int ind1, ind2, kisi_sayisi = 0;
                double maliyet, total_kazanc,bilet_fiyati;
                string[] tut = listBox2.GetItemText(listBox2.SelectedItem).Split(',');
                grafik = pictureBox1.CreateGraphics();
                Pen kalem = new Pen(System.Drawing.Color.Blue, 7);
                Graphics grafikk = pictureBox1.CreateGraphics();
                SolidBrush ciz = new SolidBrush(Color.Red);
                listBox1.Items.Clear();
                pictureBox1.Refresh();
                label19.Text = "";
                for (int j = 0; j < tut.Length; j++)
                {
                    listBox1.Items.Add("     " + il_isimleri[Convert.ToInt32(tut[j])]);
                }

                for (int j = 0; j < tut.Length - 1; j++)
                {
                    ind1 = Convert.ToInt32(tut[j]);
                    Point nokta1 = new Point(koordinat[ind1, 0], koordinat[ind1, 1]);
                    ind2 = Convert.ToInt32(tut[j + 1]);
                    Point nokta2 = new Point(koordinat[ind2, 0], koordinat[ind2, 1]);
                    grafikk.FillEllipse(ciz, new Rectangle(koordinat[ind1, 0], koordinat[ind1, 1], 10, 10));
                    grafikk.FillEllipse(ciz, new Rectangle(koordinat[ind2, 0], koordinat[ind2, 1], 10, 10));
                    grafik.DrawLine(kalem, nokta1, nokta2);
                    label19.Text += min_egim_hasaplama(komsuluk_m_matrisi[ind1, ind2], il[ind1].rakim, il[ind2].rakim) + ",";
                }
                label14.Text = list2[listBox2.SelectedIndex] + " Km";
                kisi_sayisi = 80 - d_egim[listBox2.SelectedIndex];
                label17.Text = kisi_sayisi + " Kişi";
                label15.Text = d_egim[listBox2.SelectedIndex].ToString();
                maliyet = Math.Round((Convert.ToDouble(list2[listBox2.SelectedIndex]) * 10.0), 2);
                label21.Text = maliyet + " TL";
                bilet_fiyati =((maliyet+(maliyet * 50)/100)/kisi_sayisi);
                total_kazanc = bilet_fiyati * kisi_sayisi;
                label23.Text = total_kazanc + " TL";
                label25.Text = (total_kazanc-maliyet) + " TL";
                label27.Text = bilet_fiyati+" TL";
                label28.Text = "Bilet Fiyatı";
                gecen_zaman.Stop();
                total_zaman += gecen_zaman.Elapsed;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string klasor_yolu = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            double maliyet, total_kazanc, kar, kar_orani,bilet_fiyati;
            int kisi_sayisi,ind1,ind2;
           
            DirectoryInfo di = new DirectoryInfo(klasor_yolu);
            gecen_zaman.Start();
            if (radioButton1.Checked == true)
            {
                StreamWriter yaz = new StreamWriter(klasor_yolu + "\\Problem-1.txt");               
                yaz.WriteLine(textBox1.Text.ToUpper()+"-"+textBox2.Text.ToUpper()+" İlleri Arası Maliyet Tablosu(Kardan-Zarara)");
                yaz.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                yaz.WriteLine("Başlangıç Şehri Bilgileri(" + textBox1.Text.ToUpper() + ");");
                yaz.WriteLine("Plaka Kodu:" +il[p1].plaka);
                yaz.WriteLine("Rakım:" + il[p1].rakim);
                yaz.WriteLine("Enlem:" + il[p1].enlem);
                yaz.WriteLine("Boylam:" + il[p1].boylam);
                yaz.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                yaz.WriteLine("Bitiş Şehri Bilgileri(" + textBox2.Text.ToUpper() + ");");
                yaz.WriteLine("Plaka Kodu:" + il[p2].plaka);
                yaz.WriteLine("Rakım:" + il[p2].rakim);
                yaz.WriteLine("Enlem:" + il[p2].enlem);
                yaz.WriteLine("Boylam:" + il[p2].boylam);
                yaz.WriteLine("------------------------------------------------------------------------------------------------");
                yaz.WriteLine("Yolcu Sayısı \t   Zeplin Manevrası   \t Mesafe \t   Zeplin Maliyeti \t   Top.Kazanç \t   Edilen Kar \t   Kar Oranı");
                yaz.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                for (int i = 0; i < listBox2.Items.Count; i++)
                {
                    string[] tut;
                    kisi_sayisi = 80 - d_egim[i];
                    maliyet = Math.Round((Convert.ToDouble(list2[i]) * 10.0), 2);
                    total_kazanc = Convert.ToDouble((80 - d_egim[i]) * sabit_bilet_fiyati);
                    kar = Convert.ToDouble((80 - d_egim[i]) * sabit_bilet_fiyati) - Math.Round((Convert.ToDouble(list2[i]) * 10.0), 2);
                    kar_orani = Math.Round((kar / maliyet) * 100, 1);
                    yaz.WriteLine(kisi_sayisi+ "\t\t"+d_egim[i]+"\t\t"+list2[i]+"km\t\t"+maliyet+"TL\t\t"+total_kazanc+"TL\t\t"+kar+"TL\t\t"+kar_orani);
                    yaz.Write("Rota Bilgisi==>");
                    tut = gzrgh[i].Split(',');
                    for (int j = 0; j < tut.Length; j++)
                    {
                        yaz.Write(il_isimleri[Convert.ToInt32(tut[j])]+",");
                    }
                    yaz.WriteLine("");
                    
                    yaz.Write("Şehirler Arası Eğim Bilgisi==>");
                    for (int j = 0; j < tut.Length-1; j++)
                    {
                        ind1 = Convert.ToInt32(tut[j]);                  
                        ind2 = Convert.ToInt32(tut[j + 1]);
                        yaz.Write(min_egim_hasaplama(komsuluk_m_matrisi[ind1, ind2], il[ind1].rakim, il[ind2].rakim) + ",");
                    }
                    yaz.WriteLine("");
                    yaz.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                }
                yaz.Close();
                MessageBox.Show("Yazdırma İşlemi Başarıyla Gerçekleştirildi...", "Gezgin Zeplin", MessageBoxButtons.OK);
                gecen_zaman.Stop();
                total_zaman += gecen_zaman.Elapsed;
            }
            else if (radioButton2.Checked == true)
            {
                StreamWriter yaz = new StreamWriter(klasor_yolu + "\\Problem-2.txt");
                
                yaz.WriteLine(textBox1.Text.ToUpper() + "-" + textBox2.Text.ToUpper() + " İlleri Arası Bilet Fiyatı Tablosu(Min-Max)");
                yaz.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                yaz.WriteLine("Başlangıç Şehri Bilgileri(" + textBox1.Text.ToUpper() + ");");
                yaz.WriteLine("Plaka Kodu:" + il[p1].plaka);
                yaz.WriteLine("Rakım:" + il[p1].rakim);
                yaz.WriteLine("Enlem:" + il[p1].enlem);
                yaz.WriteLine("Boylam:" + il[p1].boylam);
                yaz.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                yaz.WriteLine("Bitiş Şehri Bilgileri(" + textBox2.Text.ToUpper() + ");");
                yaz.WriteLine("Plaka Kodu:" + il[p2].plaka);
                yaz.WriteLine("Rakım:" + il[p2].rakim);
                yaz.WriteLine("Enlem:" + il[p2].enlem);
                yaz.WriteLine("Boylam:" + il[p2].boylam);
                yaz.WriteLine("------------------------------------------------------------------------------------------------");
                yaz.WriteLine("Yolcu Sayısı \t   Zeplin Manevrası   \t Mesafe \t   Bilet Fiyatı    \t Zeplin Maliyeti \t   Top.Kazanç \t   Edilen Kar");
                yaz.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                for (int i = 0; i < listBox2.Items.Count; i++)
                {
                    string[] tut;
                    kisi_sayisi = 80 - d_egim[i];
                    maliyet = Math.Round((Convert.ToDouble(list2[i]) * 10.0), 2);
                    bilet_fiyati = ((maliyet + (maliyet * 50) / 100) / kisi_sayisi);
                    total_kazanc = bilet_fiyati * kisi_sayisi;
                    kar =total_kazanc-maliyet;
                   
                    yaz.WriteLine(kisi_sayisi + "\t\t" + d_egim[i] + "\t\t" + list2[i] + "km\t\t" + bilet_fiyati + "TL\t\t" + maliyet + "TL\t\t" + total_kazanc + "TL\t\t" + kar + "TL\t\t");
                    yaz.Write("Rota Bilgisi==>");
                    tut = gzrgh[i].Split(',');
                    for (int j = 0; j < tut.Length; j++)
                    {
                        yaz.Write(il_isimleri[Convert.ToInt32(tut[j])] + ",");
                    }
                    yaz.WriteLine("");

                    yaz.Write("Şehirler Arası Eğim Bilgisi==>");
                    for (int j = 0; j < tut.Length - 1; j++)
                    {
                        ind1 = Convert.ToInt32(tut[j]);
                        ind2 = Convert.ToInt32(tut[j + 1]);
                        yaz.Write(min_egim_hasaplama(komsuluk_m_matrisi[ind1, ind2], il[ind1].rakim, il[ind2].rakim) + ",");
                    }
                    yaz.WriteLine("");
                    yaz.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                }
                yaz.Close();
                MessageBox.Show("Yazdırma İşlemi Başarıyla Gerçekleştirildi...", "Gezgin Zeplin", MessageBoxButtons.OK);
                gecen_zaman.Stop();
                total_zaman += gecen_zaman.Elapsed;
            }
            else
            {
                StreamWriter yaz = new StreamWriter(klasor_yolu + "\\Manuel_deneme.txt");
                yaz.WriteLine(textBox1.Text.ToUpper() + "-" + textBox2.Text.ToUpper() + " İlleri Arası Maliyet Tablosu(Kardan-Zarara)");
                yaz.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                yaz.WriteLine("Başlangıç Şehri Bilgileri(" + textBox1.Text.ToUpper() + ");");
                yaz.WriteLine("Plaka Kodu:" + il[p1].plaka);
                yaz.WriteLine("Rakım:" + il[p1].rakim);
                yaz.WriteLine("Enlem:" + il[p1].enlem);
                yaz.WriteLine("Boylam:" + il[p1].boylam);
                yaz.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                yaz.WriteLine("Bitiş Şehri Bilgileri(" + textBox2.Text.ToUpper() + ");");
                yaz.WriteLine("Plaka Kodu:" + il[p2].plaka);
                yaz.WriteLine("Rakım:" + il[p2].rakim);
                yaz.WriteLine("Enlem:" + il[p2].enlem);
                yaz.WriteLine("Boylam:" + il[p2].boylam);
                yaz.WriteLine("------------------------------------------------------------------------------------------------");
                yaz.WriteLine("Yolcu Sayısı \t   Zeplin Manevrası   \t Mesafe \t   Zeplin Maliyeti \t   Top.Kazanç \t   Edilen Kar \t   Kar Oranı");
                yaz.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                yaz.WriteLine(textBox3.Text + "\t\t" + label15.Text + "\t\t" + label14.Text + "\t\t" + label21.Text + "\t\t" + label23.Text + "\t\t" + label25.Text + "\t\t" + label27.Text);
                yaz.Write("Rota Bilgisi==>");
                for (int j = 0; j < listBox1.Items.Count; j++)
                {
                    yaz.Write(listBox1.Items[j].ToString() + ",");
                }
                yaz.WriteLine("");

                yaz.Write("Şehirler Arası Eğim Bilgisi==>");
                for (int j = 0; j < listBox2.Items.Count; j++)
                {
                   yaz.Write(listBox2.Items[j].ToString() + ",");
                }
                yaz.WriteLine("");
                yaz.WriteLine("------------------------------------------------------------------------------------------------------------------------------------------------------------------");
                yaz.Close();
                MessageBox.Show("Yazdırma İşlemi Başarıyla Gerçekleştirildi...","Gezgin Zeplin",MessageBoxButtons.OK);
                gecen_zaman.Stop();
                total_zaman += gecen_zaman.Elapsed;
            }
        }/*Dosyalama Butonu*/
    }
}
