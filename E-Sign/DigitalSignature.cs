using PdfSharp;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace E_Sign
{
    public partial class DigitalSignature : Form
    {
        public DigitalSignature()
        {
            InitializeComponent();


        }

        void ExportNewPDF()
        {

            PdfDocument document = new PdfDocument();

            document.Info.Title = "Created with PDFsharp";

            PdfPage page = document.AddPage();

            XGraphics gfx = XGraphics.FromPdfPage(page);


            XFont font = new XFont("Verdana", 20, XFontStyle.BoldItalic);


            // Draw 
            gfx.DrawString("Hello, World!", font, XBrushes.Black,

            new XRect(0, 0, page.Width, page.Height),

            XStringFormats.Center);

            const string filename = @"D:\Hello.pdf";

            document.Save(filename);

            Process.Start(filename);
        }

        private void GeneratePDF(string filename, string imageLoc)
        {
            PdfDocument document = new PdfDocument();

            // Create an empty page or load existing
            PdfPage page = document.AddPage();

            PdfSharp.PageSize p = page.Size;
            XUnit _x = page.Width / 2;
            XUnit _y = page.Height / 2;

            // Get an XGraphics object for drawing
            XGraphics gfx = XGraphics.FromPdfPage(page);
            DrawImage(gfx, imageLoc, 0, 0, 50, 50);

            // Save and start View
            document.Save(filename);
            Process.Start(filename);
        }

        void DrawImage(XGraphics gfx, string jpegSamplePath, int x, int y, int width, int height)
        {
            XImage image = XImage.FromFile(jpegSamplePath);
            gfx.DrawImage(image, x, y, width, height);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ExportNewPDF();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            GeneratePDF(@"D:\Hello.pdf", @"D:\vsDemo\videoStream\images\fall.png");
        }

        void OverlayDataOnPDF()
        {
            //PdfDocument document = new PdfDocument(@"D:\Hello.pdf");
            string filename = "CompareDocument1_tempfile.pdf";
            PdfDocument PDFDoc = PdfReader.Open(filename, PdfDocumentOpenMode.Import);
            //PdfSharp.Pdf.PdfDocument PDFDoc = PdfSharp.Pdf.IO.PdfReader.Open(@"D:\NQLD.pdf", PdfDocumentOpenMode.Import);
            PdfSharp.Pdf.PdfDocument PDFNewDoc = new PdfSharp.Pdf.PdfDocument();

            for (int Pg = 0; Pg < PDFDoc.Pages.Count; Pg++)
            {
                PDFNewDoc.AddPage(PDFDoc.Pages[Pg]);
            }

            // Create a new page        
            PdfPage page = PDFNewDoc.Pages[0];
            page.Orientation = PageOrientation.Portrait;

            XGraphics gfx = XGraphics.FromPdfPage(page, XPageDirection.Downwards);

            // Draw background
            gfx.DrawImage(XImage.FromFile(@"D:\vsDemo\videoStream\images\autumn.png"), 500, 500, 100, 100);

            PDFNewDoc.Save("dddd.pdf");

        }

        private void button3_Click(object sender, EventArgs e)
        {
            OverlayDataOnPDF();
        }

        void ImportFileExists()
        {
            string filename1 = "newfile.pdf",
                filename2 = "sample.pdf";

            // Open the input files

            PdfDocument inputDocument1 = PdfReader.Open(filename1, PdfDocumentOpenMode.Import);

            PdfDocument inputDocument2 = PdfReader.Open(filename2, PdfDocumentOpenMode.Import);

            PdfDocument outputDocument = new PdfDocument();

            outputDocument.PageLayout = PdfPageLayout.TwoColumnLeft;



            XFont font = new XFont("Verdana", 10, XFontStyle.Bold);

            XStringFormat format = new XStringFormat();

            format.Alignment = XStringAlignment.Center;

            format.LineAlignment = XLineAlignment.Far;

            XGraphics gfx;

            XRect box;

            int count = Math.Max(inputDocument1.PageCount, inputDocument2.PageCount);

            for (int idx = 0; idx < count; idx++)

            {

                // Get page from 1st document

                PdfPage page1 = inputDocument1.PageCount > idx ?

                inputDocument1.Pages[idx] : new PdfPage();




                // Get page from 2nd document

                PdfPage page2 = inputDocument2.PageCount > idx ?

                inputDocument2.Pages[idx] : new PdfPage();




                // Add both pages to the output document

                page1 = outputDocument.AddPage(page1);

                page2 = outputDocument.AddPage(page2);




                // Write document file name and page number on each page
                gfx = XGraphics.FromPdfPage(page1);

                box = page1.MediaBox.ToXRect();

                box.Inflate(0, -10);

                gfx.DrawString(String.Format("{0} • {1}", filename1, idx + 1),

                font, XBrushes.Red, box, format);

                gfx = XGraphics.FromPdfPage(page2);

                box = page2.MediaBox.ToXRect();

                box.Inflate(0, -10);

                gfx.DrawString(String.Format("{0} • {1}", filename2, idx + 1),

                font, XBrushes.Red, box, format);

            }

            const string filename = "CompareDocument1_tempfile.pdf";

            outputDocument.Save(filename);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            ImportFileExists();
        }
    }
}
