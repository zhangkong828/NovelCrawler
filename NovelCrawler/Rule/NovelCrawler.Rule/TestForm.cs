using NovelCrawler.Common;
using NovelCrawler.Models;
using NovelCrawler.Processer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NovelCrawler.Rule
{
    public partial class TestForm : Form
    {
        public TestForm(RuleModel rule)
        {
            InitializeComponent();
            CheckForIllegalCrossThreadCalls = false;
            Task.Run(() =>
            {
                try
                {
                    RunTest(rule);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }

            });

        }

        private void ExcuteRecord(string msg)
        {
            if (rtb_record.IsHandleCreated)
            {
                rtb_record.Invoke(new Action(() =>
                {
                    rtb_record.AppendText(msg + "\r\n");
                    rtb_record.ScrollToCaret();
                }));
            }
        }

        private void RunTest(RuleModel rule)
        {
            Logger._customAction = ExcuteRecord;
            var spider = new Spider(null, rule);
            spider.TestRule();
        }

    }
}
