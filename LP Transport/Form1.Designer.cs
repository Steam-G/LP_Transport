namespace LP_Transport
{
    partial class Form1
    {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.lbVal1 = new System.Windows.Forms.Label();
            this.lbVal2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.lbVal3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.smallPropertyBox6 = new Sys_components.SmallPropertyBox();
            this.smallPropertyBox5 = new Sys_components.SmallPropertyBox();
            this.smallPropertyBox4 = new Sys_components.SmallPropertyBox();
            this.smallPropertyBox7 = new Sys_components.SmallPropertyBox();
            this.smallPropertyBox8 = new Sys_components.SmallPropertyBox();
            this.smallPropertyBox9 = new Sys_components.SmallPropertyBox();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(52, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Забой, м";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 49);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 13);
            this.label2.TabIndex = 1;
            this.label2.Text = "Долото, м";
            // 
            // lbVal1
            // 
            this.lbVal1.AutoSize = true;
            this.lbVal1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbVal1.Location = new System.Drawing.Point(12, 22);
            this.lbVal1.Name = "lbVal1";
            this.lbVal1.Size = new System.Drawing.Size(72, 25);
            this.lbVal1.TabIndex = 2;
            this.lbVal1.Text = "Value";
            // 
            // lbVal2
            // 
            this.lbVal2.AutoSize = true;
            this.lbVal2.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbVal2.Location = new System.Drawing.Point(12, 62);
            this.lbVal2.Name = "lbVal2";
            this.lbVal2.Size = new System.Drawing.Size(72, 25);
            this.lbVal2.TabIndex = 3;
            this.lbVal2.Text = "Value";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(168, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "IP адрес";
            // 
            // lbVal3
            // 
            this.lbVal3.AutoSize = true;
            this.lbVal3.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbVal3.Location = new System.Drawing.Point(166, 22);
            this.lbVal3.Name = "lbVal3";
            this.lbVal3.Size = new System.Drawing.Size(72, 25);
            this.lbVal3.TabIndex = 2;
            this.lbVal3.Text = "Value";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(463, 9);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 5;
            this.button1.Text = "Старт";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.smallPropertyBox6);
            this.panel2.Controls.Add(this.smallPropertyBox5);
            this.panel2.Controls.Add(this.smallPropertyBox4);
            this.panel2.Controls.Add(this.smallPropertyBox7);
            this.panel2.Controls.Add(this.smallPropertyBox8);
            this.panel2.Controls.Add(this.smallPropertyBox9);
            this.panel2.Location = new System.Drawing.Point(17, 90);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(475, 213);
            this.panel2.TabIndex = 108;
            // 
            // smallPropertyBox6
            // 
            this.smallPropertyBox6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.smallPropertyBox6.Location = new System.Drawing.Point(0, 10);
            this.smallPropertyBox6.Margin = new System.Windows.Forms.Padding(1);
            this.smallPropertyBox6.Name = "smallPropertyBox6";
            this.smallPropertyBox6.Padding = new System.Windows.Forms.Padding(1);
            this.smallPropertyBox6.PropertyName = null;
            this.smallPropertyBox6.Size = new System.Drawing.Size(474, 28);
            this.smallPropertyBox6.TabIndex = 5;
            this.smallPropertyBox6.Value = null;
            // 
            // smallPropertyBox5
            // 
            this.smallPropertyBox5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.smallPropertyBox5.Location = new System.Drawing.Point(0, 40);
            this.smallPropertyBox5.Margin = new System.Windows.Forms.Padding(1);
            this.smallPropertyBox5.Name = "smallPropertyBox5";
            this.smallPropertyBox5.Padding = new System.Windows.Forms.Padding(1);
            this.smallPropertyBox5.PropertyName = null;
            this.smallPropertyBox5.Size = new System.Drawing.Size(474, 28);
            this.smallPropertyBox5.TabIndex = 4;
            this.smallPropertyBox5.Value = null;
            // 
            // smallPropertyBox4
            // 
            this.smallPropertyBox4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.smallPropertyBox4.Location = new System.Drawing.Point(0, 70);
            this.smallPropertyBox4.Margin = new System.Windows.Forms.Padding(1);
            this.smallPropertyBox4.Name = "smallPropertyBox4";
            this.smallPropertyBox4.Padding = new System.Windows.Forms.Padding(1);
            this.smallPropertyBox4.PropertyName = null;
            this.smallPropertyBox4.Size = new System.Drawing.Size(474, 28);
            this.smallPropertyBox4.TabIndex = 3;
            this.smallPropertyBox4.Value = null;
            // 
            // smallPropertyBox7
            // 
            this.smallPropertyBox7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.smallPropertyBox7.Location = new System.Drawing.Point(0, 100);
            this.smallPropertyBox7.Margin = new System.Windows.Forms.Padding(1);
            this.smallPropertyBox7.Name = "smallPropertyBox7";
            this.smallPropertyBox7.Padding = new System.Windows.Forms.Padding(1);
            this.smallPropertyBox7.PropertyName = null;
            this.smallPropertyBox7.Size = new System.Drawing.Size(474, 28);
            this.smallPropertyBox7.TabIndex = 2;
            this.smallPropertyBox7.Value = null;
            // 
            // smallPropertyBox8
            // 
            this.smallPropertyBox8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.smallPropertyBox8.Location = new System.Drawing.Point(0, 130);
            this.smallPropertyBox8.Margin = new System.Windows.Forms.Padding(1);
            this.smallPropertyBox8.Name = "smallPropertyBox8";
            this.smallPropertyBox8.Padding = new System.Windows.Forms.Padding(1);
            this.smallPropertyBox8.PropertyName = null;
            this.smallPropertyBox8.Size = new System.Drawing.Size(474, 28);
            this.smallPropertyBox8.TabIndex = 1;
            this.smallPropertyBox8.Value = null;
            // 
            // smallPropertyBox9
            // 
            this.smallPropertyBox9.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.smallPropertyBox9.Location = new System.Drawing.Point(0, 160);
            this.smallPropertyBox9.Margin = new System.Windows.Forms.Padding(1);
            this.smallPropertyBox9.Name = "smallPropertyBox9";
            this.smallPropertyBox9.Padding = new System.Windows.Forms.Padding(1);
            this.smallPropertyBox9.PropertyName = null;
            this.smallPropertyBox9.Size = new System.Drawing.Size(474, 28);
            this.smallPropertyBox9.TabIndex = 0;
            this.smallPropertyBox9.Value = null;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(686, 469);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lbVal2);
            this.Controls.Add(this.lbVal3);
            this.Controls.Add(this.lbVal1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.MinimumSize = new System.Drawing.Size(400, 180);
            this.Name = "Form1";
            this.Text = "Значения глубины";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label lbVal1;
        private System.Windows.Forms.Label lbVal2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lbVal3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel2;
        private Sys_components.SmallPropertyBox smallPropertyBox6;
        private Sys_components.SmallPropertyBox smallPropertyBox5;
        private Sys_components.SmallPropertyBox smallPropertyBox4;
        private Sys_components.SmallPropertyBox smallPropertyBox7;
        private Sys_components.SmallPropertyBox smallPropertyBox8;
        private Sys_components.SmallPropertyBox smallPropertyBox9;
    }
}

