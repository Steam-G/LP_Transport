namespace Sys_components
{
    partial class SmallPropertyBox
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

        #region Код, автоматически созданный конструктором компонентов

        /// <summary> 
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent()
        {
            this.lbl1 = new System.Windows.Forms.Label();
            this.lbl0 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbl1
            // 
            this.lbl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl1.BackColor = System.Drawing.Color.Moccasin;
            this.lbl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl1.ForeColor = System.Drawing.Color.SaddleBrown;
            this.lbl1.Location = new System.Drawing.Point(315, 1);
            this.lbl1.Name = "lbl1";
            this.lbl1.Padding = new System.Windows.Forms.Padding(5, 1, 5, 1);
            this.lbl1.Size = new System.Drawing.Size(182, 28);
            this.lbl1.TabIndex = 3;
            this.lbl1.Text = "Value";
            this.lbl1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lbl0
            // 
            this.lbl0.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lbl0.BackColor = System.Drawing.Color.PapayaWhip;
            this.lbl0.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.lbl0.ForeColor = System.Drawing.Color.Sienna;
            this.lbl0.Location = new System.Drawing.Point(3, 1);
            this.lbl0.Name = "lbl0";
            this.lbl0.Padding = new System.Windows.Forms.Padding(5, 1, 1, 5);
            this.lbl0.Size = new System.Drawing.Size(311, 28);
            this.lbl0.TabIndex = 2;
            this.lbl0.Text = "PropertyName";
            this.lbl0.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // SmallPropertyBox
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.lbl1);
            this.Controls.Add(this.lbl0);
            this.Margin = new System.Windows.Forms.Padding(1);
            this.Name = "SmallPropertyBox";
            this.Padding = new System.Windows.Forms.Padding(1);
            this.Size = new System.Drawing.Size(500, 30);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbl1;
        private System.Windows.Forms.Label lbl0;
    }
}
