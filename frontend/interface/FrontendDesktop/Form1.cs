namespace FrontendDesktop
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        // picture box solo
        private void pb_solo_MouseClick(object sender, MouseEventArgs e)
        {
            pb_solo.Image = Image.FromFile(@"img\solopress2.jpg");
        }
        private void pb_solo_MouseEnter(object sender, EventArgs e)
        {
            pb_solo.Image = Image.FromFile(@"img\soloup2.jpg");
        }

        private void pb_solo_MouseLeave(object sender, EventArgs e)
        {
            pb_solo.Image = Image.FromFile(@"img\solofix2.jpg");
        }

        private void tm_solo_Tick(object sender, EventArgs e)
        {

        }

        // picture box duo
        private void pb_duo_MouseClick(object sender, MouseEventArgs e)
        {
            pb_duo.Image = Image.FromFile(@"img\duopress.jpg");
        }

        private void pb_duo_MouseEnter(object sender, EventArgs e)
        {
            pb_duo.Image = Image.FromFile(@"img\duoup.jpg");
        }

        private void pb_duo_MouseLeave(object sender, EventArgs e)
        {
            pb_duo.Image = Image.FromFile(@"img\duofix.jpg");
        }

        // picture box placar
        private void pb_placar_MouseClick(object sender, MouseEventArgs e)
        {
            pb_placar.Image = Image.FromFile(@"img\placarpress.jpg");

        }

        private void pb_placar_MouseEnter(object sender, EventArgs e)
        {
            pb_placar.Image = Image.FromFile(@"img\placarup.jpg");
        }

        private void pb_placar_MouseLeave(object sender, EventArgs e)
        {
            pb_placar.Image = Image.FromFile(@"img\placarfix.jpg");
        }

        //picture box cores
        private void pb_cores_MouseClick(object sender, MouseEventArgs e)
        {
            pb_cores.Image = Image.FromFile(@"img\corespress.jpg");
            
        }

        private void pb_cores_MouseEnter(object sender, EventArgs e)
        {
            pb_cores.Image = Image.FromFile(@"img\coresup.jpg");
        }

        private void pb_cores_MouseLeave(object sender, EventArgs e)
        {
            pb_cores.Image = Image.FromFile(@"img\coresfix.jpg");
        }

        private void pb_solo_Click(object sender, EventArgs e)
        {
            Form2 frm = new Form2();
            frm.ShowDialog();
       }
    }
}