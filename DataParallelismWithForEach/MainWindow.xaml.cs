using System.Windows;
using System.Threading;
using System.IO;
using System;
using System.Drawing;
using System.Threading.Tasks;

namespace DataParallelismWithForEach
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private object paralleLock = new object();
        // New Window-level variable.
        private CancellationTokenSource cancelToken = new CancellationTokenSource();
        public MainWindow()
        {
            InitializeComponent();
        }

        private void cmdCancel_Click(object sender, EventArgs e)
        {
            // This will be used to tell all the worker threads to stop!
            cancelToken.Cancel();

        }
        private void cmdProcess_Click(object sender, EventArgs e)
        {
            //ProcessFiles();
            // Start a new "task" to process the files.
            Task.Factory.StartNew(() => ProcessFiles());
        }
        private void ProcessFiles()
        {

            // Load up all *.jpg files, and make a new folder for the modified data.
            string[] files = Directory.GetFiles(@".\TestPictures", "*.png", SearchOption.
            AllDirectories);
            string newDir = @".\ModifiedPictures";
            Directory.CreateDirectory(newDir);
            // Process the image data in a blocking manner.
            //foreach (string currentFile in files)
            //{
            //    string filename = Path.GetFileName(currentFile);
            //    using (Bitmap bitmap = new Bitmap(currentFile))
            //    {
            //        bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
            //        bitmap.Save(Path.Combine(newDir, filename));
            //        // Print out the ID of the thread processing the current image.
            //        this.Title = $"Processing {filename} on thread {Thread.CurrentThread.ManagedThreadId}";
            //    }
            //}

            //Process the image data in a parallel manner!
            //Parallel.ForEach(files, currentFile =>
            //{

            //    string filename = Path.GetFileName(currentFile);

            //    using (Bitmap bitmap = new Bitmap(currentFile))
            //    {
            //        bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
            //        bitmap.Save(Path.Combine(newDir, filename));
            //        // Print out the ID of the thread processing the current image. 

            //        this.Dispatcher.Invoke(() =>
            //        {
            //            this.Title = $"Processing {filename} on thread {Thread.CurrentThread.ManagedThreadId}";
            //        }
            //        );



            //    }


            //});

            try
            {

                // Use ParallelOptions instance to store the CancellationToken.
                ParallelOptions parOpts = new ParallelOptions();
                parOpts.CancellationToken = cancelToken.Token;
                parOpts.MaxDegreeOfParallelism = System.Environment.ProcessorCount;

                // Process the image data in a parallel manner!
                Parallel.ForEach(files, parOpts, currentFile =>
                {
                    parOpts.CancellationToken.ThrowIfCancellationRequested();
                    string filename = Path.GetFileName(currentFile);
                    using (Bitmap bitmap = new Bitmap(currentFile))
                    {
                        bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);
                        bitmap.Save(Path.Combine(newDir, filename));
                        this.Dispatcher.Invoke((Action)delegate
                            {
                        this.Title = $"Processing {filename} on thread {Thread.CurrentThread.ManagedThreadId}";});
                    }
                });
                this.Dispatcher.Invoke((Action)delegate
                {
                    this.Title = "Done!";
                });
            }

            catch (OperationCanceledException ex)
            {
                this.Dispatcher.Invoke((Action)delegate
                {
                    this.Title = "Exception  :" + ex.Message;
                });
            }


        }

    }
    
}
   
