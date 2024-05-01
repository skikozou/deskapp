namespace deskapp
{
	partial class Form1
	{
		/// <summary>
		///  Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		///  Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		///  Required method for Designer support - do not modify
		///  the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			button1 = new Button();
			button2 = new Button();
			button3 = new Button();
			button4 = new Button();
			textBox1 = new TextBox();
			label1 = new Label();
			checkBox1 = new CheckBox();
			SuspendLayout();
			// 
			// button1
			// 
			button1.Location = new Point(139, 12);
			button1.Name = "button1";
			button1.Size = new Size(122, 94);
			button1.TabIndex = 0;
			button1.Text = "UP";
			button1.UseVisualStyleBackColor = true;
			button1.Click += button1_Click;
			// 
			// button2
			// 
			button2.Location = new Point(139, 112);
			button2.Name = "button2";
			button2.Size = new Size(122, 85);
			button2.TabIndex = 1;
			button2.Text = "DOWN";
			button2.UseVisualStyleBackColor = true;
			button2.Click += button2_Click;
			// 
			// button3
			// 
			button3.Location = new Point(11, 71);
			button3.Name = "button3";
			button3.Size = new Size(122, 75);
			button3.TabIndex = 2;
			button3.Text = "LEFT";
			button3.UseVisualStyleBackColor = true;
			button3.Click += button3_Click;
			// 
			// button4
			// 
			button4.Location = new Point(267, 71);
			button4.Name = "button4";
			button4.Size = new Size(122, 75);
			button4.TabIndex = 3;
			button4.Text = "RIGHT";
			button4.UseVisualStyleBackColor = true;
			button4.Click += button4_Click;
			// 
			// textBox1
			// 
			textBox1.Location = new Point(11, 32);
			textBox1.Name = "textBox1";
			textBox1.Size = new Size(121, 23);
			textBox1.TabIndex = 4;
			// 
			// label1
			// 
			label1.AutoSize = true;
			label1.Location = new Point(11, 14);
			label1.Name = "label1";
			label1.Size = new Size(76, 15);
			label1.TabIndex = 5;
			label1.Text = "Object Name";
			// 
			// checkBox1
			// 
			checkBox1.AutoSize = true;
			checkBox1.Location = new Point(270, 14);
			checkBox1.Name = "checkBox1";
			checkBox1.Size = new Size(100, 19);
			checkBox1.TabIndex = 6;
			checkBox1.Text = "WASD control";
			checkBox1.UseVisualStyleBackColor = true;
			checkBox1.CheckedChanged += checkBox1_CheckedChanged;
			// 
			// Form1
			// 
			AutoScaleDimensions = new SizeF(7F, 15F);
			AutoScaleMode = AutoScaleMode.Font;
			ClientSize = new Size(408, 208);
			Controls.Add(checkBox1);
			Controls.Add(label1);
			Controls.Add(textBox1);
			Controls.Add(button4);
			Controls.Add(button3);
			Controls.Add(button2);
			Controls.Add(button1);
			Name = "Form1";
			Text = "Desktop Object Controller";
			KeyDown += Form1_KeyDown;
			ResumeLayout(false);
			PerformLayout();
		}

		#endregion

		private Button button1;
		private Button button2;
		private Button button3;
		private Button button4;
		private TextBox textBox1;
		private Label label1;
		private CheckBox checkBox1;
	}
}
