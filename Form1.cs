using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace barkod
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        SqlConnection baglanti = new SqlConnection("Data Source=.;Initial Catalog=esen;Integrated Security=True");
        DataSet daset = new DataSet();

        private void sepetListele()
        {
            baglanti.Open();
            SqlDataAdapter adtr = new SqlDataAdapter("select * from sepet", baglanti);
            adtr.Fill(daset, "sepet");
            dataGridView1.DataSource = daset.Tables["sepet"];
            baglanti.Close();
        }

        private void musEkle_Click(object sender, EventArgs e)
        {
            FrmMusteriEkle ekle = new FrmMusteriEkle();
            ekle.ShowDialog();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            frmMusteriListele mListele = new frmMusteriListele();
            mListele.ShowDialog();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            frmUrunEkle ekle = new frmUrunEkle();
            ekle.ShowDialog();
        }



        private void button8_Click(object sender, EventArgs e)
        {
            FrmUrunListele listele = new FrmUrunListele();
            listele.ShowDialog();
        }

        private void hesapla()
        {
            try
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("select sum(toplamFiyat) from sepet", baglanti);
                lblToplam.Text = komut.ExecuteScalar() + " TL";
                baglanti.Close();
            }
            catch (Exception)
            {
                ;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            sepetListele();
        }

        private void Temizle()
        {
            if (txtBarkod.Text == "")
            {
                foreach (Control item in tableLayoutPanel2.Controls)
                {
                    if (item is TextBox)
                    {
                        if (item != txtMiktar)
                        {
                            item.Text = "";
                        }
                    }
                }
            }
        }

        bool durum;
        private void barkodKontrol()
        {
            durum = true;
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select * from sepet", baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                if (txtBarkod.Text == read["barkodNo"].ToString())
                {
                    durum = false;
                }
            }
            baglanti.Close();
        }
        /*-----------------------------------------------------------------------------------------------------------------*/
        private void txtBarkod_TextChanged(object sender, EventArgs e)
        {
            Temizle();
            baglanti.Open();
            SqlCommand komut = new SqlCommand("select * from urun where barkodNo like '" + txtBarkod.Text + "'", baglanti);
            SqlDataReader read = komut.ExecuteReader();
            while (read.Read())
            {
                txtUrunAdi.Text = read["urunAdi"].ToString();
                txtSatisFiyat.Text = read["satisFiyat"].ToString();
            }
            baglanti.Close();
        }


        /*********************************************************************************************/
        private void btnEkle_Click(object sender, EventArgs e)
        {
            if(txtBarkod.Text=="")
            {
                MessageBox.Show("Olmayan barkodu nasıl ekleyebiliyosun acaba bir söyler misin?");
            }
            else
            {
                barkodKontrol();
                if (durum == true)
                {

                    baglanti.Open();
                    SqlCommand komut2 = new SqlCommand("insert into sepet(barkodNo,urunAdi,miktar,satisFiyat,toplamFiyat,tarih) values(@barkodNo,@urunAdi,@miktar,@satisFiyat,@toplamFiyat,@tarih)", baglanti);
                    komut2.Parameters.AddWithValue("@barkodNo", txtBarkod.Text);
                    komut2.Parameters.AddWithValue("@urunAdi", txtUrunAdi.Text);
                    komut2.Parameters.AddWithValue("@miktar", int.Parse(txtMiktar.Text));
                    komut2.Parameters.AddWithValue("@satisFiyat", double.Parse(txtSatisFiyat.Text));
                    komut2.Parameters.AddWithValue("@toplamFiyat", double.Parse(txtToplamFiyat.Text));
                    komut2.Parameters.AddWithValue("@tarih", DateTime.Now.ToString());
                    komut2.ExecuteNonQuery();
                    baglanti.Close();
                }
                else
                {
                    baglanti.Open();
                    SqlCommand komut3 = new SqlCommand("update sepet set miktar=miktar+'" + int.Parse(txtMiktar.Text) + "' where barkodNo='" + txtBarkod.Text + "'", baglanti);
                    komut3.ExecuteNonQuery();

                    SqlCommand komut4 = new SqlCommand("update sepet set toplamFiyat=miktar*satisFiyat where barkodNo='" + txtBarkod.Text + "'", baglanti);
                    komut4.ExecuteNonQuery();
                    baglanti.Close();
                }
            }



            txtMiktar.Text = "1";
            daset.Tables["sepet"].Clear();
            sepetListele();
            hesapla();
            foreach (Control item in tableLayoutPanel2.Controls)
            {
                if (item is TextBox)
                {
                    if (item != txtMiktar)
                    {
                        item.Text = "";
                    }
                }
            }
        }

        private void txtMiktar_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtToplamFiyat.Text = (double.Parse(txtMiktar.Text) * double.Parse(txtSatisFiyat.Text)).ToString();
            }
            catch(Exception)
            {
                ;
            }
        }

        private void txtSatisFiyat_TextChanged(object sender, EventArgs e)
        {
            try
            {
                txtToplamFiyat.Text = (double.Parse(txtMiktar.Text) * double.Parse(txtSatisFiyat.Text)).ToString();
            }
            catch (Exception)
            {
                ;
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("delete from sepet where barkodNo='"+dataGridView1.CurrentRow.Cells["barkodNo"].Value.ToString()+"'", baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Sepetten çıkarıldı");
            daset.Tables["sepet"].Clear();
            sepetListele();
            hesapla();
        }

        private void btnSatisiptal_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("delete from sepet", baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Satış iptal edildi");
            daset.Tables["sepet"].Clear();
            sepetListele();
            hesapla();
        }

        private void button9_Click(object sender, EventArgs e)
        {
            FrmSatisListele listele = new FrmSatisListele();
            listele.ShowDialog();
        }

        private void btnSatis_Click(object sender, EventArgs e)
        {
            for (int i =0; i<dataGridView1.Rows.Count-1;i++)
            {
                baglanti.Open();
                SqlCommand komut = new SqlCommand("insert into satis(barkodNo,urunAdi,miktar,satisFiyat,toplamFiyat,tarih) values(@barkodNo,@urunAdi,@miktar,@satisFiyat,@toplamFiyat,@tarih)", baglanti);
                komut.Parameters.AddWithValue("@barkodNo", dataGridView1.Rows[i].Cells["barkodNo"].Value.ToString());
                komut.Parameters.AddWithValue("@urunAdi", dataGridView1.Rows[i].Cells["urunAdi"].Value.ToString());
                komut.Parameters.AddWithValue("@miktar", int.Parse(dataGridView1.Rows[i].Cells["miktar"].Value.ToString()));
                komut.Parameters.AddWithValue("@satisFiyat", double.Parse(dataGridView1.Rows[i].Cells["satisFiyat"].Value.ToString()));
                komut.Parameters.AddWithValue("@toplamFiyat", double.Parse(dataGridView1.Rows[i].Cells["toplamFiyat"].Value.ToString()));
                komut.Parameters.AddWithValue("@tarih", DateTime.Now.ToString());
                komut.ExecuteNonQuery();
                SqlCommand komut2 = new SqlCommand("update urun set miktar=miktar-'" + int.Parse(dataGridView1.Rows[i].Cells["miktar"].Value.ToString()) + "' where barkodNo='" + dataGridView1.Rows[i].Cells["barkodNo"].Value.ToString() + "'", baglanti);
                komut2.ExecuteNonQuery();
                baglanti.Close();
            }
            baglanti.Open();
            SqlCommand komut3 = new SqlCommand("delete from sepet", baglanti);
            komut3.ExecuteNonQuery();
            baglanti.Close();
            MessageBox.Show("Bereket versin de");
            daset.Tables["sepet"].Clear();
            sepetListele();
            hesapla();
        }

        private void txtBarkod_KeyPress_1(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    }
}
