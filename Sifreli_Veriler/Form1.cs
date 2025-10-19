using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Sifreli_Veriler
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection(@"Data Source=DESKTOP-0NNHPCV\SQLEXPRESS;Initial Catalog=Test;Integrated Security=True");
        void listele()
        {
            SqlDataAdapter da = new SqlDataAdapter("Select * From veriler", baglanti);
            DataTable dt = new DataTable();
            da.Fill(dt);
            dataGridView1.DataSource = dt;
            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            dataGridView1.Columns["ID"].Width = 40;
            dataGridView1.Columns["AD"].AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
            //*dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            foreach (DataRow row in dt.Rows )
            {
                try
                {
                    row["AD"] = Encoding.ASCII.GetString(Convert.FromBase64String(row["AD"].ToString()));
                    row["SOYAD"] = Encoding.ASCII.GetString(Convert.FromBase64String(row["SOYAD"].ToString()));
                    row["MAIL"] = Encoding.ASCII.GetString(Convert.FromBase64String(row["MAIL"].ToString()));
                    row["HESAPNO"] = Encoding.ASCII.GetString(Convert.FromBase64String(row["HESAPNO"].ToString()));

                }
                catch (Exception)
                {

                    throw;
                }
            }
        }
        void temizle()
        {
            TxtAd.Clear();
            TxtHesapno.Clear();
            TxtMail.Clear();
            TxtSifre.Clear();
            TxtSoyad.Clear();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                string ad = TxtAd.Text;
                string adsifre = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(ad));

                string soyad = TxtSoyad.Text;
                string soyadsifre = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(soyad));

                string mail = TxtMail.Text;
                string mailsifre = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(mail));

                string sifre = TxtSifre.Text;
                string sifresifre = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(sifre));

                string hesapno = TxtHesapno.Text;
                string hesapnosifre = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(hesapno));

                baglanti.Open();
                // Kolon sırası tabloyla aynı: AD, SOYAD, SIFRE, HESAPNO, MAIL
                SqlCommand komut = new SqlCommand("INSERT INTO dbo.Veriler (AD,SOYAD,SIFRE,HESAPNO,MAIL) VALUES (@p1,@p2,@p3,@p4,@p5)", baglanti);
                komut.Parameters.AddWithValue("@p1", adsifre);      // AD
                komut.Parameters.AddWithValue("@p2", soyadsifre);   // SOYAD
                komut.Parameters.AddWithValue("@p3", sifresifre);   // SIFRE
                komut.Parameters.AddWithValue("@p4", hesapnosifre); // HESAPNO
                komut.Parameters.AddWithValue("@p5", mailsifre);    // MAIL
                komut.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("Veriler Başarıyla Eklendi!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("HATA DETAYI:\n\n" + ex.Message + "\n\nStack Trace:\n" + ex.StackTrace);
                if (baglanti.State == ConnectionState.Open)
                    baglanti.Close();
            }
            listele();
            temizle();


        }

        private void Form1_Load(object sender, EventArgs e)
        {
            listele();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TxtAd.Text))
            {
                MessageBox.Show("Ne işe yaradağını bilmediğin şeylere tıklama");
            }
            else
            {
                try
                {
                    string adcozum = TxtAd.Text;
                    byte[] adcozumdizi = Convert.FromBase64String(adcozum);
                    string adverisi = ASCIIEncoding.ASCII.GetString(adcozumdizi);
                    Txtcode.Text = adverisi;
                }
                catch (Exception ex)
                {

                    MessageBox.Show("Lüften şifreli kod giriniz!!!");
                }
            }
               
           
        }

        private void button3_Click(object sender, EventArgs e)
        {
            temizle();
        }
    }
}
