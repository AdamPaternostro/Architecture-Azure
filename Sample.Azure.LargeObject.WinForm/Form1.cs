using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Sample.Azure.LargeObject.WinForm
{
    public partial class Form1 : Form
    {
        private Model.LargeObject.LargeObjectModel largeObjectModel = null;

        public Form1()
        {
            InitializeComponent();
            // Configure the system based upon where it is running
            // This will set all configuration values and register all types for dependency injection
            Sample.Azure.Config.Configuration.Configure();

            largeObjectModel = CreateLargeObjectModel();
        }


        private Model.LargeObject.LargeObjectModel CreateLargeObjectModel()
        {
            Model.LargeObject.LargeObjectModel largeObjectModel = new Model.LargeObject.LargeObjectModel();

            largeObjectModel.LargeObjectId = 0;

            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            // 8 MB
            for (int i = 1; i <= 8000000; i++)
            {
                sb.Append("A");
            }
            largeObjectModel.Payload = sb.ToString();

            return largeObjectModel;
        }


        private void blobWrite_Click(object sender, EventArgs e)
        {
           // System.IO.File.WriteAllText("C:\\temp\\adam.txt", largeObjectModel.Payload);

            int loop = Int32.Parse(txtLoop.Text);

            // Register Repository (Override existing regristation)
            Sample.Azure.DI.Container.RegisterType<Sample.Azure.Interface.Repository.ILargeObjectRepository,
                Sample.Azure.Repository.File.LargeObject.LargeObjectRepository>();

            Interface.Service.ILargeObjectService largeObjectService = DI.Container.Resolve<Interface.Service.ILargeObjectService>();

            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            for (int i = 1; i <= loop; i ++)
            {
                largeObjectModel.LargeObjectId = i;
                largeObjectService.Save(largeObjectModel);
            }
            stopwatch.Stop();

            txtResult.Text = string.Empty;
            txtResult.Text += "Blob Write (" + loop.ToString() + ") took: " + stopwatch.ElapsedMilliseconds.ToString() + " milliseconds." + Environment.NewLine;
            txtResult.Text += "Blob Write (" + loop.ToString() + ") took: " + (stopwatch.ElapsedMilliseconds / 1000).ToString() + " seconds." + Environment.NewLine;
        }


        private void blobRead_Click(object sender, EventArgs e)
        {
            int loop = Int32.Parse(txtLoop.Text);

            // Register Repository (Override existing regristation)
            Sample.Azure.DI.Container.RegisterType<Sample.Azure.Interface.Repository.ILargeObjectRepository,
                Sample.Azure.Repository.File.LargeObject.LargeObjectRepository>();

            Interface.Service.ILargeObjectService largeObjectService = DI.Container.Resolve<Interface.Service.ILargeObjectService>();

            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            for (int i = 1; i <= loop; i++)
            {
                Model.LargeObject.LargeObjectModel loadedModel = largeObjectService.Get(i);
            }
            stopwatch.Stop();

            txtResult.Text = string.Empty;
            txtResult.Text += "Blob Read (" + loop.ToString() + ") took: " + stopwatch.ElapsedMilliseconds.ToString() + " milliseconds." + Environment.NewLine;
            txtResult.Text += "Blob Read (" + loop.ToString() + ") took: " + (stopwatch.ElapsedMilliseconds / 1000).ToString() + " seconds." + Environment.NewLine;
        }



        private void searchWrite_Click(object sender, EventArgs e)
        {
            int loop = Int32.Parse(txtLoop.Text);

            Interface.Repository.IIndexerRepository azureSearch = DI.Container.Resolve<Interface.Repository.IIndexerRepository>();

            Model.Search.SearchLargeObjectModel searchLargeObjectModel = new Model.Search.SearchLargeObjectModel();
            searchLargeObjectModel.LargeObjectId = largeObjectModel.LargeObjectId.ToString();
            searchLargeObjectModel.Payload = largeObjectModel.Payload;

            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            for (int i = 1; i <= loop; i++)
            {
                searchLargeObjectModel.LargeObjectId = i.ToString();
                azureSearch.UpsertLargeObject(Interface.GlobalEnum.IndexerIndexName.LargeObject, searchLargeObjectModel);
            }
            stopwatch.Stop();

            txtResult.Text = string.Empty;
            txtResult.Text += "Search Write (" + loop.ToString() + ") took: " + stopwatch.ElapsedMilliseconds.ToString() + " milliseconds." + Environment.NewLine;
            txtResult.Text += "Search Write (" + loop.ToString() + ") took: " + (stopwatch.ElapsedMilliseconds / 1000).ToString() + " seconds." + Environment.NewLine;
        }



        private void searchRead_Click(object sender, EventArgs e)
        {
            int loop = Int32.Parse(txtLoop.Text);

            Interface.Repository.IIndexerRepository azureSearch = DI.Container.Resolve<Interface.Repository.IIndexerRepository>();

            Model.Search.SearchLargeObjectModel searchLargeObjectModel = new Model.Search.SearchLargeObjectModel();
            searchLargeObjectModel.LargeObjectId = largeObjectModel.LargeObjectId.ToString();
            searchLargeObjectModel.Payload = largeObjectModel.Payload;

            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            for (int i = 1; i <= loop; i++)
            {
                azureSearch.LargeObjectSearch(Interface.GlobalEnum.IndexerIndexName.LargeObject, i.ToString());
            }
            stopwatch.Stop();

            txtResult.Text = string.Empty;
            txtResult.Text += "Read Write (" + loop.ToString() + ") took: " + stopwatch.ElapsedMilliseconds.ToString() + " milliseconds." + Environment.NewLine;
            txtResult.Text += "Read Write (" + loop.ToString() + ") took: " + (stopwatch.ElapsedMilliseconds / 1000).ToString() + " seconds." + Environment.NewLine;
        }



        private void cacheWrite_Click(object sender, EventArgs e)
        {
            // Get the current settings.
            int minWorker, minIOC;
            System.Threading.ThreadPool.GetMinThreads(out minWorker, out minIOC);
            txtResult.Text = string.Empty;
            txtResult.Text += "minWorker = " + minWorker.ToString() + Environment.NewLine;
            txtResult.Text += "minIOC = " + minWorker.ToString() + Environment.NewLine;

            int newSetting = 200;
            if (System.Threading.ThreadPool.SetMinThreads(newSetting, newSetting))
            {
                // The minimum number of threads was set successfully.
                txtResult.Text += "Thread pool set to = " + newSetting.ToString() + Environment.NewLine;
            }
            else
            {
                // The minimum number of threads was not changed.
                txtResult.Text += "ERROR: Thread pool NOT set";
            }

            int loop = Int32.Parse(txtLoop.Text);

            Interface.Repository.ICacheRepository cacheManager = DI.Container.Resolve<Interface.Repository.ICacheRepository>();
            string cacheKey = largeObjectModel.LargeObjectId.ToString();

            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            for (int i = 1; i <= loop; i++)
            {
                largeObjectModel.LargeObjectId = i;
                cacheKey = i.ToString();
                cacheManager.Set(cacheKey, largeObjectModel, TimeSpan.FromMinutes(10));
            }
            stopwatch.Stop();

            txtResult.Text = string.Empty;
            txtResult.Text += "Cache Write (" + loop.ToString() + ") took: " + stopwatch.ElapsedMilliseconds.ToString() + " milliseconds." + Environment.NewLine;
            txtResult.Text += "Cache Write (" + loop.ToString() + ") took: " + (stopwatch.ElapsedMilliseconds / 1000).ToString() + " seconds." + Environment.NewLine;

        }

        private void cacheRead_Click(object sender, EventArgs e)
        {
            int loop = Int32.Parse(txtLoop.Text);

            Interface.Repository.ICacheRepository cacheManager = DI.Container.Resolve<Interface.Repository.ICacheRepository>();
            string cacheKey = largeObjectModel.LargeObjectId.ToString();
            Model.LargeObject.LargeObjectModel loadedModel = null;

            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();
            for (int i = 1; i <= loop; i++)
            {
                cacheKey = i.ToString();
                loadedModel = cacheManager.Get<Model.LargeObject.LargeObjectModel>(cacheKey);
            }
            stopwatch.Stop();

            txtResult.Text = string.Empty;
            txtResult.Text += "Cache Read (" + loop.ToString() + ") took: " + stopwatch.ElapsedMilliseconds.ToString() + " milliseconds." + Environment.NewLine;
            txtResult.Text += "Cache Read (" + loop.ToString() + ") took: " + (stopwatch.ElapsedMilliseconds / 1000).ToString() + " seconds." + Environment.NewLine;
        }
    }
}
