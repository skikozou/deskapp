namespace deskapp
{
	public partial class Form1 : Form
	{
		public Form1()
		{
			InitializeComponent();
		}

		private void button1_Click(object sender, EventArgs e)
		{

			API.GetIconPosition(MOVE.UP, textBox1.Text);
		}

		private void button3_Click(object sender, EventArgs e)
		{
			API.GetIconPosition(MOVE.LEFT, textBox1.Text);
		}

		private void button2_Click(object sender, EventArgs e)
		{
			API.GetIconPosition(MOVE.DOWN, textBox1.Text);
		}

		private void button4_Click(object sender, EventArgs e)
		{
			API.GetIconPosition(MOVE.RIGHT, textBox1.Text);
		}

		private void Form1_KeyDown(object sender, KeyEventArgs e)
		{
			if (e.KeyCode == Keys.W)
			{
				API.GetIconPosition(MOVE.UP, textBox1.Text);
			}
			else if (e.KeyCode == Keys.A)
			{
				API.GetIconPosition(MOVE.LEFT, textBox1.Text);
			}
			else if (e.KeyCode == Keys.D)
			{
				API.GetIconPosition(MOVE.RIGHT, textBox1.Text);
			}
			else
			{
				API.GetIconPosition(MOVE.DOWN, textBox1.Text);
			}
		}

		private void checkBox1_CheckedChanged(object sender, EventArgs e)
		{
			if (checkBox1.Checked)
			{
				KeyPreview = true;
			}
			else
			{
				KeyPreview = false;
			}
		}
	}
}
