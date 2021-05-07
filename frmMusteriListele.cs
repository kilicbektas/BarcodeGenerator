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
    public partial class frmMusteriListele : Form
    {
        public frmMusteriListele()
        {
            InitializeComponent();
        }
        SqlConnection baglanti = new SqlConnection("Data Source=.;Initial Catalog=esen;Integrated Security=True");
        DataSet daset = new DataSet();
        private void frmMusteriListele_Load(object sender, EventArgs e)
        {
            kayitGoster();
        }

        private void kayitGoster()
        {
            baglanti.Open();
            SqlDataAdapter adtr = new SqlDataAdapter("select * from musteri", baglanti);
            adtr.Fill(daset, "musteri");
            dataGridView1.DataSource = daset.Tables["musteri"];
            baglanti.Close();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            txtTc.Text = dataGridView1.CurrentRow.Cells["tc"].Value.ToString();
            txtAdSoyad.Text = dataGridView1.CurrentRow.Cells["adsoyad"].Value.ToString();
            txtTel.Text = dataGridView1.CurrentRow.Cells["telefon"].Value.ToString();
            txtAdr.Text = dataGridView1.CurrentRow.Cells["adres"].Value.ToString();
            txtMail.Text = dataGridView1.CurrentRow.Cells["mail"].Value.ToString();
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("update musteri set adsoyad=@adSoyad,telefon=@telefon,adres=@adres,mail=@mail where tc=@tc", baglanti);
            komut.Parameters.AddWithValue("@tc", txtTc.Text);
            komut.Parameters.AddWithValue("@adSoyad", txtAdSoyad.Text);
            komut.Parameters.AddWithValue("@telefon", txtTel.Text);
            komut.Parameters.AddWithValue("@adres", txtAdr.Text);
            komut.Parameters.AddWithValue("@mail", txtMail.Text);
            komut.ExecuteNonQuery();
            baglanti.Close();
            daset.Tables["musteri"].Clear();
            kayitGoster();
            MessageBox.Show("Kayıt Güncellendi...");
            foreach (Control item in this.Controls)
            {
                if (item is TextBox)
                {
                    item.Text = "";
                }
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("delete from musteri where tc='"+dataGridView1.CurrentRow.Cells["tc"].Value.ToString()+"'", baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close();
            daset.Tables["musteri"].Clear();
            kayitGoster();
            MessageBox.Show("Kayıt Silindi..");
        }

        private void txtTcAra_TextChanged(object sender, EventArgs e)
        {
            DataTable tablo = new DataTable();
            baglanti.Open();
            SqlDataAdapter adtr = new SqlDataAdapter("select * from musteri where tc like '%"+txtTcAra.Text+"%'",baglanti);
            adtr.Fill(tablo);
            dataGridView1.DataSource = tablo;
            baglanti.Close();
        }

        private void txtTc_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
