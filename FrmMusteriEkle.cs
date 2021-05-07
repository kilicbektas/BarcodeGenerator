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

namespace barkod
{
    public partial class FrmMusteriEkle : Form
    {
        public FrmMusteriEkle()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=.;Initial Catalog=esen;Integrated Security=True");
        private void FrmMusteriEkle_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("insert into musteri(tc,adSoyad,telefon,adres,mail) values(@tc,@adSoyad,@telefon,@adres,@mail)",baglanti);
            komut.Parameters.AddWithValue("@tc", txtTc.Text);
            komut.Parameters.AddWithValue("@adSoyad", txtAdSoyad.Text);
            komut.Parameters.AddWithValue("@telefon", txtTel.Text);
            komut.Parameters.AddWithValue("@adres", txtAdr.Text);
            komut.Parameters.AddWithValue("@mail", txtMail.Text);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Kayıt Eklendi...");
            foreach(Control item in this.Controls)
            {
                if(item is TextBox)
                {
                    item.Text = "";
                }
            }
        }
    }
}
