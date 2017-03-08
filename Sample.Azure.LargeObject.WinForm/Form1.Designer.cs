namespace Sample.Azure.LargeObject.WinForm
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtLoop = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.blobWrite = new System.Windows.Forms.Button();
            this.txtResult = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.blobRead = new System.Windows.Forms.Button();
            this.cacheRead = new System.Windows.Forms.Button();
            this.cacheWrite = new System.Windows.Forms.Button();
            this.searchRead = new System.Windows.Forms.Button();
            this.searchWrite = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // txtLoop
            // 
            this.txtLoop.Location = new System.Drawing.Point(221, 45);
            this.txtLoop.Name = "txtLoop";
            this.txtLoop.Size = new System.Drawing.Size(100, 22);
            this.txtLoop.TabIndex = 0;
            this.txtLoop.Text = "10";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(40, 49);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(140, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Number of Iterations:";
            // 
            // blobWrite
            // 
            this.blobWrite.Location = new System.Drawing.Point(43, 118);
            this.blobWrite.Name = "blobWrite";
            this.blobWrite.Size = new System.Drawing.Size(208, 23);
            this.blobWrite.TabIndex = 2;
            this.blobWrite.Text = "Blob Write";
            this.blobWrite.UseVisualStyleBackColor = true;
            this.blobWrite.Click += new System.EventHandler(this.blobWrite_Click);
            // 
            // txtResult
            // 
            this.txtResult.Location = new System.Drawing.Point(221, 311);
            this.txtResult.Multiline = true;
            this.txtResult.Name = "txtResult";
            this.txtResult.Size = new System.Drawing.Size(439, 87);
            this.txtResult.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(40, 311);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(48, 17);
            this.label2.TabIndex = 4;
            this.label2.Text = "Result";
            // 
            // blobRead
            // 
            this.blobRead.Location = new System.Drawing.Point(320, 118);
            this.blobRead.Name = "blobRead";
            this.blobRead.Size = new System.Drawing.Size(208, 23);
            this.blobRead.TabIndex = 5;
            this.blobRead.Text = "Blob Read";
            this.blobRead.UseVisualStyleBackColor = true;
            this.blobRead.Click += new System.EventHandler(this.blobRead_Click);
            // 
            // cacheRead
            // 
            this.cacheRead.Location = new System.Drawing.Point(320, 158);
            this.cacheRead.Name = "cacheRead";
            this.cacheRead.Size = new System.Drawing.Size(208, 23);
            this.cacheRead.TabIndex = 7;
            this.cacheRead.Text = "Cache Read";
            this.cacheRead.UseVisualStyleBackColor = true;
            this.cacheRead.Click += new System.EventHandler(this.cacheRead_Click);
            // 
            // cacheWrite
            // 
            this.cacheWrite.Location = new System.Drawing.Point(43, 158);
            this.cacheWrite.Name = "cacheWrite";
            this.cacheWrite.Size = new System.Drawing.Size(208, 23);
            this.cacheWrite.TabIndex = 6;
            this.cacheWrite.Text = "Cache Write";
            this.cacheWrite.UseVisualStyleBackColor = true;
            this.cacheWrite.Click += new System.EventHandler(this.cacheWrite_Click);
            // 
            // searchRead
            // 
            this.searchRead.Location = new System.Drawing.Point(320, 197);
            this.searchRead.Name = "searchRead";
            this.searchRead.Size = new System.Drawing.Size(208, 23);
            this.searchRead.TabIndex = 9;
            this.searchRead.Text = "Search Read";
            this.searchRead.UseVisualStyleBackColor = true;
            this.searchRead.Click += new System.EventHandler(this.searchRead_Click);
            // 
            // searchWrite
            // 
            this.searchWrite.Location = new System.Drawing.Point(43, 197);
            this.searchWrite.Name = "searchWrite";
            this.searchWrite.Size = new System.Drawing.Size(208, 23);
            this.searchWrite.TabIndex = 8;
            this.searchWrite.Text = "Search Write";
            this.searchWrite.UseVisualStyleBackColor = true;
            this.searchWrite.Click += new System.EventHandler(this.searchWrite_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(691, 410);
            this.Controls.Add(this.searchRead);
            this.Controls.Add(this.searchWrite);
            this.Controls.Add(this.cacheRead);
            this.Controls.Add(this.cacheWrite);
            this.Controls.Add(this.blobRead);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.txtResult);
            this.Controls.Add(this.blobWrite);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtLoop);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtLoop;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button blobWrite;
        private System.Windows.Forms.TextBox txtResult;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button blobRead;
        private System.Windows.Forms.Button cacheRead;
        private System.Windows.Forms.Button cacheWrite;
        private System.Windows.Forms.Button searchRead;
        private System.Windows.Forms.Button searchWrite;
    }
}

