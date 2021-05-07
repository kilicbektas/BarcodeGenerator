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
    public partial class frmUrunEkle : Form
    {
        public frmUrunEkle()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=.;Initial Catalog=esen;Integrated Security=True");
        bool durum;
        private void barkodKontrol()
        {
            durum = true;
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select * from urun", baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                if (txtBarkodNo.Text==read["barkodNo"].ToString() || txtBarkodNo.Text=="")
                {
                    durum = false;
                }
            }
            baglanti.Close();
        }


        private void frmUrunEkle_Load(object sender, EventArgs e)
        {


        }



        private void btnYeniEkle_Click(object sender, EventArgs e)
        {
            barkodKontrol();
            if(durum==true)
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("insert into urun(barkodNo,urunAdi,miktar,alisFiyat,satisFiyat,tarih) values(@barkodNo,@urunAdi,@miktar,@alisFiyat,@satisFiyat,@tarih)", baglanti);
                komut.Parameters.AddWithValue("@barkodNo", txtBarkodNo.Text);
                komut.Parameters.AddWithValue("@urunAdi", txtUrunAdi.Text);
                komut.Parameters.AddWithValue("@miktar", int.Parse(txtMiktari.Text));
                komut.Parameters.AddWithValue("@alisFiyat", double.Parse(txtAlisFiyati.Text));
                komut.Parameters.AddWithValue("@satisFiyat", double.Parse(txtSatisFiyati.Text));
                komut.Parameters.AddWithValue("@tarih", DateTime.Now.ToString());
                komut.ExecuteNonQuery();
                baglanti.Close();
                MessageBox.Show("Ürün Eklendi...");
            }
            else
            {
                MessageBox.Show("Böyle bir barkod No zaten var", "Uyarı!..");
            }

            foreach(Control item in tableLayoutPanel3.Controls)
            {
                if(item is TextBox)
                {
                    item.Text = "";
                }
                if(item is ComboBox)
                {
                    item.Text = "";
                }
            }
        }

        private void barkodNoTxt_TextChanged(object sender, EventArgs e)
        {
            if(barkodNoTxt.Text=="")
            {
                foreach(Control item in tableLayoutPanel5.Controls)
                {
                    lblMiktari.Text = "";
                    if(item is TextBox)
                    {
                        item.Text = "";
                    }
                }
            }
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select * from urun where barkodNo like '"+barkodNoTxt.Text+"'", baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while(read.Read())
            {
                urunAdiTxt.Text = read["urunAdi"].ToString();
                lblMiktari.Text = read["miktar"].ToString();
                alisFiyatiTxt.Text = read["alisFiyat"].ToString();
                satisFiyatiTxt.Text = read["satisFiyat"].ToString();
            }
            baglanti.Close();

        }

        private void btnOlanEkle_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("update urun set miktar=miktar+'"+int.Parse(miktariTxt.Text)+"' where barkodNo='"+barkodNoTxt.Text+"'", baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close();
            foreach (Control item in tableLayoutPanel5.Controls)
            {
                lblMiktari.Text = "";
                if (item is TextBox)
                {
                    item.Text = "";
                }
            }
            MessageBox.Show("Var olan ürüne ekleme yapıldı...");
        }

        private void txtBarkodNo_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    }
}
