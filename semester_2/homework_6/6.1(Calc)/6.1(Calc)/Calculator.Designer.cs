namespace CalcNamespace
{
    partial class Calculator
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Calculator));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.buttonC = new System.Windows.Forms.Button();
            this.buttonCE = new System.Windows.Forms.Button();
            this.buttonPlusMinus = new System.Windows.Forms.Button();
            this.buttonDivision = new System.Windows.Forms.Button();
            this.buttonSeven = new System.Windows.Forms.Button();
            this.buttonEight = new System.Windows.Forms.Button();
            this.buttonNine = new System.Windows.Forms.Button();
            this.buttonMultiplication = new System.Windows.Forms.Button();
            this.buttonFour = new System.Windows.Forms.Button();
            this.buttonFive = new System.Windows.Forms.Button();
            this.buttonSix = new System.Windows.Forms.Button();
            this.buttonAddition = new System.Windows.Forms.Button();
            this.buttonOne = new System.Windows.Forms.Button();
            this.buttonTwo = new System.Windows.Forms.Button();
            this.buttonThree = new System.Windows.Forms.Button();
            this.buttonSubtraction = new System.Windows.Forms.Button();
            this.buttonZero = new System.Windows.Forms.Button();
            this.buttonDelimiter = new System.Windows.Forms.Button();
            this.buttonEquality = new System.Windows.Forms.Button();
            this.display = new System.Windows.Forms.Label();
            this.tableLayoutPanel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 4;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 25F));
            this.tableLayoutPanel1.Controls.Add(this.buttonC, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonCE, 1, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonPlusMinus, 2, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonDivision, 3, 1);
            this.tableLayoutPanel1.Controls.Add(this.buttonSeven, 0, 2);
            this.tableLayoutPanel1.Controls.Add(this.buttonEight, 1, 2);
            this.tableLayoutPanel1.Controls.Add(this.buttonNine, 2, 2);
            this.tableLayoutPanel1.Controls.Add(this.buttonMultiplication, 3, 2);
            this.tableLayoutPanel1.Controls.Add(this.buttonFour, 0, 3);
            this.tableLayoutPanel1.Controls.Add(this.buttonFive, 1, 3);
            this.tableLayoutPanel1.Controls.Add(this.buttonSix, 2, 3);
            this.tableLayoutPanel1.Controls.Add(this.buttonAddition, 3, 3);
            this.tableLayoutPanel1.Controls.Add(this.buttonOne, 0, 4);
            this.tableLayoutPanel1.Controls.Add(this.buttonTwo, 1, 4);
            this.tableLayoutPanel1.Controls.Add(this.buttonThree, 2, 4);
            this.tableLayoutPanel1.Controls.Add(this.buttonSubtraction, 3, 4);
            this.tableLayoutPanel1.Controls.Add(this.buttonZero, 0, 5);
            this.tableLayoutPanel1.Controls.Add(this.buttonDelimiter, 1, 5);
            this.tableLayoutPanel1.Controls.Add(this.buttonEquality, 2, 5);
            this.tableLayoutPanel1.Controls.Add(this.display, 0, 0);
            this.tableLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 6;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 16F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(434, 291);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // buttonC
            // 
            this.buttonC.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonC.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonC.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.buttonC.Location = new System.Drawing.Point(3, 61);
            this.buttonC.Name = "buttonC";
            this.buttonC.Size = new System.Drawing.Size(102, 40);
            this.buttonC.TabIndex = 0;
            this.buttonC.Text = "C";
            this.buttonC.UseVisualStyleBackColor = true;
            this.buttonC.Click += new System.EventHandler(this.OnButtonCClick);
            // 
            // buttonCE
            // 
            this.buttonCE.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonCE.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonCE.Location = new System.Drawing.Point(111, 61);
            this.buttonCE.Name = "buttonCE";
            this.buttonCE.Size = new System.Drawing.Size(102, 40);
            this.buttonCE.TabIndex = 1;
            this.buttonCE.Text = "CE";
            this.buttonCE.UseVisualStyleBackColor = true;
            this.buttonCE.Click += new System.EventHandler(this.OnButtonCEClick);
            // 
            // buttonPlusMinus
            // 
            this.buttonPlusMinus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonPlusMinus.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonPlusMinus.Location = new System.Drawing.Point(219, 61);
            this.buttonPlusMinus.Name = "buttonPlusMinus";
            this.buttonPlusMinus.Size = new System.Drawing.Size(102, 40);
            this.buttonPlusMinus.TabIndex = 2;
            this.buttonPlusMinus.Text = "+/-";
            this.buttonPlusMinus.UseVisualStyleBackColor = true;
            this.buttonPlusMinus.Click += new System.EventHandler(this.OnButtonNegationClick);
            // 
            // buttonDivision
            // 
            this.buttonDivision.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonDivision.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonDivision.Location = new System.Drawing.Point(327, 61);
            this.buttonDivision.Name = "buttonDivision";
            this.buttonDivision.Size = new System.Drawing.Size(104, 40);
            this.buttonDivision.TabIndex = 3;
            this.buttonDivision.Text = "/";
            this.buttonDivision.UseVisualStyleBackColor = true;
            this.buttonDivision.Click += new System.EventHandler(this.OnButtonOperationClick);
            // 
            // buttonSeven
            // 
            this.buttonSeven.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSeven.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonSeven.Location = new System.Drawing.Point(3, 107);
            this.buttonSeven.Name = "buttonSeven";
            this.buttonSeven.Size = new System.Drawing.Size(102, 40);
            this.buttonSeven.TabIndex = 4;
            this.buttonSeven.Text = "7";
            this.buttonSeven.UseVisualStyleBackColor = true;
            this.buttonSeven.Click += new System.EventHandler(this.OnButtonNumClick);
            // 
            // buttonEight
            // 
            this.buttonEight.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonEight.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonEight.Location = new System.Drawing.Point(111, 107);
            this.buttonEight.Name = "buttonEight";
            this.buttonEight.Size = new System.Drawing.Size(102, 40);
            this.buttonEight.TabIndex = 5;
            this.buttonEight.Text = "8";
            this.buttonEight.UseVisualStyleBackColor = true;
            this.buttonEight.Click += new System.EventHandler(this.OnButtonNumClick);
            // 
            // buttonNine
            // 
            this.buttonNine.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonNine.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonNine.Location = new System.Drawing.Point(219, 107);
            this.buttonNine.Name = "buttonNine";
            this.buttonNine.Size = new System.Drawing.Size(102, 40);
            this.buttonNine.TabIndex = 6;
            this.buttonNine.Text = "9";
            this.buttonNine.UseVisualStyleBackColor = true;
            this.buttonNine.Click += new System.EventHandler(this.OnButtonNumClick);
            // 
            // buttonMultiplication
            // 
            this.buttonMultiplication.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonMultiplication.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonMultiplication.Location = new System.Drawing.Point(327, 107);
            this.buttonMultiplication.Name = "buttonMultiplication";
            this.buttonMultiplication.Size = new System.Drawing.Size(104, 40);
            this.buttonMultiplication.TabIndex = 7;
            this.buttonMultiplication.Text = "*";
            this.buttonMultiplication.UseVisualStyleBackColor = true;
            this.buttonMultiplication.Click += new System.EventHandler(this.OnButtonOperationClick);
            // 
            // buttonFour
            // 
            this.buttonFour.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonFour.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonFour.Location = new System.Drawing.Point(3, 153);
            this.buttonFour.Name = "buttonFour";
            this.buttonFour.Size = new System.Drawing.Size(102, 40);
            this.buttonFour.TabIndex = 8;
            this.buttonFour.Text = "4";
            this.buttonFour.UseVisualStyleBackColor = true;
            this.buttonFour.Click += new System.EventHandler(this.OnButtonNumClick);
            // 
            // buttonFive
            // 
            this.buttonFive.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonFive.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonFive.Location = new System.Drawing.Point(111, 153);
            this.buttonFive.Name = "buttonFive";
            this.buttonFive.Size = new System.Drawing.Size(102, 40);
            this.buttonFive.TabIndex = 9;
            this.buttonFive.Text = "5";
            this.buttonFive.UseVisualStyleBackColor = true;
            this.buttonFive.Click += new System.EventHandler(this.OnButtonNumClick);
            // 
            // buttonSix
            // 
            this.buttonSix.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSix.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonSix.Location = new System.Drawing.Point(219, 153);
            this.buttonSix.Name = "buttonSix";
            this.buttonSix.Size = new System.Drawing.Size(102, 40);
            this.buttonSix.TabIndex = 10;
            this.buttonSix.Text = "6";
            this.buttonSix.UseVisualStyleBackColor = true;
            this.buttonSix.Click += new System.EventHandler(this.OnButtonNumClick);
            // 
            // buttonAddition
            // 
            this.buttonAddition.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonAddition.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonAddition.Location = new System.Drawing.Point(327, 153);
            this.buttonAddition.Name = "buttonAddition";
            this.buttonAddition.Size = new System.Drawing.Size(104, 40);
            this.buttonAddition.TabIndex = 11;
            this.buttonAddition.Text = "+";
            this.buttonAddition.UseVisualStyleBackColor = true;
            this.buttonAddition.Click += new System.EventHandler(this.OnButtonOperationClick);
            // 
            // buttonOne
            // 
            this.buttonOne.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonOne.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonOne.Location = new System.Drawing.Point(3, 199);
            this.buttonOne.Name = "buttonOne";
            this.buttonOne.Size = new System.Drawing.Size(102, 40);
            this.buttonOne.TabIndex = 12;
            this.buttonOne.Text = "1";
            this.buttonOne.UseVisualStyleBackColor = true;
            this.buttonOne.Click += new System.EventHandler(this.OnButtonNumClick);
            // 
            // buttonTwo
            // 
            this.buttonTwo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonTwo.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonTwo.Location = new System.Drawing.Point(111, 199);
            this.buttonTwo.Name = "buttonTwo";
            this.buttonTwo.Size = new System.Drawing.Size(102, 40);
            this.buttonTwo.TabIndex = 13;
            this.buttonTwo.Text = "2";
            this.buttonTwo.UseVisualStyleBackColor = true;
            this.buttonTwo.Click += new System.EventHandler(this.OnButtonNumClick);
            // 
            // buttonThree
            // 
            this.buttonThree.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonThree.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonThree.Location = new System.Drawing.Point(219, 199);
            this.buttonThree.Name = "buttonThree";
            this.buttonThree.Size = new System.Drawing.Size(102, 40);
            this.buttonThree.TabIndex = 14;
            this.buttonThree.Text = "3";
            this.buttonThree.UseVisualStyleBackColor = true;
            this.buttonThree.Click += new System.EventHandler(this.OnButtonNumClick);
            // 
            // buttonSubtraction
            // 
            this.buttonSubtraction.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonSubtraction.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonSubtraction.Location = new System.Drawing.Point(327, 199);
            this.buttonSubtraction.Name = "buttonSubtraction";
            this.buttonSubtraction.Size = new System.Drawing.Size(104, 40);
            this.buttonSubtraction.TabIndex = 15;
            this.buttonSubtraction.Text = "-";
            this.buttonSubtraction.UseVisualStyleBackColor = true;
            this.buttonSubtraction.Click += new System.EventHandler(this.OnButtonOperationClick);
            // 
            // buttonZero
            // 
            this.buttonZero.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonZero.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonZero.Location = new System.Drawing.Point(3, 245);
            this.buttonZero.Name = "buttonZero";
            this.buttonZero.Size = new System.Drawing.Size(102, 43);
            this.buttonZero.TabIndex = 16;
            this.buttonZero.Text = "0";
            this.buttonZero.UseVisualStyleBackColor = true;
            this.buttonZero.Click += new System.EventHandler(this.OnButtonNumClick);
            // 
            // buttonDelimiter
            // 
            this.buttonDelimiter.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonDelimiter.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonDelimiter.Location = new System.Drawing.Point(111, 245);
            this.buttonDelimiter.Name = "buttonDelimiter";
            this.buttonDelimiter.Size = new System.Drawing.Size(102, 43);
            this.buttonDelimiter.TabIndex = 17;
            this.buttonDelimiter.Text = ",";
            this.buttonDelimiter.UseVisualStyleBackColor = true;
            this.buttonDelimiter.Click += new System.EventHandler(this.OnButtonDelimiterClick);
            // 
            // buttonEquality
            // 
            this.tableLayoutPanel1.SetColumnSpan(this.buttonEquality, 2);
            this.buttonEquality.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonEquality.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.buttonEquality.Location = new System.Drawing.Point(219, 245);
            this.buttonEquality.Name = "buttonEquality";
            this.buttonEquality.Size = new System.Drawing.Size(212, 43);
            this.buttonEquality.TabIndex = 18;
            this.buttonEquality.Text = " =";
            this.buttonEquality.UseVisualStyleBackColor = true;
            this.buttonEquality.Click += new System.EventHandler(this.OnButtonEqualityClick);
            // 
            // display
            // 
            this.display.AutoSize = true;
            this.display.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.display.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tableLayoutPanel1.SetColumnSpan(this.display, 4);
            this.display.Dock = System.Windows.Forms.DockStyle.Fill;
            this.display.Font = new System.Drawing.Font("Microsoft Sans Serif", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.display.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.display.Location = new System.Drawing.Point(3, 0);
            this.display.Name = "display";
            this.display.Size = new System.Drawing.Size(428, 58);
            this.display.TabIndex = 19;
            this.display.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // Calculator
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.MenuBar;
            this.ClientSize = new System.Drawing.Size(434, 291);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(450, 330);
            this.Name = "Calculator";
            this.Text = "Calculator";
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button buttonC;
        private System.Windows.Forms.Button buttonCE;
        private System.Windows.Forms.Button buttonPlusMinus;
        private System.Windows.Forms.Button buttonDivision;
        private System.Windows.Forms.Button buttonSeven;
        private System.Windows.Forms.Button buttonEight;
        private System.Windows.Forms.Button buttonNine;
        private System.Windows.Forms.Button buttonMultiplication;
        private System.Windows.Forms.Button buttonFour;
        private System.Windows.Forms.Button buttonFive;
        private System.Windows.Forms.Button buttonSix;
        private System.Windows.Forms.Button buttonAddition;
        private System.Windows.Forms.Button buttonOne;
        private System.Windows.Forms.Button buttonTwo;
        private System.Windows.Forms.Button buttonThree;
        private System.Windows.Forms.Button buttonSubtraction;
        private System.Windows.Forms.Button buttonZero;
        private System.Windows.Forms.Button buttonDelimiter;
        private System.Windows.Forms.Button buttonEquality;
        private System.Windows.Forms.Label display;
    }
}

