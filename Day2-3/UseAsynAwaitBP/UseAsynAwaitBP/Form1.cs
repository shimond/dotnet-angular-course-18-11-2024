namespace UseAsynAwaitBP
{
    public partial class Form1 : Form
    {

        private string[] urls = ["http://www.walla.co.il", "http://www.ynet.co.il", "http://www.google.co.il",];

        public Form1()
        {
            InitializeComponent();
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            StartDownload();
            this.Text = "Download!!";
        }

        private async void StartDownload()
        {
            int sum = 0;
            foreach (var item in urls)
            {
                listBoxLogs.Items.Add($"Start download from {item}");
                try
                {
                    var bytesSize = await DownlodUrl(item);
                    sum += bytesSize;
                    listBoxLogs.Items.Add($"Download from {item} finished with {bytesSize} bytes");

                }
                catch (Exception ex)
                {
                    listBoxLogs.Items.Add($"Download from {item} Failed! with {ex.Message} error");
                }
            }
            this.Text = "Total size:" + sum.ToString();
        }

        //private int DownlodUrl(string item)
        //{
        //    Thread.Sleep(5000);
        //    if(item.ToLower().Contains("ynet"))
        //    {
        //        throw new Exception("url is not valid");
        //    }
        //    return Random.Shared.Next(10000, 100000);
        //}

        private Task<int> DownlodUrl(string item)
        {
            var t = new Task<int>(() =>
            {
                Thread.Sleep(5000);
                if (item.ToLower().Contains("ynet"))
                {
                    throw new Exception("url is not valid");
                }
                var res = Random.Shared.Next(10000, 100000);
                return res;

            });
            t.Start();
            return t;


        }
    }
}
