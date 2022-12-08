using ImageProcessing_DIP2_Activity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

using Image = System.Drawing.Image;

namespace ImageProcessing_Activity
{
    public partial class BasicImageProcessing : Form
    {
        Bitmap loaded, processed;
        public BasicImageProcessing()
        {
            InitializeComponent();
            openFileDialog1 = new OpenFileDialog();//Open dialog
            openFileDialog1.RestoreDirectory = true;
            openFileDialog1.InitialDirectory = "C:\\";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.Filter = "jpg Files (*.jpg)|*.jpg|gif Files (*.gif)|*.gif|png Files (*.png)|*.png |bmp Files (*.bmp)|*.bmp";

            saveFileDialog1 = new SaveFileDialog();//Save dialog
            saveFileDialog1.RestoreDirectory = true;
            saveFileDialog1.InitialDirectory = "C:\\";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.Filter = "jpg Files (*.jpg)|*.jpg|gif Files (*.gif)|*.gif|png Files (*.png)|*.png |bmp Files (*.bmp)|*.bmp";
        }
        /// <summary>
        /// Open file to pick image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == openFileDialog1.ShowDialog()) 
            {
                loaded = new Bitmap(openFileDialog1.FileName);
                pictureBox1.Image = loaded;
                pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            }
        }

        /// <summary>
        /// Make image Greyscale
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void greyScaleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = new Bitmap(loaded.Width, loaded.Height);
            for (int x=0; x<loaded.Width; x++)
                for(int y=0; y<loaded.Height; y++)
                {
                    Color pixel = loaded.GetPixel(x, y);
                    int grey = (pixel.R + pixel.G + pixel.B) / 3;
                    Color greyscale = Color.FromArgb(grey,grey,grey);
                    processed.SetPixel(x, y, greyscale);
                }
            pictureBox2.Image = processed;
        }
        /// <summary>
        /// Color Inversion
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void invertToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = new Bitmap(loaded.Width, loaded.Height);
            for (int x = 0; x < loaded.Width; x++)
                for (int y = 0; y < loaded.Height; y++)
                {
                    Color pixel = loaded.GetPixel(x, y);
                    processed.SetPixel(x,y,Color.FromArgb(255-pixel.R,255-pixel.G,255-pixel.B));
                }
            pictureBox2.Image = processed;
        }

       
        /// <summary>
        /// Make the image Sepia
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void sepiaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            processed = new Bitmap(loaded.Width, loaded.Height);
            //color of pixel
            Color p;

            //sepia
            for (int y = 0; y < loaded.Height; y++)
            {
                for (int x = 0; x < loaded.Width; x++)
                {
                    //get pixel value
                    p = loaded.GetPixel(x, y);

                    //extract pixel component ARGB
                    int a = p.A;
                    int r = p.R;
                    int g = p.G;
                    int b = p.B;

                    //calculate temp value
                    int tr = (int)(0.393 * r + 0.769 * g + 0.189 * b);
                    int tg = (int)(0.349 * r + 0.686 * g + 0.168 * b);
                    int tb = (int)(0.272 * r + 0.534 * g + 0.131 * b);

                    //set new RGB value
                    if (tr > 255)
                    {
                        r = 255;
                    }
                    else
                    {
                        r = tr;
                    }

                    if (tg > 255)
                    {
                        g = 255;
                    }
                    else
                    {
                        g = tg;
                    }

                    if (tb > 255)
                    {
                        b = 255;
                    }
                    else
                    {
                        b = tb;
                    }

                    //set the new RGB value in image pixel
                    processed.SetPixel(x, y, Color.FromArgb(a, r, g, b));
                }
            }

            //load sepia image in picturebox2
            pictureBox2.Image = processed;
        }
        /// <summary>
        /// Make a basic copy of image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void basicCopyToolStripMenuItem_Click(object sender, EventArgs e)
        {

            processed = new Bitmap(loaded.Width, loaded.Height);  
            for(int x =0; x<loaded.Width; x++)
                for(int y=0; y<loaded.Height; y++)
                {
                    Color p = loaded.GetPixel(x,y);
                    processed.SetPixel(x, y, p);
                }
            pictureBox2.Image = processed;
        }
        /// <summary>
        /// Make a Histogram
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void histogramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Color sample;
            Color gray;
            Byte graydata;
            processed = new Bitmap(loaded.Width, loaded.Height);
            //Grayscale Convertion;
            for (int x = 0; x <loaded.Width; x++)
            {
                for (int y = 0; y < loaded.Height; y++)
                {
                    sample = loaded.GetPixel(x, y);
                    graydata = (byte)((sample.R + sample.G + sample.B) / 3);
                    gray = Color.FromArgb(graydata, graydata, graydata);
                    processed.SetPixel(x, y, gray);
                }
            }
            //histogram 1d data;
            int[] histdata = new int[256]; // array from 0 to 255
            for (int x = 0; x < processed.Width; x++)
            {
                for (int y = 0; y < processed.Height; y++)
                {
                    sample = processed.GetPixel(x, y);
                    histdata[sample.R]++; // can be any color property r,g or b
                }
            }

            // Bitmap Graph Generation
            // Setting empty Bitmap with background color
            processed = new Bitmap(256, 800);
            for (int x = 0; x < 256; x++)
            {
                for (int y = 0; y < 800; y++)
                {
                    processed.SetPixel(x, y, Color.White);
                }
            }
            // plotting points based from histdata
            for (int x = 0; x < 256; x++)
            {
                for (int y = 0; y < Math.Min(histdata[x] / 5, processed.Height - 1); y++)
                {
                    processed.SetPixel(x, (processed.Height - 1) - y, Color.Black);
                }
            }

            pictureBox2.Image = processed;
        }

    

        private void saveFileDialog1_FileOk(object sender, CancelEventArgs e)
        {

        }
        /// <summary>
        /// Save the image
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (DialogResult.OK == saveFileDialog1.ShowDialog())
            {
                Image img = (Image)processed;
                img.Save(saveFileDialog1.FileName);
            }
            
        }

        private void subtractionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GreenScreen subtractionForm = new GreenScreen();
            subtractionForm.ShowDialog();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
