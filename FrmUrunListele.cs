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
    public partial class FrmUrunListele : Form
    {
        public FrmUrunListele()
        {
            InitializeComponent();
        }

        SqlConnection baglanti = new SqlConnection("Data Source=.;Initial Catalog=esen;Integrated Security=True");
        DataSet daset = new DataSet();



        private void FrmUrunListele_Load(object sender, EventArgs e)
        {
            urunListele();

        }

        private void urunListele()
        {
            baglanti.Open();
            SqlDataAdapter adtr = new SqlDataAdapter("select * from urun", baglanti);
            adtr.Fill(daset, "urun");
            dataGridView1.DataSource = daset.Tables["urun"];
            baglanti.Close();
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            barkodNoTxt.Text = dataGridView1.CurrentRow.Cells["barkodNo"].Value.ToString();
            urunAdiTxt.Text = dataGridView1.CurrentRow.Cells["urunAdi"].Value.ToString();
            miktariTxt.Text = dataGridView1.CurrentRow.Cells["miktar"].Value.ToString();
            alisFiyatiTxt.Text = dataGridView1.CurrentRow.Cells["alisFiyat"].Value.ToString();
            satisFiyatiTxt.Text = dataGridView1.CurrentRow.Cells["satisFiyat"].Value.ToString();

        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("update urun set urunAdi=@urunAdi,miktar=@miktar,alisFiyat=@alisFiyat,satisFiyat=@satisFiyat where barkodNo=@barkodNo", baglanti);
            komut.Parameters.AddWithValue("@barkodNo", barkodNoTxt.Text);
            komut.Parameters.AddWithValue("@urunAdi", urunAdiTxt.Text);
            komut.Parameters.AddWithValue("@miktar", int.Parse(miktariTxt.Text));
            komut.Parameters.AddWithValue("@alisFiyat", double.Parse(alisFiyatiTxt.Text));
            komut.Parameters.AddWithValue("@satisFiyat", double.Parse(satisFiyatiTxt.Text));
            komut.ExecuteNonQuery();
            baglanti.Close();
            daset.Tables["urun"].Clear();
            urunListele();
            MessageBox.Show("Güncelelme Başarılı");
            foreach(Control item in this.Controls)
            {
                if(item is TextBox)
                {
                    item.Text = "";
                }
            }
        }





        private void btnSil_Click(object sender, EventArgs e)
        {
            baglanti.Open();
            SqlCommand komut = new SqlCommand("delete from urun where barkodNo='" + dataGridView1.CurrentRow.Cells["barkodNo"].Value.ToString() + "'", baglanti);
            komut.ExecuteNonQuery();
            baglanti.Close();
            daset.Tables["urun"].Clear();
            urunListele();
            MessageBox.Show("Ürün Silindi..");
        }

        private void txtBarkodNoAra_TextChanged(object sender, EventArgs e)
        {
            DataTable tablo = new DataTable();
            baglanti.Open();
            SqlDataAdapter adtr = new SqlDataAdapter("select * from urun where barkodNo like '%" + txtBarkodNoAra.Text + "%'", baglanti);
            adtr.Fill(tablo);
            dataGridView1.DataSource = tablo;
            baglanti.Close();
        }

        private void txtAdAra_TextChanged(object sender, EventArgs e)
        {
            DataTable tablo = new DataTable();
            baglanti.Open();
            SqlDataAdapter adtr = new SqlDataAdapter("select * from urun where urunAdi like '%" + txtAdAra.Text + "%'", baglanti);
            adtr.Fill(tablo);
            dataGridView1.DataSource = tablo;
            baglanti.Close();
        }

        private void txtBarkodNoAra_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
    }
}
